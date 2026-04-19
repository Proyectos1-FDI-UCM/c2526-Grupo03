//---------------------------------------------------------
// Script encargado de la recarga de municion del personaje
// Sergio Higuera && Colaboradores:
//      Alejandro Garcia
//      Gabriel Adrian
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Scream_Reload : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Velocidad de movimiento mientras recarga
    /// </summary>
    [SerializeField] private float ReloadMovement = 3.0f;

    /// <summary>
    /// Rapidez a la cual bebe el agua
    /// </summary>
    [SerializeField] private float ReloadDuration = 1.5f;

    /// <summary>
    /// Tiempo de espera entre recargas (para que no se sature de agua el pobre)
    /// </summary>
    [SerializeField] private float ReloadCD = 5.0f;


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
    /// Indica si se esta recargando
    /// </summary>
    private bool _reloading = false;
    /// <summary>
    /// Vartiable de cuenta de tiempo desde última recarga
    /// </summary>
    private float _timePassed;

    /// <summary>
    /// Velocidad máxima del script "Movement_Player" (Max_Speed)
    /// </summary>
    private float _maxOriginalSpeed;

    /// <summary>
    /// Variable temporizador para el movimiento modificado
    /// </summary>
    private float _enOfModification;

    /// <summary>
    /// Variable que determina si el efecto de realentización está activo o no
    /// </summary>
    private bool _slowActive = false;

    /// <summary>
    /// Componente de movimiento del jugador
    /// </summary>
    private Movement_Player _playerMovement;

    /// <summary>
    /// Componente de disparo del jugador
    /// </summary>
    private Shoot _playerShoot;
    /// <summary>
    /// Indica si debemos impedir la siguiente recarga
    /// </summary>
    private bool _desactivated = false;
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
        // Cacheamos los componentes
        _maxOriginalSpeed = this.gameObject.GetComponent<Movement_Player>().getMaxVel();
        _playerMovement = this.gameObject.GetComponent<Movement_Player>();
        _playerShoot = this.gameObject.GetComponent<Shoot>();
    }

    /// <summary>
    /// FixedUpdate is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Comprobamos si puedes recargar
        if (InputManager.Instance.RelaodWasPressedThisFrame() && !_desactivated && Time.time >= _timePassed)
        {
            // Cambiamos la velocidad de movimiento a la de recarga
            _playerMovement.setMaxVel(ReloadMovement);
            // Indicamos que estamos recargando
            _reloading = true;
            // Cambiamos el tiempo de fin de recarga
            _enOfModification = Time.time + ReloadDuration;
            // Indicamos que se ha ralentizado
            _slowActive = true;
            // Modificamos el tiempo para volver a recargar
            _timePassed = Time.time + ReloadCD;
        }

        if (_slowActive && Time.time >= _enOfModification)
        {
            _reloading = false;
            _playerShoot.ReloadAmmo();
            _playerMovement.setMaxVel(_maxOriginalSpeed);
            _slowActive = false;
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

    public bool IsReloading()
    {
        return _reloading;
    }
    public void DesactivateReload()
    {
        _desactivated = true;
    }
    public void ActivateReload()
    {
        _desactivated = false;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Scream_Reload 
// namespace
