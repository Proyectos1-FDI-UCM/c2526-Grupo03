//---------------------------------------------------------
// Script encargado del manejo del movimiento del personaje jugable
// Alejandro Garcia
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System;
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

    [SerializeField] private float MaxVelocity = 5.0f;
    [SerializeField] private float Acceleration = 1.0f;

    [SerializeField] private GameObject Exclamation;

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
    private bool _rotation = false; // Rotacion/direccion en la que mira el personaje (basicamente si mira a la derecha = false o a la izquierda = true)

    private float _cooldown = .8f; // Tiempo de espera entre disparo y disparo
    private float _timeToWait = .0f; // Tiempo en el que se podra volver a disparar
    private Vector3 _bulletPositionOffset = new Vector3(2.0f, 0); // Posicion de la bala en relacion al personaje

    SpriteRenderer _spriteRenderer;
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
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(InputManager.Instance.FireWasPressedThisFrame() && Time.time >= _timeToWait)
        {
            int direction = _rotation ? -1 : 1;
            // Crea una exclamacion un poco alejado del personaje (_bulletPositionOffset) sin rotacion
            GameObject obj = Instantiate(Exclamation, this.transform.position + (_bulletPositionOffset * direction), new Quaternion());
            // Dependiendo de la direccion se dibuja en modo espejo o no
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            sr.flipX = _rotation;
            // Reestablece el tiempo de espera al siguiente disparo
            _timeToWait = Time.time + _cooldown;
        }
    }
    void FixedUpdate()
    {
        //  ---- Movimiento del Personaje ----

        // direccion de movimiento
        Vector2 dir = InputManager.Instance.MovementVector;

        // en caso de moverse a la derecha
        if (dir.x > 0)
        {
            // Guarda la direccion en la que mira el personaje
            _rotation = false;
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_speed, 0.0f) * Time.deltaTime;
            _spriteRenderer.flipX = false;

            // Acelera al personaje o fija su velocidad si ha llegado al maximo de velocidad
            _speed += Acceleration;
            if (_speed <= MaxVelocity)
            {
                _speed += Acceleration;
            }
            else _speed = MaxVelocity;

        }
        // en caso de moverse a la izquierda
        else if (dir.x < 0)
        {
            // Guarda la direccion en la que mira el personaje
            _rotation = true;
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_speed, 0.0f) * Time.deltaTime;
            _spriteRenderer.flipX = true;

            // Acelera al personaje o fija su velocidad si ha llegado al maximo de velocidad
            if (_speed >= -MaxVelocity)
            {
                _speed -= Acceleration;
            }
            else _speed = -MaxVelocity;
        }
        else _speed = .0f;
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

} // class Movement_Player 
// namespace
