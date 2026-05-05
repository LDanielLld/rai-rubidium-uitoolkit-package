using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{

    /// <summary>
    /// Componente que convierte un codigo Unicode en un icono, y permite
    /// configurar el color y el tamaño
    /// </summary>
    public class StringIcon : VisualElement
    {
        //************************************Variables************************************//
        //*********************************************************************************//   
        #region [Function] Vinculacion con UIBuilder
        /// <summary>
        /// Muestra este control en el fichero UXML
        /// </summary>
        public new class UxmlFactory : UxmlFactory<StringIcon, UxmlTraits> { }

        /// <summary>
        ///  Muestra en el fichero Uxml la propiedad progress, para modificarla
        ///  desde el constructor visual o por código.
        /// </summary>       
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //Codigo del icono
            UxmlEnumAttributeDescription<IconType> mIconTypeAttribute = new UxmlEnumAttributeDescription<IconType>
            { name = "icon-unicode", defaultValue = IconType.Reloj };

            //Tamaño del icono
            UxmlIntAttributeDescription mFontSizeAttribute = new UxmlIntAttributeDescription
            { name = "font-size", defaultValue = 24 };

            //Color del icono
            UxmlColorAttributeDescription mIconColorAttribute = new UxmlColorAttributeDescription
            { name = "icon-color", defaultValue = Color.white };

            // Utiliza el metodo Init para asignar el valor del atributo en el fichero UXML a la propiedad progress en C#.
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = (StringIcon)ve;

                ate.Type = mIconTypeAttribute.GetValueFromBag(bag, cc); //Asigna tipo de barra
                ate.FontSize = mFontSizeAttribute.GetValueFromBag(bag, cc);
                ate.IconColor = mIconColorAttribute.GetValueFromBag(bag, cc);
            }
        }


        /// <summary>
        /// Selecciona el tipo de barra de progresso, cambiando la geometria
        /// </summary>
        public IconType Type
        {
            get => mType;
            set
            {
                if (mType != value)
                {
                    mType = value;

                    //Cada vez que cambia de tipo actualiza el codigo
                    mIconLabel.text = IconMapper.Get(mType);
                }
            }
        }

        /// <summary>
        /// Flag que indica si se debe cambiar el color de la barra dependiondo de su tamaño.        
        /// </summary>
        public Color IconColor
        {
            get => mIconLabel.style.color.value;
            set => mIconLabel.style.color = value;
        }


        /// <summary>
        /// Establece el porcentaje de radio que ocupa la barra en un componente radial. Tiene un valor
        /// de 0 a 100.
        /// </summary>
        public int FontSize
        {
            // La propiedad progress se expone en C#.
            get => (int)mIconLabel.style.fontSize.value.value;
            set => mIconLabel.style.fontSize = value;
        }
        #endregion

        #region [Function] Variables     
        // Nombre de las clases USS para el control del estilo.
        public static readonly string ussClassName = "icon__fontasset"; //Estilo general     

        // Etiqueta del icono.
        Label mIconLabel;

        //El numero que se muestra en la etiqueta como un porcentaje
        private float mProgress;

        //Tipo de componente
        private IconType mType;

        //Tamaño de la barra en porcentajes
        private float mSize;
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*************************Inicializacion y configuracion**************************//
        //*********************************************************************************//   
        #region [Function] Constructor       
        /// <summary>
        /// Constructor de la barra de progreso del tiempo
        /// </summary>
        public StringIcon()
        {
            // 1. Crear el elemento de texto que contendrá el glifo
            mIconLabel = new Label();

            //Añadir estilo
            mIconLabel.AddToClassList(ussClassName);

            //Adjuntar elemento
            Add(mIconLabel);

            // 2. Controlar la destrucción y limpieza (Dispose)
            RegisterCallback<DetachFromPanelEvent>(evt => OnDispose());
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//


        private void OnDispose()
        {
            // Limpieza de eventos si tuvieras más lógicas complejas suscritas
            mIconLabel = null;
        }
    }


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

}
