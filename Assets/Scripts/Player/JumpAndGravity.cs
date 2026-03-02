//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class JumpAndGravity : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Trigger que detecta si estás en el suelo
    /// </summary>
    [SerializeField] private GameObject FloorDetector;
    /// <summary>
    /// Trigger que detecta si has tocado el techo
    /// </summary>
    [SerializeField] private GameObject RoofDetector;
    /// <summary>
    /// Distancia vertical del salto
    /// </summary>
    [SerializeField] private float JumpHeight = 2.0f;
    /// <summary>
    /// Tiempo necesario para alcanzar la altura máxima
    /// </summary>
    [SerializeField] private float TimeToReachMaxHeight = 0.5f;
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
    /// Velocidad con la que impulsamos al iniciar un salt0
    /// </summary>
    private float _initialSpeed = 0.0f;
    /// <summary>
    /// Aceleración negativa con la que simulamos gravedad
    /// </summary>
    private float _gravity = 0.0f;
    /// <summary>
    /// Altura del collider del jugador
    /// </summary>
    private float _playerHeight = 0.0f;
    /// <summary>
    /// Anchura del collider del jugador
    /// </summary>
    private float _playerWidth = 0.0f;
    /// <summary>
    /// Simula la velocidad afectada por la gravedad
    /// </summary>
    private float _speed = 0.0f;
    
    /// <summary>
    /// Comprueba si está en un salto
    /// </summary>
    private bool _hasJumped = false;
    /// <summary>
    /// Comprueba si ha terminado de subir
    /// </summary>
    private bool _hasReachedMaxHeight = false;
    /// <summary>
    /// Altura a la que debe llegar el salto
    /// </summary>
    private Vector3 _maxPosJumped; 
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
        _playerHeight = GetComponent<BoxCollider2D>().bounds.size.y;
        _playerWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        _initialSpeed = (2 * JumpHeight) / TimeToReachMaxHeight;
        _gravity = _initialSpeed / TimeToReachMaxHeight;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector2 dir = InputManager.Instance.MovementVector;
        // De momento hacemos el salto con el movimiento en el eje y para no tocar el input action
        if (dir.y > 0.5f)
        {
            // Comprobamos si el detector de suelo dice que estás en el suelo
            if (FloorDetector.GetComponent<FloorDetector>().IsGrounded)
            {
                // Iniciamos las variables necesarias
                _maxPosJumped = transform.position + new Vector3(0.0f, JumpHeight);
                _hasJumped = true;
                _hasReachedMaxHeight = false;
                _speed = _initialSpeed;
            }
        }
        // Si ha saltado
        if (_hasJumped)
        {
            // Comprobamos si ha llegado a la altura máxima o choca con el techo
            if (transform.position.y >= _maxPosJumped.y || RoofDetector.GetComponent<RoofDetector>().TouchesRoof)
            {
                _hasReachedMaxHeight = true;
                _speed = 0.0f;
            }
            // Mientras no ha llegado a la altura máxima
            if (!_hasReachedMaxHeight)
            {
                // mueve al personaje hacia arriba en base a la velocidad en el eje y
                this.transform.position += new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;
                _speed -= _gravity * Time.deltaTime;
            }
            // Cuando se ha terminado el salto
            else
            {
                _hasJumped = false;
            }
        }
        // Parte de gravedad
        else // Se ejecuta cuando ha terminado el salto para no añadir dos veces la gravedad
        {
            // Mientras no toca suelo
            if (!FloorDetector.GetComponent<FloorDetector>().IsGrounded)
            {
                // mueve al personaje hacia abajo en base a la velocidad en el eje y
                this.transform.position -= new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;
                // Limitamos la velocidad de caída hasta la velocidad de impulso
                if (_speed < _initialSpeed * 1.5f)
                {
                    _speed += _gravity * Time.deltaTime;
                }
                else _speed = _initialSpeed * 1.5f;
            }
            // Cuando toco suelo
            else
            {
                // Cambio la velocidad actual a 0 para que no siga aumentando
                _speed = 0.0f;
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

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Jump 
// namespace
