//---------------------------------------------------------
// Componente encargado del movimiento y salto del extra army
// Marcos Tedín Cueto && Colaboradores:
//      Gabriel Adrian Oltean
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
    private bool _obstacleSorted = true;
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
        // cacheamos componentes
        _warning = GetComponentInChildren<Warning>();
        _playerMovement = Player.GetComponent<Movement_Player>();
        _jumpComponent = this.gameObject.GetComponent<Jump>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Se guarda la dirección del input para poder salir del empuje del extra army.
        Vector2 dir = InputManager.Instance.MovementVector;
        if (_warning.GetDone())
        {
            // ====== Empuje al Jugador ======

            // Si el front detector detecta el player y no se pulsa ni salto ni moverse hacia la izquierda y además no se detecta una pared a la izquierda.
            if (FrontDetector.Detected() && FrontDetector.CollisionIsPlayer() && Player != null && !InputManager.Instance.JumpIsPressed() && dir.x >= 0 && !_playerMovement.LeftDetect())
            {
                // Movemos al jugador en la misma direccion y velocidad que el extra army
                Player.transform.position += new Vector3(-1f, 0f) * SpeedExtra * Time.deltaTime;
                // indicamos al movimiento del jugador que esta siendo empujado para que no pueda moverse a la derecha
                _playerMovement.ExtraArmyEstaEmpujando(true);
            }
            // Cuando deja de detectar al jugador
            else if (!FrontDetector.Detected() || (FrontDetector.Detected() && !FrontDetector.CollisionIsPlayer()))
            {

                // indicamos al movimiento del jugador que ya no esta siendo empujado para que pueda volver a moverse a la derecha
                _playerMovement.ExtraArmyEstaEmpujando(false);
            }

            // ====== Salto del extra army ======
            if (FrontDetector.Detected() && !FrontDetector.CollisionIsPlayer())
            {
                // Saltamos si tiene el componente
                if (_jumpComponent != null)
                {
                    _jumpComponent.TryStartJump();
                }
                // Impedimos el movimiento horizontal del enemigo
                _obstacleSorted = false;
            }
            else if (!FrontDetector.Detected() && !_obstacleSorted)
            {
                // Permitimos el movimiento horizontal del enemigo
                _obstacleSorted = true;
            }
            // Movemos al enemigo si no tiene obstáculo delante
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

