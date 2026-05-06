using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Posibles estados del juego
    /// </summary>
    public enum GameState
    {
        STARTING, //Al inicio    
        INGAME, //Realizacion del juego    
        FINISH, //Finalizacion
        SCORE, //Muestra datos en una tabla
        SAFETYMODE, //Modo seguro
        PAUSE, //Modo pausa
        NONE
    }

    /// <summary>
    /// Clase encargada de gestionar las partes de la interfaz grafica de usuario general, controlando los UXMLs
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager sharedInstance = null;

        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Componentes graficos de la UI
        UIDocument m_MainMenuDocument;

        //Elemento raiz
        VisualElement root;

        UIView mCurrentView;
        UIView mPreviousView;

        //Modal screens - No necesitan modificacion
        UIView mStartView;  // Vista de la cuenta atras para empezar el juego        
        UIView mPauseView;  // Vista con la pantalla de pausa
        UIView mWarningView;  // Vista con la pantalla de Warning (Problema con el robot)
        UIView mFinishView;  // Vista que marca el final del juego y la pantalla de puntuaciones

        //Modal screen - Necesitan modificacion
        UIView mGameView;  // Vista con los elementos que interactuan durante el juego

        // Lista de las vistas para eliminar recursos
        List<UIView> m_AllViews = new List<UIView>();

        // Identificadores de los UIViews, cada uno representa una rama del arbol de componentes
        public const string kStartViewName = "StartView";
        public const string kGameViewName = "GameView";
        public const string kWarningViewName = "WarningView";
        public const string kPauseViewName = "PauseView";
        public const string kFinishViewName = "FinishView";

        public UIDocument MainMenuDocument => m_MainMenuDocument;

        //Flag que indica si esta funcionando la vista actual
        public bool isFinish = false;
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*********************Inicializacion, configuración y cierre**********************//
        //*********************************************************************************//
        #region [Unity Function] Inicializacion   
        /// <summary>
        /// Se ejecuta antes del metodo start, justo al crear el componente
        /// </summary>
        private void Awake()
        {
            if (sharedInstance == null)
                sharedInstance = this;
        }

        /// <summary>
        /// Inicializa el componente
        /// </summary>
        private void Start()
        {
            m_MainMenuDocument = GetComponent<UIDocument>();
            SetupViews();
        }

        /// <summary>
        /// Inicializa las vistas de la interfaz
        /// </summary>
        private void SetupViews()
        {
            //Inicializa componente raiz
            root = m_MainMenuDocument.rootVisualElement;

            //Registra todas las vistas individuales
            mStartView = new StartView(root.Q<VisualElement>(kStartViewName)); // Pantalla de inicio            
            mWarningView = new WarningView(root.Q<VisualElement>(kWarningViewName)); // Pantalla de advertencia
            mPauseView = new PauseView(root.Q<VisualElement>(kPauseViewName)); //Pantalla de pausa
            mFinishView = new FinishView(root.Q<VisualElement>(kFinishViewName)); //Pantalla de pausa


            // Recolecta todas las vistas en un array, para gestiones de liberacion de memoria
            m_AllViews.Add(mStartView);            
            m_AllViews.Add(mWarningView);
            m_AllViews.Add(mPauseView);
            m_AllViews.Add(mFinishView);
        }
        #endregion


        #region [Unity Function] Actualizacion   
        /// <summary>
        /// Bucle de actualizacion 
        /// </summary>
        
        private void Update()
        {
            //Ejecuta el metodo update del componente actual mostrado en pantalla, para ver si se 
            // cumplen los objetivos temporales
            if (mCurrentView != null)
                isFinish = mCurrentView.Update(Time.deltaTime);

        }
        #endregion


        #region [Function] Inicializacion de la vista Game   
        /// <summary>
        /// Se crea y añade desde fuera de la clase. Enviando un UIView
        /// </summary>        
        public void SetGameView(UIView gameView)
        {
            mGameView = gameView;
            mGameView.Initialize(root.Q<VisualElement>(kWarningViewName));
            m_AllViews.Add(mGameView);
        }        
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*********************Gestion de visualizacion de la interfaz*********************//
        //*********************************************************************************//         
        #region [Function] Gestion de vistas
        public void SetUIState(GameState gameState)
        {
            //Empieza con la interfaz de cuenta atrás
            if (gameState == GameState.STARTING)
                ShowModalView(mStartView);
            else if (gameState == GameState.INGAME)
                ShowModalView(mGameView);
            else if (gameState == GameState.SAFETYMODE)
                ShowModalView(mWarningView);
            else if (gameState == GameState.PAUSE)
                ShowModalView(mPauseView);
            else if (gameState == GameState.FINISH)
                ShowModalView(mFinishView);
        }


        /// <summary>
        /// Muestra en pantalla la vista seleccionada
        /// </summary>
        /// <param name="newView"></param>
        void ShowModalView(UIView newView)
        {
            //Registra la vista actual y la anterior
            if (mCurrentView != null)
                mCurrentView.Hide(); //Quita la actual

            mPreviousView = mCurrentView;
            mCurrentView = newView;

            // Muestra en la escena la vista seleccionada
            if (mCurrentView != null)
                mCurrentView.Show();
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//

        

        //*******************************Contador de frames********************************//
        //*********************************************************************************//         
        #region [Function] Gestion del contador de frames
        /// <summary>
        /// Activa o desactiva el contador de frames
        /// </summary>            
        public void FpsCounterToogle(bool state)
        {
            UIEvents.FpsCounterToggled?.Invoke(true);
        }

        /// <summary>
        /// Establece el framerate objetivo del juego
        /// </summary>
        /// <param name="state"></param>
        public void SetFpsFrameRate(bool state)
        {
            UIEvents.TargetFrameRateSet?.Invoke(-1);
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//        
    }
}
