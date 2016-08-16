# PixelPerfect-Camera-Unity2D
A Behavior class for attaching to 'Camera' gameObject and have 'Pixel Perfect' in Unity 2D project.

Si eres desarrollador de videojuegos de la 'vieja' escuela, has usado XNA, MONOGAME, SDL... y quieres programar videojuegos 2D en 
Unity 3D, el primer inconveniente que encontrarás será que Unity es un motor 3D, aún cuando el proyecto sea 2D.
Cuando creas un proyecto 2D, Unity lo que hace es crear un proyecto 3D pero con cámara ortográfica, además de añadir varios componentes para trabajo en 2D, pero el motor del juego sigue siendo 3D.

Esto lo que produce es que nuestro 'sprites' o 'texturas' no se visualicen en pantalla en la posiciones y tamaños que nosotros queramos. Y tampoco tenemos la opción de seleccionar una resolución por defecto (320x200, 640x480...) para nuestro juego y adaptar
el tamaño de los sprites a dicha resolución.

Otro incoveniente que encuentro, si eres un programador de la vieja escuela es que ahora se acabó el fijar los FPS del juego. 
Es decir, antes, al final de cada ciclo de juego, se espera 'x' tiempo hasta que dicho ciclo durase el tiempo que le correspondía
según los FPS para los que se diseñaba. Esto evitaba que en ordenadores muy rápidos el juego fuera a toda leche. Lo que se consigue
es que el juego vaya a la misma velocidad en todos los equipos, lógicamente, si el ordenador es muy lento no hay nada que hacer y
el juego se verá relentizado.
Hoy en día se ha cambiado esta técnica por no utilizar la 'espera' al final de cada ciclo de juego. Lo que se hace es que el juego
va a su máxima velocidad en todos los equipos y el movimiento de los 'Sprites' y demás cálculos se hacen en función del 'deltaTime' o tiempo transcurrido desde el último ciclo de juego hasta el actual. Si sabes que tu sprite se moverá 1 punto en pantalla cada 0.10
segundos, solo tienes que multiplicar '0.10 * deltaTime' para saber el desplazamiento que tendrá en cada ciclo de juego.

Al utilizar el 'deltaTime' en vez de ajustar los Frames del juego provoca que el juego se vea igual de rápido en ordenadores potentes, pero en ordenadores que no dan la talla el movimiento se hace a saltos, como pasa más tiempo entre cada ciclo de juego, la distancia recorrida en cada ciclo es mayor y el sprite pasa de estar en una posición a otra bastante alejada sin que se vea el recorrido intermedio. Este problema casi no es apreciable en juegos 3D, pero en un juego 2D si que se nota.
Por eso para mi gusto, perfiero seguir programando con unos FPS fijos. 
Si el juego va lento pues va lento, como pasó toda la vida, o puede que vaya bien y que en determinado momento se relentice por que hay más sprites, pero se sabe que es por eso y que la máquina no dá para más. En el otro caso el juego parece que va bien pero notas algo raro y el jugador puede pensar que el juego es así siempre, en vez de darse cuenta que su equipo no es suficientemente potente.

Con la clase 'PixelPerfectCamera' se pueden seleccionar los FPS (Frames por segundo) para nuestro juego.
Si empezamos a ver que el juego va lento entonces hay dos opciones, o bajamos los FPS del juego (será más lento y menos suave en los movimientos) o nos toca revisar el código y optimizar nuestro código para ahorrar milisegundos en cada ciclo de juego.

Es la principal diferencia entre programar juegos o aplicaciones, en los juegos hay que pensar que cada acción posiblemente vaya a repetirse en cada ciclo del juego y según como esté programada puede afectar al funcionamiento final del programa.

PixelPerfectCamer.cs:
---------------------
Una clase 'Behavior' para añadir al gameObject de la 'Camara' de la Escena Unity.
Dándole valor a los campos del Behavior desde la escena unity se consigue obtener una resolución de pantalla que coincide con la resolución virtual del juego, es decir, si decidimos desarrollar nuestro juego en una resolución 320x200 el resultado en pantalla del juego será equivalente a cuando antiguamente se cambiaba la resolución de la tarjeta gráfica a 320x200.

El resultado visual es el mismo, lo que pasa es que nuestro dispositivo (pc, móvil...) mantiene su resolución pero la imagen del juego es 'escalada' en cada frame por Unity para que ocupe toda la pantalla.

Se obtiene 'PixelPerfect', es decir, se hace coincidir el tamaño de un punto o pixel de nuestros Sprites con el tamaño de 1 punto o pixel de nuestra 'resolución virtual' en pantalla. Para que se entienda mejor, si nuestra resolución virtual es de 320x200 y usamos en nuestro juego un sprite de ancho 320, al centrarlo en pantalla ocupará todo el ancho de la misma.

NOTA.- Para todas las texturas utilizadas en el juego, el campo 'PixelsPerUnits' debe conservar su valor por defecto '100'.

Con esta idea en cabeza el diseño de juegos se hace como se hacía antiguamente, se diseña el juego para una resolución de pantalla específica (320x200, 640x480 ...) que será nuestra 'resolución virtual' para poder saber de que tamaño deben ser los Sprites del juego o Tiles.

Además provee varios métodos para conversión de posiciones o coordenadas, ya que las posiciones de Unity son de tipo 'float' y las
de nuestra resolución virtual son tipo 'int'.
Si queremos hacer referencia a la posición (10,20) de nuestra 'resolución virtual' 320x200 por ejemplo, la posición 10 en el eje 'x' de unity no coincidirá con el punto/pixel nº10 de nuestra resolución virtual. Por defecto en Unity la posición 10 de nuestra resolución será la posición 0.10 y viceversa.
Utilizando los método de conversión de posiciones, en este caso: 

Vector3 posicion = new Vector3(10,20,0);
Vector3 posicionUnity = Position2DToWorld(posicion);

Conseguimos convertir las posiciones y desarrollar nuestro juego con una metodología 'PixelPerfect'. 

-La clase 'AspectUtility.cs' la saqué de un foro de Unity y la adapté para que utilice los campos que indican la resolución virtual en la clase 'PixelPerfectCamera.cs'. Lo que hace esta clase es evitar que la imagen de nuestro juego se deforme en pantalla manteniendo el aspect ratio de nuestra resolución virtual. Para ello modifica el 'viewPort' de nuestra cámara principal a un tamaño que conserve una relación de aspecto (aspectRatio) igual a nuestra resolución virtual, así nuestro juego no se verá en toda la pantalla, sino solo en un rectángulo cuyas proporciones ancho*alto sean iguales a nuestra resolución virtual. Cuando el motor de Unity muestre nuestro juego en pantalla, lo escalará a las dimensiones del 'viewPort' evitando que se deforme (más ancho o alta) la imagen.
Por último esta clase crea otra cámara con un 'viewPort' que ocupa toda la pantalla y con color de relleno negro. La coloca detrás de la cámara principal en la escena unity y lo que consigue es el efecto de los 2 rectángulos negros en el borde de la pantalla que cubren la superficie sin imagen cuando vemos juegos o películas que no se visualizan en toda la superficie de la pantalla.
