//---------------------------------------------------------
// Un trigger que devuelve true cuando algo le atraviesa
// Gabriel Adrian Oltean
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Detector : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

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
    /// True si se detecta un collider
    /// </summary>
    private bool _detected = false;
    /// <summary>
    /// Guarda el collider del último objeto colisionado
    /// </summary>
    private Collider2D _collisionedObject;
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
    void Update()
    {

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
    /// Dice si se ha detectado un obstaculo o no
    /// </summary>
    /// <returns>True si se detecta un collider</returns>
    public bool Detected()
    {
        return _detected;
    }
    /// <summary>
    /// Devuelve el vector de la posicion del ultimo objeto detectado
    /// </summary>
    public Vector3 GetCollisionedObjectPosition()
    {
        return _collisionedObject.gameObject.transform.position;
    }
    /// <summary>
    /// Devuelve el vector del tamaño del ultimo objeto detectado
    /// </summary>
    public Vector3 GetCollisionedObjectSize()
    {
        return _collisionedObject.bounds.size;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    private void OnTriggerStay2D(Collider2D collision)
    {
        _detected = true;
        _collisionedObject = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _detected = false;
    }

    #endregion   

} // class Detector 
// namespace
