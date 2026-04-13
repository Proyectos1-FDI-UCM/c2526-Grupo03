//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Objeto del EventSystem de la escena
    /// </summary>
    [SerializeField] GameObject EventSystem;

    /// <summary>
    /// Primer botón del panel de muerte
    /// </summary>
    [SerializeField] GameObject DeadFirstSelectedButton;
    /// <summary>
    /// Primer Botón del menú de pausa
    /// </summary>
    [SerializeField] GameObject PauseFirstSelectedButton;
    /// <summary>
    /// Primer botón del panel de victoria
    /// </summary>
    [SerializeField] GameObject VictoryFirstSelectedButton;


    /// <summary>
    /// Panel de pausa
    /// </summary>
    [SerializeField] private GameObject PauseScreen;
    /// <summary>
    /// Panel de Muerte
    /// </summary>
    [SerializeField] private GameObject Lose_Screen;
    /// <summary>
    /// Panel de victoria
    /// </summary>
    [SerializeField] private GameObject VictoryScreen;  

    /// <summary>
    /// Calidad inicial
    /// </summary>
    [SerializeField] private int StartingQuality = 100;
    /// <summary>
    /// Tiempo que tiene que pasar para que la puntuación empiece a subir
    /// </summary>
    [SerializeField] private float TimeToStartAdding = 0f;

    /// <summary>
    /// Cantidad que sube la película
    /// </summary>
    [SerializeField] private int Quality_Add = 0;
    /// <summary>
    /// Objeto padre en el que se encuentran todos los objetos a pausar
    /// </summary>
    [SerializeField] private GameObject objetos_pausados;

    // Cantidad de puntuación aumentada cada "pulso" (frecuencia del pulso definida en la cámara)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;

    /// <summary>
    /// Calidad en cada momento de la película
    /// </summary>
    private int _quality;

    /// <summary>
    /// Variable que guarda el último QTE
    /// </summary>
    private int _lastqte = -1;

    /// <summary>
    /// Componente del eventsystem
    /// </summary>
    private EventSystem _eventSystem;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour
    private void Start()
    {
        // Guardamos el componente del eventsystem
        if (EventSystem.GetComponent<EventSystem>() != null)
        {
            _eventSystem = EventSystem.GetComponent<EventSystem>();
            // Seleccionamos primero el menu de pausa
            _eventSystem.SetSelectedGameObject(PauseFirstSelectedButton);
        }
        GameManager.Instance.SetLevelToRestart(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Pausa el juego, cambia el boton seleccionado al del panel de victoria y activa el panel
    /// </summary>
    public void ChangeButtonToVictory()
    {
        _Pausa_SinPanel();
        _eventSystem.SetSelectedGameObject(VictoryFirstSelectedButton);
        VictoryScreen.SetActive(true);
    }
    /// <summary>
    /// Pausa el juego, cambia el boton seleccionado al del panel de muerte y activa el panel
    /// </summary>
    public void ChangeButtonToDead()
    {
        _Pausa_SinPanel();
        _eventSystem.SetSelectedGameObject(DeadFirstSelectedButton);
        Lose_Screen.SetActive(true);
    }
    /// <summary>
    /// Cambia el boton seleccionado al del panel de pausa y activa el panel
    /// </summary>
    public void ChangeButtonToPause()
    {
        _eventSystem.SetSelectedGameObject(PauseFirstSelectedButton);
        PauseScreen.SetActive(true);
    }

    /// <summary>
    /// Método que sube la calidad de la película
    /// </summary>


    /// <summary>
    /// Método que sube la calidad de la película en la cantidad que le digas
    /// </summary>
    public void QualityUp()
    {
        if (_quality > 0 && _quality < StartingQuality)
        {
            _quality += Quality_Add;
        }
    }
    /// <summary>
    /// Método que baja la calidad de la película dependiendo de con que objeto collisione
    /// </summary>
    /// <param name="cant">Cantidad a bajar</param>
    public void QualityDown(int cant)
    {
        if (_quality>0) _quality -= cant;
        if (_quality <= 0)
        {
            ChangeButtonToDead(); 
        }
    }
    /// <summary>
    /// Método que devuelve la puntuación actual de la película
    /// </summary>
    public float GetIntervaloParaSubir()
    {
        return TimeToStartAdding;
    }
    public int GetcurrentScore()
    {
        return _quality;
    }

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }
    /// <summary>
    /// Metodo encargado de pausar la partida 
    /// </summary>
    public void _Pausa()
    {
        /*Se manda un mensaje a todos los objetos que sean hijos de este para
        que activen el metodo "UnPause" */
        objetos_pausados.BroadcastMessage("Pause");
        // Activa el panel de pausa
        ChangeButtonToPause();
    }
    /// <summary>
    /// Metodo que pausara el juego sin activar el panel de pausa (ideal para cuando llegas a la meta)
    /// </summary>
    public void _Pausa_SinPanel()
    {
        /*Se manda un mensaje a todos los objetos que sean hijos de este para
        que activen el metodo "UnPause" */
        objetos_pausados.BroadcastMessage("Pause");

    }
    /// <summary>
    /// Metod encargado de restablecer la partida desde el punto que la dejaste 
    /// </summary>
    public void Resume()
    {
        /* Se manda un mensaje a todos los objetos que sean hijos de este para
        que activen el metodo "UnPause" */
        objetos_pausados.BroadcastMessage("UnPause");
        //Desactiva el panel de pausa
        PauseScreen.SetActive(false);
    }
    public void Restart()
    {  //Restablece el nivel al inicio
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //Desactiva el panel de pausa
        PauseScreen.SetActive(false);
    }
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Metodo que devuelve el QTE anterior
    /// </summary>
    public int LastQTE()
    {
        return _lastqte;
    }

    /// <summary>
    /// Metodo que actualiza el valor del QTE anterior
    /// </summary>
    public void CambiarValor(int valor)
    {
        _lastqte = valor;
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        _quality = StartingQuality;
    }

    /// <summary>
    /// Pausa el juego en caso de quedarse en segundo plano.
    /// </summary>
    /// <param name="FocusMode"></param>
    private void OnApplicationFocus(bool FocusMode)
    {
        if(!FocusMode)
        {
            _Pausa();
        }
    }

    #endregion
} // class LevelManager 
// namespace