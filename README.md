# Detalles del proyecto
- Es el que se encarga de recibir/enviar mensajes tanto de/a Arduino (por el puerto serie) como de las pulseras (a través de mqtt) para gestionar la lógica de la experienca
- El proyecto tiene una única escena "Main" (en UNITY_PROJECT/Makers_Aehter/Assets/Scenes)
- Se ha planteado una máquina de estados para desarrollar el flujo de la experiencia
- En este proyecto se reproducen los sonidos correspondientes a cada estado de la experiencia

Los elementos que encontramos en la instalación son: 
- ARDUINO// LEDs en las aristas, (x2 tiras leds de cada zona de juego), que indican el progreso del juego. Se controlan desde la máquina de estados
- ARDUINO// LEDs gestos, al lado de las aristas de los LEDs de progreso. Sirven para indicar el gesto que debe realizar el usuario en cada momento. Se controla desde la máquina de estados. 
- ARDUINO // LEDs feedback caja zona pulseras. Hay dos tiras de LEDs en cada zona de juego, que se encienden y apagan con un pulsador situado en la misma zona. Esto se controla desde Arduino directamente.
- ARDUINO // LEDs signos, en el medio de la estructura. Se controla desde la máquina de estados.

  ![Aether_elements](https://github.com/user-attachments/assets/017fecb8-7445-4839-8959-a48ba35a6336)


# Escena "Main"
## GameObjects en la escena 
----- TODO -------
## Máquina de estados 
[Aether_MakersPlus_StateMachine_UnityProject.pdf](https://github.com/user-attachments/files/20062858/Aether_MakersPlus_StateMachine_UnityProject.pdf)

A continuación se detallan los estados de la aplicación. Los scripts de cada estado están en: [UNITY_PROJECT/Makers_Aehter/Assets/Scripts/States/](https://github.com/MakersPlusBCN/aether-brain/tree/a03d67ad356bb02a4f93e90b83104a0131c3e57c/UNITY_PROJECT/Makers_Aehter/Assets/Scripts/States)

### Call2Action
#### Condiciones para acceder a este estado
La experiencia estará en este estado cuando no hayan participantes interactuando. Esto sucede cuando detectamos que los dos pulsadores de la zona de las pulseras estan pulsados. 

#### Luces 
Las luces de la la zona de las pulseras parpadea, de forma sutil, para llamar la atención en insitar a los participantes a que se acerquen a la zona de las pulseras para empezar la experiencia. 
El resto de luces están apagadas 

#### Sonido

#### Pulseras 
En este estado no se reciben los mensajes MQTT de las pulseras. 

#### Siguiente estado 
La condición para pasar al estado "OneUserReady", es que uno de los dos pulsadores de la zona de las pulseras se desactive, eso significa que un usuario a iniciado la experiencia. 

### OneUserReady
----- TODO -------
#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### AllUsersReady
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### SetNextPhase
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### SyncGesture
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### GestureCompleted 
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### GestureFailed 
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### RestartPhase
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

### EndExperience 
----- TODO -------

#### Condiciones para acceder a este estado

#### Luces 

#### Sonido

#### Pulseras 

#### Siguiente estado 

# Comunicación con Arduino 

Serial Port Communication

Mensajes control LEDs zona superior de la estructura: 
- "W" -> Activa el símbolo del agua - Water - 
- "A" -> Activa el símbolo del aire - Air - 
- "F" -> Activa el símbolo del fuego - Fire - 
- "E" -> Activa el símbolo de la tierra - Earth -


Otros mensajes: 
- "+" -> Avanzar un nivel en la barra de progreso
- "-" -> Disminuir un nivel en la barra de progreso
- "0" -> Para la animación del gesto 
- "O" -> Apaga las barras de progreso
- "Q" -> Apaga indicador de gestos
- "H", "V", "D" -> Encender LEDs gestos (Horizontal, Vertical, Diagonal) 
- "X" -> Activa el efecto final 
- "I" -> ??
- Números del 1 al 8 (fragmentos de la barra de progeso). Enviar primero "O" (para apagar barras), y despues numéro de segmentos que se quiere en cada nivel 





