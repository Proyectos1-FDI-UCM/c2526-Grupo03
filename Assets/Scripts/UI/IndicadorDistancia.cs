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
    /// Referencia al indicador de distancia recorrida del jugador
    /// </summary>
    [SerializeField] private Image IndicadorPlayer;
    /// <summary>
    /// Referencia al indicador de distancia recorrida por la bola del desierto
    /// </summary>
    [SerializeField] private GameObject IndicadorBolaDesierto;
    /// <summary>
    /// Imagen de la bola del desierto en el indicador de distancia
    /// </summary>
    [SerializeField] private Image ImagenBola;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Image IndicadorMeta;
    /// <summary>
    /// GameObject de la bandera del final del nivel
    /// </summary>
    [SerializeField] private GameObject Final;
    /// <summary>
    /// Referencia al jugador 
    /// </summary>
    [SerializeField] private GameObject Player;
    /// <summary>
    /// Referencia a la bola del desierto
    /// </summary>
    [SerializeField] private GameObject BolaDelDesierto;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // Posiciones de los prefabs del jugador y la bola del desierto en el eje x
    private float _posPlayer, _posBola;
    // Posicion del final e inicio del nivel en el eje x
    private float _posFinal, _posInicio;
    /// <summary>
    /// Posicion que se considera el inicio del indicador de distancia
    /// </summary>
    private float _indicadorStart;
    /// <summary>
    /// Posicion que se considera el final del indicador de distancia
    /// </summary>
    private float _indicadorEnd;
    /// <summary>
    /// Velocidad con la que rota la bola del desierto en el indicador de distancia
    /// </summary>
    private const float _velocidadRotacionBola = 200.0f;
    /// <summary>
    /// Rotacion actual de la bola del desierto en el indicador de distancia
    /// </summary>
    private float _rotacionBola = .0f;
    /// RectTransforms usados para mover de forma segura los elementos UI.
    /// </summary>
    private RectTransform _rectIndicadorPlayer, _rectIndicadorBola;

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
        // Obtener RectTransforms de los elementos UI y sus posiciones iniciales (anchoredPosition.x)
        if (IndicadorPlayer != null)
            _rectIndicadorPlayer = IndicadorPlayer.GetComponent<RectTransform>();
        if (IndicadorMeta != null)
            _rectIndicadorBola = IndicadorBolaDesierto.GetComponent<RectTransform>();
        if (_rectIndicadorPlayer != null)
            _indicadorStart = _rectIndicadorPlayer.anchoredPosition.x;
        if (_rectIndicadorBola != null)
            _indicadorEnd = _rectIndicadorBola.anchoredPosition.x;

        // Posiciones en el eje x del inicio y el final del nivel (mundo)
        if (BolaDelDesierto != null)
            _posInicio = BolaDelDesierto.transform.position.x;
        if (Final != null)
            _posFinal = Final.transform.position.x;

        // Si por alguna razón las referencias no están bien asignadas, evitar división por cero
        if (Mathf.Approximately(_posFinal, _posInicio))
        {
            // Forzamos un rango válido para evitar NaN en cálculos posteriores
            _posFinal = _posInicio + 1f;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Rotación de la imagen de la bola (indicador UI)
        if (ImagenBola != null)
        {
            _rotacionBola = ImagenBola.transform.rotation.eulerAngles.z - _velocidadRotacionBola * Time.deltaTime;
            ImagenBola.transform.rotation = Quaternion.Euler(0f, 0f, _rotacionBola);
        }

        // Posiciones en el eje x del jugador y la bola del desierto (mundo)
        if (Player != null)
            _posPlayer = Player.transform.position.x;
        if (BolaDelDesierto != null)
            _posBola = BolaDelDesierto.transform.position.x;

        // Calculamos el porcentaje avanzado del jugador en el nivel (0..1)
        float porcentajeAvanzadoPlayer = 0f;
        if (!Mathf.Approximately(_posFinal, _posInicio))
            porcentajeAvanzadoPlayer = Mathf.Clamp01((_posPlayer - _posInicio) / (_posFinal - _posInicio));

        // Convertimos ese porcentaje a la posición X en la UI y actualizamos el indicador del player
        if (_rectIndicadorPlayer != null)
        {
            float targetX = Mathf.Lerp(_indicadorStart, _indicadorEnd, porcentajeAvanzadoPlayer);
            _rectIndicadorPlayer.anchoredPosition = new Vector2(targetX, _rectIndicadorPlayer.anchoredPosition.y);
        }

        float porcentajeAvanzadoBola = 0f;
        if (!Mathf.Approximately(_posFinal, _posInicio))
            porcentajeAvanzadoBola = Mathf.Clamp01((_posBola - _posInicio) / (_posFinal - _posInicio));

        // Convertimos ese porcentaje a la posición X en la UI y actualizamos el indicador de la bola
        if (_rectIndicadorBola != null)
        {
            float targetX = Mathf.Lerp(_indicadorStart, _indicadorEnd, porcentajeAvanzadoBola);
            _rectIndicadorBola.anchoredPosition = new Vector2(targetX, _rectIndicadorBola.anchoredPosition.y);
            Debug.Log($"Bola: posX={_rectIndicadorBola.anchoredPosition}, porcentaje={porcentajeAvanzadoBola}, targetX={targetX}");
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

    private float WorldXToIndicatorX(float worldX)
    {
        float porcentaje = Mathf.Clamp01((worldX - _posInicio) / (_posFinal - _posInicio));
        return Mathf.Lerp(_indicadorStart, _indicadorEnd, porcentaje);
    }

    #endregion   

} // class IndicadorDistancia 
// namespace
