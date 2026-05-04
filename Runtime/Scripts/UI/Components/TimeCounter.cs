using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace UIInterface
{
    /// <summary>
    /// Cantidad de elementos que se muestran
    /// </summary>
    public enum TypeTime
    {
        SECONDS, //Solo se muestran segundos
        MINUTES, //Se muestran minutos y segundos
        HOURS //Se muestran horas, minutos y segundos
    }

    /// <summary>
    /// Tipo de cuenta, hacia atrás o hacia adelante
    /// </summary>
    public enum TypeTimeline
    {
        REGRESIVE, //Cuenta atras - Animacion arriba-abajo
        FORWARD, //Cuenta adelante - Animacion abajo-arriba
        NONE //Sin asignar
    }

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

        //Nombres de las etiquetas
        private string[] mLabelsName = new string[] //Nombres de todas las etiquetas
        {
            "digit-hour-decena-current", "digit-hour-decena-next", "digit-hour-unidad-current", "digit-hour-unidad-next",
            "digit-min-decena-current", "digit-min-decena-next", "digit-min-unidad-current", "digit-min-unidad-next",
            "digit-sec-decena-current", "digit-sec-decena-next", "digit-sec-unidad-current", "digit-sec-unidad-next",
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
        public TimeCounter(VisualElement tmpl) 
        {
            template = tmpl;            
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
            VisualElement mContainer = template.Q <VisualElement>("statdisplay__container-text"); //Contenedor

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
            

            // Mapea los 6 módulos a sus Frames del UXML
            segUnidad = new Digit(mLabelComp[10], mLabelComp[11], "timedisplay");
            segDecena = new Digit(mLabelComp[8], mLabelComp[9], "timedisplay"); // Segundos llegan a 59

            minUnidad = new Digit(mLabelComp[6], mLabelComp[7], "timedisplay");
            minDecena = new Digit(mLabelComp[4], mLabelComp[5], "timedisplay"); // Minutos llegan a 59

            horaUnidad = new Digit(mLabelComp[2], mLabelComp[3], "timedisplay");
            horaDecena = new Digit(mLabelComp[0], mLabelComp[1], "timedisplay"); // Horas llegan a 24 máxim           

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

            minUnidad.Init(minutes % 10, type, TypeAnim.ROLLING_DRUM);
            minDecena.Init(minutes / 10, type, TypeAnim.ROLLING_DRUM);

            horaUnidad.Init(hours % 10, type, TypeAnim.ROLLING_DRUM);
            horaDecena.Init(hours / 10, type, TypeAnim.ROLLING_DRUM);
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
            int minutes = (int)((secondsTotal % 3600) / 60f) ;
            int seconds = (int)(secondsTotal % 60f);

            // Actualiza cada rueda con su número correspondiente
            segUnidad.Set(seconds % 10, deltaTime);
            segDecena.Set(seconds / 10, deltaTime);

            minUnidad.Set(minutes % 10, deltaTime);
            minDecena.Set(minutes / 10, deltaTime);
        
            horaUnidad.Set(hours % 10, deltaTime);
            horaDecena.Set(hours / 10, deltaTime);
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
                horaDecena.Threshold = threshold;
                horaUnidad.Threshold = threshold;
                minDecena.Threshold = threshold;
                minUnidad.Threshold = threshold;
                segDecena.Threshold = threshold;
                segUnidad.Threshold = threshold;
            });                       
        }
        
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }


    /// <summary>
    /// Clase que representa cada uno de los digitos que se utilizan en el contador
    /// </summary>
    public class Digit
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Atributos del digito
        //Etiquetas reales
        private Label mLabelA;
        private Label mLabelB;

        //Etiquetas referenciadas para su distribución
        Label refVisible;
        Label refHidden;

        //Valores numericos
        private float timec = 0;
        private int cValue = 0; //Valor actual del digito      

        //Tipo de cuenta
        private TypeTimeline type;

        //Tipo de animacion
        private TypeAnim typeAnim;

        //Constantes de animacion - Odometro
        private string kAnimUp = "__text-up"; //Texto va hacia arriba
        private string kAnimCenter = "__text-center"; ////Texto va hacia el centro
        private string kAnimDown = "__text-down"; ////Texto va hacia abajo
        private string kNoAnim = "__text-no-animation"; //Bloquea animaciones

        //Constantes de animaicion Fade&Slide
        private string kAnimUpSlide = "__text-slide-up"; //Texto va hacia arriba y desaparece
        private string kAnimDownSlide = "__text-slide-down";//Texto va hacia abajo y desaparece

        //Diferentes estados del digito para controlar las animacion (evita que haya saltos
        //imprevistos y solapamientos)
        private enum StateDigit
        {
            CHECK_NUMBER_CHANGE, //Comprueba cuando cambia el numero
            COUNT, //Realiza la animacion de cambio de numeros
            ANIM, //Gestiona los tiempos de la animacion
            RESET //Tiempo para reiniciar animacion
        }

        //Esto del flujo de trabajo del digito
        private StateDigit state = StateDigit.CHECK_NUMBER_CHANGE;

        //Umbral de animacion
        private float thresholdAnim;        
        public float Threshold
        {
            get => thresholdAnim;
            set => thresholdAnim = value;
        }
    
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
        public Digit(Label a, Label b, string comp)
        {
            //Actualiza los estilos
            kAnimUp = comp + kAnimUp;
            kAnimCenter = comp + kAnimCenter;
            kAnimDown = comp + kAnimDown;
            kNoAnim = comp + kNoAnim;

            kAnimUpSlide = comp + kAnimUpSlide;            
            kAnimDownSlide = comp + kAnimDownSlide;

            // Copia las etiquetas
            mLabelA = a;
            mLabelB = b;

            //Referencias para el intercambio de etiquetas
            refVisible = mLabelA;
            refHidden = mLabelB;
        }
        #endregion

        #region [Function] Componente visual
        /// <summary>
        /// Inicializa el componentes para la animacion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tpe"></param>
        /// <param name="tpeanim"></param>
        public void Init(int value, TypeTimeline tpe, TypeAnim tpeanim )
        {
            refVisible.text = value.ToString();
            cValue = value;

            type = tpe;
            typeAnim = tpeanim;

            if (typeAnim == TypeAnim.ROLLING_DRUM)                
                Teleport(refHidden); //Desplaza las etiquetas iniciales        
            else
            {
                // Desplaza las etiquetas iniciales
                TeleportCenter(refHidden);
                refHidden.text = cValue.ToString("0");
                refHidden.RemoveFromClassList(kNoAnim);
                refHidden.RemoveFromClassList(kAnimDown); //Se quita esta, que es cuando inicia
            }
            refHidden.RemoveFromClassList(kNoAnim);
        }        
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*************************Actualizacion de los digitos****************************//
        //*********************************************************************************//
        #region [Function] Actualización 
        /// <summary>
        /// Lo establece teniendo una maquina de estados para controlar los tiempos de
        /// animacion de manera correcta
        /// </summary>
        /// <param name="newNumber"></param>      
        public void Set(int newNumber, float deltaTime)
        {
            if (typeAnim == TypeAnim.ROLLING_DRUM)
                AnimationOdometer(newNumber, deltaTime);
            else if (typeAnim == TypeAnim.FADE_SLIDE)
                AnimationFadeSlide(newNumber, deltaTime);
        }

        /// <summary>
        /// Lanza la animacion parecida a un cuenta kilometros
        /// </summary>
        /// <param name="newNumber"></param>
        /// <param name="deltaTime"></param>
        public void AnimationOdometer(int newNumber, float deltaTime)
        {
        
            //Comprueba el estado actual del digito. Permite realizar una animacion completa
            //sin que se solape con la anterior
            if (state == StateDigit.CHECK_NUMBER_CHANGE)
            {
                if (newNumber != cValue)
                {
                    cValue = newNumber;
                    state = StateDigit.COUNT;
                }
            }
            else if (state == StateDigit.COUNT)
            {
                refHidden.text = cValue.ToString();

                if (type == TypeTimeline.FORWARD)
                {
                    Move(refVisible, kAnimUp);
                    Move(refHidden, kAnimCenter);
                }
                else if (type == TypeTimeline.REGRESIVE)
                {
                    Move(refVisible, kAnimDown);
                    Move(refHidden, kAnimCenter);
                }

                state = StateDigit.ANIM; //Espera a que finalice la animacion
            }
            else if (state == StateDigit.ANIM)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.01)
                {
                    Teleport(refVisible); //Transporta la etiqueta A, hacia abajo     
                    state = StateDigit.RESET;
                    timec = 0;
                }
            }
            else if (state == StateDigit.RESET)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.03) //Da un pequeño tiempo para reiniciar
                {
                    refVisible.RemoveFromClassList(kNoAnim);
                    state = StateDigit.CHECK_NUMBER_CHANGE;
                    timec = 0;

                    //Intercambia las referencias
                    ExChangeLabel();
                }                
            }
        }


        /// <summary>
        /// Animacion Fade&Slide teniendo en cuenta la maquina de estados que control los
        /// tiempos
        /// </summary>
        /// <param name="newNumber"></param>
        /// <param name="deltaTime"></param>
        public void AnimationFadeSlide(int newNumber, float deltaTime)
        {
            //Comprueba el estado actual del digito. Permite realizar una animacion completa
            //sin que se solape con la anterior
            if (state == StateDigit.CHECK_NUMBER_CHANGE)
            {
                if (newNumber != cValue)
                {
                    refHidden.text = cValue.ToString();
                    cValue = newNumber;
                    state = StateDigit.COUNT;
                }
            }
            else if (state == StateDigit.COUNT)
            {        
                if (type == TypeTimeline.FORWARD)
                    MoveFadeSlide(refHidden, kAnimUpSlide); //Mueve solo el saliente                                    
                else if (type == TypeTimeline.REGRESIVE)
                    MoveFadeSlide(refHidden, kAnimDownSlide);                

                state = StateDigit.ANIM; //Espera a que finalice la animacion
            }
            else if (state == StateDigit.ANIM)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.01)
                {
                    TeleportCenter(refHidden); //Transporta la etiqueta que se ha ido al centro, hacia abajo     
                    state = StateDigit.RESET;
                    timec = 0;

                    refVisible.text = cValue.ToString();
                    refHidden.text = cValue.ToString();

                }
            }
            else if (state == StateDigit.RESET)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.03) //Da un pequeño tiempo para reiniciar
                {
                    refHidden.RemoveFromClassList(kNoAnim);
                    state = StateDigit.CHECK_NUMBER_CHANGE;
                    timec = 0;
                    
                }
            }
        }
        #endregion


        #region [Function] Gestion de la actualizacion
        /// <summary>
        /// Intercambia las etiquetas para el control de la animacion
        /// </summary>
        private void ExChangeLabel()
        {
            if (refVisible == mLabelA)
            {
                refVisible = mLabelB;
                refHidden = mLabelA;
            }
            else
            {
                refVisible = mLabelA;
                refHidden = mLabelB;
            }
        }               


        /// <summary>
        /// Mueve la etiqueta a la posicion deseada realizando una animacion
        /// </summary>
        /// <param name="label"></param>
        /// <param name="animation"></param>
        private void Move(Label label, string animation)
        {
            label.RemoveFromClassList(kAnimCenter);
            label.RemoveFromClassList(kAnimDown);
            label.RemoveFromClassList(kAnimUp);
            label.AddToClassList(animation);
        }


        private void MoveFadeSlide(Label label, string animation)
        {            
            label.RemoveFromClassList(kAnimDownSlide);
            label.RemoveFromClassList(kAnimUpSlide);
            label.AddToClassList(animation);
        }


        /// <summary>
        /// Desplaza la etiqueta a la parte desde donde debe aparecer
        /// </summary>
        /// <param name="label"></param>
        private void Teleport(Label label)
        {
            label.AddToClassList(kNoAnim);

            if (type == TypeTimeline.FORWARD)
                Move(label, kAnimDown);
            else
                Move(label, kAnimUp);
        }

        /// <summary>
        /// Desplaza la etiqueta a la parte desde donde debe aparecer
        /// </summary>
        /// <param name="label"></param>
        private void TeleportCenter(Label label)
        {
            label.AddToClassList(kNoAnim);
            MoveFadeSlide(label, kAnimCenter);            
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
