//---------------------------------------------------------
// Script que detecta la posición del ratón y si este está clickado para rotar la manivela de modo que siempre mire al ratón.
// En caso de usar mando la manivela transformará su ángulo en la pos del joystick.
// Víctor Román Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class QTE2 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    private GameObject BarraReperacion;
    [SerializeField]
    private Transform ReferencePoint;
    [SerializeField]
    private float CantidadSumada;
    [SerializeField]
    private float Disminucion;
    [SerializeField]
    private float VelQueDebeLlevar;
    [SerializeField]
    private float VelAngularMax;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _arrastrando = false;
    private float _ultimoAngulo;
    private Vector3 _mousePos;
    private float _anguloInicial;
    private float _anguloFinal;
    private float _velocidadAngular;
    private Slider _sliderBarra;
    private int contador = 0;

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
        _sliderBarra = BarraReperacion.GetComponent<Slider>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector2 inputdir = InputManager.Instance.MovementVector;
        if (_sliderBarra.value > 0)
        {
            _sliderBarra.value -= (Disminucion * Time.deltaTime);
        }
        ClickRaton();
        SoltarClickRaton();
        if(_arrastrando || inputdir != new Vector2(0,0))
        {
            _mousePos = Input.mousePosition;
            _mousePos.z = ReferencePoint.position.z;
            Vector2 direccion = new Vector2(0,0);
            if(_arrastrando)
            {
                direccion = _mousePos - ReferencePoint.position;
                _ultimoAngulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            }
            else if(InputManager.Instance.MovementVector != new Vector2(0, 0))
            {
                direccion = InputManager.Instance.MovementVector;
                _ultimoAngulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            }
            if (_ultimoAngulo < 0)
            {
                _ultimoAngulo += 360f;
            }

            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, _ultimoAngulo);

            if (contador == 0)  //Empieza a contar para después calcular la "velocidad angular del objeto".
            {
                _anguloInicial = gameObject.transform.rotation.z;
                contador++;
            }
            else if (contador == 2) //Si ya han pasado 5 frames calcular la "velocidad angular del objeto".
            {
                _anguloFinal = gameObject.transform.rotation.z;
                _velocidadAngular = _anguloFinal - _anguloInicial;
                Debug.Log(Mathf.Abs(_velocidadAngular));
                if(Mathf.Abs(_velocidadAngular) >= VelQueDebeLlevar && Mathf.Abs(_velocidadAngular) < VelAngularMax)
                {
                    _sliderBarra.value += CantidadSumada;
                }
                contador = 0;
            }
            else
            {
                contador++;
            }
        }
        if (_sliderBarra.value >= _sliderBarra.maxValue)
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

    private bool ClickRaton()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _arrastrando = true;
        }
        return _arrastrando;
    }

    private bool SoltarClickRaton()
    {
        if(Input.GetMouseButtonUp(0))
        {
            _arrastrando = false;
        }
        return _arrastrando;
    }

    private bool MoviendoJoyStick()
    {
        return _arrastrando;
    }

    private bool ParaJoyStick()
    {
        return _arrastrando;
    }

    #endregion   

} // class QTE2 
// namespace
