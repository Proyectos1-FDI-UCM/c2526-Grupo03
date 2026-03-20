//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// Template-P1
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;

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
    /// Panel de muerte
    /// </summary>
    [SerializeField] private GameObject LoseScreen;
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
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

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
        if (_quality<=0) LoseScreen.SetActive(true);
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        LoseScreen.SetActive(false);
        _quality = StartingQuality;
    }

    #endregion
} // class LevelManager 
// namespace