using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedMeterView
{
    //************************************Variables************************************//
    //*********************************************************************************//
    #region [Variables] Parametros del velocimetro

    private VisualElement mNeedle; //Aguja de velocidad

    //Limite de giro
    private float[] limitAngle = new float[] { -135f, 135f };

    private float m = 0; //Pendiente de conversion
    private float offset = 0;
    private bool isInit = false;

    //Tiempo para completar todo el circulo de animacion  
    private float duration = 1.0f;
    private float rSpeed = 0; //Rotacion de velocidad actual

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
    public SpeedMeterView(VisualElement tmpl)
    {
        template = tmpl;
        SetVisualElements();
    }
    #endregion

    #region [Function] Componentes visuales
    /// <summary>
    /// Registra los componentes del fichero UXML
    /// </summary>    
    public void SetVisualElements()
    {
        // Busca las partes del elemento ScoreDisplay           
        mNeedle = template.Q<VisualElement>("speedmeter_needle");
    }
    #endregion
    //*********************************************************************************//
    //*********************************************************************************//



    //************************Inicializacion del velocimetro***************************//
    //*********************************************************************************//
    #region Inicializacion
    /// <summary>
    /// Inicializa el velocimetro. Limita el movimiento maximo del velocimetro
    /// con respecto a la velocidad proporcionada
    /// </summary>    
    public void Init(float current, float[] limit)
    {
        m = (limitAngle[1] - limitAngle[0]) / (limit[1] - limit[0]);
        offset = limitAngle[1] - m * limit[1];

        //Obtiene posicion actual
        rSpeed = m * current + offset;

        //Coloca el marcador en esa posicion
        mNeedle.style.rotate = new StyleRotate(new Rotate(rSpeed));

        isInit = true;
    }
    #endregion
    //*********************************************************************************//
    //*********************************************************************************//


    //*************************Actualizacion del velocimetro***************************//
    //*********************************************************************************//
    #region [Function] Actualizacion
    /// <summary>
    /// Actualiza la rotacion en funcion de la velocidad
    /// </summary>
    public void Update(float cspeed)
    {

        if (isInit)
        {
            //Posicion buscada
            float target = m * cspeed + offset;

            //Filta valores para que no sobrepasen los limites
            target = Mathf.Clamp(target, limitAngle[0], limitAngle[1]);

            //Anima el velocimetro
            SpeedMeterAnimation(target);
        }
    }
    #endregion
    //*********************************************************************************//
    //*********************************************************************************//




    //*****************************Gestion de animaciones******************************//
    //*********************************************************************************//   
    #region [Function] Animaciones estaticas 

    /// <summary>
    /// Animacion para mover la aguja de velocidad a un objetivo
    /// </summary>
    /// <param name="timer"></param>
    private void SpeedMeterAnimation(float target)
    {
        //Se rellena la barra en un tiempo 
        float elapsed = 0f;

        //Porcentaje que se encuentra y donde tiene que llegar
        float t_init = Mathf.InverseLerp(limitAngle[0], limitAngle[1], rSpeed);
        float t_end = Mathf.InverseLerp(limitAngle[0], limitAngle[1], target);

        //Duracion de la animacion, por tramo
        float tAnim = Mathf.Abs((t_end - t_init) * duration);       

        //Ejecuta secuencia de animacion 
        var animation = mNeedle.schedule.Execute(() =>
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(t_init, t_end, elapsed / tAnim);

            //Calcula los pasos de la animacion
            float rot = Mathf.Lerp(limitAngle[0], limitAngle[1], t);

            //Aplica el valor
            mNeedle.style.rotate = new StyleRotate(new Rotate(rot));

        }).Every(0); //Se ejecuta cada frame

        animation.Until(() => elapsed >= tAnim);

        //Actualiza posicion actual
        rSpeed = target;
    }
    #endregion  
    //*********************************************************************************//
    //*********************************************************************************//
    
}

