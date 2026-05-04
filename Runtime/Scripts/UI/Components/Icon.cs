using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{

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
}
