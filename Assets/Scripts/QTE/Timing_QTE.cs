//---------------------------------------------------------
// Script que se encarga de llenar la barra cuando pulsas la tecla asignada y de reducirla cada
// x segundos. Este QTE no acaba si la barra se vacía completamente.
// Víctor Román Román, Alejandro Garcia
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
    [SerializeField] private float AumentoPorClick = 5.0f;
    /// <summary>
    /// GameObject que engloba el timing
    /// </summary>
    [SerializeField] private GameObject Timing;
    /// <summary>
    /// Script de la zona del QTE
    /// </summary>
    [SerializeField] private Timing_QTE_Zona Zona;
    /// <summary>
    /// Posicion del a zona a clicar para aumentar la barra
    /// </summary>
    [SerializeField] private RectTransform ZonaPos;
    /// <summary>
    /// Duración de la reparación automática
    /// </summary>
    [SerializeField] private float AumentoAutomático = 2.22f;
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
    /// Contiene la información del componente Repair
    /// </summary>
    private Repair _comp;
    /// <summary>
    /// Valor mínimo de la barra
    /// </summary>
    private float _minValue;
    /// <summary>
    /// Valor de la barra
    /// </summary>
    private float _value;

    /// <summary>
    /// El punto maximo que puede alcanzar la zona para que el click sea considerado un acierto, teniendo en cuenta el tamaño de la imagen de la zona.
    /// </summary>
    private float _maxZonaPos;
    //
    private int _limitesLat = 5;
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
        //Cacheamos los conponentes
        if (Zona == null)
        {
            Zona = GetComponent<Timing_QTE_Zona>();
        }
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

    private void Start()
    {
        //Calcualamos la maxima posicion
        _maxZonaPos = ZonaPos.anchoredPosition.x * 2;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Parte de reparación automática
        _componenteBarra.value += AumentoAutomático * Time.deltaTime;
        //Si está reparando y se pulsa el botón de reparar
        if (_comp.IsRepairing() && InputManager.Instance.RepairWasPressedThisFrame() && Time.time >= _comp.RepairIniTime() + _comp.ExitTime())
        {
            Timing.SetActive(true);
            _componenteBarra.value = 0;
            _comp.HasPressedExit(true);
        }
        //Si se pulsa el botón de salto (el que se usa como botón de timing) dentro de la zona la barra sube.
        if (InputManager.Instance.JumpWasPressedThisFrame() && Zona.CheckAcierto())
        {
            _componenteBarra.value += AumentoPorClick;
            // Cada vez que se pulsa el botón de salto dentro de la zona, esta se mueve a una posición aleatoria dentro del rango permitido.
            float rndPos = Random.Range(_limitesLat, _maxZonaPos-_limitesLat);
            ZonaPos.anchoredPosition = new Vector2(rndPos, ZonaPos.anchoredPosition.y);
        }
        //Si se pulsa el botón fuera de la zona.
        else if (InputManager.Instance.JumpWasPressedThisFrame() && !Zona.CheckAcierto())
        {
            //Desactivamos el timing
            Timing.SetActive(false);
        }
        //Si la barra se llena se da el objeto como reparado y acaba el QTE.
        if (_componenteBarra.value >= _componenteBarra.maxValue)
        {
            //desactivamos al padre
            transform.parent.gameObject.SetActive(false);
            //Y decimos que esta reparado
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
