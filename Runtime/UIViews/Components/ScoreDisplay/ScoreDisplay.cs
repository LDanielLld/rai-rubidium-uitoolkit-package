using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Representa el elemento que muestra en el juego la puntuacion actual
    /// y la puntuacion máxima del usuario
    /// </summary>
    public class ScoreDisplay
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Parametros del panel de puntuacion
        [SerializeField]
        private VisualTreeAsset mScoreAsset; //Elemento asset instanciado

        private Label mScore; //Etiqueta de la puntuacion actual
        private Label mCombo; //Etiqueta del combo
        private Label mMaxScore; //Etiqueta de la puntuacion maxima    

        public Gradient gradient; //Gradiente de colores para cambiar color

        // Selectores de estilos
        const string kAnimfade = "scoredisplay_fade"; //Ocultar
        const string kAnimPulse = "scoredisplay_pulse"; //Pulso

        private float comboDisplay = 0.0f; //Valor de combo mostrado en pantalla

        //Plantilla - Elemento central
        private VisualElement template;
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*********************************Inicialización**********************************//
        //*********************************************************************************//
        #region [Function] Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="tmpl"></param>
        public ScoreDisplay(VisualElement tmpl)
        {
            template = tmpl;
            SetVisualElements();
            CreateComboGradient();
        }  
        #endregion

        #region [Function] Componentes visuales
        /// <summary>
        /// Registra los componentes del fichero UXML
        /// </summary>
        /// <param name="shopItemElement"></param>
        public void SetVisualElements()
        {
            // Busca las partes del elemento ScoreDisplay           
            mScore = template.Q<Label>("score-label");
            mMaxScore = template.Q<Label>("highscore-label");            
            mCombo = template.Q<Label>("combo-label");  
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*******************Gestion visual del marcador de puntuacion*********************//
        //*********************************************************************************//
        #region Gestion de puntuacion
        /// <summary>
        /// Inicializa etiquetas del panel de puntuaciones
        /// </summary>
        /// <param name="cscore"></param>
        public void Init()
        {
            //Actualiza los textos de las puntuaciones        
            mCombo.text = ""; //Primero desconectado con 1x                       
            mScore.text = "0"; //Inicializa puntuacion de sesion
            mMaxScore.text = "----";

            //Adaptacion dinamica del tamaño de fuente
            BindAutoFontSize(mScore, template.Q("score__topsection"),0.85f);
            BindAutoFontSize(mMaxScore, template.Q("scoredisplay__panelinside"), 0.65f);
            BindAutoFontSize(mCombo, template.Q("score__topsection"), 0.35f);
        }

        /// <summary>
        /// Actualiza la puntuacion actual de sesion y realiza una animacion
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore(int score)
        {
            mScore.text = $"{score}";

            // Aplica una animacion de pulso sobre el texto.
            mScore.AddToClassList(kAnimPulse);
            mScore.schedule.Execute(() =>
                mScore.RemoveFromClassList(kAnimPulse)
            ).StartingIn(100); //Delay de 0.1s                       
        }

        /// <summary>
        /// Actualiza la puntuacion mas alta
        /// </summary>
        /// <param name="score"></param>
        public void UpdateHighScore(int score)
        {
            if (score > 0)
                mMaxScore.text = score.ToString("0");
            else
                mMaxScore.text = score.ToString("----");            
        }
        #endregion        
        //*********************************************************************************//
        //*********************************************************************************//



        //**********************Gestion visual del marcador de combo***********************//
        //*********************************************************************************//
        #region Gestion de combos
        /// <summary>
        /// Actualiza el marcador del combo
        /// </summary>
        /// <param name="score"></param>
        public void UpdateCombo(float combo, float max)
        {
            // Escalonado a incrementos de 0.5 
            float stepped = Mathf.Floor(combo / 0.5f) * 0.5f;

            // Si el combo se ha roto (comboReal == 0)
            if (combo <= 0.001f && comboDisplay > 0f)
            {
                comboDisplay = 0f;

                // Lanza animacion de desaparecer               
                mCombo.AddToClassList(kAnimfade);
            }
            else if (Mathf.Abs(stepped - comboDisplay) > 0.001f) // Solo actualiza si ha cambiado
            {
                bool wasZero = comboDisplay == 0f;
                comboDisplay = stepped;

                //Actualizar texto
                mCombo.text = $"x{(FormatNumber(comboDisplay + 1))}";
                

                // Cambiar color según rango 
                float t = combo / max; // normalizado 0–1
                mCombo.style.color = gradient.Evaluate(t);               

                //Anima en funcion de si aparece o actualiza
                if (wasZero)
                    mCombo.RemoveFromClassList(kAnimfade);                
                else
                {
                    // Aplica una animacion de pulso sobre el texto.
                    mCombo.AddToClassList(kAnimPulse);
                    mCombo.schedule.Execute(() =>
                        mCombo.RemoveFromClassList(kAnimPulse)
                    ).StartingIn(150); //Delay de 0.15s        
                }                  
            }
        }

        /// <summary>
        /// Devuelve un formato de string con decimales y sin decimales
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string FormatNumber(float value)
        {
            return (value % 1 == 0) ? ((int)value).ToString() : value.ToString("0.0");
        }

        /// <summary>
        /// Funcion para crear un gradiente de colores para la etiqueta de combos
        /// dependiendo de su valor. Es una curva de colores
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public Gradient CreateComboGradient()
        {
            gradient = new Gradient();

            GradientColorKey[] colorKeys = new GradientColorKey[5];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];

            float index = 0.25f;
            float inc = (1f - index) / 4;

            //Establece las key de colores de manera dinamica
            colorKeys[0] = new GradientColorKey(Color.green, index); index += inc;
            colorKeys[1] = new GradientColorKey(Color.yellow, index); index += inc;
            colorKeys[2] = new GradientColorKey(new Color(1f, 0.5f, 0f), index); index += inc; // naranja 
            colorKeys[3] = new GradientColorKey(Color.red, index); index += inc;
            colorKeys[4] = new GradientColorKey(new Color(0.8f, 0.1f, 0.8f), index); // púrpura

            //Key del canal alpha
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);

            //Configura el gradiente
            gradient.SetKeys(colorKeys, alphaKeys);

            return gradient;
        }
        #endregion

        #region Animacion de combos
        /// <summary>
        /// Gestor de animacion del texto de combo al cambiar de valores
        /// </summary>
        /// <param name="rect"></param>
       /* void AnimateCombo()
        {
          //  if (popRoutine != null) StopCoroutine(popRoutine);
            popRoutine = StartCoroutine(PlayComboPop());
        }*/

        /// <summary>
        /// Animacion de incremento de combo
        /// </summary>
     /*   private IEnumerator PlayComboPop()
        {
            float upTime = 0.12f;
            float downTime = 0.10f;
            float maxScale = 1.3f;

            // Escalado hacia arriba
            float t = 0f; while (t < upTime)
            {
                t += Time.deltaTime;
                float lerp = t / upTime;
                float scale = Mathf.Lerp(1f, maxScale, Mathf.SmoothStep(0f, 1f, lerp));
                lblCombo.transform.localScale = new Vector3(scale, scale, 1f);
                lblx.transform.localScale = new Vector3(scale, scale, 1f);
                yield return null;

                t += Time.deltaTime;
                float lerp = t / duration;
                target.localScale = Vector3.Lerp(original, targetScale, lerp);
                yield return null;
            }
            // Escalado hacia abajo
            t = 0f; while (t < downTime)
            {
                t += Time.deltaTime; float lerp = t / downTime;
                float scale = Mathf.Lerp(maxScale, 1f, Mathf.SmoothStep(0f, 1f, lerp));
                lblCombo.transform.localScale = new Vector3(scale, scale, 1f);
                lblx.transform.localScale = new Vector3(scale, scale, 1f); yield return null;
            }
            lblCombo.transform.localScale = Vector3.one;
            lblx.transform.localScale = Vector3.one;
        }*/


        /// <summary>
        /// Lanza corrutina para esconder combo
        /// </summary>
       /* void PlayFadeOut()
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(ComboFadeOut());
        }*/

        /// <summary>
        /// Lanza corutina para mostrar texto
        /// </summary>
        /*void PlayFadeIn()
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(ComboFadeIn());
        }*/

        /// <summary>
        /// Animacion para que desaparezca el combo
        /// </summary>
        /// <returns></returns>
        /*private IEnumerator ComboFadeOut()
        {
            float duration = 0.25f;
            float t = 0f;
            Color start = lblCombo.color;
            Color end = new Color(start.r, start.g, start.b, 0f);
            while (t < duration)
            {
                t += Time.deltaTime;
                float lerp = t / duration;
                lblCombo.color = Color.Lerp(start, end, Mathf.SmoothStep(0f, 1f, lerp));
                lblx.color = Color.Lerp(start, end, Mathf.SmoothStep(0f, 1f, lerp));
                yield return null;
            }
            lblCombo.color = end;
            lblCombo.text = "";
            lblx.color = end;
            lblx.text = "";
        }*/

        /// <summary>
        /// Animacion para que aparezca el combo de 0 a 0.5
        /// </summary>
        /// <returns></returns>
        /*IEnumerator ComboFadeIn()
        {
            float duration = 0.20f;
            float t = 0f;
            Color start = lblCombo.color;
            Color end = new Color(start.r, start.g, start.b, 1f);
            while (t < duration)
            {
                t += Time.deltaTime;
                float lerp = t / duration;
                lblCombo.color = Color.Lerp(start, end, Mathf.SmoothStep(0f, 1f, lerp));
                lblx.color = Color.Lerp(start, end, Mathf.SmoothStep(0f, 1f, lerp));
                yield return null;
            }
            lblCombo.color = end;
            lblx.color = end;
        }*/
       
        public void BindAutoFontSize(Label label, VisualElement container, float percentage)
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
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}

