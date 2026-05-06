using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Tipo de representacion de la etiqueta
    /// </summary>
    public enum TypeStat
    {
        SECUENTIAL, //Muestra tipo -----, solo incrementa y decrementa. Sin referencia
        INCREMENTAL //Muestra tipo --/--. Tiene una referencia que alcanzar
    }

    public enum TypeAnim
    {
        FADE_SLIDE, //Desaparece y se desplaza el valor
        PULSE, //Hace efecto de pulso
        ROLLING_DRUM //Efecto cuentakilometros

    }

    /// <summary>
    /// Representa el elemento que muestra en el juego, alguna caracterisita de estado
    /// (repeticiones, tiempos, puzzles)
    /// </summary>
    public class Stat
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Parametros del panel de stats
        private Label mStat; //Etiqueta con la caracateristica
        private Label mStatPrev; //Etiqueta debajo para animaciones
        private Label mSeparator; //Etiqueta separadora
        private Label mTarget; //Etiqueta con el objetivo a alcanzar

        private VisualElement mContainer; //Contenedor donde se colocan los elementos
        private StringIcon mIcon; //Icono      

        //Digitos del contador
        private Digit value;

        // Selectores de estilos        
        const string kAnimPulse = "statdisplay_pulse"; //Pulso

        //Plantilla - Elemento central
        private VisualElement template;

        //Tipo de caracteristica
        private TypeStat mType;

        //Tipo de conteo
        private TypeTimeline type = TypeTimeline.NONE;


        //Tipo de animacion
        private TypeAnim mTypeAnim = TypeAnim.ROLLING_DRUM;
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*********************************Inicialización**********************************//
        //*********************************************************************************//
        #region [Function] Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="tmpl"></param>
        public Stat(VisualElement tmpl, TypeStat type, TypeAnim typeanim)
        {
            template = tmpl;
            mType = type;
            mTypeAnim = typeanim;
            SetVisualElements();            
        }
        #endregion

        #region [Function] Componentes visuales
        /// <summary>
        /// Registra los componentes del fichero UXML y los reestructura
        /// </summary>
        /// <param name="shopItemElement"></param>
        public void SetVisualElements()
        {
            // Busca las partes del elemento           
            mStat = template.Q<Label>("stat-label-current");
            mStatPrev = template.Q<Label>("stat-label-previous");
            mSeparator = template.Q<Label>("separator-label");
            mTarget = template.Q<Label>("target-label");
            mContainer = template.Q<VisualElement>("statdisplay__container");
            mIcon = template.Q<StringIcon>("statdisplay-icon");


            //Si la animacion es de odometro, se asignas variables adicionales
            if (mTypeAnim == TypeAnim.ROLLING_DRUM || mTypeAnim == TypeAnim.FADE_SLIDE)
            {                
                // Mapea los 6 módulos a sus Frames del UXML
                value = new Digit(mStat, mStatPrev, "statdisplay");

                //Registrar tiempo de animacion
                RegisterTimeAnimation(mStat);
            }            
            //Si el tipo es secuencial reestructura el componente
            if (mType == TypeStat.SECUENTIAL)
            {                
                //Quita etiquetas extra
                mSeparator.RemoveFromHierarchy();
                mTarget.RemoveFromHierarchy();                

                //Actualiza contenedor principal para que ocupe todo el espacio
                VisualElement element = template.Q<VisualElement>("statdisplay__container-odometer");
                element.style.width = new Length(100, LengthUnit.Percent);

                //Alinea al centro los textos
                mStat.style.unityTextAlign = TextAnchor.MiddleCenter;
                mStatPrev.style.unityTextAlign = TextAnchor.MiddleCenter;               
            }           

        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
               


        //*******************Gestion visual del marcador de puntuacion*********************//
        //*********************************************************************************//
        #region Gestion de estadisticas numericas
        /// <summary>
        /// Inicializa etiquetas del panel de puntuaciones
        /// </summary>
        /// <param name="cscore"></param>
        public void Init(int cscore, int target)
        {
            //Dependiendo del valor inicial, es cuenta hacia atras o hacia delante
            if (cscore > 0)
                type = TypeTimeline.REGRESIVE;
            else
                type = TypeTimeline.FORWARD;

            //Inicializa etiqueta principal
            if(value!=null)
                value.Init(cscore, type, mTypeAnim);
            

            //Actualiza etiqueta objetivo
            UpdateTarget(target);                      

            //Actualizacion de tamaño de letra
            Extension.BindAutoFontSize(mStat, mContainer, 0.50f);
            Extension.BindAutoFontSize(mStatPrev, mContainer, 0.50f);
            Extension.BindAutoFontSize(mTarget, mContainer, 0.50f);
            Extension.BindAutoFontSize(mSeparator, mContainer, 0.50f);

            //Actualiza tamaño del icono
            Extension.BindAutoIconSize(mIcon, mContainer, 0.50f);
          
        }        

        /// <summary>
        /// Actualiza la puntuacion actual realiza una animacion que depende del tiempo como
        /// la del odometro
        /// </summary>
        /// <param name="score"></param>
        public void Update(int score, float deltaTime)
        {
            //Comprueba que no baja de cero
            if (score < 0) score = 0;

            //Ejecuta instruccion dependiendo de la animacion
            value.Set(score, deltaTime);
        }

        /// <summary>
        /// Esta funcion de actualizacion se utiliza solo al momento de realizar la 
        /// animacion con el pulso
        /// </summary>                
        public void Update(int score)
        {
            //Comprueba que no baja de cero
            if (score < 0) score = 0;

            //Actualiza valor actual
            mStat.text = $"{score}";   

            if (mTypeAnim == TypeAnim.PULSE)
            {
                // Aplica una animacion de pulso sobre el texto.
                mStat.AddToClassList(kAnimPulse);

                //Quita a los 100 ms
                mStat.schedule.Execute(() =>
                    mStat.RemoveFromClassList(kAnimPulse)
               ).StartingIn(100); //Delay de 0.1s    */
            }               
        }       

        /// <summary>
        /// Actualiza la puntuacion mas alta
        /// </summary>
        /// <param name="score"></param>
        public void UpdateTarget(int score)
        {
           mTarget.text = $"{score.ToString("0")}";
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
        
       

        //**************************Gestion visual del marcador****************************//
        //*********************************************************************************//
        #region Procesamiento USS
        /// <summary>
        /// Obtien el valor del tiempo de animacion animacion
        /// </summary>
        private void RegisterTimeAnimation(Label label)
        {
            template.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                //Obtiene el umbral
                float threshold = label.resolvedStyle.transitionDuration.First().value;

                //Actualiza umbral de los digitos
                value.Threshold = threshold;                
            });
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
