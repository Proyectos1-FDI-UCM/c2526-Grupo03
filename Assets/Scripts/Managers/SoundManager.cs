//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Sergio Higuera && Colaboradores: 
//      Tristan Sanchez 
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
    #region Audio Source Efects
    /// <summary>
    /// Audio source Jump
    /// </summary>
    [SerializeField] private AudioSource SFX_Jump;
    /// <summary>
    /// Audio source aterrizaje
    /// </summary>
    [SerializeField] private AudioSource SFX_Land;
    /// <summary>
    /// Audio source recibir golpe
    /// </summary>
    [SerializeField] private AudioSource SFX_Damage;
    /// <summary>
    /// Audio source grito 1
    /// </summary>
    [SerializeField] private AudioSource SFX_ScreamOne;
    /// <summary>
    /// Audio source grito 2
    /// </summary>
    [SerializeField] private AudioSource SFX_ScreamTwo;
    /// <summary>
    /// Audio source grito 3
    /// </summary>
    [SerializeField] private AudioSource SFX_ScreamThree;
    /// <summary>
    /// Audio source recarga regular
    /// </summary>
    [SerializeField] private AudioSource SFX_ReloadRegular;
    /// <summary>
    /// Audio source recarga especial
    /// </summary>
    [SerializeField] private AudioSource SFX_ReloadSpecial;
    /// <summary>
    /// Audio source has perdido
    /// </summary>
    [SerializeField] private AudioSource SFX_HasPerdiiido;
    /// <summary>
    /// Audio source bajada calidad
    /// </summary>
    [SerializeField] private AudioSource SFX_CualityDown;
    /// <summary>
    /// Audio source pasos
    /// </summary>
    [SerializeField] private AudioSource SFX_SandStep;
    #endregion
    #region Audio Source musica
    /// <summary>
    /// Audio source musica nivel 1 y 2 intro
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WiiDessertIntro;
    /// <summary>
    /// Audio source  musica nivel 1 y 2
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WiiDessertLoop;
    /// <summary>
    /// Audio source musica nivel completado
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WiiCourseClear;
    /// <summary>
    /// Audio source  musica tutorial intro
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WildWestIntro;
    /// <summary>
    /// Audio source  musica tutorial 
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WildWestLoop;
    /// <summary>
    /// Audio source musica QTE pista 2
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WarioWareOne;
    /// <summary>
    /// Audio source musica QTE pista  2 
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WaroWareTwo;
    /// <summary>
    /// Audio source musica settings
    /// </summary>
    [SerializeField] private AudioSource MUSIC_Duel;
    /// <summary>
    /// Audio source musica menu
    /// </summary>
    [SerializeField] private AudioSource MUSIC_WaaWaaaWaaa;
    #endregion
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
        }
        else
        {
            _instance = this;

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
    #region Metodos Play Sonidos
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
            case 6: PlayMusicDuel(); break;
        }
        SetVolumeToCurrent();
    }
    #endregion

    #region Metodos GetIsPlaying

    /// <summary>
    /// Este metodo es para quitar al enemigo una vez que el efecto ha acabado
    /// </summary>
    /// <returns></returns>
    public bool GetDmgPlaying()
    {
        return SFX_Damage.isPlaying;
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
    #endregion
    /// <summary>
    /// Método que baja la música del nivel a 0 cuando mueres, ganas o entras a reparar
    /// </summary>
    public void StopLevelMusic()
    {
        if (MUSIC_WarioWareOne.isPlaying)
        {
            MUSIC_WarioWareOne.volume = 0;
        }
        else if (MUSIC_WaroWareTwo.isPlaying)
        {
            MUSIC_WaroWareTwo.volume = 0;
        }
        else if (MUSIC_WildWestIntro.isPlaying || MUSIC_WildWestLoop.isPlaying)
        {
            MUSIC_WildWestIntro.volume = 0;
            MUSIC_WildWestLoop.volume = 0;
        }
        else if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying)
        {
            MUSIC_WiiDessertIntro.volume = 0;
            MUSIC_WiiDessertLoop.volume = 0;
        }
    }
    /// <summary>
    /// Método que baja la música a la mitad cuando estas en la pausa
    /// </summary>
    public void PauseLevelMusic()
    {
        if (MUSIC_WarioWareOne.isPlaying)
        {
            MUSIC_WarioWareOne.volume = _volumenActualMusic / 2;
        }
        else if (MUSIC_WaroWareTwo.isPlaying)
        {
            MUSIC_WaroWareTwo.volume = _volumenActualMusic / 2;
        }
        else if (MUSIC_WildWestIntro.isPlaying || MUSIC_WildWestLoop.isPlaying)
        {
            MUSIC_WildWestIntro.volume = _volumenActualMusic / 2;
            MUSIC_WildWestLoop.volume = _volumenActualMusic / 2;
        }
        else if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying)
        {
            MUSIC_WiiDessertIntro.volume = _volumenActualMusic / 2;
            MUSIC_WiiDessertLoop.volume = _volumenActualMusic / 2;
        }
    }
    /// <summary>
    /// Metodo para volver a poner la musica
    /// </summary>
    public void ResumeMusic()
    {
        if (MUSIC_WarioWareOne.isPlaying)
        {
            MUSIC_WarioWareOne.volume = _volumenActualMusic;
        }
        else if (MUSIC_WaroWareTwo.isPlaying)
        {
            MUSIC_WaroWareTwo.volume = _volumenActualMusic;
        }
        else if (MUSIC_WildWestIntro.isPlaying|| MUSIC_WildWestLoop.isPlaying)
        {
            MUSIC_WildWestIntro.volume = _volumenActualMusic;
            MUSIC_WildWestLoop.volume = _volumenActualMusic;
        }
        else if (MUSIC_WiiDessertIntro.isPlaying || MUSIC_WiiDessertLoop.isPlaying) 
        {
            MUSIC_WiiDessertIntro.volume = _volumenActualMusic;
            MUSIC_WiiDessertLoop.volume = _volumenActualMusic;
        }


    }

    /// <summary>
    /// Metodo que para parar la musica 
    /// </summary>
    public void StopMusicQTE()
    {

        if (MUSIC_WarioWareOne.isPlaying) MUSIC_WarioWareOne.Stop();
        else if (MUSIC_WaroWareTwo.isPlaying) MUSIC_WaroWareTwo.Stop();
    }
    /// <summary>
    /// Cambia volumen Music
    /// </summary>
    /// <param name="vol">volumen</param>
    public void CambiaVolumenMusic(float vol)
    {
        ChangeMusicVolume(vol);
        _volumenActualMusic = vol;
        GameManager.Instance.SetCurrentMusicVolume(vol);
    }
    /// <summary>
    /// Cambia volumen efects
    /// </summary>
    /// <param name="vol">volumen</param>
    public void CambiaVolumenEfects(float vol)
    {
        ChangeEfectsVolume(vol);
        _volumenActualEfects = vol;
        GameManager.Instance.SetCurrentEffectsVolume(vol);
    }
    /// <summary>
    /// Sube el volumen al establecido
    /// </summary>
    public void SetVolumeToCurrent()
    {
        ChangeMusicVolume(_volumenActualMusic);
        ChangeEfectsVolume(_volumenActualEfects);
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
    /// <param name="vol">volumen</param>
    private void ChangeMusicVolume(float vol)
    {
        // Cambiamos el volumen actual de todas para la siguiente vez que suenen
        _volumenActualMusic = vol;

        // Cambiamos el volumen de la que esta sonando para escuchar como va cambiando
        MUSIC_WarioWareOne.volume = vol;
        MUSIC_WaroWareTwo.volume = vol;
        MUSIC_WiiDessertIntro.volume = vol;
        MUSIC_WiiDessertLoop.volume = vol;
        MUSIC_WildWestLoop.volume = vol;
        MUSIC_WildWestIntro.volume = vol;
        MUSIC_Duel.volume = vol;
        MUSIC_WiiCourseClear.volume = vol;
        MUSIC_WaaWaaaWaaa.volume = vol;
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
        //Cogemos el volumen del GameManager
        _volumenActualEfects = GameManager.Instance.GetCurrentEfectsVolume();
        _volumenActualMusic = GameManager.Instance.GetCurrentMusicVolume();
        //ponemos tiempo y primera vez
        _audiotimer = AudioSettings.dspTime;
        _firsttime = true;
        //Miramos la escena
        _scene = SceneManager.GetActiveScene();
        //Cambiamos volumen 
        SetVolumeToCurrent();
        //Lo ponemos Play
        PlayLevelMusic();
    }
    #endregion

} // class SoundManager 
// namespace
