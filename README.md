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
https://github.com/tuusuario/mi-fireworks-package.git#1.0.0

## Configuracion inicial

### 1. Crear la capa Fireworks

- Edit Project Settings  Tags and Layers**
- En **User Layer 6** escribe `Fireworks`

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


Runtime/
FireworksRT.cs          Script principal
FireworksUI.uxml        Layout del panel de controles
FireworksUI.uss         Estilos del panel
FireworksRT.asmdef      Assembly Definition

## Licencia

MIT License, consulta el archivo LICENSE para mas detalles.
