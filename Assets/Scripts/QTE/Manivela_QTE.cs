//---------------------------------------------------------
// Script que detecta la posición del ratón y si este está clickado para rotar la manivela de modo que siempre mire al ratón.
// En caso de usar mando la manivela transformará su ángulo en la pos del joystick.
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
public class Manivela_QTE : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Objeto que contiene la barra de reparación.
    /// </summary>
    [SerializeField] private GameObject BarraReperacion;

    /// <summary>
    /// Punto de referencia para el cálculo del ángulo que forma la dirección del ratón/input, es el centro de la manivela.
    /// </summary>
    [SerializeField] private Transform ReferencePoint;

    /// <summary>
    /// Cantidad que se suma por unidad de tiempo a la barra de reparación.
    /// </summary>
    [SerializeField] private float CantidadSumada;

    /// <summary>
    /// Cantidad que se resta por unidad de tiempo de la barra de reparación.
    /// </summary>
    [SerializeField] private float Disminucion;

    /// <summary>
    /// Velocidad mínima que debe tener la manivela para empezar a subir.
    /// </summary>
    [SerializeField] private float VelQueDebeLlevar;

    /// <summary>
    /// Velocidad angular máxima que puede llevar la manivela.
    /// </summary>
    [SerializeField] private float VelAngularMax;
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
    /// Controla si se está arrastrando la manivela con el ratón.
    /// </summary>
    private bool _arrastrando = false;

    /// <summary>
    /// Último ángulo realizado por la manivela.
    /// </summary>
    private float _ultimoAngulo;

    /// <summary>
    /// Posición del ratón.
    /// </summary>
    private Vector3 _mousePos;

    /// <summary>
    /// Ángulo inicial de la manivela para el cálculo de su velocidad angular.
    /// </summary>
    private float _anguloInicial;

    /// <summary>
    /// Ángulo final de la manivela para el cálculo de su velocidad angular.
    /// </summary>
    private float _anguloFinal;

    /// <summary>
    /// Velocidad que lleva la manivela.
    /// </summary>
    private float _velocidadAngular;

    /// <summary>
    /// Componente encargado del valor de "llenado" de la barra de reparación
    /// </summary>
    private Slider _sliderBarra;

    /// <summary>
    /// Contador de frames para el cálculo de la velocidad angular.
    /// </summary>
    private int _contador = 0;

    /// <summary>
    /// Contiene la información de la variable Repair.
    /// </summary>
    private Repair _comp;
    /// <summary>
    /// Ultima posicion del mouse
    /// </summary>
    private Vector3 _lastMousePos;
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
        //Asignación de componentes a sus respectivas variables.
        _sliderBarra = BarraReperacion.GetComponent<Slider>();
        _comp = this.gameObject.GetComponentInParent<Repair>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Si está reparando y se pulsa el botón de reparar
        if (_comp.IsRepairing() && InputManager.Instance.RepairWasPressedThisFrame() && Time.time >= _comp.RepairIniTime() + _comp.ExitTime())
        {
            _sliderBarra.value = 0;
            _comp.HasPressedExit(true);
        }
        Vector2 inputdir = InputManager.Instance.ManivelaQTEVector;
        if (_sliderBarra.value > 0)
        {
            _sliderBarra.value -= (Disminucion * Time.deltaTime);
        }

        _mousePos = Input.mousePosition;
        _mousePos.z = ReferencePoint.position.z;
        //Actualizamos la posicion del mouse si se mueve
        if (_mousePos != _lastMousePos)
        {
            _lastMousePos = _mousePos;
            _arrastrando = true;
        }
        else
        {
            _arrastrando = false;
        }

        //Si se detecta que la manivela se está arrastrando con el ratón o algún input de movimiento.
        //(Esto último es sobre todo para jugar con mando, recoge la dirección del joystick).
        if (_arrastrando || inputdir != new Vector2(0, 0))
        {
            Vector2 direccion = new Vector2(0, 0);
            //Si se está usando ratón se calcula la dirección del pto de referencia al ratón y se saca el ángulo que forma.
            if (_arrastrando)
            {
                direccion = _mousePos - ReferencePoint.position;
                _ultimoAngulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            }
            //Si se está usando un input se calcula el ángulo que forma la dirección del input.
            else
            {
                direccion = inputdir;
                _ultimoAngulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            }

            //Si el ángulo es negativo se normaliza.
            if (_ultimoAngulo < 0)
            {
                _ultimoAngulo += 360f;
            }
            //Rotar la manivela para que siga al ratón (mientras esté arrastrando) o siga la dirección del input.
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, _ultimoAngulo);

            if (_contador == 0)  //Empieza a contar para después calcular la "velocidad angular del objeto".
            {
                _anguloInicial = gameObject.transform.rotation.z;
                _contador++;
            }
            else if (_contador == 2) //Si ya han pasado 2 frames calcular la "velocidad angular del objeto".
            {
                _anguloFinal = gameObject.transform.rotation.z;
                //Variación del ángulo.
                // Calculamos cuánto ha girado la manivela desde el último frame de forma limpia
                float DifAngulo = Mathf.DeltaAngle(_anguloInicial, _anguloFinal);

                // Normalizamos la velocidad angular
                if (_arrastrando)
                {
                    // El ratón es más preciso pero genera deltas más pequeños que el stick.
                    // Multiplicamos por un factor (ej. 2.5) para que cada movimiento sume más.
                    _velocidadAngular = DifAngulo * 1.5f;
                }
                else
                {
                    // El mando es analógico y constante, no necesita multiplicador
                    _velocidadAngular = DifAngulo;
                }
                //Comprobación de que la velocidad que lleva es la requerida y menor de la máxima (para evitar un bug).
                if (Mathf.Abs(_velocidadAngular) >= VelQueDebeLlevar && Mathf.Abs(_velocidadAngular) < VelAngularMax)
                {
                    _sliderBarra.value += CantidadSumada;
                }
                _contador = 0;
            }
            else
            {
                _contador++;
            }
        }
        //Si la barra se llena completamente se acaba el QTE
        if (_sliderBarra.value >= _sliderBarra.maxValue)
        {
            transform.parent.gameObject.SetActive(false);
            // Marcamos el objeto como reparado
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

    /// <summary>
    /// Detecta el click del ratón y actualiza _arrastrando a true.
    /// </summary>
    /// <returns></returns>
    private bool ClickRaton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _arrastrando = true;
        }
        return _arrastrando;
    }

    /// <summary>
    /// Detecta que se suelta el click del ratón y actualiza _arrastrando a false.
    /// </summary>
    /// <returns></returns>
    private bool SoltarClickRaton()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _arrastrando = false;
        }
        return _arrastrando;
    }

    #endregion   

} // class QTE2 
// namespace
