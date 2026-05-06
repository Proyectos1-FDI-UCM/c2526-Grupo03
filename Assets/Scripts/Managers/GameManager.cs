//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro P. Gómez Martín
// Marco A. Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    //pantalla de partida paerdida 

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;
    ///<summary>
    /// Variable que cuenta el tiempo pasado
    ///</summary>
    private float _timepassed;
    /// <summary>
    /// variable que guarda la calidad inicial
    /// </summary>
    private int InicialQuality;

    /// <summary>
    /// Nombre de la escena a reiniciar en caso necesario
    /// </summary>
    private int _levelIndex;

    /// <summary>
    /// Guarda la puntuación final despúes de cada nivel
    /// </summary>
    private float _FinalRating;
    /// <summary>
    /// bool que indica si estamos usando mando
    /// </summary>
    private bool _mando = false;
    /// <summary>
    /// bool que indica si ehemos cambiado de dispositivo utilizado
    /// </summary>
    private bool _deviceChanged = false;
    /// <summary>
    ///  bool que indica si estamos usando la no muerte
    /// </summary>
    private bool _noMuerte = false;
    /// <summary>
    ///  bool que indica si estamos usando el auto repair
    /// </summary>
    private bool _autoRepair = false;
    /// <summary>
    ///  bool que indica si estamos usando la no calidad
    /// </summary>
    private bool _noCalidad = false;
    /// <summary>
    /// Guarda el volumen de Musica entre escenas
    /// </summary>
    private float _volumenActMusica = 1f;
    /// <summary>
    /// Guarda el volumen de efectos entre escenas
    /// </summary>
    private float _volumenActEfects = 1f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de este manager al que ya existe.
            // Esto permitirá al GameManager real mantener su estado interno
            // pero acceder a los elementos de la nueva escena
            // o bien olvidar los de la escena previa de la que venimos
            TransferManagerSetup();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.


    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Método que guarda la puntuación final para otras escenas
    /// </summary>
    /// <param name="rating">Puntuación a establecer</param>
    public void SetFinalRating(float rating)
    {
        _FinalRating = rating;
    }
    public float GetFinalRating()
    {
        return _FinalRating;
    }
    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
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
    /// cierre para evitar usar el GameManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene
    /// <summary>
    /// Guarda el indice de la escena a reiniciar en caso necesario
    /// </summary>
    /// <param name="index">Indice en la build de la escena a reiniciar</param>
    public void SetLevelToRestart(int index)
    {
        _levelIndex = index;
    }
    /// <summary>
    /// Reinicia la escena del último nivel jugado
    /// </summary>
    public void RestartLevel()
    {
        //Restablece el nivel al inicio
        ChangeScene(_levelIndex);
    }
    #endregion

    /// <summary>
    /// Cierra la build
    /// </summary>
    public void Exit()
    {
        // Debug.Log("Cerrando juego");
        Application.Quit();
    }

    /// <summary>
    /// Abre la escena de créditos
    /// </summary>
    public void ChangeCreditsScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Creditos");
    }

    /// <summary>
    /// Abre el menú
    /// </summary>
    public void ChangeMenuScene()
    {
        // Debug.Log("Menú");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void ChangeMenuTestScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPruebas");
    }
    /// <summary>
    /// Cambia el estado de el boleano mando
    /// </summary>
    public void SetMando(bool estado)
    {
        _mando = estado;

        // Debug.Log(mando);
    }
    /// <summary>
    /// Devuelve si esta usando mando
    /// </summary>
    /// <returns></returns>
    public bool GetMando()
    {
        return _mando;
    }
    /// <summary>
    /// Cambia el estado de el boleano autoRepair
    /// </summary>
    public void SetAutoRepair(bool estado)
    {
        _autoRepair = estado;
    }
    /// <summary>
    /// Devuelve si esta usando autoRepair
    /// </summary>
    public bool GetAutoRepair()
    {
        return _autoRepair;
    }
    /// <summary>
    /// Cambia el estado de el boleano noCalidad
    /// </summary>
    public void SetNoCalidad(bool estado)
    {
        _noCalidad = estado;
    }
    /// <summary>
    /// Devuelve si esta usando noCalidad
    /// </summary>
    public bool GetNoCalidad()
    {
        return _noCalidad;
    }
    /// <summary>
    /// Cambia el estado de el boleano noMuerte
    /// </summary>
    public void SetNoMuerte(bool estado)
    {
        _noMuerte = estado;
    }
    /// <summary>
    /// Devuelve si esta usando noMuerte
    /// </summary>
    public bool GetNoMuerte()
    {
        return _noMuerte;
    }
    /// <summary>
    /// Cambia el valor del booleano que indica si se ha cambiado de dispositivo
    /// </summary>
    public void SetDeviceChanged(bool changed)
    {
        _deviceChanged = changed;
    }
    /// <summary>
    /// Indica si se ha cambiado el dispositivo utilizado
    /// </summary>
    /// <returns>True cuando se ha cambiado</returns>
    public bool DeviceChanged()
    {
        return _deviceChanged;
    }
    /// <summary>
    /// Guarda el volumen de la musica
    /// </summary>
    /// <param name="vol">porcentaje de volumen</param>
    public void SetCurrentMusicVolume(float vol)
    {
        _volumenActMusica = vol;
    }
    /// <summary>
    /// Guarda el volumen actual de los efectos
    /// </summary>
    /// <param name="vol">porcentaje de volumen </param>
    public void SetCurrentEffectsVolume(float vol)
    {
        _volumenActEfects = vol;
    }
    /// <summary>
    /// Metodo que devuelve el volumen de la musica actual
    /// </summary>
    public float GetCurrentMusicVolume()
    {
        return _volumenActMusica;
    }
    /// <summary>
    /// Metodo que devuelve el volumen de los efectos actual
    /// </summary>
    public float GetCurrentEfectsVolume()
    {
        return _volumenActEfects;
    }

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados 

    private void TransferManagerSetup()
    {
        // De momento no hay que transferir ningún setup
        // a otro manager
    }

    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    #endregion
} // class GameManager 
  // namespace