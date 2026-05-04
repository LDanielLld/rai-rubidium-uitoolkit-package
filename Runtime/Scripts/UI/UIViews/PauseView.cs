using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    //Diferentes estados de icono de animacion
    public enum StatePulse
    {
        AN_EXPAND, //Animacion de expandir
        AN_CONTRACT, //Animacion de contraer 
        AN_STATIC //PARADO
    }

    /// <summary>
    /// Interfaz de pausa.
    /// </summary>
    public class PauseView : UIView
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Gestion del componente UI     
        
        private StatePulse statePulse = StatePulse.AN_STATIC;

        //Valores numericos
        private float timec = 0;
        private readonly float cTime = 0.35f;       

        // Elementos visuales
        StringIcon mIconLeft; //Icono izquierda
        StringIcon mIconRight; //Icono derecha

        // Class selector for currently selected tab button        
        const string anPulsed = "panelpause-pulse"; //Estilo de pulsacion de texto     
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
        public PauseView(VisualElement topElement) : base(topElement)
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
           
            //Añade estilos a los elementos de la interfaz
            mIconLeft = mTopElement.Q<StringIcon>("panelpause-iconleft");
            mIconRight = mTopElement.Q<StringIcon>("panelpause-iconright");            
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

            //Comprueba el estado actual del icono. Permite realizar una animacion completa de pulsacion
            if (statePulse == StatePulse.AN_STATIC)
            {
                timec += deltaTime; 
                if(timec >= cTime)
                {
                    statePulse = StatePulse.AN_EXPAND;
                    mIconLeft.AddToClassList(anPulsed);
                    mIconRight.AddToClassList(anPulsed);
                    timec = 0;
                }
            }
            else if (statePulse == StatePulse.AN_EXPAND)
            {
                timec += deltaTime;
                if (timec >= cTime)
                {
                    statePulse = StatePulse.AN_CONTRACT;
                    mIconLeft.RemoveFromClassList(anPulsed);
                    mIconRight.RemoveFromClassList(anPulsed);                    
                    timec = 0;
                }                
            }
            else if (statePulse == StatePulse.AN_CONTRACT)
            {
                timec += deltaTime;
                if (timec >= cTime)
                {
                    statePulse = StatePulse.AN_STATIC; 
                    timec = 0;
                }
            }

            //Comprueba que debe salir de la pantalla de pausa
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //Solucionar el problema del robot
                /*if (isStarted)
                    TargetManager.sharedInstance.ReStart(); //Reinicia el juego 

                if (oldGameState != GameState.SAFETYMODE) //El anterior no es safety, actua normal   
                {
                    SetUIState(oldGameState);
                    currentGameState = oldGameState; //Asignar el estado del juego actual 

                }
                else //Comprueba si han pasado los segundos de inicio
                {
                    GameState state = GameState.NONE;
                    if (isStarted)
                        state = GameState.INGAME; //Modo juego
                    else
                        state = GameState.STARTING; //Modo inicio

                    SetUIState(state);
                    currentGameState = state; //Asignar el estado del juego actual
                }
                oldGameState = GameState.PAUSE; //Asignar el estado del juego anterior*/
                state = true;
            }


            return state;
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
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
