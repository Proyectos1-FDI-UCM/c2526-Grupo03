//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Gabriel Adrian Oltean && Colaboradores:
//      Víctor Román Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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


    [SerializeField] private Sprite SpriteBroken;
    [SerializeField] private Sprite SpriteRepaired;

    [SerializeField] GameObject Key;
    /// <summary>
    /// Momento en el que se ha empezado a reparar
    /// </summary>
    public float _repairIniTime = .0f;

    /// <summary>
    /// Tiempo necesario para poder salir
    /// </summary>
    public float TiempoParaPoderSalir = 1f;
    

    public bool HasPressedExit = false;

    /// <summary>
    /// Indica si estás encima del objeto a reparar
    /// </summary>
    public bool CanRepair = false;
    /// <summary>
    /// Nos indica si estamos reparando
    /// </summary>
    public bool IsRepairing = false; 
    /// <summary>
    /// Nos indica si el objeto está reparado
    /// </summary>
    public bool Repaired = false;
    /// <summary>
    /// Variable que nos indicara si has fallado el QTE
    /// </summary>
    public bool HasFailedRepairing = false;
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
    /// GameObject del jugador para restringir su movimiento durante las reparaciones
    /// </summary>
    private GameObject _player;

    /// <summary>
    /// Número que codifica el QTE seleccionado al azar
    /// </summary>
    private int _selectedQTE;

    /// <summary>
    /// Número que guarda el QTE anterior
    /// </summary>
    private int _lastQTE;

    /// <summary>
    /// Booleano que detecta si es la primera vez que se entra al QTE
    /// </summary>
    private bool _firstOpened = true;

    /// <summary>
    /// Variable que contiene la información del componente Repair
    /// </summary>
    private Repair _repairComponent;

    /// <summary>
    /// Variable que contiene la información del componente Movement_Player
    /// </summary>
    private Movement_Player _movementPlayerComponent;

    /// <summary>
    /// Variable que contiene la información del componente Jump
    /// </summary>
    private Jump _jumpComponent;

    /// <summary>
    /// Variable que contiene la información del componente Shoot
    /// </summary>
    private Shoot _shootComponent;

    /// <summary>
    /// Variable que contiene la información del componente Scream_Reload
    /// </summary>
    private Scream_Reload _screamReloadComponent;


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
        _repairComponent = this.GetComponent<Repair>();
        _movementPlayerComponent = _player.GetComponent<Movement_Player>();
        _jumpComponent = _player.GetComponent<Jump>();
        _shootComponent = _player.GetComponent<Shoot>();
        _screamReloadComponent = _player.GetComponent<Scream_Reload>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //SpriteBroken será el sprite roto del objeto reparable.
        _spriteRenderer.sprite = SpriteBroken;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (CanRepair && InputManager.Instance.RepairWasPressedThisFrame()) //Si el objeto es reparable y se ha pulsado la tecla de reparación
        {
            if (_firstOpened) //Si es la primera vez que se abre el QTE
            {
                //Randomización del QTE que va a aparecer
                _selectedQTE = Random.Range(0, 3);
                _lastQTE = LevelManager.Instance.LastQTE();
                //Si se repite el anterior se vuelve a sacar un número aleatorio
                while (_selectedQTE == _lastQTE)
                {
                    _selectedQTE = Random.Range(0, 3);
                }
                LevelManager.Instance.CambiarValor(_selectedQTE);
                _firstOpened = false;
            }
            //Se activa el QTE que ha salido random y se desactiva el icono del input de encima del objeto reparable.
            ActivateChosenQTE();
            Key.SetActive(false);
            IsRepairing = true;
        }
        if (IsRepairing && CanRepair)
        {
            //Se desactivan todas las acciones del player para prohibirle interactuar con el nivel mientras repara
            _movementPlayerComponent.DesactivateMovement();
            _jumpComponent.DesactivateJump();
            _shootComponent.DesactivateShoot();
            _screamReloadComponent.DesactivateReload();

            CanRepair = false;
            _repairIniTime = Time.time;
        }
        if (Repaired)
        {
            //Al terminar de reparar se vuelven a activar todas las acciones que se habían desactivado
            _movementPlayerComponent.ActivateMovement();
            _jumpComponent.ActivateJump();
            _shootComponent.ActivateShoot();
            _screamReloadComponent.ActivateReload();
            _repairComponent.enabled = false; // mejor desactivarlo que destruirlo
            //Se cambia el sprite del objeto por el de su versión reparada
            _spriteRenderer.sprite = SpriteRepaired;
            Key.SetActive(false);
        }
        if (HasFailedRepairing) 
        {
            //Si se falla el QTE se vuelven a activar todos las acciones que se habían desactivado
            _movementPlayerComponent.ActivateMovement();
            _jumpComponent.ActivateJump();
            _shootComponent.ActivateShoot();
            _screamReloadComponent.ActivateReload();
            _spriteRenderer.sprite = SpriteBroken;  //Se mantiene el sprite de roto
            Key.SetActive(false);
            HasFailedRepairing = false; //Se cambia a false para no volver a entrar en la siguiente vuelta del bucle
        }

        if (HasPressedExit)
        {
            //Si se sale del QTE se vuelven a activar todos las acciones que se habían desactivado
            _movementPlayerComponent.ActivateMovement();
            _jumpComponent.ActivateJump();
            _shootComponent.ActivateShoot();
            _screamReloadComponent.ActivateReload();
            DisableChosenQTE();
            Key.SetActive(true);
            IsRepairing = false;
            CanRepair = true;
            HasPressedExit = false; //Se cambia a false para no volver a entrar en la siguiente vuelta del bucle
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsRepairing)
        {
            if (collision.gameObject.GetComponent<Movement_Player>() != null)
            {
                //Si el objeto que colisiona coincide con el player y no está reparando su estado pasa a poder reparar
                //y se activa la tecla superior del input para reparar
                _player = collision.gameObject;
                Key.SetActive(true);
                CanRepair = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            //Si el objeto que sale del collider coincide con el player se le desactiva el estado de poder reparar
            //y se desactiva la tecla superior del input para reparar
            CanRepair = false;
            Key.SetActive(false);
        }
    }

    /// <summary>
    /// Activa el panel del QTE seleccionado
    /// </summary>
    private void ActivateChosenQTE()
    {
        //Dependiendo del número que resulta la elección aleatoria anterior se activa un QTE u otro.
        if(_selectedQTE == 0)
        {
            Transform Hijo = transform.Find("Canvas/QTE_1_Panel");
            Hijo.gameObject.SetActive(true);
        }
        else if( _selectedQTE == 1)
        {
            Transform Hijo = transform.Find("Canvas/QTE_2_Panel");
            Hijo.gameObject.SetActive(true);
        }
        else if (_selectedQTE == 2)
        {
            Transform Hijo = transform.Find("Canvas/QTE_3_Panel");
            Hijo.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Desactiva el panel del QTE seleccionado
    /// </summary>
    private void DisableChosenQTE()
    {
        //Dependiendo del número que resulta la elección aleatoria anterior se desactiva un QTE u otro.
        if (_selectedQTE == 0)
        {
            Transform Hijo = transform.Find("Canvas/QTE_1_Panel");
            Hijo.gameObject.SetActive(false);
        }
        else if (_selectedQTE == 1)
        {
            Transform Hijo = transform.Find("Canvas/QTE_2_Panel");
            Hijo.gameObject.SetActive(false);
        }
        else if (_selectedQTE == 2)
        {
            Transform Hijo = transform.Find("Canvas/QTE_3_Panel");
            Hijo.gameObject.SetActive(false);
        }
    }
    #endregion   

} // class Repair 
// namespace
