using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIInterface
{
    /// <summary>
    /// Clase delegada asociada a los eventos que se pueden producir durante el
    /// funcionamiento de la interfaz.
    /// Estos eventos son conceptuales determinador por una Action que permite
    /// ejecutarlos desde cualquier lugar. Sustiuye los sharedIntance al ocupar
    /// solo los metodos determinan los eventos.
    /// </summary>
    public static class UIEvents
    {

        #region Eventos de Contado FPS        
        public static Action<bool> FpsCounterToggled; //Evento de mostrar/ocultar el contador de FPS
        public static Action<int> TargetFrameRateSet; //Evento para establecer objetivos de FPS
        #endregion


        #region Eventos de vistas
        public static Action<int,float> ScoreDisplayEvent; //Realiza una accion en el componente ScoreDisplay
        public static Action<int> StatEvent; //Realiza una accion en el componente Stats
        public static Action<float> TimeProgressEvent; //Realiza una accion en el componente TimeProgress        
        #endregion

    }
}
