//---------------------------------------------------------
// Script encargado de dirigir la logica del juego durante el tutorial
// Tristan Sanchez
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Tutorial_Manager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// GameObject de Dolly
    /// </summary>
    [SerializeField] private GameObject Dolly = null;
    /// <summary>
    /// GameObject de Desert_Ball
    /// </summary>
    [SerializeField] private GameObject Desert_Ball = null;
    /// <summary>
    /// GameObject de Player
    /// </summary>
    [SerializeField] private GameObject Player = null;
    /// <summary>
    /// GameObject de GUI
    /// </summary>
    [SerializeField] private GameObject GUI = null;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private static Tutorial_Manager _instance;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        //Desactivamos los componentes del jugador
        Player.GetComponent<Jump>().enabled = false;
        Player.GetComponent<Scream_Reload>().enabled = false;
        Player.GetComponent<Shoot>().enabled = false;
        //Desactivamos los objetos no necesareos al principo
        Desert_Ball.SetActive(false);
        GUI.SetActive(false);
        Dolly.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    /// <summary>
    /// Activa a dolly a la Dessert_Ball cuando se llame al metodo
    /// </summary>
    public void Activa_Dolly()
    {
        Desert_Ball.SetActive(true);
        Dolly.SetActive(true);
       
    }
    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static Tutorial_Manager Instance
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
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void Init()
    {
    
    }

    #endregion

} // class Turorial_Manager 
// namespace
