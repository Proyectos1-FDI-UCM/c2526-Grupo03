//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class MainCameraMovement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// GameObject del Player
    /// </summary>
    [SerializeField] private GameObject Player;

    /// <summary>
    /// Suavizado del movimiento de la camara
    /// </summary>
    [SerializeField] float SmoothSpeed = 5f;

    /// <summary> 
    /// Distancia maxima a la que se adelanta la camara 
    /// </summary>
    [SerializeField] float LookAheadDistance = 2f;

    /// <summary>
    /// Altura fija de la camara
    /// </summary>
    [SerializeField] float VerticalOffset = 1f;
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
    /// Como de lejos vamos mirando delante
    /// </summary>
    private float _currentLookAhead = 0f;
    /// <summary>
    /// Posicin adelantada a la que hay que llegar
    /// </summary>
    private float _targetLookAhead = 0f;
    /// <summary>
    /// Velocidad que mira hacia delante 
    /// </summary>
    private float _lookAheadVelocity = 0f;
    /// <summary>
    /// Posicion en X de el objeto que suigue
    /// </summary>
    private float _lastTargetX;
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
        //Calculamos donde esta el player en X
        _lastTargetX = Player.transform.position.x;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Direcion de movimiento del player
        float moveDirection = Player.transform.position.x - _lastTargetX;

        // Detectar dirección del movimiento
        if (Mathf.Abs(moveDirection) > 0.01f)
        {
            _targetLookAhead = Mathf.Sign(moveDirection) * LookAheadDistance;
        }

        // Suavizar el adelantamiento
        _currentLookAhead = Mathf.SmoothDamp(
            _currentLookAhead,
            _targetLookAhead,
            ref _lookAheadVelocity,
            0.3f
        );

        // Posición objetivo
        Vector3 targetPosition = new Vector3(
            Player.transform.position.x + _currentLookAhead,
            Player.transform.position.y + VerticalOffset,
            transform.position.z
        );

        // Movimiento suave de cámara
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            SmoothSpeed * Time.deltaTime
        );
        //establecemos la ultima posicion de seguimento en X donde esta el player
        _lastTargetX = Player.transform.position.x;
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

} // class MainCameraMovement 
// namespace
