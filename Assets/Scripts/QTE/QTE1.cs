//---------------------------------------------------------
// Script que se encarga de llenar la barra cuando pulsas la tecla asignada
// Víctor Román Román
// Nombre del juego
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class QTE1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Cantidad que aumenta la barra por click del botón asignado.
    /// </summary>
    [SerializeField]
    private float AumentoPorClick = 5.0f;

    [SerializeField]
    private float Disminucion = 0.5f;
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
    /// Componente de la barra
    /// </summary>
    private Slider _componenteBarra;

    /// <summary>
    /// Valor máximo de la barra
    /// </summary>
    float _minValue;
    /// <summary>
    /// Valor de la barra
    /// </summary>
    float _value;

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
        if (_componenteBarra == null)
        {
            _componenteBarra = GetComponent<Slider>();
            _minValue = _componenteBarra.minValue;
            _value = _minValue;
            if(_componenteBarra == null)
            {
                _componenteBarra = GetComponentInChildren<Slider>();
                _minValue = _componenteBarra.maxValue;
                _value = _minValue;
            }
        }
        else
        {
            Debug.Log("No se encontró el componente Slider en " + gameObject.name);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(_componenteBarra.value > 0)
        {
            _componenteBarra.value -= Disminucion;
        }
        if(InputManager.Instance.JumpWasPressedThisFrame())
        {
            Debug.Log("Spam");
            _componenteBarra.value += AumentoPorClick;
        }
        if(_componenteBarra.value >=_componenteBarra.maxValue)
        {
            transform.parent.gameObject.SetActive(false);
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

} // class QTE1 
// namespace
