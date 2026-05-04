using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Fila de la base de datos del juego
    /// </summary>
    [Serializable]
    public class UserScoreData
    {
        public string id; //Identificador unico de registro      
        public int score; //Puntuacion del registro
        public long dateTime; //Hora del registro

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="user"></param>
        /// <param name="scr"></param>
        public UserScoreData(int user, int scr)
        {
            id = Guid.NewGuid().ToString();
            score = scr;
            dateTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Devuelve un string con la fecha actual:
        /// - Type 1: Devuelve solo fecha formato dd/MM/yyyy
        /// - Type 2: Devuelve solo hora del dia HH:mm:ss
        /// - Type 0: Devuelve solo ambos dd/MM/yyyy HH:mm:ss
        /// </summary>
        /// <returns></returns>
        public string GetDate(int type)
        {
            DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(dateTime).LocalDateTime;
            string strDate = "";
            if (type == 1)
                strDate = date.ToShortDateString();
            else if (type == 2)
                strDate = date.ToShortTimeString();
            else
                strDate = date.ToString("dd/MM/yyyy HH:mm:ss");
            return strDate;
        }
    }

    /// <summary>
    /// Fila con formato visual que presenta las puntuaciones y la clasificacion
    /// </summary>
    public class ScoreRow : VisualElement
    {
        //************************************Variables************************************//
        //*********************************************************************************// 
        #region [Function] Vinculacion con UIBuilder
        /// <summary>
        /// Muestra este control en el fichero UXML
        /// </summary>
        public new class UxmlFactory : UxmlFactory<ScoreRow, UxmlTraits> { }       
        #endregion

        #region [Function] Variables

        // Nombre de las clases USS para el control del estilo.
        public static readonly string ussClassName = "scorerow_container"; //Estilo general
        public static readonly string ussBackgroundClassName = "scorerow_background"; //Estilo general fondo
        public static readonly string ussBackgroundHighClassName = "scorerow_background-highlight"; //Estilo general fondo resaltado
                

        //Clases USS especificas
        public static readonly string ussLabelH1ClassName = "scorerow_label-h1";
        public static readonly string ussLabelH2ClassName = "scorerow_label-h2";
        public static readonly string ussLabelH3ClassName = "scorerow_label-h3";
        public static readonly string ussLabelH4ClassName = "scorerow_label-h4";
        public static readonly string ussLabelH1HighClassName = "scorerow_label-h1-highlight";
        public static readonly string ussLabelH2HighClassName = "scorerow_label-h2-highlight";
        public static readonly string ussLabelH3HighClassName = "scorerow_label-h3-highlight";

        public static readonly string ussPanelPlaceClassName = "scorerow_panel-place";
        public static readonly string ussPanelDateClassName = "scorerow_panel-date";
        public static readonly string ussPanelIconClassName = "scorerow_panel-icon";
        public static readonly string ussPanelNumPlaceClassName = "scorerow_panel-num-place";
        public static readonly string ussPanelRecordClassName = "scorerow_panel-record";          
                

        // Etiquetas de informacion de la fila de puntuacion
        private Label lblScore;
        private Label lblDate;
        private Label lblHour;

        //Elementos adicionales
        public VisualElement icon; //Icono con el trofeo
        public Label place; //Posicion 
        public VisualElement precord;
       
        public bool isRecord = false; //Fila con record              

        //Rango de acotacion para la animacion de opacidad
        private readonly float period = 0.8f;
        private float[] rangeOp = new float[] { 0.5f, 1f }; 


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
        public ScoreRow()
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
            //Establece la estructura basica la fila de puntuacion.

            //Elemento de fondo de la linea, asignando estilos css con borde
            mBackground = new VisualElement { name = "RowBackground" };
            mBackground.AddToClassList(ussBackgroundClassName);

            //Panel, icono y etiqueta con la posicion
            VisualElement panelPlace = new VisualElement { name = "PanelPlace" };
            panelPlace.AddToClassList(ussPanelPlaceClassName);

            icon = new VisualElement { name = "Icon" };
            icon.AddToClassList(ussPanelIconClassName);
            place = new Label { name = "Place" };
            place.AddToClassList(ussLabelH2ClassName);
            place.AddToClassList(ussPanelNumPlaceClassName);            

            //Estructura el panel
            panelPlace.Add(icon);
            panelPlace.Add(place);

            // Etiqueta para mostrar valores numericos de la barra
            lblScore = new Label { name = "Score" }; 
            lblScore.AddToClassList(ussLabelH1ClassName);           

            //Panel para mostrar los datos de registro
            VisualElement panelDate = new VisualElement { name = "PanelDate" };
            panelDate.AddToClassList(ussPanelDateClassName);

            // Etiqueta para mostrar el dia registro            
            lblDate = new Label { name = "Date" }; ;
            lblDate.AddToClassList(ussLabelH3ClassName);
            
            // Etiqueta para mostrar la hora de registro
            lblHour = new Label { name = "Hour" }; ;
            lblHour.AddToClassList(ussLabelH3ClassName);            

            //Estructura el panel
            panelDate.Add(lblDate);
            panelDate.Add(lblHour);

            //Creacion de panel para mostrar etiqueta de nuevo record
            precord = new VisualElement { name = "PanelRecord" };
            precord.AddToClassList(ussPanelRecordClassName);
            Label lblrecord = new Label { name = "Message" };
            lblrecord.AddToClassList(ussLabelH4ClassName);
            lblrecord.text = "¡Nuevo Record!";
            precord.Add(lblrecord);
            precord.style.visibility = Visibility.Hidden;
            

            // Genera el arbol de jerarquia
            mBackground.Add(panelPlace);
            mBackground.Add(lblScore);
            mBackground.Add(panelDate);
            mBackground.Add(precord);
            Add(mBackground);   
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
                     

                        
        //**************************Actualización de componentes***************************//
        //*********************************************************************************//   
        #region [Function] Gestion de componentes dinamicas
        /// <summary>
        /// Establecer el texto de todos los componentes
        /// </summary>
        public void SetText(int score, string date, string hour)
        {
            lblScore.text = $"{score}";
            lblDate.text = $"{date}";
            lblHour.text = $"{hour}";
        }

        /// <summary>
        /// Establece el componente icono
        /// </summary>
        public void SetIcon(int i)
        {            
            icon.AddToClassList("scorerow_place-" +(i+1));
            place.style.visibility = Visibility.Hidden;                        
        }

        /// <summary>
        /// Establece el numero de clasificacion
        /// </summary>
        public void SetPlace(int i)
        {
            icon.style.visibility = Visibility.Hidden;  //Quita panel de icono y escribe texto
            place.text = $"{ i + 1}.";
        }

        /// <summary>
        /// Establece el tamaño de altura de la fila
        /// </summary>
        public void SetHeight(float height)
        {
            style.height = new StyleLength(Length.Percent(height)); //Ajusta el tamaño
        }

        /// <summary>
        /// Resalta la puntuacion de esta linea
        /// </summary>
        public void Highlight()
        {   
            //Actualiza el estilo del actual - Hace mas grande y cambia color de letra
            lblScore.AddToClassList(ussLabelH1HighClassName);            
            lblDate.AddToClassList(ussLabelH3HighClassName);
            lblHour.AddToClassList(ussLabelH3HighClassName);

            //Actualiza fondo
            mBackground.AddToClassList(ussBackgroundHighClassName);                   

            //Activa animacion de parpadeo
            ShowNewRecordMessage();            
        }

        /// <summary>
        /// Conecta el panel de nuevo record
        /// </summary>
        public void SetNewRecord()
        {
            precord.style.visibility = Visibility.Visible;
            isRecord = true;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*****************************Gestion de animaciones******************************//
        //*********************************************************************************//   
        #region [Function] Animaciones dinamicas
        /// <summary>
        /// Animar texto de la puntuacion
        /// </summary>
        private void ShowNewRecordMessage()
        {
            // Asegurar que empieza visible usando la funcion coseno (empieza en 1)            
            float freq = (2 * Mathf.PI) / period;

            //Offset para obtener el rango de amplitud (0.5 y 1)
            float offset = rangeOp.Average();

            //Amplitud
            float ampl = rangeOp[1] - rangeOp[0];          

            
            lblScore.schedule.Execute(() =>
            {
                // Usa Mathf.Sin para obtener un valor de opacidad - Funcion personalizada                 
                float opacity = offset + ampl*Mathf.Sin(freq * Time.time);

                // Aplicamos la opacidad
                lblScore.style.opacity = opacity;
                lblDate.style.opacity = opacity;
                lblHour.style.opacity = opacity;

            }).Every(16);

        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
