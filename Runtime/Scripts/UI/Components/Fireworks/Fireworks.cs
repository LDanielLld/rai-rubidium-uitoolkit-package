using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// ================================================================
//  FireworksRT.cs  -  Fuegos artificiales con RenderTexture
//  Unity 2022.3.15f1
//
//  Arquitectura:
//    - uGUI Canvas (Sort Order 0) con RawImage -> muestra la RenderTexture
//    - UIDocument  (Sort Order 1) -> panel de controles transparente por encima
//    - Camara dedicada -> renderiza ParticleSystems a la RenderTexture
//
//  CONFIGURACION EN UNITY:
//  1. Edit -> Project Settings -> Tags and Layers
//       . Crear User Layer 6: "Fireworks"//       
//  2. Crear GameObject "FireworksManager"
//       . Anadir componente UIDocument -> Source Asset = FireworksUI.uxml
//       . Anadir este script
//  3. En el Panel Settings del UIDocument:
//       . Scale Mode: Scale With Screen Size
//       . Sort Order: 1   <- importante, por encima del Canvas uGUI
//  4. En la camara principal (Main Camera):
//       . Culling Mask: desmarcar la capa "Fireworks"
//  5. Play y hacer clic en el cielo
// ================================================================


/// <summary>
/// Elemento encargada de gestionar todo lo relacionado con la generacion
/// de fuegos artificiales
/// </summary>
public class Fireworks
{  
    // ------ Capa exclusiva de particulas --------------------------------
    // Crea en Edit -> Project Settings -> Tags and Layers una capa
    // llamada "Fireworks" y pon su indice aqui (por defecto la 6).
    [Tooltip("Indice de la capa 'Fireworks' (crear en Tags and Layers)")]
    public int fireworksLayer = 6;

    // ------ Referencias creadas en runtime ------------------------------
    private Camera _fwCamera;
    private RenderTexture _rt;
    private VisualElement _rtBackground;

    // ------ Parametros --------------------------------------------------
    private bool isConnected = false;

    // ------ Sistema de particulas para simular fuegos artificiales ------
    private GameObject fireworkInstance;
    private string k_FireworkPath = "Fireworks/FireworkPrefab";    
    private ParticleSystem fireworks;

    [SerializeField]    
    private VisualElement template; //Plantilla - Elemento central



    //****************************Inicialización y cierre******************************//
    //*********************************************************************************//
    #region [Function] Constructor
    /// <summary>
    /// Constructor de la clase
    /// </summary>
    /// <param name="tmpl"></param>
    public Fireworks(VisualElement tmpl)
    {
        template = tmpl;       

        SetupRenderTexture();
        SetVisualElements();
        SetupParticleSystem();
    }
    #endregion


    #region [Function] Componentes visuales
    /// <summary>
    /// Genera el render texture y la camara dedicada para la visualizacion
    /// </summary>
    private void SetupRenderTexture()
    {
        // Crear RenderTexture con alpha
        _rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        _rt.antiAliasing = 2;
        _rt.Create();

        // Crear camara dedicada que solo ve la capa "Fireworks"
        var camGO = new GameObject("FireworksCamera");
        _fwCamera = camGO.AddComponent<Camera>();
        _fwCamera.orthographic = true;
        _fwCamera.orthographicSize = 5f;
        _fwCamera.clearFlags = CameraClearFlags.SolidColor;
        _fwCamera.backgroundColor = new Color(0, 0, 0, 0);
        _fwCamera.cullingMask = 1 << fireworksLayer;
        _fwCamera.targetTexture = _rt;
        _fwCamera.depth = -10;
        _fwCamera.nearClipPlane = 0.1f;
        _fwCamera.farClipPlane = 100f;
        camGO.transform.position = new Vector3(0, 0, -10f);       
    }

    /// <summary>
    /// Registra los componentes del fichero UXML
    /// </summary>
    /// <param name="shopItemElement"></param>
    private void SetVisualElements()
    {
        //Panel donde se mostrarán los fuegos artificiales
        _rtBackground = template.Q<VisualElement>("rt-background");
        if (_rtBackground == null)
        {
            Debug.LogError("[FireworksRT] No se encontro 'rt-background' en el UXML.");
            return;
        }

        // Usar generateVisualContent para redibujar la RT cada frame.
        // Es la unica forma fiable en UI Toolkit de mostrar una RenderTexture
        // que cambia continuamente sin crear artefactos de cache.
        _rtBackground.generateVisualContent += OnGenerateVisualContent;

        // Cuando el panel cambia de tamaño, ajustar la RT
        _rtBackground.RegisterCallback<GeometryChangedEvent>(OnRTBackgroundGeometryChanged);
    }

