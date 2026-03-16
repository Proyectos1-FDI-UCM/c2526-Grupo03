//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class QTE3 : MonoBehaviour
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
    [SerializeField] private int PulsacionesCorrectasNecesarias = 10;
    /// <summary>
    /// Cantidad que disminuye cuando no pulsas nada
    /// </summary>
    [SerializeField] private float Disminucion = 0.05f;
    /// <summary>
    /// Tiempo entre cambio de flecha
    /// </summary>
    [SerializeField] private float TiempoPorFlecha = 2.0f;


    /// <summary>
    /// Objeto de tipo Image de la flecha actual
    /// </summary>
    [SerializeField] private Image ChangingArrow;

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
    /// Componente de la Barra que se va llenando
    /// </summary>
    private Slider _componenteBarra;

    /// <summary>
    /// Momento en el que hay que cambiar de flecha
    /// </summary>
    private float _timeToChange = 0.0f;

    /// <summary>
    /// Variable que nos indica si ha presionado una flecha
    /// </summary>
    private bool _hasPressed = false;

    /// <summary>
    /// Variable que nos dice si ha hecho una pulsación correcta
    /// </summary>
    private bool _correctPress = false;

    /// <summary>
    /// Dirección de la flecha actual
    /// </summary>
    private int _dirFlechaActual;

    /// <summary>
    /// Dirección de la última flecha
    /// </summary>
    private int _dirFlechaAnterior;

    /// <summary>
    /// Dirección del input codificado
    /// </summary>
    private int _dirFlechaInput;

    /// <summary>
    /// Cantidad que sube el slider por cada acierto
    /// </summary>
    private float _aumentoPorAcierto;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 


    void Awake()
    {
        _componenteBarra = GetComponent<Slider>();
        if (_componenteBarra == null)
        {
            _componenteBarra = GetComponentInChildren<Slider>();
        }
        if (_componenteBarra == null)
        {
            Debug.Log("No se encontró el componente Slider");
        }
        _aumentoPorAcierto = _componenteBarra.maxValue / PulsacionesCorrectasNecesarias;
        // Seleccionamos flecha inicial aleatoria
        _dirFlechaActual = Random.Range(1, 4);
        _dirFlechaAnterior = _dirFlechaActual;
        // Cambiamos el sprite de la flecha
        switch (_dirFlechaActual)
        {
            // 1 = flecha arriba; 2 = flecha abajo; 3 = flecha izquierda; 4 = flecha derecha
            case 1: ChangingArrow.sprite = ArrowUp; break;
            case 2: ChangingArrow.sprite = ArrowDown; break;
            case 3: ChangingArrow.sprite = ArrowLeft; break;
            case 4: ChangingArrow.sprite = ArrowRight; break;
        }

        // Actualizamos el momento de cambiar
        _timeToChange = Time.time + TiempoPorFlecha;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Comprobamos si es momento de cambiar la flecha
        if (Time.time >= _timeToChange || (_hasPressed && _correctPress))
        {

            // Seleccionamos flecha aleatoria
            do
            { // Volvemos a cambiar si la flecha coincide con la última
                _dirFlechaActual = Random.Range(1, 4);

            } while (_dirFlechaActual == _dirFlechaAnterior);

            // Guardamso la dirección nueva en la dirección anterior para el siguiente cambio
            _dirFlechaAnterior = _dirFlechaActual;

            // Cambiamos el sprite de la flecha
            switch (_dirFlechaActual)
            {
                // 1 = flecha arriba; 2 = flecha abajo; 3 = flecha izquierda; 4 = flecha derecha
                case 1: ChangingArrow.sprite = ArrowUp; break;
                case 2: ChangingArrow.sprite = ArrowDown; break;
                case 3: ChangingArrow.sprite = ArrowLeft; break;
                case 4: ChangingArrow.sprite = ArrowRight; break;
            }

            // Volvemos a cambiar la pulsación a false
            if (_hasPressed)
            {
                _hasPressed = false;
                _correctPress = false;
            }

            // Actualizamos el momento de cambiar
            _timeToChange = Time.time + TiempoPorFlecha;
        }

        // Recogida de input
        Vector2 input = InputManager.Instance.ArrowsQTE;

        // Comprobamos input
        if (input.x < -0.5f || input.x > 0.5f || input.y < -0.5f || input.y > 0.5f)
        {
            _hasPressed = true;
        }

        // Comprobamos si se ha pulsado
        if (_hasPressed)
        {
            // Hacia arriba
            if (input.y > 0.5f)
            {
                _dirFlechaInput = 1;
            }
            // Hacia abajo
            else if (input.y < -0.5f)
            {
                _dirFlechaInput = 2;
            }
            // Hacia izquierda
            else if (input.x < -0.5f)
            {
                _dirFlechaInput = 3;
            }
            // Hacia derecha
            else if (input.x > 0.5f)
            {
                _dirFlechaInput = 4;
            }

            // Comprobamos si el input es correcto
            if (_dirFlechaInput == _dirFlechaActual)
            {
                _correctPress = true;
            }
            // Devolvemos el input codificado al estado original para evitar que suba varias veces de golpe
            _dirFlechaInput = 0;
        }

        // Lógica de la Barra que aumenta

        // Si la pulsación es correcta
        if (_correctPress)
        {
            // Le sumamos al valor del slider el aumento por acierto
            _componenteBarra.value += _aumentoPorAcierto;
        }
        // Si el valor del slider es el máximo
        if (_componenteBarra.value >= _componenteBarra.maxValue)
        {
            // Desactivamos el panel
            transform.parent.gameObject.SetActive(false);
            // Marcamos el objeto como reparado
            this.gameObject.GetComponentInParent<Repair>().Repaired = true;
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
