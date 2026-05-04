using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIInterface
{
    /// <summary>
    /// Codigos de los iconos para su seleccion desde el constructor UI
    /// </summary>
    [Serializable]
    public enum IconType
    {
        // --- TIEMPO Y PROGRESO ---
        Reloj,
        RelojArena,
        Cronometro,
        Calendario,
        Fuego,

        // --- INTERFAZ Y MENÚS ---
        Ajustes,
        Usuario,
        Casa,
        Candado,
        CandadoAbierto,
        Basura,
        Buscar,
        Info,
        Advertencia,
        Pausa,
        Mano,

        // --- JUGABILIDAD / RPG ---
        Espada,
        Escudo,
        Corazon,
        Pocima,
        Craneo,
        Cofre,
        Moneda,
        Mochila,

        // --- DIRECCIONES Y FLECHAS ---
        FlechaArriba,
        FlechaAbajo,
        FlechaIzquierda,
        FlechaDerecha,
        Recargar,

            // --- PROPIAS ---
        Repeticion,
        Rotate, 
        Redo, 
        History, 
        Hourglass,
        ArrowsRepeat,
        TrendUp, 
        Heartbeat,
        Infinity, 
        Cogs, 
        Hammer, 
        Screwdriver, 
        Tasks,
        Route, 

    }

    /// <summary>
    /// Mapeador de typos de iconos con sus respectivos codigos
    /// </summary>
    public static class IconMapper
    {
        private static readonly Dictionary<IconType, string> Iconos = new Dictionary<IconType, string>
        {
            // --- TIEMPO Y PROGRESO ---
            { IconType.Reloj, "\uf017" },
            { IconType.RelojArena, "\uf253" },
            { IconType.Cronometro, "\uf2f2" },
            { IconType.Calendario, "\uf133" },
            { IconType.Fuego, "\uf06d" },
            
            // --- INTERFAZ Y MENÚS ---
            { IconType.Ajustes, "\uf013" },
            { IconType.Usuario, "\uf007" },
            { IconType.Casa, "\uf015" },
            { IconType.Candado, "\uf023" },
            { IconType.CandadoAbierto, "\uf09c" },
            { IconType.Basura, "\uf1f8" },
            { IconType.Buscar, "\uf002" },
            { IconType.Info, "\uf05a" },
            { IconType.Advertencia, "\uf071" },
            { IconType.Pausa, "\u23F8" },
            { IconType.Mano, "\u270B" },

            // --- JUGABILIDAD / RPG ---
            { IconType.Espada, "\uf6ec" },
            { IconType.Escudo, "\uf132" },
            { IconType.Corazon, "\uf004" },
            { IconType.Pocima, "\uf0c3" },
            { IconType.Craneo, "\uf714" },
            { IconType.Cofre, "\uf5cd" },
            { IconType.Moneda, "\uf51e" },
            { IconType.Mochila, "\uf5d3" },

            // --- DIRECCIONES Y FLECHAS ---
            { IconType.FlechaArriba, "\uf062" },
            { IconType.FlechaAbajo, "\uf063" },
            { IconType.FlechaIzquierda, "\uf060" },
            { IconType.FlechaDerecha, "\uf061" },
            { IconType.Recargar, "\uf021" },

            // --- PROPIAS ---
            { IconType.Repeticion, "\uf079" },
            { IconType.Rotate, "\uf021" },
            { IconType.Redo, "\uf01e" },
            { IconType.History, "\uf1da" },
            { IconType.Hourglass, "\uf252" },
            { IconType.ArrowsRepeat, "\uf365" },
            { IconType.TrendUp, "\ufe01" },
            { IconType.Heartbeat, "\uf21e" },
            { IconType.Infinity, "\uf534" },
            { IconType.Cogs, "\uf085" },
            { IconType.Hammer, "\uf6e3" },
            { IconType.Screwdriver, "\uf7d9" },
            { IconType.Tasks, "\uf0ae" },
            { IconType.Route, "\uf4d7" }
        };

        public static string Get(IconType name)
        {
            if (Iconos.TryGetValue(name, out string unicode))
            {
                return unicode;
            }
            return "\uf017"; // Icono de reloj por defecto si no lo encuentra
        }
    }


   /* // --- TIEMPO Y PROGRESO ---
    public static string Reloj = "\uf017";
    public const string RelojArena = "\uf253";
    public const string Cronometro = "\uf2f2";
    public const string Calendario = "\uf133";
    public const string Fuego = "\uf06d";

    // --- INTERFAZ Y MENÚS ---
    public const string Ajustes = "\uf013";
    public const string Usuario = "\uf007";
    public const string Casa = "\uf015";
    public const string Candado = "\uf023";
    public const string CandadoAbierto = "\uf09c";
    public const string Basura = "\uf1f8";
    public const string Buscar = "\uf002";
    public const string Info = "\uf05a";
    public const string Advertencia = "\uf071";

    // --- JUGABILIDAD / RPG ---
    public const string Espada = "\uf6ec";
    public const string Escudo = "\uf132";
    public const string Corazon = "\uf004";
    public const string Pocima = "\uf0c3";
    public const string Craneo = "\uf714";
    public const string Cofre = "\uf5cd";
    public const string Moneda = "\uf51e";
    public const string Mochila = "\uf5d3";

    // --- DIRECCIONES Y FLECHAS ---
    public const string FlechaArriba = "\uf062";
    public const string FlechaAbajo = "\uf063";
    public const string FlechaIzquierda = "\uf060";
    public const string FlechaDerecha = "\uf061";
    public const string Recargar = "\uf021";*/
}
