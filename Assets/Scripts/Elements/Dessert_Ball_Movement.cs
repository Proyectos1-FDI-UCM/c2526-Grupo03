//---------------------------------------------------------
// Movimiento y rotación de la bola del desierto
// Alejandro García Diaz && Colaboradores:
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
public class Dessert_Ball_Movement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Velocidad de la bola
    /// </summary>
    [SerializeField] private float Speed = 0.7f;
    /// <summary>
    /// Velocidad de rotación de la bola
    /// </summary>
    [SerializeField] private float RotationSpeed = 100.0f;
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
    /// Posicion inicial de la bola
    /// </summary>
    private Vector3 _iniPos;

    private float _posX;
    private float _posIniY;
    private float _anguloEjeZ;
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
        // Carga la posicion inicial nada mas carga el script
        _posX = transform.position.x;
        _posIniY = transform.position.y;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Basicamente recrea la ecuacion y = |sin(k*x)|
        float posY = math.abs(Mathf.Sin(_posX + Speed * Time.deltaTime)) + _posIniY;
        _posX += Speed * Time.deltaTime;
        transform.position = new Vector3(_posX, posY);

        // Rotación 
        _anguloEjeZ = transform.rotation.eulerAngles.z - RotationSpeed*Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, _anguloEjeZ);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    /// <summary>
    /// Metodo encargado de cambiar la velocidad a la bola del desierto
    /// </summary>
    /// <param name="num"> velocidad deseada </param>
    public void SetSpeed(float num)
    {
        Speed = num;
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Dessert_Ball_Movement 
// namespace
