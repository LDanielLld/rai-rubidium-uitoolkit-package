using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Componente de interfaz para mostrar titulos con una geometria de trapezoide
    /// </summary>
    public class TittleComponent : VisualElement
    {
        //************************************Variables************************************//
        //*********************************************************************************//   
        #region [Function] Vinculacion con UIBuilder
        /// <summary>
        /// Muestra este control en el fichero UXML
        /// </summary>
        public new class UxmlFactory : UxmlFactory<TittleComponent, UxmlTraits> { }

        /// <summary>
        ///  Muestra en el fichero Uxml la propiedad progress, para modificarla
        ///  desde el constructor visual o por codigo.
        /// </summary>       
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //Atributo de progreso
            UxmlStringAttributeDescription mTitleAttribute = new UxmlStringAttributeDescription
            { name = "title", defaultValue = "" };            

            //Atributo de anchura de borde
            UxmlFloatAttributeDescription mSizeAttribute = new UxmlFloatAttributeDescription
            { name = "size", defaultValue = 4.0f };

            //Atributo de curvatura de las esquinas
            UxmlFloatAttributeDescription mRadiusAttribute = new UxmlFloatAttributeDescription
            { name = "radius", defaultValue = 25f };

            //Atributo de cambio de colores para el estado warning
            UxmlFloatAttributeDescription mOpacityAttribute = new UxmlFloatAttributeDescription
            { name = "opacity", defaultValue = 100f };

            //Color de la linea superior
            UxmlColorAttributeDescription mColorUpAttribute = new UxmlColorAttributeDescription
            { name = "color_up", defaultValue = new Color(13f / 255f, 18f / 255f, 23f / 255f) };

            //Color de la linea inferior
            UxmlColorAttributeDescription mColorDownAttribute = new UxmlColorAttributeDescription
            { name = "color_down", defaultValue = new Color(44f / 255f, 62f / 255f, 80f / 255f) };


            //Color de la linea superior
            UxmlColorAttributeDescription mColorInAttribute = new UxmlColorAttributeDescription
            { name = "color_in", defaultValue = new Color(27f / 255f, 38f / 255f, 49 / 255f) };


            // Utiliza el metodo Init para asignar el valor del atributo en el fichero UXML a la propiedad progress en C#.
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var titlecomponent = (TittleComponent)ve;               
                titlecomponent.title = mTitleAttribute.GetValueFromBag(bag, cc); //Asigna titulo
                
                float rawValue = mSizeAttribute.GetValueFromBag(bag, cc); //Asigna anchura de borde
                titlecomponent.size = Mathf.Clamp(rawValue, 1f, 10f);

                rawValue = mRadiusAttribute.GetValueFromBag(bag, cc); //Asigna radio de esquinas
                titlecomponent.radius = Mathf.Clamp(rawValue, 20f, 60f);

                titlecomponent.color_up = mColorUpAttribute.GetValueFromBag(bag, cc); //Asgina color superior
                titlecomponent.color_down = mColorDownAttribute.GetValueFromBag(bag, cc); //Asigna color inferior      
                titlecomponent.color_in = mColorInAttribute.GetValueFromBag(bag, cc); //Asigna color interior        

                rawValue = mOpacityAttribute.GetValueFromBag(bag, cc); //Opacidad
                titlecomponent.opacity = Mathf.Clamp(rawValue, 0f, 100f);                
            }
        }


        /// <summary>
        /// Titulo que se mostrará en la etiqueta
        /// </summary>        
        public string title
        {
            // La propiedad progress se expone en C#.
            get => mTitle;
            set
            {
                // Modifica la etiqueta.
                mTitle = value;
                mLabel.text = value;
                mLabel.MarkDirtyRepaint();
            }
        }

        /// <summary>
        /// Cambia el radio de las esquinas
        /// </summary>        
        public float radius
        {
            // La propiedad progress se expone en C#.
            get => mRadius;
            set
            {
                // Modifica la etiqueta.
                mRadius = value;
                //Debe repintar todo el componente 
                ReDrawAll();
            }
        }

        /// <summary>
        /// Establece el ancho de la linea de bordeado
        /// </summary>
        public float size
        {
            // La propiedad progress se expone en C#.
            get => mSize;
            set
            {
                // Cambia el color
                mSize = value;

                //Debe repintar todo el componente
                ReDrawAll();
            }
        }


        /// <summary>
        /// Cambia el color interior.
        /// </summary>      
        public Color color_in
        {
            // La propiedad progress se expone en C#.
            get => mColorIn;
            set
            {
                // Cambia el color
                mColorIn = value;

                //Repinta
                ReDrawAll();
            }
        }


        /// <summary>
        /// Cambia el color de la linea superior.
        /// </summary>      
        public Color color_up
        {
            // La propiedad progress se expone en C#.
            get => mColorUp;
            set
            {
                // Cambia el color
                mColorUp = value;
                
                //Repinta
                ReDrawAll();
            }
        }



        /// <summary>
        /// Cambia el color de la linea inferior.
        /// </summary>      
        public Color color_down
        {
            // La propiedad progress se expone en C#.
            get => mColorDown;
            set
            {
                // Cambia el color
                mColorDown = value;

                //Repinta
                ReDrawAll();
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

        // Nombre de las clases USS para el control del estilo.
        public static readonly string ussClassName = "titlecustom_container"; //Estilo general
        public static readonly string ussBackgroundClassName = "titlecustom_background"; //Estilo general fondo          
        public static readonly string ussLabelClassName = "titlecustom_text-label";        
       

        //Colores delo progress bar
        private Color mColorUp = new Color(13f / 255f, 18f / 255f, 23f / 255f);
        private Color mColorDown  = new Color(44f / 255f, 62f / 255f, 80f / 255f);
        private Color mColorIn = new Color(27f / 255f, 38f / 255f, 49 / 255f);

        // Etiqueta que muestra el porcentaje.
        private Label mLabel;

        //El numero que se muestra en la etiqueta como un porcentaje
        private string mTitle;       

        //Tamaño de la line a de borde
        private float mSize;

        //Radio de las esquinas
        private float mRadius;

        //Opacidad del elemento
        private float mOpacity;        

        //Elementos para la barra de progreso lineal        
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
        public TittleComponent()
        {           
            //Estructura el componente
            Structure();
            AddToClassList(ussClassName); // Añade el nombre de la clase general.    
        }
        #endregion

        #region [Function] Estructura       
        /// <summary>
        /// Establece la estructura del componente dependiendo del tipo
        /// </summary>
        private void Structure()
        {
            //Elemento de fondo la etiqueta, asignando estilos css con borde
            mBackground = new VisualElement { name = "TitleBackground" };
            mBackground.AddToClassList(ussBackgroundClassName);
           
            // Etiqueta para mostrar el titulo de la etiqueta personalizada
            mLabel = new Label();
            mLabel.AddToClassList(ussLabelClassName);
            mLabel.text = mTitle; //Inicializa etiqueta

            // Genera el arbol de jerarquia
            Add(mBackground);            
            Add(mLabel);

            //Configura las subscripciones para dibujar componentes                
            mBackground.generateVisualContent -= OnDrawTrapecio;  // Quita parte estatica
            mBackground.generateVisualContent += OnDrawTrapecio; // Añade                                                   
        }
       
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //**********************Metodologia de dibujado de elementos***********************//
        //*********************************************************************************//           

        #region [Function] Dibujar fondo
        /// <summary>
        /// Se encarga de redibujar la barra de progreso, que es lo unico que se tiene que
        /// repintar
        /// </summary>
        private void OnDrawTrapecio(MeshGenerationContext mgc)
        {
            var painter = mgc.painter2D;
            float w = contentRect.width;
            float h = contentRect.height;
            float r = mRadius; // Radio de redondeo          


            //Inicializa los puntos base            
            Vector2 pTL = new Vector2(r, 0);
            Vector2 pTR = new Vector2(w - r, 0);

            Vector2 bzTR1 = new Vector2(w + r / 2, 0);
            Vector2 bzTR2 = new Vector2(w + r / 2, -5);
            Vector2 bzTR3 = new Vector2(w - r / 2, 2 * r);


            painter.BeginPath();

            //1. Lado superior: Empieza después del radio
            painter.MoveTo(pTL);
            painter.LineTo(pTR);
                        
            //2. Curva Cúbica Superior Derecha
            painter.BezierCurveTo(bzTR1, bzTR2, bzTR3);

            //Genera la funcion de la recta para que los siguientes puntos sean tangentes a la curva inicial
            float m = (bzTR3.y - bzTR2.y)/((bzTR3.x - bzTR2.x));
            float offset = bzTR3.y - m * bzTR3.x;

            //2. Lado derecho: - A una altura de h-r/2, la x sera  x= (y-b)/m
            float y = h - r / 2; //125
            float x_sup = (y - offset)/m; //413
            painter.LineTo(new Vector2(x_sup, y));

            //3. Curva Cúbica Inferior Derecha
            float x_inf = (h - offset)/m; //401.2f
            painter.BezierCurveTo(new Vector2(x_inf, h), new Vector2(x_inf, h), new Vector2(x_inf - r/2, h));

            //4. Lado inferior
            painter.LineTo(new Vector2(w - (x_inf - r / 2), h));

            //5. Curva Cúbica Inferior Izquierda            
            painter.BezierCurveTo(new Vector2(w - x_inf, h), new Vector2(w - x_inf, h), new Vector2(w - x_sup, h-r/2));           

            //6. Lado izquierdo
            painter.LineTo(new Vector2(r/2, 2*r));


            //7. Curva Cúbica Superior Izquierda
            painter.BezierCurveTo(new Vector2(-r/2, -5), new Vector2(-r/2, 0), new Vector2(r, 0));

            //Cierra la forma trapezoidal
            painter.ClosePath();            
            painter.fillColor = mColorIn;
            painter.Fill();

            //Pinta linea de sombras y luces
            DrawLineLight(painter, w, r, h, x_sup, x_inf);
            DrawLineShadow(painter, pTR, bzTR1, bzTR2, bzTR3, w, r, h, x_sup, x_inf, y);            
        }


        /// <summary>
        /// Pinta linea superior para la luz
        /// </summary>
        private void DrawLineLight(Painter2D painter, float w, float r, float h, float x_sup, float x_inf)
        {
            //Crea lineas de sombreado utilizando el mismo patron del trapecio anterior
            painter.BeginPath(); //Primero la linea superiro que va desde la curva
                                    //izquierda inferior a la linea superior

            // 1. Coloca el punto de inicio en la curva inferior izquierda
            painter.MoveTo(new Vector2(w - (x_inf - r / 2), h));


            //2. Curva Cúbica Inferior Izquierda            
            painter.BezierCurveTo(new Vector2(w - x_inf, h), new Vector2(w - x_inf, h), new Vector2(w - x_sup, h - r / 2));

            //3. Lado izquierdo
            painter.LineTo(new Vector2(r / 2, 2 * r));

            //4. Curva Cúbica superior Izquierda 
            painter.BezierCurveTo(new Vector2(-r / 2, -5), new Vector2(-r / 2, 0), new Vector2(r, 0));

            //5. Linea superior
            painter.LineTo(new Vector2(w - r, 0));

            //Pinta la linea con el color establecido
            painter.lineWidth = mSize;
            painter.strokeColor = mColorUp;
            painter.Stroke();
        }

        /// <summary>
        /// Pinta la linea inferior para la sombras
        /// </summary>        
        private void DrawLineShadow( Painter2D painter, Vector2 pTR, Vector2 bzTR1, Vector2 bzTR2, Vector2 bzTR3, 
            float w, float r, float h, float x_sup, float x_inf, float y)
        {
            //Lineas de sombreado
            painter.BeginPath();

            //1. Coloca punto inicial de pintado al final de la linea superior
            painter.MoveTo(pTR);

            //2. Curva Cúbica Superior Derecha
            painter.BezierCurveTo(bzTR1, bzTR2, bzTR3);

            //3. Lado derecho
            painter.LineTo(new Vector2(x_sup, y));

            //4. Curva Cúbica Inferior Derecha                     
            painter.BezierCurveTo(new Vector2(x_inf, h), new Vector2(x_inf, h), new Vector2(x_inf - r / 2, h));

            //5. Lado inferior
            painter.LineTo(new Vector2(w - (x_inf - r / 2), h));


            painter.lineWidth = mSize;
            painter.strokeColor = mColorDown;
            painter.Stroke();            
        }


        /// <summary>
        /// Metodo que repinta todo el componente
        /// </summary>
        private void ReDrawAll()
        {
            //Dependiendo del tipo se repinta
            mBackground.MarkDirtyRepaint(); 
        }        
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//


    }
}
