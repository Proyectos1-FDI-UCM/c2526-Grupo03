//---------------------------------------------------------
// Script encargador de controlar el indicador de distancia entre el jugador y el objetivo
// Alejandro Garcia
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
public class IndicadorDistancia : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Componente Slider del objeto de SliderPlayer cacheado
    /// </summary>
    [SerializeField] private Slider SliderPlayer;
    /// <summary>
    /// Componente Slider del objeto de SliderDolly cacheado
    /// </summary>
    [SerializeField] private Slider SliderDolly;
    /// <summary>
    /// Transform de la meta del nivel
    /// </summary>
    [SerializeField] private Transform Meta;
    /// <summary>
    /// Transform del jugador
    /// </summary>
    [SerializeField] private Transform Player;
    /// <summary>
    /// Transform de la camara Dolly
    /// </summary>
    [SerializeField] private Transform Dolly;

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
    /// Posicion de la meta en el eje X
    /// </summary>
    private float _posMeta;
    /// <summary>
    /// Posicion inicial de dolly en el eje X
    /// </summary>
    private float _iniDollyPos;

    /// <summary>
    /// Posicion actual de la dolly en el eje X
    /// </summary>
    private float _actDollyPos;
    /// <summary>
    /// Posicion actual del jugador en el eje X
    /// </summary>
    private float _actPlayerPos;

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
        // ====== Inicializamos las variables necesarias ======
        _iniDollyPos = Dolly.position.x;
        _actDollyPos = Dolly.position.x;
        _posMeta = Meta.position.x;
        _actPlayerPos = Player.position.x;

        // ====== Ajustamos los valores de los sliders ======
        // Slider del jugador
        if (SliderPlayer !=  null)
        {
            // Ajustamos los valores
            SliderPlayer.minValue = _iniDollyPos;
            SliderPlayer.maxValue = _posMeta;
            SliderPlayer.value = _actPlayerPos;
        }
        // Slider de dolly
        if (SliderDolly != null)
        {
            // Ajustamos los valores
            SliderDolly.minValue = _iniDollyPos;
            SliderDolly.maxValue = _posMeta;
            SliderDolly.value = _actDollyPos;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // ====== Actualizamos las posiciones del jugador y de dolly ======
        _actDollyPos = Dolly.position.x;
        _actPlayerPos = Player.position.x;
        // ====== Cambiamos los valoeres en los sliders ======
        // Slider del jugador
        if (SliderPlayer != null)
        {
            // Cambiamos el valor
            SliderPlayer.value = _actPlayerPos;
        }
        // Slider de dolly
        if (SliderDolly != null)
        {
            // Cambiamos el valor
            SliderDolly.value = _actDollyPos;
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

} // class IndicadorDistancia 
// namespace
