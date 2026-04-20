//---------------------------------------------------------
// Componente de la señal de advertencia del extra army
// Sergio Higuera Gil && Gabriel Adrian Oltean
//      Gabriel Adrian Oltean
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

    /// <summary>
    /// Objeto de la señal de advertencia
    /// </summary>
    [SerializeField] private GameObject WarningSign;

    /// <summary>
    /// Jugador punto de referencia
    /// </summary>
    [SerializeField] private Transform Player;

    /// <summary>
    /// Tiempo que dura la advertencia
    /// </summary>
    [SerializeField] private float WarningDuration;

    /// <summary>
    /// Tiempo entre parpadeos de la señal
    /// </summary>
    [SerializeField] private float TimeToShift;

    /// <summary>
    /// Margen a dejar en X para la señal
    /// </summary>
    [SerializeField] private float XPlayerOffset;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Variable que dice si se ha detectado al jugador
    /// </summary>
    private bool _detected;

    /// <summary>
    /// Determina cuando se ha acabado la advertencia
    /// </summary>
    private bool _done;

    /// <summary>
    /// Determina si la señal ya fue activada
    /// </summary>
    private bool _touched;

    /// <summary>
    /// Momento en el que acaba la advertencia
    /// </summary>
    private float _timePassed;

    /// <summary>
    /// Tiempo en el que se tiene que cambiar el estado de la señal (Parpadeo)
    /// </summary>
    private float _shiftingTime;

    /// <summary>
    /// Componente de vida del enemigo
    /// </summary>
    private Enemies_Health _health;

    /// <summary>
    /// Posicion de la huelga
    /// </summary>
    private Vector2 _armyposition;

    /// <summary>
    /// Posicion fija en X
    /// </summary>
    float _fixedX;

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
        // Desactivamos la señal de advertencia
        WarningSign.SetActive(false);
        // Inicializamos las variables necesarias
        _detected = false;
        _done = false;
        _touched = false;
        _armyposition = transform.parent.position;
        // Desactivamos la vida para que no pueda matarlo sin haber activado el warning
        if (this.gameObject.GetComponentInParent<Enemies_Health>() != null)
        {
            _health = this.gameObject.GetComponentInParent<Enemies_Health>();
            _health.enabled = false;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    { 
        _fixedX = Player.position.x + XPlayerOffset;

        WarningSign.transform.position = new Vector2(_fixedX, _armyposition.y);
        // Cuando ha empezado la advertencia
        if (_detected && !_done)
        {
            // ====== Parpadeo ======
            if (Time.time >= _shiftingTime)
            {
                // Si ya estaba activada la desactivamos
                if (WarningSign.activeSelf)
                {
                    WarningSign.SetActive(false);
                }
                // Si estaba desactivada la activamos
                else
                {
                    WarningSign.SetActive(true);
                }
                // Actualizamos el tiempo de cambio
                _shiftingTime = Time.time + TimeToShift;
            }

            // ====== Finalización de la advertencia ======
            if (Time.time >= _timePassed)
            {
                // Desactivamos la señal
                WarningSign.SetActive(false);
                // Activamos la vida y daño
                if (_health != null)
                {
                    _health.enabled = true;
                }
                // Acabamos la advertencia
                _done = true;
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
        // Si aún no ha sido activada y es un jugador
        if (!_touched && collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            // Iniciamos la advertencia
            _detected = true;
            // Inicializamos las variables del parpadeo
            _shiftingTime = Time.time + TimeToShift;
            _timePassed = Time.time + WarningDuration;
            // Activamos la señal
            WarningSign.SetActive(true);
            // Indicamos que ya ha sido activada
            _touched = true;
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
