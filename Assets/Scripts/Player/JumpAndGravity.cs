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
public class JumpAndGravity : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float JumpHeight = 2.0f; // Altura que alcanza el salto
    [SerializeField] private float JumpDuration = 1.0f; // Velocidad del salto del personaje
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float _initialSpeed = 0.0f;
    private float _gravity = 0.0f; // Aceleración del personaje en el salto
    private float _playerHeight = 0.0f;
    private float _playerWidth = 0.0f;
    private float _speed = 0.0f; // Velocidad con aceleración caída
    private bool _hasJumped = false; // Booleano que comprueba si quiere saltar
    private bool _hasReachedMaxHeight = false; // Booleano que comprueba si se ha alcanzado la altura máxima
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
        _playerHeight = GetComponent<BoxCollider2D>().bounds.size.y;
        _playerWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        _initialSpeed = (2 * JumpHeight) / JumpDuration;
        _gravity = _initialSpeed / JumpDuration;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector2 dir = InputManager.Instance.MovementVector;
        // De momento hacemos el salto con el movimiento en el eje y para no tocar el input action
        if (dir.y > 0.5f)
        {
            // Comprobamos si estás encima de algo sólido (algo con collider que no es trigger)
            if (IsGrounded())
            {
                // Iniciamos las variables necesarias
                _maxPosJumped = transform.position + new Vector3(0.0f, JumpHeight);
                _hasJumped = true;
                _hasReachedMaxHeight = false;
                _speed = _initialSpeed;
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
                // mueve al personaje hacia arriba en base a la velocidad en el eje y (sin aceleración al subir)
                this.transform.position += new Vector3(0.0f, 1.0f) * _speed * Time.deltaTime;
                _speed -= _gravity * Time.deltaTime;
            }
            // Cuando se ha terminado el salto
            else
            {
                _hasJumped = false;
            }
        }
        // Parte de gravedad
    }
    private bool IsGrounded()
    {
        bool grounded = false;
        // Puntos de lanzamiento de los rayos
        Vector3 BottomCenter = transform.position - new Vector3(0.0f, _playerHeight / 2);
        Vector3 BottomLeft = transform.position - new Vector3(_playerWidth / 2, _playerHeight / 2);
        Vector3 BottomRight = transform.position - new Vector3(-1*(_playerWidth / 2), _playerHeight / 2);

        float RaycastMagnitude = 0.1f;
        // Comprobamos si tiene suelo justo debajo en el centro
        RaycastHit2D hit2 = Physics2D.Raycast(BottomCenter, new Vector3(0.0f, -1), RaycastMagnitude);

        // Comprobamos si tiene suelo justo debajo de la esquina inferior izquierda
        RaycastHit2D hit = Physics2D.Raycast(BottomLeft, new Vector3(0.0f, -1), RaycastMagnitude);

        // Comprobamos si tiene suelo justo debajo de la esquina inferior derecha
        RaycastHit2D hit3 = Physics2D.Raycast(BottomRight, new Vector3(0.0f, -1), RaycastMagnitude);

        // Comprobamos si hay algo debajo
        if ((hit.collider != null && !hit.collider.isTrigger) || (hit2.collider != null && !hit2.collider.isTrigger) || (hit3.collider != null && !hit3.collider.isTrigger))
        {
            grounded = true;
        }
        return grounded;
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
