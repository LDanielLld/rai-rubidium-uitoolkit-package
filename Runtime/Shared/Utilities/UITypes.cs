using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fichero que almacena los enum que utiliza el sistema de interfaz
/// </summary>
namespace UIInterface
{
    /// <summary>
    /// Diferentes estados de animacion de pulso
    /// </summary>
    public enum StatePulse
    {
        AN_EXPAND, //Animacion de expandir
        AN_CONTRACT, //Animacion de contraer 
        AN_STATIC //PARADO
    }

    /// <summary>
    /// Cantidad de elementos que se muestran en una etiqueta. TimeCounter
    /// </summary>
    public enum TypeTime
    {
        SECONDS, //Solo se muestran segundos
        MINUTES, //Se muestran minutos y segundos
        HOURS //Se muestran horas, minutos y segundos
    }

    /// <summary>
    /// Tipo de cuenta, hacia atrás o hacia adelante. Digit
    /// </summary>
    public enum TypeTimeline
    {
        REGRESIVE, //Cuenta atras - Animacion arriba-abajo
        FORWARD, //Cuenta adelante - Animacion abajo-arriba
        NONE //Sin asignar
    }

    /// <summary>
    /// Tipos de componente (Radial o lineal) TimeProgress
    /// </summary>
    public enum TypeTimeProgress
    {
        RADIAL,
        LINEAL
    }

    /// <summary>
    /// Registro de la base de datos del juego
    /// </summary>
    [Serializable]
    public class UserScoreData
    {
        public string id; //Identificador unico de registro      
        public int score; //Puntuacion del registro
        public long dateTime; //Hora del registro

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="user"></param>
        /// <param name="scr"></param>
        public UserScoreData(int user, int scr)
        {
            id = Guid.NewGuid().ToString();
            score = scr;
            dateTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Devuelve un string con la fecha actual:
        /// - Type 1: Devuelve solo fecha formato dd/MM/yyyy
        /// - Type 2: Devuelve solo hora del dia HH:mm:ss
        /// - Type 0: Devuelve solo ambos dd/MM/yyyy HH:mm:ss
        /// </summary>
        /// <returns></returns>
        public string GetDate(int type)
        {
            DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(dateTime).LocalDateTime;
            string strDate = "";
            if (type == 1)
                strDate = date.ToShortDateString();
            else if (type == 2)
                strDate = date.ToShortTimeString();
            else
                strDate = date.ToString("dd/MM/yyyy HH:mm:ss");
            return strDate;
        }
    }
}
