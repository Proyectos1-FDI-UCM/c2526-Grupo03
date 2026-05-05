//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Higuera
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SoundManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    ///<summary>
    ///Aplicación de todos los sonidos
    ///</summary>
    [SerializeField] private AudioSource SFX_Jump;
    [SerializeField] private AudioSource SFX_Land;
    [SerializeField] private AudioSource SFX_Damage;
    [SerializeField] private AudioSource SFX_ScreamOne;
    [SerializeField] private AudioSource SFX_ScreamTwo;
    [SerializeField] private AudioSource SFX_ScreamThree;
    [SerializeField] private AudioSource SFX_ReloadRegular;
    [SerializeField] private AudioSource SFX_ReloadSpecial;
    [SerializeField] private AudioSource SFX_HasPerdiiido;
    [SerializeField] private AudioSource MUSIC_WiiDessertIntro;
    [SerializeField] private AudioSource MUSIC_WiiDessertLoop;
    [SerializeField] private AudioSource MUSIC_WiiCourseClear;
    [SerializeField] private AudioSource MUSIC_WildWestIntro;
    [SerializeField] private AudioSource MUSIC_WildWestLoop;
    [SerializeField] private AudioSource MUSIC_WarioWareOne;
    [SerializeField] private AudioSource MUSIC_WaroWareTwo;
    [SerializeField] private AudioSource MUSIC_Duel;
    [SerializeField] private AudioSource MUSIC_WaaWaaaWaaa;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // Creación de Instancia
    private static SoundManager _instance;

    // Variable que saca el tiempo dsp
    private double _audiotimer;

    // Variable diferenciadora de escenas
    private Scene _scene;

    // Variable que determina si es la primera vez que se llama a la ejecucion de música o no
    private bool _firsttime;

    // Variable que guarda la longitud del clip de audio
    private double _cliplength;

    // Variable de volumen máximo
    private float _maxvolume = 1f;

    // Variable que inicia la secuencia (se usará para evitar errores al reiniciar la escena)
    private bool _start = true;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    protected void Awake()
    {
        // Singleton
        if (_instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } // if-else somos instancia nueva o no.
    }
    private void Start()
    {
        _audiotimer = AudioSettings.dspTime;
        _firsttime = true;
        _scene = SceneManager.GetActiveScene();
        PlayLevelMusic();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    // Instancia pública
    public static SoundManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    // Todos los métodos que reproducen un audio
    //Efectos de sonido
    public void PlaySFXJump()
    {
        SFX_Jump.Play();
    }

    public void PlaySFXLand()
    {
        SFX_Land.Play();
    }

    public void PlaySFXDamage()
    {
        SFX_Damage.Play();
    }

    public void PlaySFXScreamOne()
    {
        SFX_ScreamOne.Play();
    }

    public void PlaySFXScreamTwo()
    {
        SFX_ScreamTwo.Play();
    }

    public void PlaySFXScreamThree()
    {
        SFX_ScreamThree.Play();
    }

    public void PlaySFXReloadReg()
    {
        SFX_ReloadRegular.Play();
    }

    public void PlaySFXReloadSpec()
    {
        SFX_ReloadSpecial.Play();
    }

    public void PlaySFXHasPerdiido()
    {
        SFX_HasPerdiiido.Play();
    }

    // Reproducción de música independiente de la escena
    public void PlayMusicWarioWareOne()
    {
        MUSIC_WarioWareOne.Play();
    }

    public void PlayMusicWarioWare2()
    {
        MUSIC_WaroWareTwo.Play();
    }

    public void PlayMusicWaaWaaWaaa()
    {
        MUSIC_WaaWaaaWaaa.Play();
    }

    public void PlayMusicCourseClear()
    {
        MUSIC_WiiCourseClear.Play();
    }

    // Método que pausa la música de nivel
    public void PauseLevelMusic()
    {
        if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying) ChangeLevelMusicVol(0f);
        else if (MUSIC_WildWestIntro.isPlaying || MUSIC_WildWestLoop.isPlaying) ChangeTutoMusicVol(0f);
    }

    public void ReduceLevelMusic()
    {
        if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying) ChangeLevelMusicVol(_maxvolume / 2);
        else if (MUSIC_WildWestIntro.isPlaying || MUSIC_WildWestLoop.isPlaying) ChangeTutoMusicVol(_maxvolume / 2);
    }

    // Método que pausa la música del QTE
    public void PauseQTEMusic()
    {
        if (MUSIC_WarioWareOne.isPlaying || MUSIC_WaroWareTwo.isPlaying) PauseMusicWarioWare();
    }

    // Método que permite reproducir música desde cualquier script
    public void PlayLevelMusic()
    {
        switch (_scene.buildIndex)
        {
            case 0: PlayMusicDuel(); break;
            case 1: PlayMusicWii(); break;
            case 2: PlayMusicWii(); break;
            case 3: PlayMusicDuel(); break;
            case 4: PlayMusicWildWest(); break;
            case 5: PlayMusicDuel(); break;
        }
    }

    // Diferenciar entre música de QTE y de nivel
    public bool GetWiiMusicPlaying()
    {
        if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying) return true;
        else return false;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    // Cambio de volumen de música
    private void ChangeLevelMusicVol(float vol)
    {
        if (MUSIC_WiiDessertIntro.isPlaying) MUSIC_WiiDessertIntro.volume = vol;
        else if (MUSIC_WiiDessertLoop.isPlaying) MUSIC_WiiDessertLoop.volume = vol;
    }
    private void ChangeTutoMusicVol(float vol)
    {
        if (MUSIC_WildWestLoop.isPlaying) MUSIC_WildWestLoop.volume = vol;
        else if (MUSIC_WildWestIntro.isPlaying) MUSIC_WildWestIntro.volume = vol;
    }

    // Pausa de mñusica de QTE
    private void PauseMusicWarioWare()
    {
        if (MUSIC_WarioWareOne.isPlaying) MUSIC_WarioWareOne.Stop();
        else if (MUSIC_WaroWareTwo.isPlaying) MUSIC_WaroWareTwo.Stop();
    }

    /// <summary>
    /// Reproducción de música dependiente de escena
    /// </summary>
    private void PlayMusicWii()
    {
        if (_firsttime)
        {
            MUSIC_WiiDessertIntro.Play();
            _cliplength = MUSIC_WiiDessertIntro.clip.length;
            MUSIC_WiiDessertLoop.PlayScheduled(_audiotimer + _cliplength);

            _firsttime = false;
        }
        else
        {
            MUSIC_WiiDessertIntro.volume = _maxvolume;
            MUSIC_WiiDessertLoop.volume = _maxvolume;
        }
    }

    private void PlayMusicWildWest()
    {
        if (_firsttime)
        {
            MUSIC_WildWestIntro.Play();
            _cliplength = MUSIC_WildWestIntro.clip.length;
            MUSIC_WildWestLoop.PlayScheduled(_audiotimer + _cliplength);
            _firsttime = false;
        }
        else
        {
            MUSIC_WildWestIntro.volume = _maxvolume;
            MUSIC_WildWestLoop.volume = _maxvolume;
        }
    }

    private void PlayMusicDuel()
    {
        MUSIC_Duel.Play();
    }
    #endregion

} // class SoundManager 
// namespace
