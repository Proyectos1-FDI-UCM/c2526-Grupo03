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
public class Extra_Regular : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private int Resistance = 2;
    [SerializeField] private int Distance = 5;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private float _speed = 1f; // Velocidad del extra
    private Vector3 iniPos; // Posicion inicial del extra 
    private Vector3 maxPosL;
    private Vector3 maxPosR;
    private float _extraHeight = 0.0f;
    private float _extraWidth = 0.0f;
    private int dir = -1;
    SpriteRenderer _spriteRenderer; // sprite del extra
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _extraHeight = GetComponent<BoxCollider2D>().bounds.size.y;
        _extraWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        // Establece la posicion inicial del extra a la extablecida por el editor
        iniPos = transform.position;
        maxPosL = transform.position;
        maxPosL.x -= Distance; 
        maxPosR = transform.position;
        maxPosR.x += Distance;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!Dectection())
        {// cambia la direccion si no detecta suelo
            if (dir == 1) dir = -1;
            else if (dir == -1) dir = 1;
        }
    }
    void FixedUpdate()
    {  //Mira si se ha pasado de su maximo hacia cada lado para cambiar de direcion
        if (transform.position.x <= maxPosL.x)
        {
            dir = 1;
        }
        if (transform.position.x >= maxPosR.x)
        {
            dir = -1;
        }
        if (dir == 1)//mueve el extra hacia la derecha
        {
            this.transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;
        }if(dir == -1)//mueve el extra hacia la izquierda
        {
            this.transform.position -= new Vector3(_speed, 0, 0) * Time.deltaTime;
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
    private bool Dectection()
    {
        bool Detection = false;
        // Puntos de lanzamiento de los rayos
        Vector3 Center = transform.position - new Vector3(0.0f, _extraHeight / 2);
        Vector3 Left = transform.position - new Vector3(_extraWidth / 2, _extraHeight / 2);
        Vector3 Right = transform.position - new Vector3(-1 *(_extraWidth / 2+_extraWidth/4), _extraHeight / 2);

        float RaycastMagnitude = 0.01f;
        // Comprobamos si tiene suelo justo debajo en el centro
        RaycastHit2D CenterRay = Physics2D.Raycast(Center, new Vector3(0.0f, -1), RaycastMagnitude);

        // Comprobamos si tiene suelo justo debajo de la esquina inferior izquierda
        RaycastHit2D LeftRay = Physics2D.Raycast(Left, new Vector3(0.0f, -1), RaycastMagnitude);

        // Comprobamos si tiene suelo justo debajo de la esquina inferior derecha
        RaycastHit2D RightRay = Physics2D.Raycast(Right, new Vector3(0.0f, -1), RaycastMagnitude);

        // Comprobamos si hay algo debajo
        if ((LeftRay.collider != null ) && (RightRay.collider != null )&& (CenterRay.collider != null))
        {
            Detection = true;
            
        }
        return Detection;
    }
    #endregion

} // class Extra_Regular 
// namespace
