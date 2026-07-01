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

### 1. Inicializacion de los componentes de la interfaz

- En el proyecto de Unity se crea la carpeta Assets/UI donde se incorporan todos los componentes
  que se van a utilizar en la interfaz de la actividad implementada
  > [!NOTE]
  > Cada actividad puede tener diferentes componentes. Depende del diseño y los objetivos del juego.
- Copiar los archivos de estilo de los elementos y la interfaz general desde los siguientes directorios:
  1. El archivo UIInterface.uxml representa toda la interaz del sistema y se copia desde GUIToolkit/Runtime/UIViews/.
     Este elemento recolecta los bloques de interfaz que se mostrarán en los diferentes estados del juego: Inicio, juego, pausa, warning y finalizacion.     
  2. La unica vista que hay que modificar de UIInterface.uxml es la que se muestra durente la fase de juego. Por lo tanto,
     se añade GameView.cs y GameView.uxml.
  3. Los componentes individuales se extraen desde sus respectivas carpetas en GUIToolkit/Runtime/UIViews/Components. 
     Estos elementos se utilizarán para configurar el fichero GameView.uxml
- Se colocan en la carpeta Assets para tener estos componentes visibles en el UIBuilder.

### 2. Crear gestor de la interfaz

- Crea un GameObject con los componentes GUIManager y UIDocument
- UIDocument es el encargado de renderizar la interfaz junto a sus componentes de la libreria
  1. En Panel Settings se vincula el asset: GUIToolkit/Runtime/Shared/PanelSettings/UISettings.asset
  2. En Source Asset se incorpora el script de plantilla con el diseño planteado: UI/UIInterface.uxml

### 2. Crear el tag de explosiones

- En la misma ventana, seccion **Tags**
- Añade el tag `FireworkExplosion`

### 3. Configurar la Main Camera

- Selecciona tu **Main Camera**
- En **Culling Mask**, desmarca la capa `Fireworks`

### 4. Anadir a la escena

- Crea un **GameObject vacio** y nombralo `FireworksManager`
- Anade el componente **UIDocument**
  - Source Asset: `FireworksUI.uxml`
  - Panel Settings: Scale With Screen Size, Sort Order = 1
- Añade el script **FireworksRT**
  - Fireworks Layer: `6`

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
    │   ├── Fonts/
    │   │   ├── *.otf                # Archivo CSV con los datos de los usuarios del estudio
    │   │   └── *.assets             # Archivo CSV con los datos de los usuarios del estudio
    │   ├── PanelSettings/
    │   │   ├── *.assets             # Archivo CSV con los datos de los usuarios del estudio
    │   │   └── Themes/
    │   │       ├── *.tss            # Archivo CSV con los datos de los usuarios del estudio
    │   │       └── ThemesStyles/
    │   │           └── *.uss
    │   ├── Styles/
    │   │   └── *.uss                # Archivo CSV con los datos de los usuarios del estudio
    │   └── Utilities/
    │       └── *.cs                 # Archivo CSV con los datos de los usuarios del estudio
    └── UIViews
        ├── Components/
        │   ├── Digit/
        │   ├── Fireworks/
        │   │   ├── *.uss            # Arhivos de estilo
        │   │   ├── *.uss            # Arhivos de estilo
        │   │   ├── *.uss            # Arhivos de estilo
        │   ├── ScoreDisplay/
        │   │   ├── *.uss            # Arhivos de estilo
        │   ├── ScoreDisplay/
        │   │   ├── *.uss            # Arhivos de estilo
        │   ├── SpeedMeter/
        │   │   ├── *.uss            # Arhivos de estilo
        │   ├── SpeedMeter/
        │   │   ├── *.uss            # Arhivos de estilo
        │   ├── Stat/
        │   ├── StringIcon/
        │   ├── TimeCounter/
        │   ├── TimeProgress/        
        │   └── TitleComponent/             
        └── Views/                   # Vista de los estados correspondientes
            ├── FinishView/          # Vista del estado "Final" del juego
            │   ├── *.uss            # Arhivos de estilo
            │   ├── *.uxml           # Plantilla de componente
            │   └── *.cs             # Comportamiento del componente
            ├── GameView/            # Vista del estado "Jugando" del juego
            │   ├── *.uss            # Arhivos de estilo
            │   ├── *.uxml           # Plantilla de la vista
            │   └── *.cs             # Comportamiento de la vista
            ├── PauseView/           # Vista del estado "Pausa" del juego
            │   ├── *.uss            # Arhivos de estilo
            │   ├── *.uxml           # Plantilla de la vista
            │   └── *.cs             # Comportamiento de la vista
            ├── StartView/           # Vista del estado "Inicio" del juego
            │   ├── *.uss            # Arhivos de estilo
            │   ├── *.uxml           # Plantilla de la vista
            │   └── *.cs             # Comportamiento de la vista
            └── WarningView/         # Vista del estado "Warning" del juego
                ├── *.uss            # Arhivos de estilo
                ├── *.uxml           # Plantilla de la vista
                └── *.cs             # Comportamiento de la vista

├── _out/                           # Almacena las figuras generadas al procesar los datos
│   ├── figures/
│   └── ...
├── log/                            # Almacena archivos log del procesamiento
│   ├── .main_process_dt.log
│   └── .main_process_parfor.log
├── addpath/                           # Scripts de Matlab para el procesamiento
└── workspace.m                     # Script para organizar y ordenar el procesamiento de los datos.
```

Runtime/
FireworksRT.cs          Script principal
FireworksUI.uxml        Layout del panel de controles
FireworksUI.uss         Estilos del panel
FireworksRT.asmdef      Assembly Definition

## Licencia

MIT License, consulta el archivo LICENSE para mas detalles.
