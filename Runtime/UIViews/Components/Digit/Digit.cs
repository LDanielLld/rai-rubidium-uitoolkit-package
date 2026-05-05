using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    /// <summary>
    /// Clase que representa cada uno de los digitos que se utilizan en el contador
    /// </summary>
    public class Digit
    {
        //************************************Variables************************************//
        //*********************************************************************************//
        #region [Variables] Atributos del digito
        //Etiquetas reales
        private Label mLabelA;
        private Label mLabelB;

        //Etiquetas referenciadas para su distribución
        Label refVisible;
        Label refHidden;

        //Valores numericos
        private float timec = 0;
        private int cValue = 0; //Valor actual del digito      

        //Tipo de cuenta
        private TypeTimeline type;

        //Tipo de animacion
        private TypeAnim typeAnim;

        //Constantes de animacion - Odometro
        private string kAnimUp = "__text-up"; //Texto va hacia arriba
        private string kAnimCenter = "__text-center"; ////Texto va hacia el centro
        private string kAnimDown = "__text-down"; ////Texto va hacia abajo
        private string kNoAnim = "__text-no-animation"; //Bloquea animaciones

        //Constantes de animaicion Fade&Slide
        private string kAnimUpSlide = "__text-slide-up"; //Texto va hacia arriba y desaparece
        private string kAnimDownSlide = "__text-slide-down";//Texto va hacia abajo y desaparece

        //Diferentes estados del digito para controlar las animacion (evita que haya saltos
        //imprevistos y solapamientos)
        private enum StateDigit
        {
            CHECK_NUMBER_CHANGE, //Comprueba cuando cambia el numero
            COUNT, //Realiza la animacion de cambio de numeros
            ANIM, //Gestiona los tiempos de la animacion
            RESET //Tiempo para reiniciar animacion
        }

        //Esto del flujo de trabajo del digito
        private StateDigit state = StateDigit.CHECK_NUMBER_CHANGE;

        //Umbral de animacion
        private float thresholdAnim;
        public float Threshold
        {
            get => thresholdAnim;
            set => thresholdAnim = value;
        }

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
        public Digit(Label a, Label b, string comp)
        {
            //Actualiza los estilos
            kAnimUp = comp + kAnimUp;
            kAnimCenter = comp + kAnimCenter;
            kAnimDown = comp + kAnimDown;
            kNoAnim = comp + kNoAnim;

            kAnimUpSlide = comp + kAnimUpSlide;
            kAnimDownSlide = comp + kAnimDownSlide;

            // Copia las etiquetas
            mLabelA = a;
            mLabelB = b;

            //Referencias para el intercambio de etiquetas
            refVisible = mLabelA;
            refHidden = mLabelB;
        }
        #endregion

        #region [Function] Componente visual
        /// <summary>
        /// Inicializa el componentes para la animacion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tpe"></param>
        /// <param name="tpeanim"></param>
        public void Init(int value, TypeTimeline tpe, TypeAnim tpeanim)
        {
            refVisible.text = value.ToString();
            cValue = value;

            type = tpe;
            typeAnim = tpeanim;

            if (typeAnim == TypeAnim.ROLLING_DRUM)
                Teleport(refHidden); //Desplaza las etiquetas iniciales        
            else
            {
                // Desplaza las etiquetas iniciales
                TeleportCenter(refHidden);
                refHidden.text = cValue.ToString("0");
                refHidden.RemoveFromClassList(kNoAnim);
                refHidden.RemoveFromClassList(kAnimDown); //Se quita esta, que es cuando inicia
            }
            refHidden.RemoveFromClassList(kNoAnim);
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//



        //*************************Actualizacion de los digitos****************************//
        //*********************************************************************************//
        #region [Function] Actualización 
        /// <summary>
        /// Lo establece teniendo una maquina de estados para controlar los tiempos de
        /// animacion de manera correcta
        /// </summary>
        /// <param name="newNumber"></param>      
        public void Set(int newNumber, float deltaTime)
        {
            if (typeAnim == TypeAnim.ROLLING_DRUM)
                AnimationOdometer(newNumber, deltaTime);
            else if (typeAnim == TypeAnim.FADE_SLIDE)
                AnimationFadeSlide(newNumber, deltaTime);
        }

        /// <summary>
        /// Lanza la animacion parecida a un cuenta kilometros
        /// </summary>
        /// <param name="newNumber"></param>
        /// <param name="deltaTime"></param>
        public void AnimationOdometer(int newNumber, float deltaTime)
        {

            //Comprueba el estado actual del digito. Permite realizar una animacion completa
            //sin que se solape con la anterior
            if (state == StateDigit.CHECK_NUMBER_CHANGE)
            {
                if (newNumber != cValue)
                {
                    cValue = newNumber;
                    state = StateDigit.COUNT;
                }
            }
            else if (state == StateDigit.COUNT)
            {
                refHidden.text = cValue.ToString();

                if (type == TypeTimeline.FORWARD)
                {
                    Move(refVisible, kAnimUp);
                    Move(refHidden, kAnimCenter);
                }
                else if (type == TypeTimeline.REGRESIVE)
                {
                    Move(refVisible, kAnimDown);
                    Move(refHidden, kAnimCenter);
                }

                state = StateDigit.ANIM; //Espera a que finalice la animacion
            }
            else if (state == StateDigit.ANIM)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.01)
                {
                    Teleport(refVisible); //Transporta la etiqueta A, hacia abajo     
                    state = StateDigit.RESET;
                    timec = 0;
                }
            }
            else if (state == StateDigit.RESET)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.03) //Da un pequeño tiempo para reiniciar
                {
                    refVisible.RemoveFromClassList(kNoAnim);
                    state = StateDigit.CHECK_NUMBER_CHANGE;
                    timec = 0;

                    //Intercambia las referencias
                    ExChangeLabel();
                }
            }
        }


        /// <summary>
        /// Animacion Fade&Slide teniendo en cuenta la maquina de estados que control los
        /// tiempos
        /// </summary>
        /// <param name="newNumber"></param>
        /// <param name="deltaTime"></param>
        public void AnimationFadeSlide(int newNumber, float deltaTime)
        {
            //Comprueba el estado actual del digito. Permite realizar una animacion completa
            //sin que se solape con la anterior
            if (state == StateDigit.CHECK_NUMBER_CHANGE)
            {
                if (newNumber != cValue)
                {
                    refHidden.text = cValue.ToString();
                    cValue = newNumber;
                    state = StateDigit.COUNT;
                }
            }
            else if (state == StateDigit.COUNT)
            {
                if (type == TypeTimeline.FORWARD)
                    MoveFadeSlide(refHidden, kAnimUpSlide); //Mueve solo el saliente                                    
                else if (type == TypeTimeline.REGRESIVE)
                    MoveFadeSlide(refHidden, kAnimDownSlide);

                state = StateDigit.ANIM; //Espera a que finalice la animacion
            }
            else if (state == StateDigit.ANIM)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.01)
                {
                    TeleportCenter(refHidden); //Transporta la etiqueta que se ha ido al centro, hacia abajo     
                    state = StateDigit.RESET;
                    timec = 0;

                    refVisible.text = cValue.ToString();
                    refHidden.text = cValue.ToString();

                }
            }
            else if (state == StateDigit.RESET)
            {
                timec += deltaTime;
                if (timec > thresholdAnim + 0.03) //Da un pequeño tiempo para reiniciar
                {
                    refHidden.RemoveFromClassList(kNoAnim);
                    state = StateDigit.CHECK_NUMBER_CHANGE;
                    timec = 0;

                }
            }
        }
        #endregion


        #region [Function] Gestion de la actualizacion
        /// <summary>
        /// Intercambia las etiquetas para el control de la animacion
        /// </summary>
        private void ExChangeLabel()
        {
            if (refVisible == mLabelA)
            {
                refVisible = mLabelB;
                refHidden = mLabelA;
            }
            else
            {
                refVisible = mLabelA;
                refHidden = mLabelB;
            }
        }


        /// <summary>
        /// Mueve la etiqueta a la posicion deseada realizando una animacion
        /// </summary>
        /// <param name="label"></param>
        /// <param name="animation"></param>
        private void Move(Label label, string animation)
        {
            label.RemoveFromClassList(kAnimCenter);
            label.RemoveFromClassList(kAnimDown);
            label.RemoveFromClassList(kAnimUp);
            label.AddToClassList(animation);
        }


        private void MoveFadeSlide(Label label, string animation)
        {
            label.RemoveFromClassList(kAnimDownSlide);
            label.RemoveFromClassList(kAnimUpSlide);
            label.AddToClassList(animation);
        }


        /// <summary>
        /// Desplaza la etiqueta a la parte desde donde debe aparecer
        /// </summary>
        /// <param name="label"></param>
        private void Teleport(Label label)
        {
            label.AddToClassList(kNoAnim);

            if (type == TypeTimeline.FORWARD)
                Move(label, kAnimDown);
            else
                Move(label, kAnimUp);
        }

        /// <summary>
        /// Desplaza la etiqueta a la parte desde donde debe aparecer
        /// </summary>
        /// <param name="label"></param>
        private void TeleportCenter(Label label)
        {
            label.AddToClassList(kNoAnim);
            MoveFadeSlide(label, kAnimCenter);
        }
        #endregion
        //*********************************************************************************//
        //*********************************************************************************//
    }
}