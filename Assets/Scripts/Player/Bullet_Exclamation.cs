//---------------------------------------------------------
// Script encargado del comportamiento de la bala/exclamacion
// Alejandro Garcia
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Bullet_Exclamation : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Velociada de las balas
    /// </summary>
    [SerializeField] private float Speed = 7.0f;

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
    /// SpriteRenderer del objeto
    /// </summary>
    private SpriteRenderer _spriteRenderer;
    /// <summary>
    /// Direcion de la bala
    /// </summary>
    private int _direction = 0;
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
    void Awake()
    {
        //Obtenemos el sprite render del objeto
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Miramos la direcion hacia la que apunta el sprite para dispara al mismo lado
        _direction = _spriteRenderer.flipX ? -1 : 1;
        //Cambiamos su posicion 
        transform.position += new Vector3(Speed * _direction, .0f) * Time.deltaTime;
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
    private void OnTriggerEnter2D(Collider2D col)
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// Metodo que detecta si se ha salido de la vison de la camara 
    /// </summary>
    private void OnBecameInvisible()
    {
        //Desactivamos el objeto
        this.gameObject.SetActive(false);
    }

    #endregion   

} // class Bullet_Exclamation 
// namespace
