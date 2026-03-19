//---------------------------------------------------------
// Script encargado de reproducir el efecto de gravedad en el prefab aplicado
// Alejandro Garcia Diaz
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Gravity : MonoBehaviour
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
    /// Distancia vertical del salto
    /// </summary>
    [SerializeField] private float JumpHeight = 2.0f;
    /// <summary>
    /// Tiempo necesario para alcanzar la altura máxima
    /// </summary>
    [SerializeField] private float TimeToReachMaxHeight = 0.5f;
    /// <summary>
    /// Velocidad maxima alcanzada al caer
    /// </summary>
    [SerializeField] private float MaxSpeedDown = 1.0f;
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
    /// Simula la velocidad afectada por la gravedad
    /// </summary>
    private float _speed = 0.0f;
    /// <summary>
    /// Indica si el detector de suelo dice que estás en el suelo
    /// </summary>
    private bool _landed;

    private bool _gravityOn = true;


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
        _initialSpeed = (2 * JumpHeight) / TimeToReachMaxHeight;
        _gravity = _initialSpeed / TimeToReachMaxHeight;
    }

    void FixedUpdate()
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!_gravityOn) return;

        _landed = FloorDetector.GetComponent<Detector>().Detected;

        // Mientras no toca suelo
        if (!_landed)
        {
            // mueve al personaje hacia abajo en base a la velocidad en el eje y
            this.transform.position -= new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;
            // Limitamos la velocidad de caída hasta la velocidad de impulso
            if (_speed < MaxSpeedDown / 1.1)
            {
                _speed += _gravity * Time.deltaTime;
            }
            else
            {
                _speed += _gravity / 2 * Time.deltaTime;
            }
        }
        // Cuando toco suelo
        else
        {
            Detector _floorDetector = FloorDetector.GetComponent<Detector>();
            // Calculamos la posicion del Jugador justo al lado de la pared
            float _posEncimaSuelo = _floorDetector.CollisionedObject.gameObject.transform.position.y + _floorDetector.CollisionedObject.bounds.size.y / 2;
            float _posY = _posEncimaSuelo + this.gameObject.GetComponent<Collider2D>().bounds.size.y / 2 + 0.1f;
            float _aumentoY = _posY - transform.position.y;

            // Movemos al personaje justo al lado de la pared

            // teletransporte
            if (transform.position.y != _posY) transform.position += new Vector3(.0f, _aumentoY);
            _speed = 0.0f;
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

    public void gravitySwitch()
    {
        _gravityOn = !_gravityOn;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Gravity 
// namespace
