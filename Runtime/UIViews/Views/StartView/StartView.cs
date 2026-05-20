using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Interfaz de inicio del ejercicio. Cuenta atras para empezar y animaciones de 
    /// texto y paneles
    /// </summary>
    public class StartView : UIView
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Gestion del componente UI         
        /// <summary>
        /// Estado actual de la intefaz de inicio
        /// </summary>
        private enum StateStartView
        {
            SSV_INITIALIZE, //Inicializa visualemente la etiqueta
            SSV_COUNTDOWN, //Cuenta atras,
            SSV_STARTING, //Señal de empezar
            SSV_PAUSE, //Indica pausa
            SSV_ANIMATE_STARTING //Aanimacion de cambio
        }

        private StateStartView cState = StateStartView.SSV_COUNTDOWN; //Empieza con la cuenta atras

        // Elementos visuales
        VisualElement mPanel; //Panel de secuencia
        Label mSequence; //Secuencia atrás

        // Class selector for currently selected tab button
        const string anShringText = "countdownseq-text-shrink"; //Estilo de encoger texto
        const string anExpandPanel = "countdownseq-label-started"; //Estilo de expandir panel
        const string anPulseText = "countdownseq__num-pulse"; //Estilo de pulsacion de texto

        //Contador de tiempo       
        private float timerTotalGame;
        private int SEC_TO_START = 3;
        private float timer = 0.0f; //Cada segundo cambia la etiqueta
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
        public StartView(VisualElement topElement) : base(topElement)
        {
            //UIEvents.initCosas += InitShinkLabel;                        
        }
        #endregion

        #region [Function] Configuración
        /// <summary>
        /// Establece los elementos visuales del componente UI. Se sobreescribe para personalizar.
        /// </summary>
        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            //Añade estilos a los elementos de la interfaz
            mPanel = mTopElement.Q<VisualElement>("countdown-panel");
            mSequence = mTopElement.Q<Label>("countdown-sequence");

            //Establece valor de conteo inicial
            UpdateSeconds();
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
            if (cState == StateStartView.SSV_COUNTDOWN)
            {
                if (SEC_TO_START > 0) //Está en la cuenta atras
                {
                    timer += deltaTime;
                    if (timer >= 1.0f) //Reduce cada segundo
                    {
                        SEC_TO_START--;
                        timer = 0.0f;
                        UpdateVisuals();                        
                    }
                }
                else //Acaba la cuenta atras y muestra el mensaje de Empieza
                {
                    cState = StateStartView.SSV_ANIMATE_STARTING;
                    mSequence.AddToClassList(anShringText);

                    //Aplica animacion de mostrar el panel de Empieza
                    mSequence.schedule.Execute(() =>
                    {
                        mPanel.AddToClassList(anExpandPanel); //Expande el panel
                        mSequence.text = "¡EMPIEZA!";
                        mSequence.RemoveFromClassList(anShringText);

                        //Lanza sonido para finalizar vista
                        UIEvents.SoundEndStartView?.Invoke();
                    }).StartingIn(150);

                    timer = 0.0f;
                }
            }
            else if(cState == StateStartView.SSV_ANIMATE_STARTING)
            {
                //Pasa un segundo para mostrar el empieza
                timer += deltaTime;
                if (timer >= 1.0f)                
                    state = true;                
            }            
            return state;
        }

        /// <summary>
        /// Actualiza la cuenta atras aplicando una animaicon de cambio de numero.
        /// </summary>
        private void UpdateVisuals()
        {
            if (SEC_TO_START > 0)
            {
                UpdateSeconds();
                // Efecto de pulso usando una corrutina rápida
                mSequence.AddToClassList(anPulseText);
                //Aplica una animacion de pulso sobre el texto.
                mSequence.schedule.Execute(() =>
                    mSequence.RemoveFromClassList(anPulseText)
                ).StartingIn(150); //Delay de 150ms
            }            
        }

        /// <summary>
        /// Actualiza los segundos de la cuenta atras
        /// </summary>
        private void UpdateSeconds()
        {
            mSequence.text = $"{SEC_TO_START}";

            //Lanza sonido para indicar paso del tiempo
            UIEvents.SoundInitStartView?.Invoke();
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
            //UIEvents.initCosas -= InitShinkLabel;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//

    }
}