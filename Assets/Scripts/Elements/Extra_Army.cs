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
    /// Trigger que detecta si tiene un bloque delante para saltar
    /// </summary>
    [SerializeField] private Detector FrontDetector;
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
    /// <summary>
    /// Componente del salto del extra army
    /// </summary>
    private Jump _jumpComponent;
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
        _obstacleSorted = true;
        _playerMovement = Player.GetComponent<Movement_Player>();
        _jumpComponent = this.gameObject.GetComponent<Jump>();
        // ====== Guardamos la altura del extra ======

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
            // ====== Empuje al Jugador ======

            // Si el front detector detecta el player y no se pulsa ni salto ni moverse hacia la izquierda y además no se detecta una pared a la izquierda.
            if (FrontDetector.Detected() && FrontDetector.CollisionIsPlayer() && Player != null && !InputManager.Instance.JumpIsPressed() && dir.x >= 0 && !_playerMovement.LeftDetect())
            {
                Player.transform.position += new Vector3(-1f, 0f) * SpeedExtra * Time.deltaTime;
                _playerMovement.ExtraArmyEstaEmpujando(true);
            }

            else if (!FrontDetector.Detected() || (FrontDetector.Detected() && !FrontDetector.CollisionIsPlayer()))
            {
                _playerMovement.ExtraArmyEstaEmpujando(false);
            }

            // ====== Salto del extra army ======
            if (FrontDetector.Detected() && !FrontDetector.CollisionIsPlayer())
            {
                if (_jumpComponent != null)
                {
                    _jumpComponent.TryStartJump();
                }
                _obstacleSorted = false;
            }
            else if (!FrontDetector.Detected() && !_obstacleSorted)
            {
                _obstacleSorted = true;
            }
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
    #endregion

    // class Extra_Army 
    // namespace
}

