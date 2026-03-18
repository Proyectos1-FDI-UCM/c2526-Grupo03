//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dolly_Detection_System : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private float IntervaloParaSubir = 0f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    ///<summary>
    /// Variable que cuenta el tiempo pasado
    ///</summary>
    private float _timepassed;

    ///<summary>
    /// Variable que determina si se ha encontrado un objeto interactuable o no
    ///</summary>
    private bool _detected;

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
    void Awake()
    {
        LoseScreen.SetActive(false);
        _detected = false;
    }

    /// <summary>
    /// Update is called every frame the monovehaviour is active
    /// </summary>
    private void Update()
    {
        if (_detected)
        {
            _timepassed = Time.time + IntervaloParaSubir;
            _detected = false;
        }
        else if (!_detected && Time.time >= _timepassed)
        {
            _timepassed = Time.time + IntervaloParaSubir;
            GameManager.Instance.QualityUp();
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            LoseScreen.SetActive(true);
            GameManager.Instance.PlayerDetected();
        }
        else if (collision.gameObject.TryGetComponent<Repair>(out var Repair) && !Repair.Repaired)
        {
            GameManager.Instance.UnrepairedElementDetected();
            _detected = true;
        }
        else if (collision.gameObject.GetComponent<Extra_Regular>() != null)
        {
            GameManager.Instance.ExtraRegularDetected();
            _detected = true;
        }
        else if (collision.gameObject.GetComponent<Extra_Army>() != null)
        {
            GameManager.Instance.ExtraArmyDetected();
            _detected = true;
        }
        
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Dolly_Detection_System1 
// namespace
