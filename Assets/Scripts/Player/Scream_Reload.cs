//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
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

    // Velocidad de movimiento mientras recarga
    [SerializeField] private float ReloadMovement = 3.0f;

    // Rapidez a la cual bebe el agua
    [SerializeField] private float ReloadSpeed = 1.5f;

    // Tiempo de espera entre recargas (para que no se sature de agua el pobre)
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

    // Vartiable de cuenta de tiempo desde última recarga
    private float _timePassed;

    // Munición del script "Movement_Player" (Ammo)
    private int _cords;

    // Velocidad máxima del script "Movement_Player" (Max_Speed)
    private float _maxOriginalSpeed;

    // Variable temporizador para el movimiento modificado
    private float _enOfModification;

    // Variable que determina si el efecto de realentización está activo o no
    private bool _slowActive = false;
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
        _cords = Movement_Player.Ammo;
        _maxOriginalSpeed = Movement_Player.MaxVelocity;
    }

    /// <summary>
    /// FixedUpdate is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        bool reloading = InputManager.Instance.RelaodWasPressedThisFrame();

        if (reloading && Time.time >= _timePassed)
        {
            Movement_Player.Ammo = _cords;
            Movement_Player.MaxVelocity = ReloadMovement;

            _enOfModification = Time.time + ReloadSpeed;
            _slowActive = true;

            _timePassed = Time.time + ReloadCD;
        }

        if (_slowActive && Time.time >= _enOfModification)
        {
            Movement_Player.MaxVelocity = _maxOriginalSpeed;
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
