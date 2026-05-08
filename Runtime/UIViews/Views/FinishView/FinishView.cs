using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Interfaz de finalizacion del ejercicio. Muestra etiqueta de actividad finalizada y
    /// presenta las mejores puntuaciones. Todo mediante transiciones de animaciones
    /// </summary>
    public class FinishView : UIView
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Gestion del componente UI         
        /// <summary>
        /// Estado actual de la intefaz de finalizacion
        /// </summary>
        private enum StateFinishView
        {
            SFV_APPEARSIGNAL, //Aparece la señal desde el centro
            SFV_SHOWSIGNAL, //Tiempo de muestra de la primera etiqueta
            SFV_DISAPPEAR, //Animacion de desaparicion del primer cartel
            SFV_APPEAR, //Animacion de aparicion del panel de puntuaciones
            SFV_COMPLETING, //Completa las filas de puntuacion            
            SFV_FINISH //Tiempo para cerrar programa
        }

        private StateFinishView cState = StateFinishView.SFV_APPEARSIGNAL; //Empieza con la inicializacion

        // Elementos visuales
        private VisualElement mSignal; //Panel que muestra finalización 
        private VisualElement mPanel; //Panel general
        private VisualElement mPanelScore; //Contenido de puntuaciones     

        private Label lblUser; //Etiqueta de usuario

        private VisualElement mCountDownPanel; //Panel de la cuenta atras
        private VisualElement mCountDown; //Cuenta atras para cerrar
        private Label mCountDownLbl; //Etiqueta de usuario


        //Fuegos artificiales
        private Fireworks fireworks;
        private VisualElement mPanelFireworks; //Panel de fuegos artificiales

        // Selectores de estilos
        private const string anShrinkPanel = "finishview-signal-shrink"; //Estilo de encoger texto
        private const string anShrinkRow = "scorerow_panel-shrink"; //Estilo de encoger fila    
        private const string anFadePanel = "finishview-countdown-fade"; //Estilo de encoger fila    
        private const string anPulseText = "finishview-num-pulse"; //Estilo de encoger fila  

        //Valores temporales
        private float timec = 0;
        private readonly int SEC_TO_CHANGE = 3;
        private int SEC_TO_EXIT = 5;
        private readonly float cTime = 0.50f;

        //Registro de puntuaciones
        private int nbRows = 9; //Numero de filas que aparecen como puntuaciones   
        private List<ScoreRow> rows = new List<ScoreRow>(); //Lista de las filas
        private int iRow = 0;
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*************************Inicializacion y configuración**************************//
        //*********************************************************************************//
        #region [Function] Inicializacion  
        /// <summary>
        /// Constructor del control de la interfaz
        /// </summary>
        /// <param name="topElement"></param>
        public FinishView(VisualElement topElement) : base(topElement)
        {            
        }
        #endregion
       

        #region [Function] Configuración
        /// <summary>
        /// Establece los elementos visuales del componente UI. Se sobreescribe para personalizar.
        /// </summary>
        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            //Registra componentes de la interfaz
            mSignal = mTopElement.Q<VisualElement>("finishview-container-signal");

            mPanel = mTopElement.Q<VisualElement>("finishview-container-scores"); 
            
            mPanelScore = mTopElement.Q<VisualElement>("finishview-panel-scores");
            mPanelFireworks = mTopElement.Q<VisualElement>("rt-background");
            
            lblUser = mTopElement.Q<Label>("finishview-text-user");

            mCountDownPanel = mTopElement.Q<VisualElement>("finishview-countdown-panel");
            mCountDown = mTopElement.Q<VisualElement>("finishview-countdown");
            mCountDownLbl = mTopElement.Q<Label>("finishview-countdown-sequence");
            mCountDownLbl.text = $"{SEC_TO_EXIT}";//Establece valor de conteo inicial            


            //Configura los componentes visibles
            mPanel.style.visibility = Visibility.Hidden;
            mPanel.AddToClassList(anShrinkPanel);
            mSignal.style.visibility = Visibility.Visible;
        }

        /// <summary>
        /// Registra los eventos
        /// </summary>
        protected override void RegisterUICallbacks()
        {
            UIEvents.FillPanelScore += FillScorePanel;
        }


        /// <summary>
        /// Rellena el panel de puntuacion con los datos que se proporcionan
        /// </summary>
        private void FillScorePanel(List<UserScoreData> topScores, string id, string username)
        {
            //Establece tamaño relativo de una celda dependiendo de los que se quieren mostrar
            float height = 100f / nbRows;

            //Asignar nombre de usuario
            lblUser.text = username;

            //Cantidad de filas
            iRow = topScores.Count();

            //Crear las filas con las puntuaciones, y las esconde para hacer animacion de aparicion
            for (int i = 0; i < iRow; i++)
            {
                ScoreRow row = AddRow(height, i, id, topScores[i]);
                rows.Add(row);
                mPanelScore.Add(row); //Incorpora elemento
            }
        }

        /// <summary>
        /// Metodo para incorporar un registro en la lista de puntuaciones
        /// </summary>        
        private ScoreRow AddRow(float height, int i, string id, UserScoreData score)
        {
            ScoreRow row = new ScoreRow();
            row.SetHeight(height); //Ajusta el tamaño
            row.AddToClassList(anShrinkRow);


            //Actualiza texto de la fila
            row.SetText(score.score, score.GetDate(1), score.GetDate(2));

            //Añadir icono a los 3 primeros
            if (i >= 0 && i <= 2)
                row.SetIcon(i);
            else
                row.SetPlace(i);

            //Resaltar puntuacion actual
            if (score.id == id) //Compara con el id actual
            {
                //Señala la fila actual, y activa la animacion de parpadeo
                row.Highlight();

                //Señala si es un nuevo record            
                if (i == 0) //Si es el primer registro, señala record
                {
                    //Muestra etiqueta nuevo record
                    row.SetNewRecord();

                    //Inicializa los fuegos artificiales
                    fireworks = new Fireworks(mPanelFireworks);
                }
            }
            return row;
        }       
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //**************************Actualizacion de la interfaz***************************//
        //*********************************************************************************//
        #region [Function] Actualizacion de estados              
        /// <summary>
        /// Metodo encargado de gestionar el timelapse de los estados
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public override bool Update(float deltaTime)
        {
            bool state = false;

            //Maquina de estados para controla flujo de estados
            if (cState == StateFinishView.SFV_APPEARSIGNAL)
            {
                //Aparece realizando una animacion
                mSignal.RemoveFromClassList(anShrinkPanel);
                cState = StateFinishView.SFV_SHOWSIGNAL;               
            }
            else if (cState == StateFinishView.SFV_SHOWSIGNAL)
            {
                timec += deltaTime;
                if (timec > SEC_TO_CHANGE) //Cuando pasa los segundos 
                {
                    //Lanza la animacion de cambio de estilo
                    mSignal.AddToClassList(anShrinkPanel);
                    cState = StateFinishView.SFV_DISAPPEAR;
                    timec = 0.0f;
                }
            }
            else if (cState == StateFinishView.SFV_DISAPPEAR)
            {
                timec += deltaTime;
                if (timec > cTime) //Acaba la animacion
                {
                    //Configura los componentes visibles
                    mPanel.style.visibility = Visibility.Visible;
                    mSignal.style.visibility = Visibility.Hidden;

                    //Lanza la animacion de quitando estilo
                    mPanel.RemoveFromClassList(anShrinkPanel);

                    cState = StateFinishView.SFV_APPEAR;
                    timec = 0.0f;
                }
            }
            else if (cState == StateFinishView.SFV_APPEAR) //Muestra panel de puntuaciones
            {
                timec += deltaTime;
                if (timec > cTime) //Acaba la animacion de aparicion
                {
                    cState = StateFinishView.SFV_COMPLETING;
                    timec = 0.0f;
                }
            }
            else if (cState == StateFinishView.SFV_COMPLETING) //Cada cierto tiempo muestra un registro
            {
                timec += deltaTime;
                if (timec > cTime) //Acaba la animacion de aparicion
                {
                    if (rows.Count > 0)
                    {
                        rows[iRow - 1].RemoveFromClassList(anShrinkRow);

                        //Comprueba el actual para determinar si hay que activar los fuegos por el record
                        if (rows[iRow - 1].isRecord)
                            fireworks.Connect();

                        iRow--;
                        timec = 0.0f;
                        if (iRow <= 0)
                        {
                            cState = StateFinishView.SFV_FINISH;

                            //Animacion de muestra del contador final
                            mCountDownPanel.RemoveFromClassList(anFadePanel);

                            //Animacion de giro
                            RotationAnimation();
                        }
                    }
                    else
                    {
                        cState = StateFinishView.SFV_FINISH;

                        //Animacion de muestra del contador final
                        mCountDownPanel.RemoveFromClassList(anFadePanel);

                        //Animacion de giro
                        RotationAnimation();

                        timec = 0.0f;
                    }
                }
            }
            else if (cState == StateFinishView.SFV_FINISH) //Al pasar un tiempo, cierra el programa
            {
                //Actualizacion de los fuegos artificiales
                if (fireworks != null)
                    fireworks.Update();

                if (SEC_TO_EXIT > 0) //Está en la cuenta atras
                {
                    timec += deltaTime;
                    if (timec >= 1.0f) //Reduce cada segundo
                    {
                        SEC_TO_EXIT--;
                        timec = 0.0f;
                        UpdateVisuals();
                    }
                }
                else
                    state = true;

            }
            return state;
        }


        /// <summary>
        /// Actualiza la cuenta atras aplicando una animacion de cambio de numero.
        /// </summary>
        private void UpdateVisuals()
        {
            if (SEC_TO_EXIT > 0)
            {
                mCountDownLbl.text = $"{SEC_TO_EXIT}";
                // Efecto de pulso usando una corrutina rápida
                mCountDownLbl.AddToClassList(anPulseText);
                //Aplica una animacion de pulso sobre el texto.
                mCountDownLbl.schedule.Execute(() =>
                    mCountDownLbl.RemoveFromClassList(anPulseText)
                ).StartingIn(150); //Delay de 150ms
            }
        }
        #endregion

        #region [Function] Animaciones dinamicas
        /// <summary>
        /// Animar una rotacion
        /// </summary>
        private void RotationAnimation()
        {
            // Asegurar que empieza visible usando la funcion coseno (empieza en 1)            
            float freq = (2 * Mathf.PI) / 2;

            //Offset para obtener la media en el rango de amplitud
            float offset = 0;

            //Amplitud
            float ampl = 360;

            mCountDown.schedule.Execute(() =>
            {
                // Usa Mathf.Sin para obtener un valor de opacidad - Funcion personalizada                 
                float opacity = offset + ampl * Mathf.Sin(freq * Time.time);

                // Aplicamos la opacidad
                mCountDown.style.rotate = new Rotate(opacity);                

            }).Every(16);

        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //****************************Liberación de la memoria*****************************//
        //*********************************************************************************//
        #region Libera memoria
        /// <summary>
        /// Libera memoria
        /// </summary>
        public override void Dispose()
        {        
            base.Dispose();
            if (fireworks != null)
            {
                fireworks.Disconnect();
                fireworks.Dispose();
            }            
            
            UIEvents.FillPanelScore -= FillScorePanel;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//

    }    
}
