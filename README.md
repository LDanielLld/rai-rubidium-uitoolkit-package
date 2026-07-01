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

### 2. Añadir interfaz a la escena

- Crea un GameObject y nombralo **GUIManager**
- Añade el componente `UIDocument`para renderizar la interfaz junto a sus componentes de la libreria
  1. En Panel Settings se vincula el asset: GUIToolkit/Runtime/Shared/PanelSettings/UISettings.asset
  2. En Source Asset se incorpora el script de plantilla con el diseño planteado: UI/UIInterface.uxml


### 5. Prefabs de particulas

Crea tres Prefabs de ParticleSystem en `Assets/Resources/Prefabs/:

| Prefab | Shader recomendado |
|---|---|
| Rocket.prefab | Mobile/Particles/Additive |
| Trail.prefab | Mobile/Particles/Additive |
| Explosion.prefab | Mobile/Particles/Additive |

Asi�gnalos en el Inspector del componente **FireworksRT**.

## Uso

| Accion | Resultado |
|---|---|
| Clic en el cielo | Lanza un cohete hacia ese punto |
| Slider Particulas | Numero de chispas por explosion |
| Slider Gravedad | Intensidad de la caída |
| Slider Velocidad | Fuerza de lanzamiento |
| Slider Dispersion | Apertura lateral de los cohetes |
| Slider Cadencia | Tiempo entre lanzamientos automaticos |
| Slider Cola | Duracion de la estela del cohete |
| Aleatorio / Dorado / Arcoiris | Paleta de colores |
| Modo Automatico | Lanza cohetes solos |
| Limpiar | Borra todo de la pantalla |

## Estructura del package

```
GUIToolkit/
└── Runtime/                         # Carpeta con los datos brutos de BCI2000. Debe llamarse _data_
    ├── Shared/
    │   ├── Fonts/o
    │   ├── PanelSettings/
    │   │   └── Themes/
    │   │       └── ThemesStyles/
    │   ├── Styles/
    │   └── Utilities/
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
        │   ├── StringIcon/
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
