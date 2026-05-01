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
public class Timing_QTE_Rayita : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Velocidad de la rayita.
    /// </summary>
    [SerializeField] private float Speed = .0f;

    /// <summary>
    /// Límite derecho del movimiento de la rayita.
    /// </summary>
    [SerializeField] private RectTransform MaxPos;

    /// <summary>
    /// Límite izquierdo del movimiento de la rayita.
    /// </summary>
    [SerializeField] private RectTransform MinPos;
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
    /// entero para cambiar la dirección en la que se mueve la rayita.
    /// </summary>
    private int _signo = 1;

    /// <summary>
    /// variable que guarda el rect transform de la rayita.
    /// </summary>
     private RectTransform _rectTransform;


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
        //Sacamos el componente RectTranform
        _rectTransform = GetComponent<RectTransform>();
       
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Usamos el rectTransform.anchoredPosition porque al ser un canvas 
        _rectTransform.anchoredPosition += new Vector2(Speed, 0f) * Time.deltaTime * _signo ;
        //Miramos si se ha pasado de los limites para cambiar de direccion
        if (_rectTransform.anchoredPosition.x > MaxPos.anchoredPosition.x)
        {
            _signo = -1;
        }
        else if (_rectTransform.anchoredPosition.x < MinPos.anchoredPosition.x)
        {
            _signo = 1;
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

} // class Timing_QTE_Rayita 
// namespace
