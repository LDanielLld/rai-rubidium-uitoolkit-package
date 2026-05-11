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
    /// ejecutarlos desde cualquier lugar. Sustituye los sharedIntance al ocupar
    /// solo los metodos determinan los eventos.
    /// </summary>
    public static class UIEvents
    {

        #region Eventos de Contado FPS        
        public static Action<bool> FpsCounterToggled; //Evento de mostrar/ocultar el contador de FPS
        public static Action<int> TargetFrameRateSet; //Evento para establecer objetivos de FPS
        #endregion


        #region Eventos de vistas
        //Display de puntuaciones
        public static Action<int> ScoreDisplayScore; //Actualiza la puntuacion en el componente ScoreDisplay
        public static Action<int> ScoreDisplayHighScore; //Establece la puntuacion maxima
        public static Action<float, float> ScoreDisplayCombo; //Establece condiciones de combo

        public static Action<int> StatEvent; //Realiza una accion en el componente Stats

        public static Action<bool> StateTimeProgress; //Conecta o desconecta el componente TimeProgress        
        public static Func<float> GetTimeProgress; //Obtiene el tiempo actual de progreso        
        #endregion

        #region Eventos de FinishView       
        public static Action<List<UserScoreData>, string, string> FillPanelScore; //Para rellenar los paneles de puntuacion        
        #endregion


    }
}
