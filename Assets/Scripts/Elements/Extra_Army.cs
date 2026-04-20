//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System;
using UnityEngine;
using Unity.Mathematics;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Extra_Army : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Velocidad del extra army
    /// </summary>
    [SerializeField] private float SpeedExtra = 2f;
    /// <summary>
    /// Trigger que detecta si estás en el suelo
    /// </summary>
    [SerializeField] private Detector FloorDetector;
    /// <summary>
    /// Trigger que detecta si has tocado el techo
    /// </summary>
    [SerializeField] private Detector RoofDetector;
    /// <summary>
    /// Trigger que detecta si tiene un bloque delante para saltar
    /// </summary>
    [SerializeField] private Detector FrontDetector;
    /// <summary>
    /// Distancia vertical del salto
    /// </summary>
    [SerializeField] private float JumpHeight = 2.0f;
    /// <summary>
    /// Tiempo necesario para alcanzar la altura máxima
    /// </summary>
    [SerializeField] private float TimeToReachMaxHeight = 0.5f;
    /// <summary>
    /// Velocidad máxima de caíad
    /// </summary>
    [SerializeField] private float MaxFallSpeed;
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
    /// Simula la velocidad afectada por la gravedad
    /// </summary>
    private float _speed = 0.0f;
    /// <summary>
    /// Aceleración negativa con la que simulamos gravedad
    /// </summary>
    private float _gravity = 0.0f;
    /// <summary>
    /// Comprueba si está en un salto
    /// </summary>
    private bool _hasJumped = false;
    /// <summary>
    /// Altura a la que debe llegar el salto
    /// </summary>
    private Vector3 _maxPosJumped;
    /// <summary>
    /// Velocidad inicial del salto
    /// </summary>
    private float _jumpSpeed;
    /// <summary>
    /// Altura del Objeto asignado
    /// </summary>
    private float _objectHeight;
    /// <summary>
    /// Comprueba si ha terminado de subir
    /// </summary>
    private bool _stopGoingUp = false;
    /// <summary>
    /// Dice si se esta detectando suelo (Detectamos en el fixed)
    /// </summary>
    private bool _floorDetected;
    /// <summary>
    /// Dice si se está detectando techo (Detectamos en el fixed)
    /// </summary>
    private bool _roofDetected;
    /// <summary>
    /// Booleano que detecta si ha conseguido saltar el obstáculo
    /// </summary>
    private bool _obstacleSorted;
    /// <summary>
    /// Booleano que detecta si no ha conseguido saltar el obstáculo y ha llegado al suelo
    /// </summary>
    private bool _hasFinishedJump;
    /// <summary>
    /// Se sacará si se puede mover extra_army o no desde aquí
    /// </summary>
    private Warning _warning;
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
        _warning = GetComponentInChildren<Warning>();
        _gravity = (2 * JumpHeight) / math.pow(TimeToReachMaxHeight, 2);
        _jumpSpeed = _gravity * TimeToReachMaxHeight;
        _obstacleSorted = true;
        _hasFinishedJump = false;

        // ====== Guardamos la altura del extra ======
        _objectHeight = GetComponent<Collider2D>().bounds.size.y;

    }

    /// <summary>
    /// FixedUpdate is called many times every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        // ====== Detecciones del motor de físicas ======
        _floorDetected = FloorDetector.Detected();
        _roofDetected = RoofDetector.Detected();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_warning.GetDone() && !_hasFinishedJump)
        {
            if (FrontDetector.Detected() && FloorDetector.Detected())
            {
                StartJump();
            }

            // ====== Comprobaciones de subida ======
            if (_hasJumped)
            {
                // ====== Comprobaciónes terminación de subida ======
                if (!_stopGoingUp)
                {
                    if (_roofDetected)
                    {
                        // ====== Situamos al extra army debajo del techo cuando esta atravesándolo ======

                        // Cambiamos la velocidad a 0
                        _speed = 0.0f;

                        // Calculamos la posicion del extra army justo debajo del techo
                        float _posDebajoTecho = RoofDetector.GetCollisionedObjectPosition().y - RoofDetector.GetCollisionedObjectSize().y / 2;
                        float _posY = _posDebajoTecho - _objectHeight / 2;
                        float _aumentoY = _posY - transform.position.y;

                        // teletransporte
                        if ((math.abs(_aumentoY) < 0.8) && (transform.position.y > _posY)) transform.position += new Vector3(.0f, _aumentoY);

                        // ====== Terminamos la subida ======

                        _stopGoingUp = true;
                        if (FrontDetector.Detected())
                        {
                            _obstacleSorted = false;
                        }
                    }
                    // Si has llegado al punto más alto la velocidad ya es 0 (o muy cerca)
                    else if (transform.position.y >= _maxPosJumped.y)
                    {
                        _stopGoingUp = true;
                        if (FrontDetector.Detected())
                        {
                            _obstacleSorted = false;
                        }
                    }
                }
            }
            // ====== Comprobaciones de caída y aceleración ======
            if (!_floorDetected)
            {


                // Aplicamos gravedad a la velocidad
                if (_speed > -MaxFallSpeed / 1.1)
                {
                    _speed -= _gravity * Time.deltaTime;
                }
                else
                {
                    _speed -= _gravity / 2 * Time.deltaTime;
                }
            }
            // Si estabas cayendo y detectas el suelo
            else if (_speed < 0.0f)
            {
                // ====== Cambiamos la velocidad a 0 para que no siga bajando ======

                _speed = 0.0f;


                // ====== Situamos al extra army encima del suelo cuando esta atravesándolo ======

                // Calculamos la posicion del extra army justo encima del suelo
                float _posEncimaSuelo = FloorDetector.GetCollisionedObjectPosition().y + FloorDetector.GetCollisionedObjectSize().y / 2;
                float _posY = _posEncimaSuelo + _objectHeight / 2;
                float _aumentoY = _posY - transform.position.y;

                // teletransporte si no es muy basto
                if ((math.abs(_aumentoY) < 0.8) && (transform.position.y < _posY)) transform.position += new Vector3(.0f, _aumentoY);

                // ====== Permitimos el siguiente salto ======

                _hasJumped = false;
                if (!_obstacleSorted)
                {
                    _hasFinishedJump = true;
                }


            }
            // ====== Movemos al personaje en el eje Y según la velocidad ======
            if (_speed != 0) this.transform.position += new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;
            if (_obstacleSorted)transform.position += new Vector3(-1f, 0f) * SpeedExtra * Time.deltaTime;
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
    /// <summary>
    /// Cambia la velocidad actual a la velocidad de impulso calculada mediante los atributos del inspector
    /// </summary>
    private void StartJump()
    {
        if (!_hasJumped)
        {
            // Iniciamos las variables necesarias
            _maxPosJumped = transform.position + new Vector3(0.0f, 1.0f) * JumpHeight;
            _hasJumped = true;
            _stopGoingUp = false;
            _speed = _jumpSpeed;
        }
       
    }
    #endregion

    // class Extra_Army 
    // namespace
}

