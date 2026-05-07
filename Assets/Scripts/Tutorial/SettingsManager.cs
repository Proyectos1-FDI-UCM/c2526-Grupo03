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
    /// Slider del volumen de musica
    /// </summary>
    [SerializeField] private Slider Music;
    /// <summary>
    /// Slider del volumen de efects
    /// </summary>
    [SerializeField] private Slider Efects;
    /// <summary>
    /// Bool que nos indica si esta en el tutorial
    /// </summary>
    [SerializeField] private bool Tutorial = false;
    //-------- Cheats------------
    /// <summary>
    /// Toggle que nos indica si esta activo la NoMuerte
    /// </summary>
    [SerializeField] private Toggle NoMuerte;
    /// <summary>
    /// Toggle que nos indica si esta activo el AutoRepair
    /// </summary>
    [SerializeField] private Toggle AutoRepair;
    /// <summary>
    /// Toggle que nos indica si esta activo la NoCalidad
    /// </summary>
    [SerializeField] private Toggle NoCalidad;
    /// <summary>
    /// Toggle que nos indica si esta activo el cheat de desbloquear niveles
    /// </summary>
    [SerializeField] private Toggle DesbloquearNiveles;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    //------Cheats-------
    /// <summary>
    /// Bool que lleva si esta activo la NoMuerte
    /// </summary>
    private bool _noMuerteBool;
    /// <summary>
    /// Bool que lleva si esta activo el AutoRepair
    /// </summary>
    private bool _autoRepairBool;
    /// <summary>
    /// Bool que lleva si esta activo la NoCalidad
    /// </summary>
    private bool _noCalidadBool;
    /// <summary>
    /// Bool que lleva si esta activo el cheat de niveles desbloqueados
    /// </summary>
    private bool _nivelesDesbloqueados;

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
        _autoRepairBool = GameManager.Instance.GetAutoRepair();
        _noCalidadBool = GameManager.Instance.GetNoCalidad();
        _noMuerteBool = GameManager.Instance.GetNoMuerte();
        _nivelesDesbloqueados = GameManager.Instance.GetLevelsCheat();
        //Guardamos el volumen que usa el jugador
        Music.value = GameManager.Instance.GetCurrentMusicVolume();
        Efects.value = GameManager.Instance.GetCurrentEfectsVolume();
        //revisamos que no sea null
        if (AutoRepair != null)
        {
            //actualizamos
            ActualizaAutoRepair();
        }
        if (NoCalidad != null)
        {
            //actualizamos
            ActualizaNoCalidad();
        }
        if (NoMuerte != null)
        {
            //actualizamos
            ActualizaNoMuerte();
        }
        if (DesbloquearNiveles != null)
        {
            //actualizamos
            ActualizaDesbloquearNiveles();
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
    /// Cambia entre si usas autoRepair o no
    /// </summary>
    public void CambiaAutoRepair()
    {
        _autoRepairBool = !_autoRepairBool;
        GameManager.Instance.SetAutoRepair(_autoRepairBool);
        ActualizaAutoRepair();
    }
    /// <summary>
    /// Cambia entre si usas el noCalidad o no
    /// </summary>
    public void CambiaNoCalidad()
    {
        _noCalidadBool = !_noCalidadBool;
        GameManager.Instance.SetNoCalidad(_noCalidadBool);
        ActualizaNoCalidad();
    }
    /// <summary>
    /// Cambia entre si usas el noMuerte o no
    /// </summary>
    public void CambiaNoMuerte()
    {
        _noMuerteBool = !_noMuerteBool;
        GameManager.Instance.SetNoMuerte(_noMuerteBool);
        ActualizaNoMuerte();
    }
    /// <summary>
    /// Cambia entre si usas o no el cheat de desbloquear niveles
    /// </summary>
    public void CambiaDesbloquearNiveles()
    {
        _nivelesDesbloqueados = !_nivelesDesbloqueados;
        GameManager.Instance.SetLevelCheats(_nivelesDesbloqueados);
        ActualizaDesbloquearNiveles();
    }
    /// <summary>
    /// MEtodo que cambia el volumen de la musica
    /// </summary>
    public void CambiaMusicVolume()
    {
        //Cambiamos el volumen en el sound manager (asi el jugador lo nota)
        SoundManager.Instance.CambiaVolumenMusic(Music.value);
        //Guardamos el valor para usarlo entre escenas
        GameManager.Instance.SetCurrentMusicVolume(Music.value);
    }
    /// <summary>
    /// Metodo que cambia el volumen de los efectos
    /// </summary>
    public void CambiaEfectsVolume()
    {
        //Cambiamos el volumen en el sound manager (asi el jugador lo nota)
        SoundManager.Instance.CambiaVolumenEfects(Efects.value);
        //Guardamos el valor para usarlo entre escenas
        GameManager.Instance.SetCurrentEffectsVolume(Efects.value);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// actualiza el boton para si se esta usando el autoRepair
    /// </summary>
    private void ActualizaAutoRepair()
    {
        //Cambiamos el boton del toggle del autoRepair
        AutoRepair.SetIsOnWithoutNotify(_autoRepairBool);

    }
    /// <summary>
    /// actualiza el boton para si se esta usando el noMuerte
    /// </summary>
    private void ActualizaNoMuerte()
    {
        //Cambiamos el boton del toggle del noMuerte
        NoMuerte.SetIsOnWithoutNotify(_noMuerteBool);
    }
    /// <summary>
    /// actualiza el boton para si se esta usando la noCalidad
    /// </summary>
    private void ActualizaNoCalidad()
    {
        //Cambiamos el boton del toggle del noCalidad
        NoCalidad.SetIsOnWithoutNotify(_noCalidadBool);
    }
    /// <summary>
    /// actualiza el boton para si se esta usando el cheat de desbloquear niveles
    /// </summary>
    private void ActualizaDesbloquearNiveles()
    {
        //Cambiamos el boton del toggle del noCalidad
        DesbloquearNiveles.SetIsOnWithoutNotify(_nivelesDesbloqueados);
    }

    #endregion

} // class SettingsManager 
// namespace
