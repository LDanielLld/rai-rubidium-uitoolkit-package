using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Clase base para una unidad funcional de componente UI. Este elemento puede abarcar
    /// una interfaz entera o una parte de una.
    /// </summary>
    public class UIView : IDisposable //Garantiza la liberacion manual de memoria y recursos. 
    {                                 //Recomendado para clases dque gestionan eventos de UI

        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Gestion del componente UI               
        protected bool mIsOverlay; // La UI revela otras interfaces subyacentes, parcialmente transparentes.

        protected VisualElement mTopElement; //Elemento raiz del componente
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//

        

        //*************************Inicializacion y configuración**************************//
        //*********************************************************************************//
        #region [Function] Inicializacion  
        /// <summary>
        /// Contructor que inicializa una nueva instancia de la clase UIView
        /// </summary>
        /// <param name="topElement">El elemento superior en la jerarquia UXML</param>
        public UIView(VisualElement topElement)
        {
            //Asigna elemento raiz y si no existe lanza una excepcion de argumento
            mTopElement = topElement ?? throw new ArgumentException(nameof(topElement));
            Initialize();
        }

        /// <summary>
        /// Contructor vacio
        /// </summary>        
        public UIView()
        {            
        }

        /// <summary>
        /// Metodo para inicializar el componente UI. Se puede personalizar al sobreescribir
        /// </summary>
        public virtual void Initialize()
        {
            //El primer paso es ocultar el elemento al crearlo
            Hide();

            //Se inicializan elementos visuales y eventos, en caso de necesitarlos
            SetVisualElements();
            RegisterUICallbacks();            
        }

        /// <summary>
        /// Metodo para inicializar el componente UI, incorporando el elemento Top
        /// </summary>
        public virtual void Initialize(VisualElement topElement)
        {
            //Asigna elemento raiz y si no existe lanza una excepcion de argumento
            mTopElement = topElement ?? throw new ArgumentException(nameof(topElement));

            //El primer paso es ocultar el elemento al crearlo
            Hide();

            //Se inicializan elementos visuales y eventos, en caso de necesitarlos
            SetVisualElements();
            RegisterUICallbacks();
        }
        #endregion

        #region [Function] Configuracion          
        /// <summary>
        /// Establece los elementos visuales del componente UI. Se sobreescribe para personalizar.
        /// </summary>
        protected virtual void SetVisualElements(){}

        
        /// <summary>
        /// Registra los callbacks de eventos. Se sobreescribe para personalizar.
        /// </summary>
        protected virtual void RegisterUICallbacks(){}
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*********************Gestión de la visualizacion en pantalla*********************//
        //*********************************************************************************//
        #region [Function] Actualizacion de componente
        /// <summary>
        /// Actualiza los componentes internos de la vista actual
        /// </summary>
        public virtual bool Update(float data)
        {
            return false;
        }
        #endregion

        #region [Function] Visualizacion
        /// <summary>
        /// Muestra el componente UI
        /// </summary>
        public virtual void Show()
        {
            mTopElement.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Oculta el componente UI
        /// </summary>
        public virtual void Hide()
        {
            mTopElement.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Oculta un elemento de la interfaz
        /// </summary>
        /// <param name="visualElement"></param>
        /// <param name="state"></param>
        void ShowVisualElement(VisualElement visualElement, bool state)
        {
            if (visualElement == null)
                return;

            visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //******************************Liberacion de memoria******************************//
        //*********************************************************************************//
        #region [Function] Libera recursos  
        /// <summary>
        /// Cancela los registros de callbacks o gestores de eventos. Se sobreescribe para personalizar al elemento
        /// </summary>
        public virtual void Dispose(){ }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*******************************Obtener propiedades*******************************//
        //*********************************************************************************//
        #region [Variables] Gestion del componente UI                       
        /// <summary>
        /// Devuelve el elemento raiz
        /// </summary>
        public VisualElement Root => mTopElement;

        /// <summary>
        /// Devuelve si el elemento está transparente
        /// </summary>
        public bool IsTransparent => mIsOverlay;

        /// <summary>
        /// Comprueba si el elemento raiz está oculto
        /// </summary>
        public bool IsHidden => mTopElement.style.display == DisplayStyle.None;
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
