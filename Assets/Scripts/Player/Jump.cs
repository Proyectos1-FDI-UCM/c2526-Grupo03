//---------------------------------------------------------
// Componente encargado del salto de cualquier personaje.
// Gabriel Adrian Oltean && Colaboradores:
//      Alejandro Garcia Diaz
//      Víctor Román Román
// && Colaboradores: Tristán Sánchez López 
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using Unity.Mathematics;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Jump : MonoBehaviour
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
    [SerializeField] private Detector FloorDetector;
    /// <summary>
    /// Trigger que detecta si has tocado el techo
    /// </summary>
    [SerializeField] private Detector RoofDetector;

    /// <summary>
    /// Distancia vertical del salto
    /// </summary>
    [SerializeField] private float JumpHeight;

    /// <summary>
    /// Tiempo para llegar a la altura máxima
    /// </summary>
    [SerializeField] private float TimeToReachMaxHeight;

    /// <summary>
    /// Velocidad máxima de caída
    /// </summary>
    [SerializeField] private float MaxFallSpeed;

    /// <summary>
    /// Sonido de salto
    /// </summary>
    [SerializeField] private AudioSource JumpSound;
    /// <summary>
    /// Sonido de aterrizaje
    /// </summary>
    [SerializeField] private AudioSource LandSound;
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
    /// Velocidad inicial del salto
    /// </summary>
    private float _jumpSpeed;
    /// <summary>
    /// Velocidad afectada por la gravedad
    /// </summary>
    private float _speed = 0.0f;
    /// <summary>
    /// Aceleración negativa con la que simulamos gravedad
    /// </summary>
    private float _gravity = 0.0f;

    /// <summary>
    /// Altura del Objeto asignado
    /// </summary>
    private float _objectHeight;

    /// <summary>
    /// Comprueba si has saltado y aun estas subiendo
    /// </summary>
    private bool _goingUp = false;
    /// <summary>
    /// Comprueba si estas cayendo
    /// </summary>
    private bool _falling = false;

    /// <summary>
    /// Contiene la información del componente Animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Componente del Jugador asociado
    /// </summary>
    private Movement_Player _movement_Player;

    /// <summary>
    /// Dice si se esta detectando suelo (Detectamos en el fixed)
    /// </summary>
    private bool _floorDetected;
    /// <summary>
    /// Dice si se está detectando techo (Detectamos en el fixed)
    /// </summary>
    private bool _roofDetected;
    /// <summary>
    /// Indica si debemos impedir el siguiente salto por componentes externos
    /// </summary>
    private bool _desactivated = false;
    /// <summary>
    /// Comprueba si el jugador está saltando
    /// </summary>
    private bool _isJumping = false;
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
        // ====== Cálculo de Gravedad y velocidad de impulso ======
        _gravity = (2 * JumpHeight) / math.pow(TimeToReachMaxHeight, 2);
        _jumpSpeed = _gravity * TimeToReachMaxHeight;


        // ====== Guardamos la altura del jugador ======
        _objectHeight = GetComponent<Collider2D>().bounds.size.y;

        // ====== Guardamos el componente del animator ======
        _animator = GetComponent<Animator>();

        _movement_Player = GetComponent<Movement_Player>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // ====== Variable que guarda el tiempo ======
        float time = Time.deltaTime;

        // ====== Detecciones del motor de físicas ======
        _floorDetected = FloorDetector.Detected();
        _roofDetected = RoofDetector.Detected();

        // ====== Parte de caida y gravedad ======
        if (!_floorDetected || _speed > 0.0f)
        {
            // Si has dejado de subir
            if (!_goingUp)
            {
                _falling = true;
                // ====== Animacion de caida ======
                if (_animator && _movement_Player != null)
                {
                    _animator.SetBool("IsJumpingAnim", true);
                    _animator.SetBool("IsFalling", true);
                    _animator.SetBool("IsWalkingAnim", false);
                    _animator.SetBool("Landed", false);
                    _movement_Player.OnlyWalking(false);
                }
            }
            // Aplicamos gravedad a la velocidad
            if (_speed > -MaxFallSpeed / 1.1)
            {
                _speed -= _gravity * time;
            }
            else
            {
                _speed -= _gravity / 2 * time;
            }
        }
        // ====== Parte de aterrizaje ======
        if (_floorDetected)
        {
            // ====== Si estas cayendo ======
            if (_falling)
            {
                // ====== Velocidad a cero para no seguir cayendo ======
                _speed = 0.0f;

                // ====== Quitamos animacion de caida ======
                if (_animator && _movement_Player != null)
                {
                    _animator.SetBool("IsFalling", false);
                    _animator.SetBool("IsWalkingAnim", true);
                    _animator.SetBool("Landed", true);
                    _animator.SetBool("IsJumpingAnim", false);
                    _movement_Player.OnlyWalking(true);
                }

                // ====== Situamos al jugador encima del suelo cuando esta atravesándolo ======

                // Calculamos la posicion del Jugador justo encima del suelo
                float _posEncimaSuelo = FloorDetector.GetCollisionedObjectPosition().y + FloorDetector.GetCollisionedObjectSize().y / 2;
                float _posY = _posEncimaSuelo + _objectHeight / 2;
                float _aumentoY = _posY - transform.position.y;

                // teletransporte si no es muy basto
                if ((math.abs(_aumentoY) < _objectHeight) && (transform.position.y < _posY)) transform.position += new Vector3(.0f, _aumentoY);

                // ====== Actualizamos variable de caida ======
                _falling = false;

                if (LandSound != null) LandSound.Play();
                _isJumping = false;
            }
        }

        // ====== Comprobaciones de subida ======
        if (_goingUp)
        {
            if (_roofDetected)
            {
                // Cambiamos la velocidad a 0
                _speed = 0.0f;

                // ====== Situamos al jugador debajo del techo cuando esta atravesándolo ======

                // Calculamos la posicion del Jugador justo debajo del techo
                float _posDebajoTecho = RoofDetector.GetCollisionedObjectPosition().y - RoofDetector.GetCollisionedObjectSize().y / 2;
                float _posY = _posDebajoTecho - _objectHeight / 2;
                float _aumentoY = _posY - transform.position.y;

                // teletransporte
                if ((math.abs(_aumentoY) < _objectHeight) && (transform.position.y > _posY)) transform.position += new Vector3(.0f, _aumentoY);

                // ====== Terminamos la subida ======
                _goingUp = false;
                _falling = true;

                // ====== Cambiamos a animación de caida ======
                if (_animator && _movement_Player != null)
                {
                    _animator.SetBool("IsFalling", true);
                    _animator.SetBool("Landed", false);
                    _movement_Player.OnlyWalking(false);
                }
            }
            // Si has llegado al punto más alto la velocidad ya es 0 (o muy cerca)
            else if (_speed <= 0.0f)
            {
                // ====== Terminamos la subida ======
                _goingUp = false;
                _falling = true;

                // ====== Cambiamos a animación de caida ======
                if (_animator && _movement_Player != null)
                {
                    _animator.SetBool("IsFalling", true);
                    _animator.SetBool("Landed", false);
                    _movement_Player.OnlyWalking(false);
                }
            }
        }
        // ====== Movemos al personaje en el eje Y según la velocidad ======
        if (_speed != 0) this.transform.position += new Vector3(0.0f, 1.0f) * _speed * time;
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
    /// Método que impide el salto
    /// </summary>
    public void DesactivateJump()
    {
        _desactivated = true;
    }
    /// <summary>
    /// Método que activa el salto
    /// </summary>
    public void ActivateJump()
    {
        _desactivated = false;
    }

    public bool IsJumping()
    {
        return _isJumping;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Comprueba si puede iniciar el salto y lo inicia en caso afirmativo
    /// </summary>
    public void TryStartJump()
    {
        // Comprobamos si quiere y puede saltar
        if (!_desactivated && _floorDetected && !_falling && !_goingUp)
        {
            //Animacion de salto empieza
            if (_animator && _movement_Player != null)
            {
                _animator.SetBool("IsJumpingAnim", true);
                _animator.SetBool("IsFalling", false);
                _animator.SetBool("IsWalkingAnim", false);
                _animator.SetBool("Landed", false);
                _movement_Player.OnlyWalking(false);
            }

            // Iniciamos las variables
            _goingUp = true;
            _isJumping = true;
            _speed = _jumpSpeed;
            if (JumpSound != null) JumpSound.Play();
        }
    }
    #endregion

} // class Jump 
// namespace
