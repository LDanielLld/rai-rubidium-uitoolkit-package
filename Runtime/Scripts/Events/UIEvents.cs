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


        public static Action initCosas;
       

        
    }
}
