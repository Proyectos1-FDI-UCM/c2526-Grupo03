//---------------------------------------------------------
// Script encargado del manejo del movimiento del personaje jugable.
// Alejandro Garcia && Aportadores:
//      - Gabriel Adrian Oltean
//      - Víctor Román Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
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

    [SerializeField] private float MaxVelocity = 5.0f;
    [SerializeField] private float Acceleration = 1.0f;

    [SerializeField] private GameObject Exclamation;

    [SerializeField] private GameObject RightDetector;
    [SerializeField] private GameObject LeftDetector;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _speed = .0f; // Velocidad del personaje

    SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Indica si has sido empujado
    /// </summary>
    private bool _empujado = false;
    /// <summary>
    /// Velocidad actual durante el empuje
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
    /// Indica si debemos impedir el siguiente movimiento
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
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //  ---- Movimiento del Personaje ----

        // direccion de movimiento
        Vector2 dir = InputManager.Instance.MovementVector;

        // en caso de moverse a la derecha
        if (dir.x > 0 && !RightDetector.GetComponent<Detector>().Detected && !_empujado && !_desactivated)
        {
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_speed, 0.0f) * Time.deltaTime;
            _spriteRenderer.flipX = false;
            if(_animator && _onlyWalking)
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
        else if (dir.x < 0 && !LeftDetector.GetComponent<Detector>().Detected && !_desactivated)
        {
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_speed, 0.0f) * Time.deltaTime;
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

        if (LeftDetector.GetComponent<Detector>().Detected)
        {
            Detector _leftDetector = LeftDetector.GetComponent<Detector>();
            // Calculamos la posicion del Jugador justo al lado de la pared
            float _posAlLadoPared = _leftDetector.GetCollisionedObjectPosition().x + _leftDetector.GetCollisionedObjectSize().x / 2;
            float _posX = _posAlLadoPared + this.gameObject.GetComponent<Collider2D>().bounds.size.x / 2;
            float _aumentoX = _posX - transform.position.x;

            // Movemos al personaje justo al lado de la pared

            // teletransporte
            if (transform.position.x < _posX && math.abs(_aumentoX) < 1) transform.position += new Vector3(_aumentoX, 0.0f);
        }
        else if (RightDetector.GetComponent<Detector>().Detected && !_empujado)
        {
            Detector _rightDetector = RightDetector.GetComponent<Detector>();
            // Calculamos la posicion del Jugador justo al lado de la pared
            float _posAlLadoPared = _rightDetector.GetCollisionedObjectPosition().x - _rightDetector.GetCollisionedObjectSize().x / 2;
            float _posX = _posAlLadoPared - this.gameObject.GetComponent<Collider2D>().bounds.size.x / 2;
            float _aumentoX = _posX - transform.position.x;

            // Movemos al personaje justo al lado de la pared

            // teletransporte
            if (transform.position.x > _posX && math.abs(_aumentoX) < 1) transform.position += new Vector3(_aumentoX, 0.0f);
        }

        // ---- Empuje del personaje ----

        if (_empujado)
        {
            if (_velEmpuje >= 0.0f)
            {
                _empujado = false;
            }
            else if (LeftDetector.GetComponent<Detector>().Detected)
            {
                _velEmpuje = 0.0f;
                _empujado = false;
            }
            else
            {

                this.transform.position += new Vector3(_velEmpuje, 0.0f) * Time.deltaTime;

                if (_velEmpuje < 0.0f)
                {
                    _velEmpuje += Acceleration;
                }
                else _velEmpuje = 0.0f;
            }
            // --- Menu de pausa activacion ---

            

        }
        if (InputManager.Instance.PauseWasPressedThisFrame())
            {
                // Debug.Log("Pausando");
                LevelManager.Instance._Pausa();
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

    public void Empuja(int emp)
    {
        if (!LeftDetector.GetComponent<Detector>().Detected)
        {
            _velEmpuje = -emp;
        }
        _empujado = true;
        
    }

    public float getMaxVel()
    {
        return MaxVelocity;
    }
    public void setMaxVel(float rlMovement)
    {
        MaxVelocity = rlMovement;
    }

    public bool OnlyWalking(bool IsOnlyWalking)
    {
        _onlyWalking = IsOnlyWalking;
        return _onlyWalking;
    }

    public void DesactivateMovement()
    {
        _desactivated = true;
    }
    public void ActivateMovement()
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

} // class Movement_Player 
// namespace
