//---------------------------------------------------------
// Script que se encarga de llenar la barra cuando pulsas la tecla asignada y de reducirla cada
// x segundos. Este QTE no acaba si la barra se vacía completamente.
// Víctor Román Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Timing_QTE : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Cantidad que aumenta la barra por acierto
    /// </summary>
    [SerializeField]
    private float AumentoPorClick = 5.0f;
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
    /// Contiene la información del componente Repair.
    /// </summary>
    private Repair _comp;

    /// <summary>
    /// Valor máximo de la barra
    /// </summary>
    float _minValue;
    /// <summary>
    /// Valor de la barra
    /// </summary>
    float _value;
    /// <summary>
    /// booleano que detecta si la rayita está en la zona verde
    /// </summary>
    bool acertado = false;


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
        _comp = this.gameObject.GetComponentInParent<Repair>();
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
        //Si está reparando y se pulsa el botón de reparar
        if (_comp.IsRepairing() && InputManager.Instance.RepairWasPressedThisFrame() && Time.time >= _comp.RepairIniTime() + _comp.ExitTime())
        {
            _componenteBarra.value = 0;
            _comp.HasPressedExit(true);
        }
        //Si se pulsa el botón de salto (el que se usa como botón de spam) la barra sube.
        if (InputManager.Instance.JumpWasPressedThisFrame() && acertado)
        {
            //Debug.Log("Spam");
            _componenteBarra.value += AumentoPorClick;
        }
        //Si la barra se llena se da el objeto como reparado y acaba el QTE.
        if (_componenteBarra.value >= _componenteBarra.maxValue)
        {
            transform.parent.gameObject.SetActive(false);
            _comp.Repaired(true);
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

    /// <summary>
    /// Método encargado de cambiar el bool acertado
    /// </summary>
    public void Acierto(bool acierto)
    {
        acertado = acierto;
    }

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
