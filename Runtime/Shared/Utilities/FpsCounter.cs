using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Contador simple de frames (fps: frame per second)
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        public const int kTargetFrameRate = -1;//Nunero de frames objetivo

        //Buffer de calculo del frame rate
        private float[] mDeltaTimeBuffer;
        private int mCurrentIndex; //Indice del buffer
        public const int kBufferSize = 50; //Numero de frames por muestra

        private Label mFpsLabel; //Etiqueta del contador
        private bool mIsEnabled;//Flag de visibilidad

        //Valor actual de fps
        private float mFpsValue;
        public float FpsValue => mFpsValue;


        //Para asignar en el inspector de Unity
        [SerializeField] UIDocument mDocument;

        //*********************Inicializacion, configuración y cierre**********************//
        //*********************************************************************************//
        #region [Unity Function] Inicializacion  
        /// <summary>
        /// Funcion awake del componente. Se inicia antes que todos los Start
        /// </summary>
        void Awake()
        {
            //Inicializa buffer de calculo
            mDeltaTimeBuffer = new float[kBufferSize];

            //Asigna frames ojetivo
            Application.targetFrameRate = kTargetFrameRate;
        }

        /// <summary>
        /// Funcion OnEnble de Unity. Inicializ el componente cada vez que el script se 
        /// activa. Ideal para establecer conexiones que deben reiniciarse tras una pausa
        /// </summary>
        void OnEnable()
        {
            UIEvents.FpsCounterToggled += OnFpsCounterToggled;
            UIEvents.TargetFrameRateSet += OnTargetFrameRateSet;

            var root = mDocument.rootVisualElement;

            //Asigna contador del documento UI
            mFpsLabel = root.Q<Label>("fps-counter");

            //Alerta por si no existe
            if (mFpsLabel == null)
            {
                Debug.LogWarning("[FPSCounter]: Display label is null.");
                return;
            }
        }


        /// <summary>
        /// Gestiona la limpieza y desconexión del componente justo antes de que el 
        /// el script se desactiven. Mantiene la integridad de la memoria del sistema.
        /// </summary>        
        void OnDisable()
        {
            UIEvents.FpsCounterToggled -= OnFpsCounterToggled;
            UIEvents.TargetFrameRateSet -= OnTargetFrameRateSet;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //***************************Actualizacion del contador****************************//
        //*********************************************************************************//
        #region [Unity Function] Actualizacion del contador  
        /// <summary>
        /// Metodo de Unity que se ejecuta una vez por frame. Actualiza el buffer datos 
        /// calcula el frame rate y lo muestra en pantalla
        /// </summary>
        void Update()
        {
            if (mIsEnabled)
            {
                mDeltaTimeBuffer[mCurrentIndex] = Time.deltaTime; //Actualiza buffer de datos
                mCurrentIndex = (mCurrentIndex + 1) % mDeltaTimeBuffer.Length; //Incrementa indice
                mFpsValue = Mathf.RoundToInt(CalculateFps()); //Calcula valor y lo redondea

                //Muestra valor en etiqueta
                mFpsLabel.text = $"FPS: {mFpsValue}";
            }
        }

        /// <summary>
        /// Metodo para calcular los fps a partir del buffer de datos. Calcula una media
        /// </summary>
        /// <returns></returns>
        private float CalculateFps()
        {
            float totalTime = 0f;
            foreach (float deltaTime in mDeltaTimeBuffer)            
                totalTime += deltaTime;
            return mDeltaTimeBuffer.Length / totalTime;

        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //****************************Funcionamiento de eventos****************************//
        //*********************************************************************************//
        #region [Function] Eventos         
        /// <summary>
        /// Establece el ratio de frames (frame rate): -1 = lo mas rápido posible o otros para reducir
        /// </summary>
        /// <param name="newFrameRate"></param>
        void OnTargetFrameRateSet(int newFrameRate)
        {
            Application.targetFrameRate = newFrameRate;
        }

        // Event-handling methods
        /// <summary>
        /// Metodo para ocultar o mostrar el contador de frames
        /// </summary>
        /// <param name="state"></param>
        void OnFpsCounterToggled(bool state)
        {
            mIsEnabled = state;
            mFpsLabel.style.visibility = (state) ? Visibility.Visible : Visibility.Hidden;
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}
