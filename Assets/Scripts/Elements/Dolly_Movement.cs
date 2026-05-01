//---------------------------------------------------------
// Script que hace a la cámara Dolly seguir la bola.
// Víctor Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dolly_Movement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Objeto que sigue la camara en todo momento
    /// </summary>
    [SerializeField] private GameObject ObjetoASeguir;
    /// <summary>
    /// Offset de la camara en X
    /// </summary>
    [SerializeField] private float OffsetX = 1f;
    /// <summary>
    /// Offset de la camara en Y
    /// </summary>
    [SerializeField] private float OffsetY = 1f;

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
    /// Variable de seguimiento en X
    /// </summary>
    private float _followingObjectPositionXAxis;
    /// <summary>
    /// Variable de seguimiento en Y
    /// </summary>
    private float _followingObjectPositionYAxis;

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
        //Calculamos la posicion seguida en Y
        _followingObjectPositionYAxis = ObjetoASeguir.transform.position.y + OffsetY;
        //Calcualamos el aumento en Y
        float AumentoY = _followingObjectPositionYAxis - this.gameObject.transform.position.y;
        //Calcualamos la posicion seguida en X
        _followingObjectPositionXAxis = ObjetoASeguir.transform.position.x + OffsetX;
        //Calculamos el aumento en Y
        float AumentoX = _followingObjectPositionXAxis - this.gameObject.transform.position.x;
        //Establecemos la posicion 
        transform.position += new Vector3(AumentoX, AumentoY);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Volvemos a calcular la posicion seguida en X
        _followingObjectPositionXAxis = ObjetoASeguir.transform.position.x + OffsetX;
        // Y el aumento en X
        float AumentoX = _followingObjectPositionXAxis - this.gameObject.transform.position.x;
        //Actualizamos la posicion
        transform.position += new Vector3(AumentoX, 0.0f);
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

} // class Dolly_Movement 
// namespace
