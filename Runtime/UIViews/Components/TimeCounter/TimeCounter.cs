using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace UIInterface
{
    /// <summary>
    /// Representa el elemento que muestra en el juego, alguna caracterisita de estado
    /// (repeticiones, tiempos, puzzles)
    /// </summary>
    public class TimeCounter
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Etiquetas de hora, minuto y segundos
        //Tipo de conteo
        private TypeTimeline type = TypeTimeline.NONE;

        //Digitos del contador
        private Digit segUnidad, segDecena;
        private Digit minUnidad, minDecena;
        private Digit horaUnidad, horaDecena;

        //Tipo de contador (segundos,minutos u horas)
        private TypeTime typeCounter = TypeTime.NONE;

        //Nombres de las etiquetas
        private string[] mLabelsName = new string[] //Nombres de todas las etiquetas
        {
            "digit-hour-decena-current", "digit-hour-decena-next", "digit-hour-unidad-current", "digit-hour-unidad-next",
            "digit-min-decena-current", "digit-min-decena-next", "digit-min-unidad-current", "digit-min-unidad-next",
            "digit-sec-decena-current", "digit-sec-decena-next", "digit-sec-unidad-current", "digit-sec-unidad-next",
        };

        //Nombres de los componentes con digitos - Por si hay que reestructurar
        private string[] mDigitPanelName = new string[]
        {
            "timedisplay__hour-component", "timedisplay__minu-component", "timedisplay__secnd-component"
        };
        private string[] mSepararorName = new string[] //Nombre de separadores
        {
            "separator_point-hour-minute", "separator_point-minute-second"
        };

        //Plantilla - Elemento central
        private VisualElement template;
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
        public TimeCounter(VisualElement tmpl, TypeTime typc)
        {
            template = tmpl;
            typeCounter = typc;
            SetVisualElements();
        }
        #endregion

        #region [Function] Inicializacion de componentes visuales
        /// <summary>
        /// Registra los componentes del fichero UXML y los reestructura
        /// </summary>
        /// <param name="shopItemElement"></param>
        public void SetVisualElements()
        {
            //Inicializa contender
            VisualElement mContainer = template.Q<VisualElement>("statdisplay__container-text"); //Contenedor

            //Inicializa array de labels  
            //[Hora Decena Actual, Hora Decena Next, Hora Unidad Actual, Hora Unidad Next]                         
            Label[] mLabelComp = new Label[mLabelsName.Length];


            // Busca las etiquetas del componente
            for (int i = 0; i < mLabelsName.Length; i++)
            {
                mLabelComp[i] = template.Q<Label>(mLabelsName[i]);

                //Actualizacion de tamaño de letra
                Extension.BindAutoFontSize(mLabelComp[i], mContainer, 0.60f);
            }

            //Actualiza tamaño de icono
            StringIcon mIcon = template.Q<StringIcon>("timedisplay-icon");
            Extension.BindAutoIconSize(mIcon, mContainer, 0.75f);

            //Actualiza tamaño de separador de puntos
            Label separator1 = template.Q<Label>("separator_point-minute-second");
            Extension.BindAutoFontSize(separator1, mContainer, 0.50f);
            Label separator2 = template.Q<Label>("separator_point-hour-minute");
            Extension.BindAutoFontSize(separator2, mContainer, 0.50f);


            // Mapea los módulos de segundos a sus Frames del UXML
            segUnidad = new Digit(mLabelComp[10], mLabelComp[11], "timedisplay");
            segDecena = new Digit(mLabelComp[8], mLabelComp[9], "timedisplay"); // Segundos llegan a 59

            if (typeCounter == TypeTime.SECONDS) //Solo añade segundos
            {
                template.Q<VisualElement>(mDigitPanelName[0]).style.visibility = Visibility.Hidden; //Esconde horas
                template.Q<VisualElement>(mDigitPanelName[1]).style.visibility = Visibility.Hidden; // y minutos
                template.Q<VisualElement>(mSepararorName[0]).style.visibility = Visibility.Hidden; //Esconde puntos horas-minutos
                template.Q<VisualElement>(mSepararorName[1]).style.visibility = Visibility.Hidden; //Esconde puntos minutos-segundos

                //Redistribuye los paneles
                template.Q<VisualElement>(mDigitPanelName[0]).style.width = new Length(0.0f, LengthUnit.Percent);
                template.Q<VisualElement>(mDigitPanelName[1]).style.width = new Length(0.0f, LengthUnit.Percent);
                template.Q<VisualElement>(mDigitPanelName[2]).style.width = new Length(100f, LengthUnit.Percent);
                template.Q<VisualElement>(mSepararorName[0]).style.width = new Length(0, LengthUnit.Percent);
                template.Q<VisualElement>(mSepararorName[1]).style.width = new Length(0, LengthUnit.Percent);

            }
            else if (typeCounter == TypeTime.MINUTES) //Añade minutos y segundos
            {
                template.Q<VisualElement>(mDigitPanelName[0]).style.visibility = Visibility.Hidden; //Esconde horas
                template.Q<VisualElement>(mSepararorName[0]).style.visibility = Visibility.Hidden; //Esconde puntos horas-minutos                

                //Redistribuye los paneles
                template.Q<VisualElement>(mDigitPanelName[0]).style.width = new Length(0.0f, LengthUnit.Percent);
                template.Q<VisualElement>(mDigitPanelName[1]).style.width = new Length(47.5f, LengthUnit.Percent);
                template.Q<VisualElement>(mDigitPanelName[2]).style.width = new Length(47.5f, LengthUnit.Percent);
                template.Q<VisualElement>(mSepararorName[0]).style.width = new Length(0, LengthUnit.Percent);

                // Mapea los módulos de minutos y horas a sus Frames del UXML
                minUnidad = new Digit(mLabelComp[6], mLabelComp[7], "timedisplay");
                minDecena = new Digit(mLabelComp[4], mLabelComp[5], "timedisplay"); // Minutos llegan a 59
            }
            else if (typeCounter == TypeTime.HOURS) //Añade todos los digitos
            {
                // Mapea los módulos de minutos y horas a sus Frames del UXML
                minUnidad = new Digit(mLabelComp[6], mLabelComp[7], "timedisplay");
                minDecena = new Digit(mLabelComp[4], mLabelComp[5], "timedisplay"); // Minutos llegan a 59

                horaUnidad = new Digit(mLabelComp[2], mLabelComp[3], "timedisplay");
                horaDecena = new Digit(mLabelComp[0], mLabelComp[1], "timedisplay"); // Horas llegan a 24 máxim  
            }

            //Registrar tiempo de animacion
            RegisterTimeAnimation(mLabelComp[0]);
        }

        /// <summary>
        /// Inicializa etiquetas del panel de puntuaciones
        /// </summary>
        /// <param name="cscore"></param>
        public void Init(double cscore, int target)
        {
            //Dependiendo del valor inicial, es cuenta hacia atras o hacia delante
            if (cscore > 0)
                type = TypeTimeline.REGRESIVE;
            else
                type = TypeTimeline.FORWARD;

            //Calculo matematico del tiempo a partir de los segundos
            int hours = (int)(cscore / 3600f);
            int minutes = (int)((cscore % 3600) / 60f);
            int seconds = (int)(cscore % 60f);

            // Actualiza cada rueda con su número correspondiente
            segUnidad.Init(seconds % 10, type, TypeAnim.ROLLING_DRUM);
            segDecena.Init(seconds / 10, type, TypeAnim.ROLLING_DRUM);

            if (minDecena != null && minUnidad != null)
            {
                minUnidad.Init(minutes % 10, type, TypeAnim.ROLLING_DRUM);
                minDecena.Init(minutes / 10, type, TypeAnim.ROLLING_DRUM);
            }

            if (horaUnidad != null && horaDecena != null)
            {
                horaUnidad.Init(hours % 10, type, TypeAnim.ROLLING_DRUM);
                horaDecena.Init(hours / 10, type, TypeAnim.ROLLING_DRUM);
            }
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
        

            
        //**************************Gestion visual del marcador****************************//
        //*********************************************************************************//
        #region Actualizacion de los digitos
        /// <summary>
        /// Actualiza el contador dependiendo del tiempo. 
        /// Las 6 ruedas se posicionarán solas instantaneamente, calculando las horas,
        /// minutos y segundos
        /// </summary>
        /// <param name="score"></param>
        public void Update(double secondsTotal, float deltaTime)
        {
            //Comprueba que no baja de cero
            if (secondsTotal < 0) secondsTotal = 0;

            if (type == TypeTimeline.REGRESIVE)
                secondsTotal = Mathf.CeilToInt((float)secondsTotal);
            else if (type == TypeTimeline.FORWARD)
                secondsTotal = Mathf.FloorToInt((float)secondsTotal);

            //Calculo matematico del tiempo a partir de los segundos
            int hours = (int)(secondsTotal / 3600f);
            int minutes = (int)((secondsTotal % 3600) / 60f);
            int seconds = (int)(secondsTotal % 60f);

            // Actualiza cada rueda con su número correspondiente
            segUnidad.Set(seconds % 10, deltaTime);
            segDecena.Set(seconds / 10, deltaTime);

            if (minDecena != null && minUnidad != null)
            {
                minUnidad.Set(minutes % 10, deltaTime);
                minDecena.Set(minutes / 10, deltaTime);
            }

            if (horaUnidad != null && horaDecena != null)
            {
                horaUnidad.Set(hours % 10, deltaTime);
                horaDecena.Set(hours / 10, deltaTime);
            }
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
                if (horaDecena != null) horaDecena.Threshold = threshold;
                if (horaUnidad != null) horaUnidad.Threshold = threshold;
                if (minDecena != null) minDecena.Threshold = threshold;
                if (minUnidad != null) minUnidad.Threshold = threshold;
                segDecena.Threshold = threshold;
                segUnidad.Threshold = threshold;
            });                       
        }
        
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }    
}
