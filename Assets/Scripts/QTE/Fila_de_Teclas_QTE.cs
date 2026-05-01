//---------------------------------------------------------
// Componente del QTE de flechas.
// Reparación de tipo automática.
// Gabriel Adrian Oltean && Colaboradores
//      Víctor Román Román
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
public class Fila_de_Teclas_QTE : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Cantidad que sube la barrita por acierto
    /// </summary>
    [SerializeField] private int NumFlechas_Max6 = 6;

    /// <summary>
    /// Duración de la reparación automática
    /// </summary>
    [SerializeField] private float AumentoAutomático = 2.22f;

    /// <summary>
    /// Sprite de la flecha hacia arriba
    /// </summary>
    [SerializeField] private Sprite ArrowUp;
    /// <summary>
    /// Sprite de la flecha hacia abajo
    /// </summary>
    [SerializeField] private Sprite ArrowDown;
    /// <summary>
    /// Sprite de la flecha hacia la izquierda
    /// </summary>
    [SerializeField] private Sprite ArrowLeft;
    /// <summary>
    /// Sprite de la flecha hacia la derecha
    /// </summary>
    [SerializeField] private Sprite ArrowRight;

    /// <summary>
    /// Imagenes de las flechas
    /// </summary>
    [SerializeField] private GameObject[] ObjetosDeFlechas;
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
    /// Componente de la Barra que se va llenando.
    /// </summary>
    private Slider _componenteBarra;

    /// <summary>
    /// Indica si has fallado el QTE.
    /// </summary>
    private bool _failedQTE = false;

    /// <summary>
    /// Indica si ha presionado una flecha.
    /// </summary>
    private bool _hasPressed = false;

    /// <summary>
    /// Variable que nos dice si ha hecho una pulsación correcta.
    /// </summary>
    private bool _correctPress = false;

    /// <summary>
    /// Direcciónes de todas las flechas (1 = arriba || 2 = abajo || 3 = izquierda || 4 = derecha).
    /// </summary>
    private _arrowDirection[] _dirFlechas;

    /// <summary>
    /// Dirección del input codificado.
    /// </summary>
    private _arrowDirection _dirFlechaInput;

    /// <summary>
    /// Dirección de la flecha que debemos acertar.
    /// </summary>
    private _arrowDirection _dirFlechaActual;

    /// <summary>
    /// Indice de la flecha actual.
    /// </summary>
    private int _indiceFlechaActual = 0;
    /// <summary>
    /// Indice de la ultima flecha a activar.
    /// </summary>
    private int _indiceUltimaFlecha;


    /// <summary>
    /// Tiempo entre cambio de flecha.
    /// </summary>
    private float _aumentoAcierto = 0f;

    /// <summary>
    /// Objeto de la flecha que vamos a cambiar en cada momento.
    /// </summary>
    private GameObject _arrowToManipulate;

    /// <summary>
    /// Momento en que se hizo la última pulsación.
    /// </summary>
    private float _lastInputTime = 0.0f;

    /// <summary>
    /// Contiene la información de la variable Repair.
    /// </summary>
    private Repair _comp;
    /// <summary>
    /// Enumerado con las direciones de las flechas
    /// </summary>
    private enum _arrowDirection { ARRIBA, ABAJO, IZQUIERDA, DERECHA };
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    void Awake()
    {
        //Cacheamos los componentes
        _comp = this.gameObject.GetComponentInParent<Repair>();
        _componenteBarra = GetComponent<Slider>();
        if (_componenteBarra == null)
        {
            _componenteBarra = GetComponentInChildren<Slider>();
        }
        if (_componenteBarra == null)
        {
            Debug.Log("No se encontró el componente Slider");
        }

        // Selección de flechas a cambiar
        if (NumFlechas_Max6 > ObjetosDeFlechas.Length)
        {
            Debug.Log("NumFlechas es demasiado grande: Eligo automáticamente el máximo (6)");
            NumFlechas_Max6 = ObjetosDeFlechas.Length;
        }

        // Calculo aumento por acierto
        _aumentoAcierto = 100 / NumFlechas_Max6;

        // Elegimos la última flecha a activar
        _indiceUltimaFlecha = NumFlechas_Max6 - 1;

        // Direcciónes de flechas
        _dirFlechas = new _arrowDirection[ObjetosDeFlechas.Length];

        int i = 0;
        // Activamos las flechas necesarias
        while (i <= _indiceUltimaFlecha)
        {
            // Escogemos la flecha a activar
            _arrowToManipulate = ObjetosDeFlechas[i];

            // Escogemos direccion aleatoria para la flecha actual
            int[] rnd = new int[ObjetosDeFlechas.Length];
            rnd[i] = Random.Range(1, 5);
            // Cambiamos el sprite según la dirección
            switch (rnd[i])
            {
                case 1:
                    _arrowToManipulate.GetComponent<Image>().sprite = ArrowUp;
                    _dirFlechas[i] = _arrowDirection.ARRIBA;
                    break;
                case 2:
                    _arrowToManipulate.GetComponent<Image>().sprite = ArrowDown;
                    _dirFlechas[i] = _arrowDirection.ABAJO;
                    break;
                case 3:
                    _arrowToManipulate.GetComponent<Image>().sprite = ArrowLeft;
                    _dirFlechas[i] = _arrowDirection.IZQUIERDA;
                    break;
                case 4:
                    _arrowToManipulate.GetComponent<Image>().sprite = ArrowRight;
                    _dirFlechas[i] = _arrowDirection.DERECHA;
                    break;
            }
            // Activamos la flecha
            _arrowToManipulate.SetActive(true);
            // Pasamos a la siguiente flecha
            i++;
        }
        _arrowToManipulate = ObjetosDeFlechas[0];

        // Seleccionamos la dirección de la flecha inicial
        _dirFlechaActual = _dirFlechas[0];
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (_comp.IsRepairing() && InputManager.Instance.RepairWasPressedThisFrame() && Time.time >= _comp.RepairIniTime() + _comp.ExitTime())
        {
            _componenteBarra.value = 0;
            _failedQTE = false;
            int i = 0;
            // Activamos las flechas
            while (i <= _indiceUltimaFlecha)
            {
                // Escogemos la flecha a activar
                _arrowToManipulate = ObjetosDeFlechas[i];
                // Activamos la flecha
                _arrowToManipulate.SetActive(true);
                // Pasamos a la siguiente flecha
                i++;
            }
            _arrowToManipulate = ObjetosDeFlechas[0];
            _indiceFlechaActual = 0;
            _dirFlechaActual = _dirFlechas[_indiceFlechaActual];

            _comp.HasPressedExit(true);
        }

        // Recogida de input
        Vector2 input = InputManager.Instance.ArrowsQTE;

        // Comprobamos si ha pulsado
        if (input.x < -0.5f || input.x > 0.5f || input.y < -0.5f || input.y > 0.5f)
        {
            // Confirmamos pulsación si ha pasado suficiente tiempo desde la pulsación anterior para evitar pulsaciones múltiples
            if (Time.time > _lastInputTime + 0.25f)
            {
                _hasPressed = true;
                _lastInputTime = Time.time;
            }
        }

        // Codificamos la dirección del input
        if (_hasPressed)
        {
            if (input.y > 0.5f)
            {
                _dirFlechaInput = _arrowDirection.ARRIBA;
            }
            else if (input.y < -0.5f)
            {
                _dirFlechaInput = _arrowDirection.ABAJO;
            }
            else if (input.x < -0.5f)
            {
                _dirFlechaInput = _arrowDirection.IZQUIERDA;
            }
            else if (input.x > 0.5f)
            {
                _dirFlechaInput = _arrowDirection.DERECHA;
            }

            // Comprobamos si el input es correcto
            if (_dirFlechaInput == _dirFlechaActual)
            {
                _correctPress = true;
            }
            // Devolvemos el input codificado al estado original para evitar que suba varias veces de golpe
            _dirFlechaInput = 0;
            // pasamos a la siguiente flecha
            if (_indiceFlechaActual < NumFlechas_Max6 - 1)
            {
                _indiceFlechaActual++;
            }
            _dirFlechaActual = _dirFlechas[_indiceFlechaActual];
        }

        // Lógica de la Barra que aumenta

        // Parte de reparación automática
        _componenteBarra.value += AumentoAutomático * Time.deltaTime;

        // Parte de reparación por QTE
        if (!_failedQTE)
        {
            // Si la pulsación es correcta
            if (_hasPressed)
            {
                // Si la pulsación es correcta
                if (_correctPress)
                {
                    ObjetosDeFlechas[_indiceFlechaActual - 1].SetActive(false);
                    // Le sumamos al valor del slider el aumento por acierto
                    _componenteBarra.value += _aumentoAcierto;
                    // En caso de que sea la última flecha, nos aseguramos de que llegue al 100%
                    if (_indiceFlechaActual == NumFlechas_Max6)
                    {
                        _componenteBarra.value = _componenteBarra.maxValue;
                    }
                }
                // Si la pulsación es errónea
                else
                {
                    // indicamos que ha fallado el QTE para que ya no acceda a la parte de reparacion por QTE
                    _failedQTE = true;
                    // Desactivamos todas las flechas
                    int i = 0;
                    while (i <= _indiceUltimaFlecha)
                    {
                        // Escogemos la flecha a desactivar
                        _arrowToManipulate = ObjetosDeFlechas[i];
                        // Desactivamos la flecha
                        _arrowToManipulate.SetActive(false);
                        // Pasamos a la siguiente flecha
                        i++;
                    }
                }
                _correctPress = false;
                _hasPressed = false;
            }
        }

        // Si el valor del slider es el máximo
        if (_componenteBarra.value >= _componenteBarra.maxValue)
        {
            // Desactivamos el panel
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

    #endregion

} // class QTE3 
// namespace
