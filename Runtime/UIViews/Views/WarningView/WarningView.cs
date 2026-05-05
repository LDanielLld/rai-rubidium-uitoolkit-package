using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Interfaz de inicio del ejercicio. Cuenta atras para empezar y animaciones de 
    /// texto y paneles. Tiene una parte que se actualiza
    /// </summary>
    public class WarningView : UIView
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
        const string anPulsed = "panelwarning-pulse"; //Estilo de pulsacion de texto
     
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
        public WarningView(VisualElement topElement) : base(topElement)
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
            mIconLeft = mTopElement.Q<StringIcon>("panelwarning-iconleft");
            mIconRight = mTopElement.Q<StringIcon>("panelwarning-iconright");            
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

            //Comprueba que debe salir de la pantalla de warning
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Ejecuta la accion registrada
                UIEvents.WarningAction?.Invoke();

                state = Warning(); //
                //Solucionar el problema del robot
                /* bool isFixed =
                     RobotPlayerController.sharedInstance.modeControl.ClearProblemAndRestart();

                 if (isFixed) //Si esta solucionado arregla la interfaz
                 {
                     SetUIState(oldGameState);
                     currentGameState = oldGameState; //Asignar el estado del juego actual 
                     oldGameState = GameState.SAFETYMODE; //Asignar el estado del juego anterior 

                     if (isStarted)
                         TargetManager.sharedInstance.ReStart(); //Reinicia el juego 

                     //Envia el gameState en caso de ser necesario        
                     RobotPlayerController.sharedInstance.modeControl.Auxiliar(TypeState.GAME_STATE);
                 }*/
                //state = true;
            }


            return state;
        }


        protected virtual bool Warning() { return false; }
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
