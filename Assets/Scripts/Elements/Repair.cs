//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Gabriel Adrian Oltean
// Aportadores: Víctor Román Román
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
    public bool HasFinishedRepairing = false;
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
    /// booleano que detecta si es la primera vez que se entra al QTE
    /// </summary>
    private bool _firstOpened = true;
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = SpriteBroken;
       
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (CanRepair && InputManager.Instance.RepairWasPressedThisFrame())
        {
            if (_firstOpened)
            {
                _selectedQTE = Random.Range(0, 3);
                _lastQTE = LevelManager.Instance.LastQTE();
                while (_selectedQTE == _lastQTE)
                {
                    _selectedQTE = Random.Range(0, 3);
                }
                LevelManager.Instance.CambiarValor(_selectedQTE);
                _firstOpened = false;
            }
            ActivateChosenQTE();
            Key.SetActive(false);
            IsRepairing = true;
        }
        if (IsRepairing && CanRepair)
        {
            _player.GetComponent<Movement_Player>().DesactivateMovement();
            _player.GetComponent<Jump>().DesactivateJump();
            _player.GetComponent<Shoot>().DesactivateShoot();
            _player.GetComponent<Scream_Reload>().DesactivateReload();

            CanRepair = false;
            _repairIniTime = Time.time;
        }
        if (Repaired)
        {
            _player.GetComponent<Movement_Player>().ActivateMovement();
            _player.GetComponent<Jump>().ActivateJump();
            _player.GetComponent<Shoot>().ActivateShoot();
            _player.GetComponent<Scream_Reload>().ActivateReload();
            this.GetComponent<Repair>().enabled = false; // mejor desactivarlo que destruirlo
            _spriteRenderer.sprite = SpriteRepaired;
            Key.SetActive(false);
        }
        if (HasFinishedRepairing) 
        {
            _player.GetComponent<Movement_Player>().ActivateMovement();
            _player.GetComponent<Jump>().ActivateJump();
            _player.GetComponent<Shoot>().ActivateShoot();
            _player.GetComponent<Scream_Reload>().ActivateReload();
            _spriteRenderer.sprite = SpriteBroken;
            Key.SetActive(false);
            HasFinishedRepairing = false;
        }

        if (HasPressedExit)
        {
            _player.GetComponent<Movement_Player>().ActivateMovement();
            _player.GetComponent<Jump>().ActivateJump();
            _player.GetComponent<Shoot>().ActivateShoot();
            _player.GetComponent<Scream_Reload>().ActivateReload();
            DisableChosenQTE();
            Key.SetActive(true);
            IsRepairing = false;
            CanRepair = true;
            HasPressedExit = false;
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
            CanRepair = false;
            Key.SetActive(false);
        }
    }

    /// <summary>
    /// Activa el panel del QTE seleccionado
    /// </summary>
    private void ActivateChosenQTE()
    {
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
