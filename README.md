# Interface UI Toolkit

Paquete de configuracion de interfaz grafica de usuario para todos los juegos del sistema REVIRE-Rubidium. Contiene todos los componentes visuales, estilos, plantillas, sprites y prefabs necesarios para construir la UI de cada juego, organizados de forma modular para que cada proyecto solo conecte los componentes que necesita.

## Caracteristicas

El paquete esta organizado en componentes independientes. Cada componente tiene su propio conjunto de archivos y puede integrarse de forma individual en cualquier juego del sistema sin necesidad de importar el resto.

- Archivo de codigo(.cs): Logica y comportamiento del componente.
- Archivo de plantilla(.uxml): Estructura visual del elemento.
- Archivo de estilos(.uss): Apariencia y temas visuales.
- Sprites: Iconos y graficos asociados al componente.
- Materiales: Envoltorio visual del componente dentro del juego
- Prefabs: Configuraciones de ParticleSystem u otros elementos de Unity listos para usar.



## Requisitos

- Unity 2022.3.15f1 o superior
- UI Toolkit habilitado en el proyecto
- Universal Render Pipeline (URP) o Built-in Render Pipeline

## Instalacion Via Git URL

1. Abre Window->Package Manager
2. Clic en + y Add package from git URL...
3. Introduce:
https://github.com/RAI-UMH/rai-rubidium-games-score.git

## Configuracion inicial

### 1. Inicializacion de componentes

- En el proyecto de Unity se crea la carpeta Assets/UI donde se incorporan todos los componentes
  que se van a utilizar en la interfaz de la actividad implementada
  > [!NOTE]
  > Cada actividad puede tener diferentes componentes. Depende del diseño y los objetivos del juego.
- Copia los archivos de estilo de los elementos y la interfaz general desde los siguientes directorios:
  1. El archivo UIInterface.uxml representa toda la interaz del sistema y se copia desde GUIToolkit/Runtime/UIViews/.
     Este elemento recolecta los bloques de interfaz que se mostrarán en los diferentes estados del juego: Inicio, juego, pausa, warning y finalizacion.     
  2. La unica vista que hay que modificar de UIInterface.uxml es la que se muestra durente la fase de juego. Por lo tanto,
     se añade GameView.cs y GameView.uxml.
  3. Los componentes individuales se extraen desde sus respectivas carpetas en GUIToolkit/Runtime/UIViews/Components. 
     Estos elementos se utilizarán para configurar el fichero GameView.uxml
- Se colocan en la carpeta Assets para tener estos componentes visibles en el UIBuilder.

### 2. Preparacion de componentes

- En el UIBuilder se configura el archivo GameView.uxml añadiendo componentes visuales de la libreria.
- Incorpora la vista GameView.uxml dentro del archivo de plantilla UIInterface.uxml tambien desde el UIBuilder
  1. Establece `Size` a 100% en `Width` y `Height`.
  2. Establece `Display` en **None**. 
- Modifica el archivo GameView.cs para inicializar y configurar los componentes, asi como registrar los eventos para la actualización de estos componentes visuales.

### 3. Añadir interfaz a la escena

- Crea un GameObject y nombralo **GUIManager**
- Añade el componente `UIDocument`para renderizar la interfaz junto a sus componentes de la libreria
  1. En Panel Settings se vincula el asset: GUIToolkit/Runtime/Shared/PanelSettings/UISettings.asset
  2. En Source Asset se incorpora el script de plantilla con el diseño planteado: UI/UIInterface.uxml

### 5. Uso y actualización de componentes

Los componentes se utilizan y actualizan desde cualquier script dentro del juego, haciendo uso de los eventos registrados en GameView.
  - Ejemplo: UIEvents.SoundEndStartView?.Invoke();
En la siguiente tabla se muestran los eventos que se pueden registrar: 

| Componente | Evento | Resultado |
|---|---|---|
| FpsCounter   | FpsCounterToggled     | Actualiza la puntuacion general |
|              | TargetFrameRateSet    | Numero de chispas por explosion |
| ScoreDisplay | ScoreDisplayScore     | Actualiza la puntuacion general del componente |
|              | ScoreDisplayHighScore | Establece la puntuacion maxima |
|              | ScoreDisplayCombo     | Establece condiciones de combo |
| Stat         | StatEvent             | Incrementa o decrementa valores numéricos |
| TimeProgress | StateTimeProgress     | Conecta o desconecta el componente |
|              | GetTimeProgress       | Obtiene el tiempo actual de progreso |
| SpeedMeter   | SpeedMeterEvent       | Establece la velocidad |
| StartView    | SoundInitStartView    | Lanza sonido en cada cambio de la cuenta atras|
|              | SoundInitStartView    | Lanza sonido final de la cuenta atras |
| FinishView   | FillPanelScore        | Rellena el panel de puntuación |
|              | SoundPanelScore       | Genera un sonuido al aparecer cada registro |
|              | FireworksPanelScore   | Activa los fuegos artificiales |

## Estructura del package

```
GUIToolkit/
└── Runtime/                         # Carpeta con todos los componentes de la libreria GUIToolkit
    ├── Shared/                      # Elemnentos que pueden compartir parte de los componentes
    │   ├── Fonts/                   # Fuentes de texto     
    │   ├── PanelSettings/           # Configuracion de la interfaz
    │   │   └── Themes/              # Tema de la interfaz
    │   │       └── ThemesStyles/    # Estilo del tema de la interfaz
    │   ├── Styles/                  # Estilos compartidos
    │   └── Utilities/               # Utilidades donde se procesan diferentes eventos
    └── UIViews
        ├── Components/              # Componentes que forman parte de las vistas de estado
        │   ├── Digit/               # Control de digitos numericos
        │   ├── Fireworks/           # Componente que simula unos fuegos artificiales 
        │   │   ├── Materials/               
        │   │   ├── Prefabs/         
        │   │   └── Textures/                  
        │   │       ├── Point/                 
        │   │       └── Trail/                 
        │   ├── ScoreDisplay/        # Marcador de puntuación con combos de bonificación
        │   │   └── Textures/                  
        │   ├── ScoreRow/            # Cada una de las marcas que aparecen en la tabla de puntuacion 
        │   │   └── Textures/         
        │   ├── SpeedMeter/          # Velocimetro animado
        │   │   └── Textures/        
        │   ├── Stat/                # Marcador con alguna caracteristica seleccionable
        │   ├── StringIcon/          #  
        │   ├── TimeCounter/         # Contador de tiempo
        │   ├── TimeProgress/        # Barra de progresion lineal o radial
        │   └── TitleComponent/      # Componente de titulo
        └── Views/                   # Vista de los estados correspondientes
            ├── FinishView/          # Vista del estado "Final" del juego            
            ├── GameView/            # Vista del estado "Jugando" del juego            
            ├── PauseView/           # Vista del estado "Pausa" del juego            
            ├── StartView/           # Vista del estado "Inicio" del juego
            └── WarningView/         # Vista del estado "Warning" del juego            
```

## Licencia

MIT License, consulta el archivo LICENSE para mas detalles.
