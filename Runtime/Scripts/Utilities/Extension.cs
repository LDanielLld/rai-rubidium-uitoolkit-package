using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{ 
    /// <summary>
    /// Libreria estatica de metodos extendidos
    /// </summary>
    public static class Extension
    {
        //*******************************Ajuste de tamaños*********************************//
        //*********************************************************************************//
        #region Autoescalado de texto    
        /// <summary>
        /// Actualiza el tamaño de la letra en funcion de su contenedor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="container"></param>
        /// <param name="percentage"></param>
        public static void BindAutoFontSize(Label label, VisualElement container, float percentage)
        {
            container.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                // El tamaño de la letra será el 50% de la altura del contenedor
                float newHeight = evt.newRect.height;
                float calculatedSize = newHeight * percentage;

                // Limitamos para que no sea ni muy pequeño ni muy grande
                label.style.fontSize = Mathf.Clamp(calculatedSize, 5, 90);
            });
        }

        /// <summary>
        /// Actualiza el tamaño del icono en funcion de  su contenedor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="container"></param>
        /// <param name="percentage"></param>
        public static void BindAutoIconSize(StringIcon label, VisualElement container, float percentage)
        {
            container.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                // El tamaño de la letra será un porcentaje de la altura del contenedor
                float newHeight = evt.newRect.height;
                float calculatedSize = newHeight * percentage;

                //Se limita para que no sea ni muy pequeño ni muy grande
                label.FontSize = (int)Mathf.Clamp(calculatedSize, 5, 90);
            });
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//


        /// <summary>
        /// Genera posiciones aleatorias en diferentes cuadrantes en porcentajes similares
        /// </summary>
        public class RandomHelper
        {

            private float[] widthSize = new float[] { 0.12f, 0.88f }; //Porcentaje normalizado
            private float[] heightSize = new float[] { 0.10f, 0.50f };

            private int _leftCount = 0;
            private int _rightCount = 0;


            //Valores en pixeles de la altura y la anchura del canvas
            private float width;
            private float height;

            private float dialSize;


            // Límites de tolerancia (0.6 significa 60%)
            private const double MaxPercentage = 0.6;

            public enum Side { Left, Right, None }


            private static System.Random _random = new System.Random();


            public RandomHelper(float percentage)
            {
                //Tamaño del cuadrante dependiendo porcentaje
                dialSize = ((widthSize[1] - widthSize[0]) / 2) * percentage;
            }



            public Vector2 GetNextRandom()
            {
                int total = _leftCount + _rightCount;

                Side cSide = Side.None;

                // Caso inicial: Si no hay nada, se decide aleatoriamente
                if (total == 0)
                {
                    cSide = GenerateSide();
                }
                else
                {
                    bool canGoLeft = (float)(_leftCount + 1) / (total + 1) <= MaxPercentage;
                    bool canGoRight = (float)(_rightCount + 1) / (total + 1) <= MaxPercentage;

                    // Si ambos están permitidos, elegimos al azar (esto permite las "rachas")
                    if (canGoLeft && canGoRight)
                    {
                        cSide = GenerateSide();
                    }
                    // Si solo uno está permitido, forzamos ese lado para mantener el equilibrio
                    else if (canGoLeft)
                        cSide = Side.Left;
                    else
                        cSide = Side.Right;
                }

                //Incremento de los registros
                Increment(cSide);

                PrintStats();

                //Obtener el valor para devolver
                return Generate(cSide);
            }


            /// <summary>
            /// Controla los porcentajes de un lado y otro
            /// </summary>    
            private void Increment(Side side)
            {
                if (side == Side.Left) _leftCount++;
                else _rightCount++;
            }



            public void PrintStats()
            {
                int total = _leftCount + _rightCount;
                Debug.Log($"Total: {total} | Izq: {(_leftCount * 100.0 / total):F1}% | Der: {(_rightCount * 100.0 / total):F1}%");
            }


            /// <summary>
            /// Genera la posicion aleatoria del cuadrante seleccionado
            /// </summary>
            /// <param name="side"></param>
            /// <returns></returns>
            private Vector2 Generate(Side side)
            {
                Vector2 value = Vector2.zero;

                //Dependiendo del lado, se calcula valor de anchura
                if (side == Side.Left) //Izquierda
                    value.x = Random.Range(widthSize[0], widthSize[0] + dialSize);
                else //Derecha
                    value.x = Random.Range(widthSize[1] - dialSize, widthSize[1]);


                //Valor de altura
                value.y = Random.Range(heightSize[0], heightSize[1]);
                return value;
            }


            /// <summary>
            /// Genera un lado
            /// </summary>    
            private Side GenerateSide()
            {
                //Obtiene (1 o 2)
                int i = Random.Range(1, 3);
                return i == 1 ? Side.Left : Side.Right;
            }


        }
    }

}
