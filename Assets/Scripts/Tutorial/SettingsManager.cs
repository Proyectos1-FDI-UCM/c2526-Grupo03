//---------------------------------------------------------
// Este Script se encargara de controlar el menu de settings
// Tristan Sanchez Lopez
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
public class SettingsManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Toggle de mando
    /// </summary>
    [SerializeField] private Toggle Mando;
    /// <summary>
    /// Bool que nos indica si esta en el tutorial
    /// </summary>
    [SerializeField] private bool Tutorial =false;

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
    /// Booleano que indica si usa mando
    /// </summary>
    private bool _mandoBool;


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
        //inicializamos
        _mandoBool = GameManager.Instance.GetMando();
        //revisamos que no sea null
        if (Mando != null)
        {
            //actualizamos
            ActualizaMando();

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
    /// <summary>
    /// Cambia entre si usas mando o no
    /// </summary>
    public void CambiaMando()
    {
        _mandoBool = !_mandoBool;
        GameManager.Instance.SetMando(_mandoBool);
        ActualizaMando();

        // Debug.Log(mando);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// actualiza el boton para si se esta usando mando
    /// </summary>
    private void ActualizaMando()
    {
        //Cambiamos el boton del toggle del mando
        Mando.SetIsOnWithoutNotify(_mandoBool);
        //Miramos si estamos en el tutorial para volver a poner las instruciones
        if (Tutorial)
        {
            Tutorial_Manager.Instance.ActualizaInst();
        }
    }

    #endregion

} // class SettingsManager 
// namespace
