//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using Unity.Mathematics;
using UnityEngine;
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
    /// <summary>
    /// Objeto del jugador
    /// </summary>
    [SerializeField] private GameObject Player;
    /// <summary>
    /// Distancia del centro del extra army al sitio donde va a quedar el player cuando le empuje
    /// </summary>
    [SerializeField] private float OffsetEmpujePlayer = 0f;
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
    /// Velocidad inicial del salto
    /// </summary>
    private float _jumpSpeed;
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
    /// <summary>
    /// Contiene la información del script Movement_Player
    /// </summary>
    private Movement_Player _playerMovement;
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
        _playerMovement = Player.GetComponent<Movement_Player>();

        // ====== Guardamos la altura del extra ======
        _objectHeight = GetComponent<Collider2D>().bounds.size.y;

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Se guarda la dirección del input para poder salir del empuje del extra army.
        Vector2 dir = InputManager.Instance.MovementVector;
        if (_warning.GetDone() && !_hasFinishedJump)
        {
            //Si el front detector detecta el player y no se pulsa ni salto ni moverse hacia la izquierda.
            if(FrontDetector.Detected() && FrontDetector.CollisionIsPlayer() && Player != null && !InputManager.Instance.JumpIsPressed() && dir.x >= 0)
            {
                float NuevaPosX = transform.position.x - OffsetEmpujePlayer;
                Player.transform.position = new Vector3(NuevaPosX, Player.transform.position.y);
                _playerMovement.ExtraArmyEstaEmpujando(true);
            }
            else if (FrontDetector.Detected() && FloorDetector.Detected() && !FrontDetector.CollisionIsPlayer())
            {
                _obstacleSorted = false;
                StartJump();
            }
            else if (!FrontDetector.Detected() && !_obstacleSorted)
            {
                _obstacleSorted = true;
                if(Player != null)
                {
                    _playerMovement.ExtraArmyEstaEmpujando(false);
                }
            }

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

                    // ====== Situamos al jugador encima del suelo cuando esta atravesándolo ======

                    // Calculamos la posicion del Jugador justo encima del suelo
                    float _posEncimaSuelo = FloorDetector.GetCollisionedObjectPosition().y + FloorDetector.GetCollisionedObjectSize().y / 2;
                    float _posY = _posEncimaSuelo + _objectHeight / 2;
                    float _aumentoY = _posY - transform.position.y;

                    // teletransporte si no es muy basto
                    if ((math.abs(_aumentoY) < _objectHeight) && (transform.position.y < _posY)) transform.position += new Vector3(.0f, _aumentoY);

                    if (!_obstacleSorted && !FrontDetector.Detected())
                    {
                        _obstacleSorted = true;
                    }
                    // ====== Actualizamos variable de caida ======
                    _falling = false;


                    if (!_goingUp && !_obstacleSorted)
                    {
                        _hasFinishedJump = true;
                    }
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
                    if (!FrontDetector.Detected())
                    {
                        _obstacleSorted = true;
                    }
                }
                // Si has llegado al punto más alto la velocidad ya es 0 (o muy cerca)
                else if (_speed <= 0.0f)
                {
                    // ====== Terminamos la subida ======
                    _goingUp = false;
                    _falling = true;
                    if (!FrontDetector.Detected())
                    {
                        _obstacleSorted = true;
                    }
                }
            }

            // ====== Movemos al personaje en el eje Y según la velocidad ======
            if (_speed != 0) this.transform.position += new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;
            if (_obstacleSorted) transform.position += new Vector3(-1f, 0f) * SpeedExtra * Time.deltaTime;
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
        if (!_goingUp)
        {
            // Iniciamos las variables
            _goingUp = true;
            _speed = _jumpSpeed;
        }

    }
    #endregion

    // class Extra_Army 
    // namespace
}

