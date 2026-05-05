//---------------------------------------------------------
// Componente del comportamiento de los objetos reparables. En él se establecen las condiciones para que un elemento pueda
// ser reparado y cuando pasa a estarlo.
// Gabriel Adrian Oltean && Colaboradores: Sergio Higuera
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
public class Repair : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Sprite del objeto reparable en su estado roto.
    /// </summary>
    [SerializeField] private Sprite SpriteBroken;

    /// <summary>
    /// Sprite del objeto reparable en su estado reparado.
    /// </summary>
    [SerializeField] private Sprite SpriteRepaired;

    /// <summary>
    /// Sprite del objeto roto con borde blanco
    /// </summary>
    [SerializeField] private Sprite SpriteWhiteBorder;

    /// <summary>
    /// GameObject que contiene el jugador de la escena.
    /// </summary>
    [SerializeField] private GameObject Player;

    /// <summary>
    /// GameObject que contiene el QTE de spam.
    /// </summary>
    [SerializeField] private GameObject SpamQTE;

    /// <summary>
    /// GameObject que contiene el QTE de la manivela.
    /// </summary>
    [SerializeField] private GameObject ManivelaQTE;

    /// <summary>
    /// GameObject que contiene el QTE de fila de teclas.
    /// </summary>
    [SerializeField] private GameObject TeclasQTE;

    /// <summary>
    /// GameObject que contiene el QTE de timing.
    /// </summary>
    [SerializeField] private GameObject TimingQTE = null;

    /// <summary>
    /// Cuando es true -> Obliga al objeto a realizar el ForcedQTE 
    /// </summary>
    [SerializeField] private bool ForceQTE;

    /// <summary>
    /// QTE obligatorio cuando ForceQTE es true
    /// </summary>
    [SerializeField] private int ForcedQTE;
    /// <summary>
    /// GameObject que contiene la tecla a mostrar.
    /// </summary>
    [SerializeField] GameObject Keybind;
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
    /// Componente con el que cambiaremos el sprite por el reparado
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Número que codifica el QTE seleccionado al azar
    /// </summary>
    private int _selectedQTE;

    /// <summary>
    /// Variable que contiene la información del componente Movement_Player
    /// </summary>
    private Movement_Player _movementPlayerComponent;

    /// <summary>
    /// Momento en el que se ha empezado a reparar
    /// </summary>
    private float _repairIniTime = .0f;

    /// <summary>
    /// Tiempo necesario para poder salir
    /// </summary>
    private float _exitTime = 1f;

    /// <summary>
    /// Indica si el jugador ha salido del QTE
    /// </summary>
    private bool _hasPressedExit = false;

    /// <summary>
    /// Indica si estás encima del objeto a reparar
    /// </summary>
    private bool _canRepair = false;

    /// <summary>
    /// Nos indica si estamos reparando
    /// </summary>
    private bool _isRepairing = false;

    /// <summary>
    /// Nos indica si el objeto está reparado
    /// </summary>
    private bool _repaired = false;

    /// <summary>
    /// Nos indica si es la primera vez que se abre el QTE
    /// </summary>
    private bool _firstOpen = true;
    /// <summary>
    /// Nos indica si ya se a elegido el QTE la primera vez que entras a reparar
    /// </summary>
    private bool _QTEWasSelected = false;
    /// <summary>
    /// Variable que nos indicara si ya has cambiado el estado a reparado
    /// </summary>
    private bool _changedToRepaired = false;
    /// <summary>
    /// Variable que nos indica si estamos usando cheats o no
    /// </summary>
    private bool _cheats = false;
    /// <summary>
    /// Variable aleatoria para musica
    /// </summary>
    private int _rnd;
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
        //Se cachean todos los componentes que se van a usar en variables.
        _movementPlayerComponent = Player.GetComponent<Movement_Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _cheats = GameManager.Instance.GetAutoRepair();
        //SpriteBroken será el sprite roto del objeto reparable.
        _spriteRenderer.sprite = SpriteBroken;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Si el objeto es reparable y se ha pulsado la tecla de reparación
        if (_canRepair && InputManager.Instance.RepairWasPressedThisFrame())
        {
            // Se elige el QTE la primera vez que abres el reparable para que no te toque el mismo QTE que el que acabas de hacer
            if (_firstOpen && !_QTEWasSelected)
            {
                // ====== Forzar un QTE ======
                if (ForceQTE)
                {
                    // Elegimos el QTE forzado
                    _selectedQTE = ForcedQTE;
                }
                // ====== Seleccionar uno aleatorio ======
                else
                {
                    //Randomización del QTE que va a aparecer
                    _selectedQTE = Random.Range(0, 4);
                    int _lastQTE = LevelManager.Instance.LastQTE();
                    //Si se repite el anterior se vuelve a sacar un número aleatorio
                    while (_selectedQTE == _lastQTE)
                    {
                        //Randomización del QTE que va a aparecer
                        _selectedQTE = Random.Range(0, 4);
                    }
                }
                // Cambiamos el valor del ultimo QTE seleccionado
                LevelManager.Instance.CambiarValor(_selectedQTE);
                // Marcamos que ya se ha elegido el QTE para que no se repita esta parte
                _QTEWasSelected = true;
            }
            if (_cheats) // parte de la autoreparacion
            {
                _repaired = true;
                _isRepairing = true;
            }
            else
            {
                // Se pausa la música el nivel y se reproduce una canción de QTE al azar
                SoundManager.Instance.PauseLevelMusic();
                //Se activa el QTE que ha salido random y se desactiva el icono del input de encima del objeto reparable.
                ActivateChosenQTE();

                // Ocultamos las teclas de reparación
                Keybind.SetActive(false);

                //Se desactivan todas las acciones del player para prohibirle interactuar con el nivel mientras repara
                _movementPlayerComponent.DisablePlayer();

                _isRepairing = true;
                _canRepair = false;
                _repairIniTime = Time.time;
            }
            
        }
        if (_repaired && !_changedToRepaired)
        {
            // Se pausa la música del QTE y se continua la música del nivel
            SoundManager.Instance.PauseQTEMusic();
            SoundManager.Instance.PlayLevelMusic();
            //Al terminar de reparar se vuelven a activar todas las acciones que se habían desactivado
            _movementPlayerComponent.ActivatePlayer();
            // mejor desactivarlo que destruirlo
            this.enabled = false;
            //Se cambia el sprite del objeto por el de su versión reparada
            _spriteRenderer.sprite = SpriteRepaired;
            Keybind.SetActive(false);
            _changedToRepaired = true;
        }

        if (_isRepairing && (_hasPressedExit || _movementPlayerComponent.EstaSiendoEmpujado()))
        {
            //Si se sale del QTE se vuelven a activar todos las acciones que se habían desactivado
            _movementPlayerComponent.ActivatePlayer();
            // Se pausa la música del QTE y se continua la música del nivel
            SoundManager.Instance.PauseQTEMusic();
            SoundManager.Instance.PlayLevelMusic();
            DisableChosenQTE();
            Keybind.SetActive(true);
            _isRepairing = false;
            _canRepair = true;
            if (_hasPressedExit) _hasPressedExit = false; //Se cambia a false para no volver a entrar en el siguiente frame
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
    /// Comunica a componentes externos el valor del tiempo inicial de reparación de un QTE.
    /// </summary>
    /// <returns></returns>
    public float RepairIniTime()
    {
        return _repairIniTime;
    }

    /// <summary>
    /// Comunica a componentes externos el valor del tiempo de salida de un QTE.
    /// </summary>
    /// <returns></returns>
    public float ExitTime()
    {
        return _exitTime;
    }

    /// <summary>
    /// Comunica a componentes si se ha pulsado la tecla para salid de un QTE.
    /// </summary>
    /// <returns></returns>
    public void HasPressedExit(bool hasPressed)
    {
        _hasPressedExit = hasPressed;
    }

    /// <summary>
    /// Comunica a componentes si se está reparando un QTE.
    /// </summary>
    /// <returns></returns>
    public bool IsRepairing()
    {
        return _isRepairing;
    }

    /// <summary>
    /// Comunica a componentes si está reparado el QTE activo.
    /// </summary>
    /// <returns></returns>
    public void Repaired(bool repaired)
    {
        _repaired = repaired;
    }

    /// <summary>
    /// Comunica a componentes si se ha reparado el QTE activo.
    /// </summary>
    /// <returns></returns>
    public bool IsRepaired()
    {
        return _repaired;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //actualiza para ver si se han activado los cheats
        ActualizaCheats();
        if (!_isRepairing)
        {
            //Miramos si es el player
            if (collision.gameObject.GetComponent<Movement_Player>() != null)
            {
                
                //Si el objeto que colisiona coincide con el player y no está reparando su estado pasa a poder reparar
                //y se activa la tecla superior del input para reparar
                Player = collision.gameObject;
                _spriteRenderer.sprite = SpriteWhiteBorder;
                Keybind.SetActive(true);
                _canRepair = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Miramos si es el player
        if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            //Si el objeto que sale del collider coincide con el player se le desactiva el estado de poder reparar
            //y se desactiva la tecla superior del input para reparar
            _canRepair = false;
            if (!_repaired) _spriteRenderer.sprite = SpriteBroken;
            Keybind.SetActive(false);
        }
    }

    /// <summary>
    /// Activa el panel del QTE seleccionado
    /// </summary>
    private void ActivateChosenQTE()
    {
        //Dependiendo del número que resulta la elección aleatoria anterior se activa un QTE u otro.
        switch (_selectedQTE)
        {
            case 0: SpamQTE.SetActive(true); break;
            case 1: ManivelaQTE.SetActive(true); break;
            case 2: TeclasQTE.SetActive(true); break;
            case 3: TimingQTE.SetActive(true); break;
        }
        _rnd = Random.Range(0, 2);
        if (_rnd == 0) SoundManager.Instance.PlayMusicWarioWareOne();
        else SoundManager.Instance.PlayMusicWarioWare2();
    }

    /// <summary>
    /// Desactiva el panel del QTE seleccionado
    /// </summary>
    private void DisableChosenQTE()
    {
        //Dependiendo del número que resulta la elección aleatoria anterior se desactiva un QTE u otro.
        switch (_selectedQTE)
        {
            case 0: SpamQTE.SetActive(false); break;
            case 1: ManivelaQTE.SetActive(false); break;
            case 2: TeclasQTE.SetActive(false); break;
            case 3: TimingQTE.SetActive(false); break;
        }
    }
    /// <summary>
    /// Metodo que actualiza los cheats
    /// </summary>
    private void ActualizaCheats()
    {
        _cheats = GameManager.Instance.GetAutoRepair();
    }
    #endregion   

} // class Repair 
// namespace
