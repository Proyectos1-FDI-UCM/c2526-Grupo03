//---------------------------------------------------------
// Se encarga del movimiento del extra que se puede configurar su distancia de recorrido
// Tristan Sanchez Lopez
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Extra_Regular : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoint

    /// <summary>
    /// Detector de suelo
    /// </summary>
    [SerializeField] private Detector FloorDetector;
    /// <summary>
    /// Distancia caminada
    /// </summary>
    [SerializeField] private int Distance = 5;
    /// <summary>
    /// Velocidad del extra
    /// </summary>
    [SerializeField] private float Speed = 1f;
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
    /// Posicion inicial 
    /// </summary>
    private Vector3 _iniPos;

    /// <summary>
    /// Maxima posicion de izquierda
    /// </summary>
    private Vector3 _maxPosL;

    /// <summary>
    /// Maxima posicion derecha
    /// </summary>
    private Vector3 _maxPosR;

    /// <summary>
    /// Dire
    /// </summary>
    private int _dir = -1;

    /// <summary>
    /// sprite del extra
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Contiene la información del componente Animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Indica si el detector de suelo dice que estamos en el suelo
    /// </summary>
    private bool _landed;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Awake is called 
    /// </summary>
    void Awake()
    {
        //Acedemos al sprite y a las animaciones
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        // Guardamos la posicion inicial del extra a la extablecida por el editor
        _iniPos = transform.position;
        //Calculamos la posicion maxima de la izquierda
        _maxPosL = transform.position;
        _maxPosL.x -= Distance;
        //Calculamos la posicion maxima de la derecha
        _maxPosR = transform.position;
        _maxPosR.x += Distance;
        //Miramos si la velocidad es mayor que 0 para que empieze hacia la izquierda
        if (Speed > 0)
        {
            //Giramos el sprite
            _spriteRenderer.flipX = true;
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Miramos si ha detectado el suelo
        _landed = FloorDetector.Detected();
        if (!_landed)
        {// cambia la direccion si no detecta suelo
            if (_dir == 1)
            {
                _dir = -1;
            }
            else if (_dir == -1)
            {
                _dir = 1;
            }
        }//Mira si se pasa de la distancia que recorre para volver 
        if (transform.position.x <= _maxPosL.x)
        {
            _dir = 1;
        }
        else if (transform.position.x >= _maxPosR.x)
        {
            _dir = -1;
        }
        if (_dir == 1)//mueve el extra hacia la derecha
        {
            transform.position += new Vector3(Speed, 0, 0) * Time.deltaTime;
            _spriteRenderer.flipX = false;
        }
        else if (_dir == -1)//mueve el extra hacia la izquierda
        {
            transform.position -= new Vector3(Speed, 0, 0) * Time.deltaTime;
            _spriteRenderer.flipX = true;
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

} // class Extra_Regular 
// namespace
