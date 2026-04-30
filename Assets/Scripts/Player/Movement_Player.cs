//---------------------------------------------------------
// Script encargado del manejo del movimiento del personaje jugable.
// Alejandro Garcia && Colaboradores:
//      Gabriel Adrian Oltean
//      Víctor Román Román
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
public class Movement_Player : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Velocidad máxima horizontal
    /// </summary>
    [SerializeField] private float MaxVelocity = 5.0f;
    /// <summary>
    /// Aceleración horizontal
    /// </summary>
    [SerializeField] private float Acceleration = 1.0f;

    /// <summary>
    /// Prefab de la bala
    /// </summary>
    [SerializeField] private GameObject Exclamation;

    /// <summary>
    /// Detector derecho
    /// </summary>
    [SerializeField] private Detector RightDetector;
    /// <summary>
    /// Detector izquierdo
    /// </summary>
    [SerializeField] private Detector LeftDetector;


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
    /// Velocidad del personaje.
    /// </summary>
    private float _speed = .0f;

    /// <summary>
    /// Componente SpriteRenderer cacheado
    /// </summary>
    SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Indica si has sido empujado.
    /// </summary>
    private bool _empujado = false;
    /// <summary>
    /// Velocidad actual durante el empuje.
    /// </summary>
    private float _velEmpuje = 0.0f;
    /// <summary>
    /// Contiene la información del componente Animator.
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Controla si sólo está andando para mostrar la animación correcta.
    /// </summary>
    private bool _onlyWalking = true;

    /// <summary>
    /// Collider del personaje.
    /// </summary>
    private Collider2D _playerCollider;

    /// <summary>
    /// Indica si debemos impedir el siguiente movimiento.
    /// </summary>
    private bool _desactivated = false;

    /// <summary>
    /// Dirección a la que saldrá disparado el personaje al tocar un CactusMan.
    /// </summary>
    private Vector3 _dirEmpuje;
    /// <summary>
    /// Guarda el booleano detected del detector derecho
    /// </summary>
    private bool _rightDetected;
    /// <summary>
    /// Guarda el booleano detected del detector izquierdo
    /// </summary>
    private bool _leftDetected;

    /// <summary>
    /// Determina si el player está siendo empujado por el extra army.
    /// </summary>
    private bool _extraArmyEmpujando = false;

    /// <summary>
    /// Variable que contiene la información del componente Jump
    /// </summary>
    private Jump _jumpComponent;

    /// <summary>
    /// Variable que contiene la información del componente Shoot
    /// </summary>
    private Shoot _shootComponent;

    /// <summary>
    /// Variable que contiene la información del componente Scream_Reload
    /// </summary>
    private Scream_Reload _screamReloadComponent;
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

        // Cacheamos componentes a desactivar cuando queramos detener al jugador
        _jumpComponent = this.gameObject.GetComponent<Jump>();
        _shootComponent = this.gameObject.GetComponent<Shoot>();
        _screamReloadComponent = this.gameObject.GetComponent<Scream_Reload>();

        // Cacheamos el resto de componentes
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //  ---- Movimiento del Personaje ----
        float time = Time.deltaTime;
        _rightDetected = RightDetector.Detected();
        _leftDetected = LeftDetector.Detected();
        // direccion de movimiento
        Vector2 dir = InputManager.Instance.MovementVector;

        // en caso de moverse a la derecha
        if (dir.x > 0 && !_extraArmyEmpujando && !_empujado && !_rightDetected && !_desactivated)
        {
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_speed, 0.0f) * time;
            _spriteRenderer.flipX = false;
            if (_animator && _onlyWalking)
            {
                _animator.SetBool("IsWalkingAnim", true);
            }
            // Acelera al personaje o fija su velocidad si ha llegado al maximo de velocidad
            if (_speed <= MaxVelocity)
            {
                _speed += Acceleration;
            }
            else _speed = MaxVelocity;
        }
        // en caso de moverse a la izquierda
        else if (dir.x < 0 && !_empujado && !_leftDetected && !_desactivated)
        {
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_speed, 0.0f) * time;
            _spriteRenderer.flipX = true;
            if (_animator && _onlyWalking)
            {
                _animator.SetBool("IsWalkingAnim", true);
            }
            // Acelera al personaje o fija su velocidad si ha llegado al maximo de velocidad
            if (_speed >= -MaxVelocity)
            {
                _speed -= Acceleration;
            }
            else _speed = -MaxVelocity;
        }
        else
        {
            _speed = .0f;
            if (_animator)
            {
                _animator.SetBool("IsWalkingAnim", false);
            }
        }

        // ---- Detección de paredes ----

        if (_leftDetected && (_speed < 0 || _velEmpuje < 0))
        {
            _velEmpuje = 0;
            _speed = 0;
            // Calculamos la posicion del Jugador justo al lado de la pared
            float _posAlLadoPared = LeftDetector.GetCollisionedObjectPosition().x + LeftDetector.GetCollisionedObjectSize().x / 2;
            float _posX = _posAlLadoPared + _playerCollider.bounds.size.x / 2;
            float _aumentoX = _posX - transform.position.x;

            // Movemos al personaje justo al lado de la pared

            // teletransporte
            if (transform.position.x < _posX && math.abs(_aumentoX) < 1) transform.position += new Vector3(_aumentoX, 0.0f);
        }
        else if (_rightDetected && _speed > 0 && !_empujado)
        {
            _speed = 0;
            // Calculamos la posicion del Jugador justo al lado de la pared
            float _posAlLadoPared = RightDetector.GetCollisionedObjectPosition().x - RightDetector.GetCollisionedObjectSize().x / 2;
            float _posX = _posAlLadoPared - _playerCollider.bounds.size.x / 2;
            float _aumentoX = _posX - transform.position.x;

            // Movemos al personaje justo al lado de la pared

            // teletransporte
            if (transform.position.x > _posX && math.abs(_aumentoX) < 1) transform.position += new Vector3(_aumentoX, 0.0f);
        }

        // ---- Empuje del personaje ----

        if (_empujado)
        {
            // Fin del empuje por colisiones
            if ((_dirEmpuje.x > 0.0f && _rightDetected) || (_dirEmpuje.x < 0.0f && _leftDetected))
            {
                _empujado = false;
                _velEmpuje = 0.0f;
                _speed = 0;

                if (_dirEmpuje.x > 0.0f)
                {
                    // Calculamos la posicion del Jugador justo al lado de la pared
                    float _posAlLadoPared = RightDetector.GetCollisionedObjectPosition().x - RightDetector.GetCollisionedObjectSize().x / 2;
                    float _posX = _posAlLadoPared - _playerCollider.bounds.size.x / 2;
                    float _aumentoX = _posX - transform.position.x;

                    // Movemos al personaje justo al lado de la pared

                    // teletransporte
                    if (transform.position.x > _posX) transform.position += new Vector3(_aumentoX, 0.0f);
                }
                else
                {
                    // Calculamos la posicion del Jugador justo al lado de la pared
                    float _posAlLadoPared = LeftDetector.GetCollisionedObjectPosition().x + LeftDetector.GetCollisionedObjectSize().x / 2;
                    float _posX = _posAlLadoPared + _playerCollider.bounds.size.x / 2;
                    float _aumentoX = transform.position.x - _posX;

                    // Movemos al personaje justo al lado de la pared

                    // teletransporte
                    if (transform.position.x < _posX) transform.position -= new Vector3(_aumentoX, 0.0f);
                }
            }
            // Fin del empuje por velocidad
            else
            {
                // Movemos al personaje en base a la velocidad y la direccion de empuje
                this.transform.position += (_dirEmpuje * _velEmpuje) * time;

                // Aplicamos aceleración a la velocidad de empuje
                if (_velEmpuje > 0.0f)
                {
                    _velEmpuje -= Acceleration;
                }
                else _velEmpuje = 0.0f;

                // Indicamos que ya no estás siendo empujado si la velocidad es cero
                if (_velEmpuje == 0.0f) _empujado = false;
            }
        }

        // ====== Salto del personaje ======
        if (InputManager.Instance.JumpIsPressed())
        {
            _jumpComponent.TryStartJump();
        }

        // --- Menu de pausa activacion ---
        if (InputManager.Instance.PauseWasPressedThisFrame())
        {
            // Debug.Log("Pausando");
            LevelManager.Instance.Pausa();
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
    /// Método que inicia el empuje de CactusMAN
    /// </summary>
    public void Empuja(float emp, Vector3 dir)
    {
        _dirEmpuje = dir;
        if ((dir.x > 0.0f && !RightDetector.Detected()) || (dir.x < 0.0f && !LeftDetector.Detected()))
        {
            _velEmpuje = emp;
            _empujado = true;
        }
    }

    public float getMaxVel()
    {
        return MaxVelocity;
    }
    public void setMaxVel(float rlMovement)
    {
        MaxVelocity = rlMovement;
    }

    /// <summary>
    /// Comprueba si el personaje sólo está andando.
    /// </summary>
    public bool OnlyWalking(bool IsOnlyWalking)
    {
        _onlyWalking = IsOnlyWalking;
        return _onlyWalking;
    }

    /// <summary>
    /// Cambia el estado del player a empujado o no empujado
    /// </summary>
    public void ExtraArmyEstaEmpujando(bool EstaEmpujando)
    {
        _extraArmyEmpujando = EstaEmpujando;
    }
    /// <summary>
    /// Controla si el player está siendo empujado por un extra army
    /// </summary>
    public bool EstaSiendoEmpujado()
    {
        return _extraArmyEmpujando;
    }
    /// <summary>
    /// Devuelve si detecta algo el detector izquierdo del player
    /// </summary>
    public bool LeftDetect()
    {
        return _leftDetected;
    }
    /// <summary>
    /// Desactiva el movimiento, el salto, el disparo y la recarga
    /// </summary>
    public void DisablePlayer()
    {
        _jumpComponent.DesactivateJump();
        _shootComponent.DesactivateShoot();
        _screamReloadComponent.DesactivateReload();
        _desactivated = true;
    }
    /// <summary>
    /// Activa el movimiento, el salto, el disparo y la recarga
    /// </summary>
    public void ActivatePlayer()
    {

        _jumpComponent.ActivateJump();
        _shootComponent.ActivateShoot();
        _screamReloadComponent.ActivateReload();
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

} // class Movement_Player 
// namespace
