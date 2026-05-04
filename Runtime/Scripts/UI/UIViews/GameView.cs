using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIInterface
{
    public class GameView : UIView
    {

        private enum StateGameView
        {
            SSV_INITIALIZE, //Inicializa visualemente la etiqueta
            SSV_COUNTDOWN, //Cuenta atras,
            SSV_STARTING, //Señal de empezar
            SSV_ANIMATE_STARTING //Aanimacion de cambio
        }
        private StateGameView cState = StateGameView.SSV_COUNTDOWN; //Empieza con la cuenta atras

        // Elementos visuales
        VisualElement mPanel; //Panel de secuencia
        Label mSequence; //Secuencia atrás



        [SerializeField] VisualElement mDisplayScore;
        [SerializeField] VisualElement mRepetitionStat;
        [SerializeField] VisualElement mTimeCounter;
   

        //Componentes de la interfaz de juego
        private ScoreDisplay scoreDisplay;
        private TimeProgress barTime;
        private TimeProgress radialTime;
        private Stat repetitionStat;
        private TimeCounter timeCounter;


        //Contador de tiempo       
        private float timerTotalGame;
        private int SEC_TO_START = 3;
        private float timer = 0.0f; //Cada segundo cambia la etiqueta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topElement"></param>
        public GameView(VisualElement topElement) : base(topElement)
        {
            //UIEvents.initCosas += InitShinkLabel;

        }

        public override void Dispose()
        {
            base.Dispose();
            //UIEvents.initCosas -= InitShinkLabel;
        }


        private float timeTotal = 0f;
        private float points = 0.0f;
        float combomax = 3.0f;
        float combo = 1.0f;

        private double timeTotalRefresh = 10000;
        private float timeRefresh = 0f;

        int rep = 0;
        public override bool Update(float deltaTime)
        {
            

            timeTotal += deltaTime;
            timeRefresh += deltaTime;
            timeTotalRefresh -= deltaTime;


            if (timeTotal > 2)
            {
                points += 100;
                combo += 0.15f;
                rep++;
                scoreDisplay.UpdateScore((int)points);

             //  repetitionStat.Update(rep);
                

                if (combo < combomax)
                    scoreDisplay.UpdateCombo(combo, combomax);
                else
                {
                    combo = 0;
                    scoreDisplay.UpdateCombo(combo, combomax);
                }             

                timeTotal = 0;
            }

            //Actualiza barras de tiempo
            float m = -100f/2f;
            float progress = timeTotal * m + 100;
            barTime.progress = progress;
            radialTime.progress = progress;


            // if (timeRefresh > 0.10)
            //{
            //    timeCounter.Update(timeTotalRefresh);// TickReloj();

            //    timeRefresh = 0;
            //}
            repetitionStat.Update(rep, deltaTime);
            timeCounter.Update(timeTotalRefresh, deltaTime);// TickReloj()

            if (Input.GetKeyDown(KeyCode.Space))
            {
                timeTotalRefresh += 61.0;                
            }

            

            //   timeRefresh = 0;
            //}

            bool state = false;
            return state;
        }

        private void UpdateVisuals()
        {
            if (SEC_TO_START > 0)
            {
               // mSequence.text = SEC_TO_START > 0 ? SEC_TO_START.ToString() : "¡EMPIEZA!";

                // Efecto de pulso usando una corrutina rápida
                //mSequence.AddToClassList(anPulseText);
                //mSequence.schedule.Execute(() => mSequence.RemoveFromClassList(anPulseText)).StartingIn(150);
            }

        }

        /// <summary>
        /// Actualiza el metodo de mostrar componente, para añadir animacion
        /// </summary>
        public override void Show()
        {
            base.Show();

            //Lanza animacion de aparicion
            mPanel.schedule.Execute(() => {                
                mPanel.AddToClassList("view-visible");
            }).StartingIn(10);

        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            mPanel = mTopElement.Q<VisualElement>("game__container");
            /*mSequence = mTopElement.Q<Label>("countdown-sequence");

            mSequence.text = $"{SEC_TO_START}";*/


            mDisplayScore = mTopElement.Q<VisualElement>("ScoreDisplay");
            scoreDisplay = new ScoreDisplay(mDisplayScore);
            scoreDisplay.Init(2000);

            //Barras de tiempo
            barTime = mTopElement.Q<TimeProgress>("BarTime");
            radialTime = mTopElement.Q<TimeProgress>("RadialTime");

            //Caractaristica
            mRepetitionStat = mTopElement.Q<VisualElement>("StatView");
            repetitionStat = new Stat(mRepetitionStat, TypeStat.SECUENTIAL, TypeAnim.FADE_SLIDE);
            repetitionStat.Init(0, 100);

            //Contador de tiempo            
            mTimeCounter = mTopElement.Q<VisualElement>("TimeView");
            timeCounter = new TimeCounter(mTimeCounter);
            timeCounter.Init(timeTotalRefresh, 100);
        }


        // De-select a specific tab
        public void InitShinkLabel()
        {
           /* mPanel.AddToClassList(anExpandPanel);
            mSequence.RemoveFromClassList(anShringText);*/
        }

    }
}