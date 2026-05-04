using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Tipos de componente
    /// </summary>
    public enum TypeTimeProgress
    {
        RADIAL,
        LINEAL
    }

    /// <summary>
    /// Clase especifica para mostrar un progreso de tiempo (Circular o lineal)
    /// </summary>
    public partial class TimeProgress : VisualElement
    {   
        //************************************Variables************************************//
        //*********************************************************************************//   
        #region [Function] Vinculacion con UIBuilder
        /// <summary>
        /// Muestra este control en el fichero UXML
        /// </summary>
        public new class UxmlFactory : UxmlFactory<TimeProgress, UxmlTraits> { }

        /// <summary>
        ///  Muestra en el fichero Uxml la propiedad progress, para modificarla
        ///  desde el constructor visual o por código.
        /// </summary>       
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //Atributo de progreso
            UxmlFloatAttributeDescription mProgressAttribute = new UxmlFloatAttributeDescription
            { name = "progress", defaultValue = 50 };

            //Atributo de tipo
            UxmlEnumAttributeDescription<TypeTimeProgress> mTypeAttribute = new UxmlEnumAttributeDescription<TypeTimeProgress>
            { name = "type", defaultValue = TypeTimeProgress.RADIAL };

            //Atributo de anchura de barra (porcentaje)
            UxmlFloatAttributeDescription mSizeAttribute = new UxmlFloatAttributeDescription
            { name = "size", defaultValue = 35.0f };

            //Atributo de direccion de progreso de barra (porcentaje)
            UxmlEnumAttributeDescription<ArcDirection> mDirectionAttribute = new UxmlEnumAttributeDescription<ArcDirection>
            { name = "direction", defaultValue = ArcDirection.CounterClockwise };

            //Atributo de cambio de colores en funcion del porcentaje
            UxmlBoolAttributeDescription mChangeAttribute = new UxmlBoolAttributeDescription
            { name = "color_reactive", defaultValue = false };

            //Atributo de cambio de colores para el estado critic
            UxmlFloatAttributeDescription mCriticAttribute = new UxmlFloatAttributeDescription
            { name = "valuecritic", defaultValue = 15f };

            //Atributo de cambio de colores para el estado warning
            UxmlFloatAttributeDescription mWarningAttribute = new UxmlFloatAttributeDescription
            { name = "valuewarning", defaultValue = 35f };

            //Atributo de cambio de colores para el estado warning
            UxmlFloatAttributeDescription mOpacityAttribute = new UxmlFloatAttributeDescription
            { name = "opacity", defaultValue = 100f };
            


            // Utiliza el metodo Init para asignar el valor del atributo en el fichero UXML a la propiedad progress en C#.
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var timeprogress = (TimeProgress)ve;
                float rawValue = mProgressAttribute.GetValueFromBag(bag, cc); //Asigna progreso
                timeprogress.progress = Mathf.Clamp(rawValue, 0f, 100f);
                timeprogress.type = mTypeAttribute.GetValueFromBag(bag, cc); //Asigna tipo de barra
                rawValue = mSizeAttribute.GetValueFromBag(bag, cc); //Asigna anchura de barra
                timeprogress.size = Mathf.Clamp(rawValue, 0f, 100f);
                timeprogress.direction = mDirectionAttribute.GetValueFromBag(bag, cc); //Asigna direccion de progreso
                timeprogress.color = mChangeAttribute.GetValueFromBag(bag, cc);
                rawValue = mOpacityAttribute.GetValueFromBag(bag, cc); //Opacidad
                timeprogress.opacity = Mathf.Clamp(rawValue, 0f, 100f); 

                //Hay que asegurarse que los valores de cambio de estados son correctos.
                //El warning no puede ser menos que el critico                 
                timeprogress.valuewarning = Mathf.Clamp(mWarningAttribute.GetValueFromBag(bag, cc), 0f, 100f);
                timeprogress.valuecritic = Mathf.Clamp(mCriticAttribute.GetValueFromBag(bag, cc), 0f, timeprogress.valuewarning);               
            }
        }


        /// <summary>
        /// Actualiza la barra de progreso a partir de un valor externo. Estos valores van desde 
        /// 0 a 100
        /// </summary>        
        public float progress
        {
            // La propiedad progress se expone en C#.
            get => mProgress;
            set
            {
                // Whenever the progress property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                mProgress = value;
                if (mLabel != null)
                    mLabel.text = Mathf.Clamp(Mathf.Round(value), 0, 100) + "%";

                //Si tiene activado el flag, cambia de color conforme avanza
                if (mColorReactive)
                {
                    //El estado actual cambia, realiza el cambio de color
                    ColorStateProgress newState = ColorStateAssign(progress);
                    if (newState != currentState)
                    {
                        currentState = newState;
                        GetColorByState();
                    }
                }

                //Dependiendo del tipo se repinta
                ReDraw();
            }
        }


        /// <summary>
        /// Flag que indica si se debe cambiar el color de la barra dependiondo de su tamaño.        
        /// </summary>
        public bool color
        {
            // La propiedad progress se expone en C#.
            get => mColorReactive;
            set
            {
                // Whenever the progress property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                mColorReactive = value;

                //Dependiendo del tipo se repinta
                ReDraw();
            }
        }

        /// <summary>
        /// Asigna la direccion de reduccion de la barra de progreso. 
        /// Tiene un valor entre 0 y 100.
        /// </summary>
        public ArcDirection direction
        {
            // La propiedad progress se expone en C#.
            get => mDirection;
            set
            {
                // Whenever the progress property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                mDirection = value;
                mbProgress.MarkDirtyRepaint();
            }
        }
       
        
        /// <summary>
        /// Selecciona el tipo de barra de progresso, cambiando la geometria
        /// </summary>
        public TypeTimeProgress type
        {
            get => mType;
            set
            {
                if (mType != value)
                {
                    mType = value;

                    //Cada vez que cambia de componente actualiza estructura
                    Structure();

                    //Debe repintar todo el componente dependiendo el tipo
                    ReDrawAll();
                }
            }
        }


        /// <summary>
        /// Establece el porcentaje de radio que ocupa la barra en un componente radial. Tiene un valor
        /// de 0 a 100.
        /// </summary>
        public float size
        {
            // La propiedad progress se expone en C#.
            get => mSize;
            set
            {
                // Whenever the progress property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                mSize = value;

                //Debe repintar todo el componente dependiendo el tipo
                ReDrawAll();
            }
        }

        /// <summary>
        /// Valor donde cambia a estado critico
        /// </summary>
        public float valuecritic
        {
            get => mValueCritic;
            set
            {
                mValueCritic = value;
                //Dependiendo del tipo se repinta
                ReDraw();
            }
        }

        /// <summary>
        /// Valor donde cambia a estado warning
        /// </summary>        
        public float valuewarning
        {
            get => mValueWarning;
            set
            {
                mValueWarning = value;                
                //Dependiendo del tipo se repinta
                ReDraw();
            }
        }


        /// <summary>
        /// Establecer opacidad del elemento
        /// </summary>        
        public float opacity
        {
            get => mOpacity;
            set
            {
                mOpacity = value;

                //Adicionalmente debe actualizar fondo de lineal
                mBackground.style.opacity = opacity / 100f;               

                //Dependiendo del tipo se repinta
                ReDrawAll();
            }
        }
        #endregion

        #region [Function] Variables
        /// <summary>
        /// Estado de la barra de tiempo para asignar colores
        /// </summary>
        private enum ColorStateProgress
        {
            SAFE,
            WARNING,
            CRITICAL,
            NOTHING
        }
        private ColorStateProgress currentState = ColorStateProgress.NOTHING; // Estado inicial
               

        // Nombre de las clases USS para el control del estilo.
        public static readonly string ussClassName = "bartime__progress"; //Estilo general
        public static readonly string ussBackgroundClassName = "bartime__background"; //Estilo general fondo
        public static readonly string ussTrackClassName = "bartime__track"; //Estilo general barra       

        //Clases USS especificas
        public static readonly string ussLabelClassName = "bartime__radial-progress__label";
        public static readonly string ussLinealBackgroundClassName = "bartime__lineal-background";
        public static readonly string ussLinealProgressClassName = "bartime__lineal-progress";

        // Acceso a las propiedades personalizadas en un estilo
        static CustomStyleProperty<Color> s_TrackColor = new CustomStyleProperty<Color>("--track-color");
        static CustomStyleProperty<Color> s_ProgressColorGreen = new CustomStyleProperty<Color>("--progress-color_green");
        static CustomStyleProperty<Color> s_ProgressColorYellow = new CustomStyleProperty<Color>("--progress-color_yellow");
        static CustomStyleProperty<Color> s_ProgressColorRed = new CustomStyleProperty<Color>("--progress-color_red");

        //Colores delo progress bar
        Color mTrackColor = Color.gray;
        Color mProgressColor = Color.white;

        // Etiqueta que muestra el porcentaje.
        Label mLabel;

        //El numero que se muestra en la etiqueta como un porcentaje
        private float mProgress;

        //Tipo de componente
        private TypeTimeProgress mType;

        //Tamaño de la barra en porcentajes
        private float mSize;

        //Indica si se deben cambiar los colores
        private bool mColorReactive;

        //Direccion de reduccion de la barra
        private ArcDirection mDirection;

        //Opacidad del elemento
        private float mOpacity;

        //Valores de cambio de estado(para el cambio de colores)
        private float mValueCritic;
        private float mValueWarning;


        //Elementos para la barra de progreso lineal
        private VisualElement mbProgress;
        private VisualElement mBackground;
        #endregion        
        //*********************************************************************************//
        //*********************************************************************************//



        //*************************Inicializacion y configuracion**************************//
        //*********************************************************************************//   
        #region [Function] Constructor       
        /// <summary>
        /// Constructor de la barra de progreso del tiempo
        /// </summary>
        public TimeProgress()
        {
            //Inicializar estado de la barra al iniciar
            if (mColorReactive)
                currentState = ColorStateAssign(progress); // Estado inicial
            else
                currentState = ColorStateProgress.SAFE;

            //Estructura el componente
            Structure();            
            AddToClassList(ussClassName); // Añade el nombre de la clase general.

            //Registra callback de personalizacion del estilo
            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));

            // Crea un bucle de animación que dure 1 segundo
            //this.schedule.Execute(UpdateAnimation).Every(16); // ~60 FPS      
        }
        #endregion

        #region [Function] Estructura       
        /// <summary>
        /// Establece la estructura del componente dependiendo del tipo
        /// </summary>
        private void Structure()
        {
            //Si el nuevo estado es el mismo que el anterior, no se actualiza
            ResetStructure();

            //Establece la estructura basica del time bar. //Primero crea los elementos visuales. 

            //Elemento de fondo de la barra de tiempo, asignando estilos css con borde
            mBackground = new VisualElement { name = "TimebarBackground" };
            mBackground.AddToClassList(ussBackgroundClassName);
           
            //Elemento representante de la barra de tiempo
            mbProgress = new VisualElement { name = "TimeBarProgress" };
            mbProgress.AddToClassList(ussTrackClassName);

            // Etiqueta para mostrar valores numericos de la barra
            mLabel = new Label();
            mLabel.AddToClassList(ussLabelClassName);
            mLabel.text = Mathf.Clamp(Mathf.Round(mProgress), 0, 100) + "%"; //Inicializa etiqueta

            // Genera el arbol de jerarquia
            Add(mBackground);
            mBackground.Add(mbProgress);
            Add(mLabel);

            //Dependiendo del tipo, realiza una asignacion u otra
            if (mType == TypeTimeProgress.LINEAL)
            {
                //Establece estilos adicionales
                mBackground.AddToClassList(ussLinealBackgroundClassName);
                mBackground.AddToClassList("interface__panel");                
                mbProgress.AddToClassList(ussLinealProgressClassName);

                //Inicializa tamaño de la barra
                DrawLinealProgress();
            }
            else
            {
                //Configura las subscripciones para dibujar componentes                
                mBackground.generateVisualContent -= OnDrawBackground;  // Quita parte estatica
                mBackground.generateVisualContent += OnDrawBackground; // Añade

                mbProgress.generateVisualContent -= OnDrawProgress;  // Quita parte dinamica
                mbProgress.generateVisualContent += OnDrawProgress; // Añade                                    
            }


        }


        /// <summary>
        /// Elimina la estructura establecida en el componente
        /// </summary>
        private void ResetStructure()
        {
            Clear();
            mBackground = null;
            mbProgress = null;
            mLabel = null;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
                          


        //**********************Metodologia de dibujado de elementos***********************//
        //*********************************************************************************//   
        #region [Function] Cambio de colores
        /// <summary>
        /// Obtiene el estado de la barra en funcion del valor de process
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ColorStateProgress ColorStateAssign(float data)
        {
            ColorStateProgress state = ColorStateProgress.NOTHING;
            if (data < mValueCritic)
                state = ColorStateProgress.CRITICAL;
            else if (data < mValueWarning)
                state = ColorStateProgress.WARNING;
            else
                state = ColorStateProgress.SAFE;
            return state;
        }
        
        /// <summary>
        /// Obtiene el color en funcion del estado actual de la barra que se actualiza con 
        /// la variable process
        /// </summary>
        private void GetColorByState()
        {
            if (currentState == ColorStateProgress.CRITICAL)
                customStyle.TryGetValue(s_ProgressColorRed, out mProgressColor);
            else if (currentState == ColorStateProgress.WARNING)
                customStyle.TryGetValue(s_ProgressColorYellow, out mProgressColor);
            else
                customStyle.TryGetValue(s_ProgressColorGreen, out mProgressColor);
        }

        /// <summary>
        /// Subcripcion de evento para el cambio de estilo
        /// </summary>
        /// <param name="evt"></param>
        private static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            TimeProgress element = (TimeProgress)evt.currentTarget;
            element.UpdateCustomStyles();
        }


        /// <summary>
        /// Actualiza el estilo de los componentes
        /// </summary>
        private void UpdateCustomStyles()
        {
            bool repaint = false;
            if (currentState == ColorStateProgress.CRITICAL)
            {
                if (customStyle.TryGetValue(s_ProgressColorRed, out mProgressColor))
                    repaint = true;
            }
            else if (currentState == ColorStateProgress.WARNING)
            {
                if (customStyle.TryGetValue(s_ProgressColorYellow, out mProgressColor))
                    repaint = true;
            }
            else
            {
                if (customStyle.TryGetValue(s_ProgressColorGreen, out mProgressColor))
                    repaint = true;
            }

            if (customStyle.TryGetValue(s_TrackColor, out mTrackColor))
                repaint = true;

            if (repaint)
                ReDraw();
        }
        #endregion

        #region [Function] Dibujar elementos
        /// <summary>
        /// Se encarga de dibujar el fondo. Solo se realizar una vez. De esta manera gestiona
        /// mejor el rendimiento
        /// </summary>
        private void OnDrawBackground(MeshGenerationContext context)
        {
            //Tamaño del componente
            float width = contentRect.width;
            float height = contentRect.height;

            //Calcula del tamaño de progress
            float size = (width * mSize) / 100f;

            var painter = context.painter2D;
            painter.lineCap = LineCap.Butt;

            //Calcula radio para ver que tiene que ocupar el circulo central y que ocupa la anchura de barra            
            float pixel_in_bar = size; //Barra
            float pixel_in_fill = width - pixel_in_bar; //Circulo central

            float radius_bar = pixel_in_bar / 2; //Barra
            float radius_fill = pixel_in_fill / 2; //Circulo central

            //mLabel.text = $"Tamaño total:{width} - Borde:{size} - {pixel_in_fill}";            

            // En primer lugar se dibuja el circulo central donde esta colocado el label            
            painter.fillColor = new Color(27f / 255f, 38f / 255f, 49f / 255f);
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), radius_fill, 0.0f, 360.0f);
            painter.Fill();

            //En segundo lugar, se dibuja la barra que hace de track de fondo
            painter.strokeColor = new Color(mTrackColor.r, mTrackColor.g, mTrackColor.b); 
            painter.lineWidth = radius_bar;
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), radius_fill + radius_bar / 2, 0.0f, 360.0f);
            painter.Stroke();
            

            //Finalmente se dibujan las lineas de luces y sombras
            // Luces internas             
            painter.lineWidth = 3.0f;
            painter.strokeColor = new Color(44f / 255f, 62f / 255f, 80f / 255f);
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), radius_fill, 315, 135.0f);
            painter.Stroke();
            //Sombras internas
            painter.strokeColor = new Color(13f / 255f, 18f / 255f, 23f / 255f);
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), radius_fill, 135.0f, 315.0f);
            painter.Stroke();

            // Luces externas              
            painter.strokeColor = new Color(93f / 255f, 173 / 255f, 226 / 255f); 
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), radius_fill + radius_bar, 135.0f, 315.0f);
            painter.Stroke();
            //Sombras externas
            painter.strokeColor = new Color(27f / 255f, 38f / 255f, 49 / 255f); 
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), radius_fill + radius_bar, 315.0f, 135.0f);
            painter.Stroke();
        }

        /// <summary>
        /// Se encarga de redibujar la barra de progreso, que es lo unico que se tiene que
        /// repintar
        /// </summary>
        private void OnDrawProgress(MeshGenerationContext context)
        {
            //Tamaño del componente
            float width = contentRect.width;
            float height = contentRect.height;

            //Calcula del tamaño de progress
            float size = (width * mSize) / 100f;

            var painter = context.painter2D;
            painter.lineCap = LineCap.Butt;

            //Calcula radio para ver que tiene que ocupar el circulo central y que ocupa la anchura de barra            
            float pixel_in_bar = size; //Barra
            float pixel_in_fill = width - pixel_in_bar; //Circulo central

            float radius_bar = pixel_in_bar / 2; //Barra
            float radius_fill = pixel_in_fill / 2; //Circulo central

            //Pintaoca la barra de progreso que se reducira o incrementará                                   
            //Dependiendo del tipo de direccion se calcula un principio y un final de arco
            painter.strokeColor = new Color(mProgressColor.r, mProgressColor.g, mProgressColor.b);
            painter.lineWidth = radius_bar - 3;
            painter.BeginPath();
            if (mDirection == ArcDirection.Clockwise)
                painter.Arc(new Vector2(width * 0.5f, height * 0.5f),
                    radius_fill + radius_bar / 2, -90, 360.0f * (progress / 100.0f) - 90, mDirection);
            else
                painter.Arc(new Vector2(width * 0.5f, height * 0.5f),
                    radius_fill + radius_bar / 2, 270, 360.0f * ((100f - progress) / 100.0f) - 90, mDirection);
            painter.Stroke();
        }

        /// <summary>
        /// Actualiza la barra lineal a traves de estilos. No como en la radial, que se
        /// hace a traves de geometria
        /// </summary>
        private void DrawLinealProgress()
        {
            if (mProgress >= 0)
            {
                mbProgress.style.width = new StyleLength(Length.Percent(mProgress));
                mbProgress.style.backgroundColor = mProgressColor;
                    //new Color(mProgressColor.r, mProgressColor.g, mProgressColor.b, opacity / 100f);               
            }
        }        

        /// <summary>
        /// Metodo que repinta todo el componente
        /// </summary>
        private void ReDrawAll()
        {
            //Dependiendo del tipo se repinta
            if (type == TypeTimeProgress.RADIAL)
            {
                mBackground.MarkDirtyRepaint();
                mbProgress.MarkDirtyRepaint();                
            }
            else if (type == TypeTimeProgress.LINEAL)
                DrawLinealProgress();
        }

        /// <summary>
        /// Metodo que manda repintar la parte dinamica del dibujo
        /// </summary>
        private void ReDraw()
        {
            //Dependiendo del tipo se repinta
            if (type == TypeTimeProgress.RADIAL)
                mbProgress.MarkDirtyRepaint();
            else if (type == TypeTimeProgress.LINEAL)
                DrawLinealProgress();
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*****************************Gestion de animaciones******************************//
        //*********************************************************************************//   
        #region [Function] Animaciones dinamicas
        float _radius = 0.0f;
        /// <summary>
        /// Generacion de una animacion de pulso constante
        /// </summary>
        /// <param name="timer"></param>
        private void UpdateAnimation(TimerState timer)
        {
            // Oscilamos el radio entre 20 y 50 usando el tiempo
            _radius = 50f + Mathf.Sin(Time.realtimeSinceStartup * 5f) * 15f;

            // FORZAMOS EL REDIBUJO DEL PAINTER2D
            ReDrawAll();
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
