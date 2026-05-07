//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] EventSystem EventSystem;

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
    /// Primer botón del panel de settings
    /// </summary>
    [SerializeField] GameObject SettingsFirstSelectedButton;

    /// <summary>
    /// Panel de pausa
    /// </summary>
    [SerializeField] private GameObject PauseScreen;

    /// <summary>
    /// Panel de Muerte
    /// </summary>
    [SerializeField] private GameObject LoseScreen;

    /// <summary>
    /// Panel de victoria
    /// </summary>
    [SerializeField] private GameObject VictoryScreen;

    /// <summary>
    /// Panel de settings
    /// </summary>
    [SerializeField] private GameObject SettingsPanel;

    /// <summary>
    /// Calidad inicial
    /// </summary>
    [SerializeField] private float StartingQuality = 100;

    /// <summary>
    /// Objeto padre en el que se encuentran todos los objetos a pausar
    /// </summary>
    [SerializeField] private GameObject ObjetosPausados =  null;

    /// <summary>
    /// Posibilidades de que salga sonido de derrota especial
    /// </summary>
    [SerializeField] private int ChanceForSpecialSound;

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
    private float _quality;

    /// <summary>
    /// Variable que guarda el último QTE
    /// </summary>
    private int _lastQte = 0;

    /// <summary>
    /// Indica si el jugador ha llegado a la meta
    /// </summary>
    private bool _playerFinished = false;

    private VictoryPanel _panelVictoria;

    /// <summary>
    /// Variable aleatoria
    /// </summary>
    private int _rnd;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour
    private void Start()
    {
        // Guardamos el componente del eventsystem
        if (EventSystem != null)
        {
            
            // Seleccionamos primero el menu de pausa
            EventSystem.SetSelectedGameObject(PauseFirstSelectedButton);
        }
        GameManager.Instance.SetLevelToRestart(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        _panelVictoria = VictoryScreen.GetComponent<VictoryPanel>();
    }
    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            // Hacemos init
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
        //Puasamos el juego sin activar el panel
        PausaSinPanel();
        //Cambiamos el boton selecionado al de pausa
        EventSystem.SetSelectedGameObject(VictoryFirstSelectedButton);
        //Activamos el panel de victoria 
        VictoryScreen.SetActive(true);
        _panelVictoria.PonEstrellas();
        SoundManager.Instance.StopLevelMusic();
        SoundManager.Instance.PlayMusicCourseClear();

    }
    /// <summary>
    /// Pausa el juego, cambia el boton seleccionado al del panel de muerte y activa el panel
    /// </summary>
    public void ChangeButtonToDead()
    {
        //Pausamos el juego sin usar el panel
        PausaSinPanel();
        //Cambiamos el boton selecionado al del Lose Screen
        EventSystem.SetSelectedGameObject(DeadFirstSelectedButton);
        //Activamos la Lose Screen
        LoseScreen.SetActive(true);
        SoundManager.Instance.StopMusicQTE();
        SoundManager.Instance.StopLevelMusic();
        _rnd = Random.Range(0, ChanceForSpecialSound);
        if (_rnd == ChanceForSpecialSound - 1) SoundManager.Instance.PlayMusicWaaWaaWaaa();
        else SoundManager.Instance.PlaySFXHasPerdiido();
    }
    /// <summary>
    /// Cambia el boton seleccionado al del panel de pausa y activa el panel
    /// </summary>
    public void ChangeButtonToPause()
    {
        SettingsPanel.SetActive(false);
        //Cambiamos el boton selecionado al de pausa
        EventSystem.SetSelectedGameObject(PauseFirstSelectedButton);
        //Activamos el Pause Screen
        PauseScreen.SetActive(true);
        //Pausamos múisca correspondiente
        SoundManager.Instance.PauseLevelMusic();
    }
    /// <summary>
    /// Cambia el boton seleccionado al del panel de settings y activa el panel
    /// </summary>
    public void ChangeButtonToSettings()
    {
        PauseScreen.SetActive(false);
        //Cambiamos el boton selecionado al de settings
        EventSystem.SetSelectedGameObject(SettingsFirstSelectedButton);

        //Activamos el Settings Screen
        SettingsPanel.SetActive(true);

    }
    /// <summary>
    /// Cambia el bool que indica si el jugador ha llegado a la meta 
    /// </summary>
    /// <param name="finished">Bool por el que cambiar</param>
    public void SetPlayerFinished(bool finished)
    {
        _playerFinished = finished;
    }
    /// <summary>
    /// Método que baja la calidad de la película dependiendo de con que objeto collisione
    /// </summary>
    /// <param name="cant">Cantidad a bajar</param>
    public void QualityDown(float cant)
    {
        //Miramos que la calidad sea mayor que 0
        if (_quality>0) _quality -= cant;
        //Miramos si ya has perdido
        if (_quality <= 0)
        {
            //Cambiamos al boton de muerte
            ChangeButtonToDead(); 
        }
    }
    /// <summary>
    /// Método que devuelve la puntuacion actual de la película
    /// </summary>
    public float GetcurrentScore()
    {
        return _quality;
    }
    /// <summary>
    /// Indica si el jugador ha llegado a la meta
    /// </summary>
    /// <returns>True cuando ha llegado y false cuando no ha llegado</returns>
    public bool GetPlayerFinished()
    {
        return _playerFinished;
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
    public void Pausa()
    {
        SoundManager.Instance.PauseQTEMusic();
        /*Se manda un mensaje a todos los objetos que sean hijos de este para
        que activen el metodo "UnPause" */
        PausaSinPanel();
        // Activa el panel de pausa
        ChangeButtonToPause();
    }
    /// <summary>
    /// Metodo que pausara el juego sin activar el panel de pausa (ideal para cuando llegas a la meta)
    /// </summary>
    public void PausaSinPanel()
    {
        /*Se manda un mensaje a todos los objetos que sean hijos de este para
        que activen el metodo "UnPause" */
        ObjetosPausados.SetActive(false);

    }
    /// <summary>
    /// Metod encargado de restablecer la partida desde el punto que la dejaste 
    /// </summary>
    public void Resume()
    {
        /* Se manda un mensaje a todos los objetos que sean hijos de este para
        que activen el metodo "UnPause" */
        ObjetosPausados.SetActive(true);
        //Desactiva el panel de pausa
        PauseScreen.SetActive(false);
        SoundManager.Instance.ResumeMusic();
    }
    /// <summary>
    /// Metodo que Resetea la escena
    /// </summary>
    public void Restart()
    {
        // Restablece el nivel al inicio
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // Desactiva el panel de pausa
        PauseScreen.SetActive(false);
    }
    /// <summary>
    /// Metodo que carga la escena del menu
    /// </summary>
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Metodo que devuelve el QTE anterior
    /// </summary>
    public int LastQTE()
    {
        return _lastQte;
    }

    /// <summary>
    /// Metodo que actualiza el valor del QTE anterior
    /// </summary>
    public void CambiarValor(int valor)
    {
        _lastQte = valor;
    }


    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// inicialización de puntuacion.
    /// </summary>
    private void Init()
    {
        _quality = StartingQuality;
        SoundManager.Instance.SetVolumeToCurrent();
    }

    /// <summary>
    /// Pausa el juego en caso de quedarse en segundo plano.
    /// </summary>
    /// <param name="FocusMode">bool que indica si esta en primer plano</param>
    private void OnApplicationFocus(bool FocusMode)
    {
        //Miramos si no esta en focus mode
        if(!FocusMode)
        {
            //Pausa el juego
            Pausa();
        }
    }

    #endregion
} // class LevelManager 
// namespace