    /// <summary>
    /// Referencia el sistema de particulas que se mostrará
    /// </summary>
    void SetupParticleSystem()
    {
        // 1. Instanciar el prefab en la posición deseada
        GameObject fireworksPrefab = Resources.Load<GameObject>(k_FireworkPath);
        fireworkInstance = Object.Instantiate(fireworksPrefab);

        fireworkInstance.transform.position = new Vector3(0f, -5f, 0f); //Offset hacia abajo

        //Establece el layer de fuegos artificales
        fireworkInstance.layer = fireworksLayer;
        foreach (Transform children in fireworkInstance.transform)
        {
            children.gameObject.layer = fireworksLayer;
            foreach (Transform sub in children)
                sub.gameObject.layer = fireworksLayer;
        }

        // 2. Obtener el componente Particle System de la instancia
        fireworks = fireworkInstance.GetComponent<ParticleSystem>();

        //3. Desconecta el componente
        if (fireworks != null)
            fireworks.Stop();
    }
    #endregion


    #region [Function] Liberacion de memoria
    /// <summary>
    /// Libera memoria y elimina componentes
    /// </summary>
    /// <param name="tmpl"></param>
    public void Dispose()
    {
        if (_rt != null) { _rt.Release(); Object.Destroy(_rt); }
        if (fireworks != null) { Object.Destroy(fireworks.gameObject); }
    }
    #endregion
    //*********************************************************************************//
    //*********************************************************************************//



    //**********************Actualizacion del componente visual************************//
    //*********************************************************************************//
    #region [Function] Actualizacion general
    /// <summary>
    /// Actualiza el componente para que se visualice en el panel UIToolkit
    /// </summary>
    public void Update()
    {
        if(isConnected)
            // Forzar redibujado del elemento para que OnGenerateVisualContent
            // se llame cada frame y la RT se vea actualizada
            _rtBackground?.MarkDirtyRepaint();
    }
    #endregion

    #region [Function] Eventos
    /// <summary>
    /// Evento de cambio de geometria para actualizar al cambiar el tamaño del componente
    /// </summary>
    /// <param name="evt"></param>      
    private void OnRTBackgroundGeometryChanged(GeometryChangedEvent evt)
    {
        int w = (int)_rtBackground.resolvedStyle.width;
        int h = (int)_rtBackground.resolvedStyle.height;
        if (w < 8 || h < 8) return;
        if (w == _rt.width && h == _rt.height) return;

        _rt.Release();
        _rt.width = w;
        _rt.height = h;
        _rt.Create();

        _fwCamera.targetTexture = _rt;
        _fwCamera.orthographicSize = 5f;
        _fwCamera.aspect = (float)w / h;
    }

    /// <summary>
    /// Este callback se llama automaticamente cada frame gracias a MarkDirtyRepaint().
    /// Es la forma de mostrar una RenderTexture que cambia continuamente dentro de un VisualElement.
    /// </summary>
    /// <param name="ctx"></param>
    private void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        if (_rt == null || !_rt.IsCreated()) return;

        float w = _rtBackground.resolvedStyle.width;
        float h = _rtBackground.resolvedStyle.height;
        if (w < 1f || h < 1f) return;

        // Allocar mesh de 2 triangulos (un quad) para mostrar la RT
        var mesh = ctx.Allocate(4, 6, _rt);

        // UV: en UI Toolkit la Y de UV esta invertida respecto a la RT
        mesh.SetNextVertex(new Vertex { position = new Vector3(0, 0, Vertex.nearZ), uv = new Vector2(0, 1), tint = Color.white });
        mesh.SetNextVertex(new Vertex { position = new Vector3(w, 0, Vertex.nearZ), uv = new Vector2(1, 1), tint = Color.white });
        mesh.SetNextVertex(new Vertex { position = new Vector3(w, h, Vertex.nearZ), uv = new Vector2(1, 0), tint = Color.white });
        mesh.SetNextVertex(new Vertex { position = new Vector3(0, h, Vertex.nearZ), uv = new Vector2(0, 0), tint = Color.white });

        mesh.SetNextIndex(0); mesh.SetNextIndex(1); mesh.SetNextIndex(2);
        mesh.SetNextIndex(0); mesh.SetNextIndex(2); mesh.SetNextIndex(3);
    }

    #endregion
    //*********************************************************************************//
    //*********************************************************************************//


    //*************************Gestion de conexion/desconexion*************************//
    //*********************************************************************************//
    #region [Function] Conexion/Desconexion

    /// <summary>
    /// Conecta la simulacion de fuegos artificiales
    /// </summary>    
    public void Connect()
    {
        if (fireworks != null) fireworks.Play();
        isConnected = true;
    }

    /// <summary>
    /// Para los fuegos artificiales
    /// </summary>
    /// <param name="t"></param>
    public void Disconnect()
    {
        if (fireworks != null) fireworks.Stop();
        isConnected = false;
    }
    #endregion
    //*********************************************************************************//
    //*********************************************************************************//
}



