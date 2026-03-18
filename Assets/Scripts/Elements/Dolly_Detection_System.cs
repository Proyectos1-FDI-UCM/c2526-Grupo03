//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Dolly_Detection_System : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private float IntervaloParaSubir = 0f;

    // Cantidad de puntos restados por cada elemento
    [SerializeField] private int Extra_Penalty = 0;
    [SerializeField] private int Army_Penalty = 0;
    [SerializeField] private int Unrepaired_Penalty = 0;

    // Cantidad de puntuación aumentada cada "pulso" (frecuencia del pulso definida en la cámara)
    [SerializeField] private int Quality_Up = 0;

    // Puntuación de inicio de nivel
    [SerializeField] private int Starting_Quality = 0;

    // A borrar
    [SerializeField] private TextMeshProUGUI score;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    ///<summary>
    /// Variable que cuenta el tiempo pasado
    ///</summary>
    private float _timepassed;

    ///<summary>
    /// Variable que determina si se ha encontrado un objeto interactuable o no
    ///</summary>
    private bool _detected;

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
        LoseScreen.SetActive(false);
        _detected = true;
        GameManager.Instance.SetQuality(Starting_Quality);
        score.text = $"Score: {GameManager.Instance.GetCurrentQuality()}";
    }

    /// <summary>
    /// Update is called every frame the monovehaviour is active
    /// </summary>
    void Update()
    {
        if (_detected)
        {
            _timepassed = Time.time + IntervaloParaSubir;
            _detected = false;
        }
        else if (!_detected && Time.time >= _timepassed)
        {
            _timepassed = Time.time + IntervaloParaSubir;
            GameManager.Instance.QualityUp(Quality_Up);
            score.text = $"Score: {GameManager.Instance.GetCurrentQuality()}";
        }
        if (GameManager.Instance.GetCurrentQuality() <= 0) 
        { 
            // Instanciar aqui la losescreen
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            _detected = true;
            GameManager.Instance.QualityDown(GameManager.Instance.GetCurrentQuality());
            score.text = $"Score: {GameManager.Instance.GetCurrentQuality()}";
        }
        else if (collision.gameObject.TryGetComponent<Repair>(out var Repair) && !Repair.Repaired)
        {
            _detected = true;
            GameManager.Instance.QualityDown(Unrepaired_Penalty);
            score.text = $"Score: {GameManager.Instance.GetCurrentQuality()}";
        }
        else if (collision.gameObject.GetComponent<Extra_Regular>() != null)
        {
            _detected = true;
            GameManager.Instance.QualityDown(Extra_Penalty);
            score.text = $"Score: {GameManager.Instance.GetCurrentQuality()}";
        }
        else if (collision.gameObject.GetComponent<Extra_Army>() != null)
        { 
            _detected = true;
            GameManager.Instance.QualityDown(Army_Penalty);
            score.text = $"Score: {GameManager.Instance.GetCurrentQuality()}";
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Dolly_Detection_System1 
// namespace
