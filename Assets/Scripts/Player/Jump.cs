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
public class Jump : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float Height = 2.0f; // Altura que alcanza el salto
    [SerializeField] private float MaxVelocity = 5.0f; // Velocidad del salto del personaje
    [SerializeField] private float Acceleration = 1.0f; // Aceleración del personaje en el salto
    [SerializeField] private float PlayerHeight = 3; // Altura del jugador (Necesaria para Raycast)
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float _speed = 0.0f; // Velocidad con aceleración caída
    private bool _hasJumped = false; // Booleano que comprueba si quiere saltar
    private bool _hasReachedMaxHeight = false; // Booleano que comprueba si se ha alcanzado la altura máxima
    private bool _hasEndedJump = false; // Booleano que comprueba si se ha terminado el salto
    private Vector3 _maxPosJumped; // Posicion más alta que alcanza el salto
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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Vector2 dir = InputManager.Instance.MovementVector;
        // De momento hacemos el salto con el movimiento en el eje y para no tocar el input action
        if (dir.y > 0.0f)
        {
            // Comprobamos si tiene suelo justo debajo
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0.0f, -1), PlayerHeight/2 + 0.05f);
            if (hit.collider != null)
            {
                // Iniciamos las variables necesarias
                _maxPosJumped = transform.position + new Vector3(0.0f, Height);
                _hasJumped = true;
                _hasEndedJump = false;
                _hasReachedMaxHeight = false;
            }
        }
        // Si ha saltado
        if (_hasJumped)
        {
            // Comprobamos si ha llegado a la altura máxima
            if (transform.position.y >= _maxPosJumped.y)
            {
                _hasReachedMaxHeight = true;
            }
            // Mientras no ha llegado a la altura máxima
            if (!_hasReachedMaxHeight)
            {
                // mueve al personaje en base a la velocidad en el eje y (sin aceleración al subir)
                this.transform.position += new Vector3(0.0f, 1.0f) * MaxVelocity * Time.deltaTime;
            }
            // Cuando ha llegado a la altura máxima y aún no ha terminado el salto
            else if (_hasReachedMaxHeight && !_hasEndedJump)
            {
                // Comprobamos si ha tocado algo con collider
                RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(0.0f, -1), PlayerHeight / 2 + 0.05f);
                if (hit.collider != null)
                {
                    // Terminamos salto
                    _hasEndedJump = true;
                }
                // mueve al personaje en base a la velocidad (_speed) en el eje y
                this.transform.position -= new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;

                // subimos la velocidad hasta que llega a la velocidad máxima
                _speed += Acceleration;
                if (_speed <= MaxVelocity)
                {
                    _speed += Acceleration;
                }
                else _speed = MaxVelocity;
                // mueve al personaje en base a la velocidad (_velocity) en el eje y
            }
            // Cuando se ha terminado el salto
            else
            {
                _hasJumped = false;
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
