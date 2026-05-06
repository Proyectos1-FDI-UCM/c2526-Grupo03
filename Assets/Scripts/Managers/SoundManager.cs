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
    [SerializeField] private AudioSource SFX_CualityDown;
    [SerializeField] private AudioSource SFX_SandStep;
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

    /// <summary>
    /// Creación de Instancia
    /// </summary>
    private static SoundManager _instance;

    /// <summary>
    /// Variable que saca el tiempo dsp
    /// </summary>
    private double _audiotimer;

    /// <summary>
    /// Variable diferenciadora de escenas
    /// </summary>
    private Scene _scene;

    /// <summary>
    /// Variable que determina si es la primera vez que se llama a la ejecucion de música o no
    /// </summary>
    private bool _firsttime;

    /// <summary>
    /// Variable que guarda la longitud del clip de audio
    /// </summary>
    private double _cliplength;

    /// <summary>
    /// Variable de volumen máximo
    /// </summary>
    private float _maxvolume = 1f;

    /// <summary>
    /// Variable que inicia la secuencia (se usará para evitar errores al reiniciar la escena)
    /// </summary>
    private bool _start = true;
    /// <summary>
    /// Volumen actual de la music
    /// </summary>
    private float _volumenActualMusic = 0.75f;
    /// <summary>
    /// 
    /// </summary>
    private float _volumenActualEfects = 1f;
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
            Init();
        }
        else
        {
            _instance = this;
            // Queremos sobrevivir a cambios de escena.
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
    }
    private void Start()
    {
        Init();
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
    /// <summary>
    /// Efectos de sonido de salto
    /// </summary>
    public void PlaySFXJump()
    {
        SFX_Jump.Play();
    }
    /// <summary>
    /// Efectos de sonido de Caida
    /// </summary>
    public void PlaySFXLand()
    {
        SFX_Land.Play();
    }
    /// <summary>
    /// Efectos de sonido de Daño
    /// </summary>

    public void PlaySFXDamage()
    {
        SFX_Damage.Play();
    }

    /// <summary>
    /// Este metodo es para quitar al enemigo una vez que el efecto ha acabado
    /// </summary>
    /// <returns></returns>
    public bool GetDmgPlaying()
    {
        return SFX_Damage.isPlaying;
    }
    /// <summary>
    /// Efectos de sonido de Grito 1
    /// </summary>
    public void PlaySFXScreamOne()
    {
        SFX_ScreamOne.Play();
    }
    /// <summary>
    /// Efectos de sonido de Grito 2
    /// </summary>
    public void PlaySFXScreamTwo()
    {
        SFX_ScreamTwo.Play();
    }
    /// <summary>
    /// Efectos de sonido de Grito 3
    /// </summary>
    public void PlaySFXScreamThree()
    {
        SFX_ScreamThree.Play();
    }
    /// <summary>
    /// Efectos de sonido de Recarga
    /// </summary>
    public void PlaySFXReloadReg()
    {
        SFX_ReloadRegular.Play();
    }
    /// <summary>
    /// Efectos de sonido de recarga
    /// </summary>
    public void PlaySFXReloadSpec()
    {
        SFX_ReloadSpecial.Play();
    }
    /// <summary>
    /// Efectos de sonido de perdido
    /// </summary>
    public void PlaySFXHasPerdiido()
    {
        SFX_HasPerdiiido.Play();
    }
    /// <summary>
    /// Efectos de sonido de bajada de calidad
    /// </summary>
    public void PlaySFXCualityDown()
    {
        SFX_CualityDown.Play();
    }
    /// <summary>
    /// Efecto de sonido de andar (sin uso)
    /// </summary>
    public void PlaySFXSandStep()
    {
        SFX_SandStep.Play();
    }

    //----- Reproducción de música independiente de la escena ----

    /// <summary>
    /// Efectos de sonido QTE 1
    /// </summary>
    public void PlayMusicWarioWareOne()
    {
        MUSIC_WarioWareOne.Play();
    }
    /// <summary>
    /// Efectos del qte 2
    /// </summary>
    public void PlayMusicWarioWare2()
    {
        MUSIC_WaroWareTwo.Play();
    }
    /// <summary>
    /// Musica de derrota
    /// </summary>
    public void PlayMusicWaaWaaWaaa()
    {
        MUSIC_WaaWaaaWaaa.Play();
    }
    /// <summary>
    ///Musica de ganar
    /// </summary>
    public void PlayMusicCourseClear()
    {
        MUSIC_WiiCourseClear.Play();
    }

    /// <summary>
    /// Método que pausa la música de nivel
    /// </summary>
    public void PauseLevelMusic()
    {
        if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying)
        {
            MUSIC_WiiDessertLoop.volume = 0;
            MUSIC_WiiDessertIntro.volume = 0;
        }
        else if (MUSIC_WildWestIntro.isPlaying || MUSIC_WildWestLoop.isPlaying)
        {
            MUSIC_WildWestLoop.volume = 0;
            MUSIC_WiiDessertIntro.volume = 0;
        }
    }
    public void ResumeLevelMusic()
    {
        if (MUSIC_WarioWareOne.isPlaying || MUSIC_WaroWareTwo.isPlaying)
        {
            MUSIC_WarioWareOne.volume = _volumenActualMusic;
            MUSIC_WaroWareTwo.volume = _volumenActualMusic;

        }
        else if (MUSIC_WildWestIntro.isPlaying || MUSIC_WildWestLoop.isPlaying)
        {
            MUSIC_WildWestLoop.volume = _volumenActualMusic;
            MUSIC_WiiDessertIntro.volume = _volumenActualMusic;
        }
        else if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying)
        {
            MUSIC_WiiDessertLoop.volume = _volumenActualMusic;
            MUSIC_WiiDessertIntro.volume = _volumenActualMusic;
        }
       
        
    }

    /// <summary>
    /// Método que pausa la música del QTE
    /// </summary>
    public void PauseQTEMusic()
    {
        if (MUSIC_WarioWareOne.isPlaying || MUSIC_WaroWareTwo.isPlaying) PauseMusicWarioWare();
    }

    /// <summary>
    /// Método que permite reproducir música desde cualquier script
    /// </summary>
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
        SetVolumeToCurrent();
    }

    /// <summary>
    /// Diferenciar entre música de QTE y de nivel
    /// </summary>
    /// <returns></returns>
    public bool GetWiiMusicPlaying()
    {
        if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying) return true;
        else return false;
    }
    /// <summary>
    /// Cambia el volumen al establecido
    /// </summary>
    /// <returns></returns>
    public void CambiaVolumenMusic(float vol)
    {
        ChangeMusicVolume(vol);
        _volumenActualMusic = vol;
    }
    /// <summary>
    /// Metodo que devuelve el volumen de la musica
    /// </summary>
    /// <returns></returns>
    public float GetVolumenMusic()
    {
        return _volumenActualMusic;
    }
    /// <summary>
    /// Cambia volumen efects
    /// </summary>
    /// <param name="vol"></param>
    public void CambiaVolumenEfects(float vol)
    {
        ChangeEfectsVolume(vol);
        _volumenActualEfects = vol;
    }
    /// <summary>
    /// Metodo que devuelve
    /// </summary>
    /// <returns></returns>
    public float GetVolumenEfects()
    {
        return _volumenActualEfects;
    }
    public void SetVolumeToCurrent()
    {
        ChangeMusicVolume(_volumenActualMusic);
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Cambio de volumen de música
    /// </summary>
    /// <param name="vol"></param>
    private void ChangeMusicVolume(float vol)
    {
        MUSIC_WiiDessertIntro.volume = vol;
        MUSIC_WiiDessertLoop.volume = vol;
        MUSIC_WildWestLoop.volume = vol;
        MUSIC_WildWestIntro.volume = vol;
        _volumenActualMusic = vol;
    }


    /// <summary>
    /// Pausa de musica de QTE
    /// </summary>
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
    }
    /// <summary>
    /// Musica de wild west
    /// </summary>
    private void PlayMusicWildWest()
    {
        if (_firsttime)
        {
            MUSIC_WildWestIntro.Play();
            _cliplength = MUSIC_WildWestIntro.clip.length;
            MUSIC_WildWestLoop.PlayScheduled(_audiotimer + _cliplength);
            _firsttime = false;
        }
        
    }
    /// <summary>
    /// Musica duelo
    /// </summary>
    private void PlayMusicDuel()
    {
        MUSIC_Duel.Play();
    }
    /// <summary>
    /// Metodo que inicializa
    /// </summary>
    private void ChangeEfectsVolume(float vol)
    {
        SFX_Jump.volume = vol;
        SFX_Land.volume = vol;
        SFX_Damage.volume = vol;
        SFX_ScreamOne.volume = vol;
        SFX_ScreamTwo.volume = vol;
        SFX_ScreamThree.volume = vol;
        SFX_ReloadRegular.volume = vol;
        SFX_ReloadSpecial.volume = vol;
        SFX_HasPerdiiido.volume = vol;
        SFX_CualityDown.volume = vol;
        SFX_SandStep.volume = vol;
    }
    
    private void Init()
    {
        _audiotimer = AudioSettings.dspTime;
        _firsttime = true;
        _scene = SceneManager.GetActiveScene();
        PlayLevelMusic();
    }
    #endregion

} // class SoundManager 
// namespace
