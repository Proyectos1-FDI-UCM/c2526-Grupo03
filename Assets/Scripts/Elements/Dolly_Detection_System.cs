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
    /// <summary>
    /// Intervalo para subir la puntuacion
    /// </summary>
    [SerializeField] private float IntervaloParaSubir = 0f;
    /// <summary>
    /// Objeto de la meta
    /// </summary>
    [SerializeField] private Finish _Finish = null;
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
        _detected = true;  
    }

    /// <summary>
    /// Update is called every frame the monovehaviour is active
    /// </summary>
    void FixedUpdate()
    {
        if (_detected)
        {
            _timepassed = Time.time + IntervaloParaSubir;
            _detected = false;
        }
        else if (!_detected && Time.time >= _timepassed)
        {
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
            //Miramos si es reparable 
            if (collision.gameObject.GetComponent<Repair>() != null && collision.gameObject.GetComponent<Repair>().Repaired)
            {
                return;
            }
            //Miramos si no es reparable para sumar puntuacion 
            _detected = true;
            LevelManager.Instance.QualityDown(collision.gameObject.GetComponent<DetectableObject>().GetQualityDown());
            
        }
        //Miramos si es el jugador  
        else if (collision.gameObject.GetComponent<Movement_Player>() != null){
            //revisamos si existe una meta

            if( _Finish != null)
            {
                //Si existe la meta miramos si tiene el componente finish y si has ganado 
                if (!_Finish.GetComponent<Finish>().HasWin())
                {
                    //Restamos puntuacion para perder
                    LevelManager.Instance.QualityDown(LevelManager.Instance.GetcurrentScore());
                }
                
            }
            else
            {
                //sino hay meta perdemos directamente
                LevelManager.Instance.QualityDown(LevelManager.Instance.GetcurrentScore());
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
