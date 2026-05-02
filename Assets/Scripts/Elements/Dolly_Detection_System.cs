//---------------------------------------------------------
// Componente que detecta cuándo un objeto entra dentro del campo de visión de la cámara Dolly.
// Sergio Higuera Gil && Colaboradores:
//      Víctor Román Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
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

    /// <summary>
    /// Intervalo para subir la puntuacion
    /// </summary>
    [SerializeField] private float IntervaloParaSubir = 0f;

    /// <summary>
    /// Objeto de la meta
    /// </summary>
    [SerializeField] private Finish Meta = null;
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
    /// <summary>
    /// variable que dertemina si los cheats de no muerte estan activos
    /// </summary>
    private bool _cheatsNoMuerte;
    /// <summary>
    ///  variable que dertemina si los cheats de no calidad estan activos
    /// </summary>
    private bool _cheatsNoCalidad;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    ///Awake is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {
        //Ponemos detecte en true
        _detected = true;
    }

    /// <summary>
    /// Update is called every frame the monovehaviour is active
    /// </summary>
    void FixedUpdate()
    {
        //Inicializamos para ver si estan los cheats
        _cheatsNoMuerte = GameManager.Instance.GetNoMuerte();
        _cheatsNoCalidad = GameManager.Instance.GetNoCalidad();
        //Si ha detectado un objeto
        if (_detected)
        {
            //calcualamos el tiempo que ha pasado
            _timepassed = Time.time + IntervaloParaSubir;
            //Y ponemos el detected a false
            _detected = false;
        } //Y si no hemos detectado
        else if (!_detected && Time.time >= _timepassed)
        {
            //Subimos la calidad
            LevelManager.Instance.QualityUp();
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
        //Miramos si es un objeto detectable 
        if (collision.gameObject.GetComponent<DetectableObject>() != null)
        {
            //Miramos si no es reparable para restar puntuacion
            if (collision.gameObject.GetComponent<Repair>() == null || collision.gameObject.GetComponent<Repair>().IsRepaired() == false)
            {
                //Cambiamos a detectado para resetear la subida de puntos sola
                _detected = true;
                //Bajamos la calidad
                if (!_cheatsNoCalidad)
                {
                    LevelManager.Instance.QualityDown(collision.gameObject.GetComponent<DetectableObject>().GetQualityDown());
                }
            }
        }
        //Miramos si es el jugador  
        else if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            //revisamos si existe una meta
            if (Meta != null)
            {
                //Si existe la meta miramos no has ganado 
                if (!Meta.HasWin())
                {
                    if (!_cheatsNoMuerte)
                    {
                        //Restamos puntuacion para perder
                        LevelManager.Instance.QualityDown(LevelManager.Instance.GetcurrentScore());
                    }
                }
            }
            else
            {
                if (!_cheatsNoMuerte)
                {
                    //sino hay meta perdemos directamente
                    LevelManager.Instance.QualityDown(LevelManager.Instance.GetcurrentScore());
                }
            }
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
