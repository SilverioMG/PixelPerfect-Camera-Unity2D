// Más información sobre este archivo y su uso:
// http://wiki.unity3d.com/index.php?title=AspectRatioEnforcer
//Code Modified by Silverio Martínez García.

using UnityEngine;

public class AspectUtility : MonoBehaviour 
{
	
	public float _wantedAspectRatio; //Variable para mostrar el valor en el 'Inspector' de Unity.
	public float _currentAspectRatio; //Variable para mostrar el valor en el 'Inspector' de Unity.
	static private float wantedAspectRatio;
	static private Camera cam;
	static private Camera backgroundCam;
	public float viewPortWidth = 1f, viewPortHeight = 1f; //Variables para mostrar el valor en el 'Inspector' de Unity.
	
	public void Awake () {
		cam = GetComponent<Camera>();
		if (!cam) {
			cam = Camera.main;
		}
		if (!cam) {
			Debug.LogError ("No camera available");
			return;
		}

		//Se usa la resolucion establecida en la clase 'PixelPerfectCamera' para calcular el Aspect Ratio deseado.
		//La resolucion establecida en 'PixelPerfectCamera' es con la que yo programo el juego (resolucion virtual).
		//Luego esta la resolucion real, que es la que tiene la tarjeta de video del dispositivo donde se ejecuta
		//el juego en ese momento.
		_wantedAspectRatio = (float) PixelPerfectCamera.myWidthResolution / PixelPerfectCamera.myHeightResolution;
		wantedAspectRatio = _wantedAspectRatio;
		_currentAspectRatio = (float)Screen.width / Screen.height;
		SetCamera();

		viewPortWidth = cam.rect.width;
		viewPortHeight = cam.rect.height;
	}
	
	public static void SetCamera () 
	{
		float currentAspectRatio = (float)Screen.width / Screen.height;
		// If the current aspect ratio is already approximately equal to the desired aspect ratio,
		// use a full-screen Rect (in case it was set to something else previously)
		if ((int)(currentAspectRatio * 100) / 100.0f == (int)(wantedAspectRatio * 100) / 100.0f) 
		{
			cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			if (backgroundCam) 
			{
				Destroy(backgroundCam.gameObject);
			}
			return;
		}

		// Pillarbox
		if (currentAspectRatio > wantedAspectRatio) 
		{//La 'resolucion real' de la pantalla es mas ancha que la 'resolucion virtual' del juego. Se cambia el tamaño
		 //del ViewPort para que su ancho sea mas pequeño y tenga el mismo aspect ratio (ancho/alto) que la 'resolucion virtual'
		 //del juego, asi cuando Unity escale la imagen a mostrar en pantalla, como la escala dentro del ViewPort, la relacion
		 //ancho/alto (aspectRatio) se mantiene y los graficos no estaran deformados.
			float inset = 1.0f - wantedAspectRatio/currentAspectRatio;
			cam.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
		}

		// Letterbox
		else 
		{//La 'resolucion real' de la pantalla es mas alta que la 'resolucion virtual' del juego. Se cambia el tamaño
			//del ViewPort para que su alto sea mas pequeño y tenga el mismo aspect ratio (ancho/alto) que la 'resolucion virtual'
			//del juego, asi cuando Unity escale la imagen a mostrar en pantalla, como la escala dentro del ViewPort, la relacion
			//ancho/alto (aspectRatio) se mantiene y los graficos no estaran deformados.
			float inset = 1.0f - currentAspectRatio/wantedAspectRatio;
			cam.rect = new Rect(0.0f, inset/2, 1.0f, 1.0f-inset);
		}

		if (!backgroundCam) 
		{
			// Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
			backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
			backgroundCam.depth = int.MinValue;
			backgroundCam.clearFlags = CameraClearFlags.SolidColor;
			backgroundCam.backgroundColor = Color.black;
			backgroundCam.cullingMask = 0; //No dibuja ningún 'Layer'. Los 'gameObjects' se asignan a un 'Layer' y cada camara dibuja los gameObjects de determinados 'Layers' (por defecto de todos).
		}
	}
	
	public static int screenHeight 
	{
		get 
		{
			return (int)(Screen.height * cam.rect.height);
		}
	}
	
	public static int screenWidth 
	{
		get 
		{
			return (int)(Screen.width * cam.rect.width);
		}
	}
	
	public static int xOffset 
	{
		get 
		{
			return (int)(Screen.width * cam.rect.x);
		}
	}
	
	public static int yOffset 
	{
		get 
		{
			return (int)(Screen.height * cam.rect.y);
		}
	}
	
	public static Rect screenRect 
	{
		get 
		{
			return new Rect(cam.rect.x * Screen.width, cam.rect.y * Screen.height, cam.rect.width * Screen.width, cam.rect.height * Screen.height);
		}
	}
	
	public static Vector3 mousePosition 
	{
		get 
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.y -= (int)(cam.rect.y * Screen.height);
			mousePos.x -= (int)(cam.rect.x * Screen.width);
			return mousePos;
		}
	}
	
	public static Vector2 guiMousePosition 
	{
		get 
		{
			Vector2 mousePos = Input.mousePosition;
			mousePos.y = Mathf.Clamp(mousePos.y, cam.rect.y * Screen.height, cam.rect.y * Screen.height + cam.rect.height * Screen.height);
			mousePos.x = Mathf.Clamp(mousePos.x, cam.rect.x * Screen.width, cam.rect.x * Screen.width + cam.rect.width * Screen.width);
			return mousePos;
		}
	}
}
