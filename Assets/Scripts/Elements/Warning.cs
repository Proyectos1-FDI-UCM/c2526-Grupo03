//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Warning : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    // Señal de warning
    [SerializeField] private GameObject WarningSign;

    // Tiempo que dura la advertencia
    [SerializeField] private float WarningDuration;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // Variable activadora de señal
    private bool _detected;

    // Variable que determina cuando acabó la advertencia
    private bool _done;

    // Variable que determina si ya fue tocado el trigger o no
    private bool _touched;

    // Variable de control de tiempo de señal
    private float _timePassed;

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
        WarningSign.SetActive(false);
        _detected = false;
        _done = false;
        _touched = false;
        // Desactivamos la vida para que no pueda matarlo sin haber activado el warning
        if(this.gameObject.GetComponentInParent<Enemies_Health>() != null)
        {
            this.gameObject.GetComponentInParent<Enemies_Health>().enabled = false;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_detected && Time.time > _timePassed)
        {
            WarningSign.SetActive(false);
            _done = true;
            // Activamos la vida y daño
            if (this.gameObject.GetComponentInParent<Enemies_Health>() != null)
            {
                this.gameObject.GetComponentInParent<Enemies_Health>().enabled = true;
            }
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
    /// Función llamada cuando detecta al jugador
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            _detected = true;
            if (!_touched)
            {
                _timePassed = Time.time + WarningDuration;
                WarningSign.SetActive(true);
                _touched = true;
            }
        }
    }

    /// <summary>
    /// Función que devuelve el estado de "_done"
    /// </summary>
    public bool GetDone()
    {
        return _done;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Warning 
// namespace
