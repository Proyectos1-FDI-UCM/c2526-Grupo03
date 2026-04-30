//---------------------------------------------------------
// Encargado del disparo del jugador y el pool de balas
// Alejandro Garcia, Víctor Román y Gabriel Adrian
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Shoot : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// GameObject de la bala
    /// </summary>
    [SerializeField] private GameObject Bullet;
    /// <summary>
    /// Máximo de balas que puedes disparar sin recargar
    /// </summary>
    [SerializeField] private int MaxAmmo = 4;
    /// <summary>
    /// Controla el tiempo que debe pasar entre disparos.
    /// </summary>
    [SerializeField] private float Cooldown = .8f;
    /// <summary>
    ///  Posicion de la bala en relacion al personaje.
    /// </summary>
    [SerializeField] private Vector3 BulletPositionOffset = new Vector3(0.8f, 0);
    /// <summary>
    /// Número de balas en el "pool"
    /// </summary>
    [SerializeField] private int NumBulletsPool = 7;
    /// <summary>
    /// Contenedor del pool de balas
    /// </summary>
    [SerializeField] private Transform BulletsContainer;
    /// <summary>
    /// Sonidos de grito
    /// </summary>
    [SerializeField] private AudioSource Scream1;
    [SerializeField] private AudioSource Scream2;
    [SerializeField] private AudioSource Scream3;

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
    /// Controla el número de balas que tiene el jugador.
    /// </summary>
    private int _ammo;

    /// <summary>
    /// Tiempo en el que se podra volver a disparar
    /// </summary>
    private float _timeToWait = .0f;

    /// <summary>
    /// Contiene el sprite del personaje.
    /// </summary>
    private SpriteRenderer _playerSpriteRenderer;

    /// <summary>
    /// Pool de balas máximas que podrán aparecer.
    /// </summary>
    private GameObject[] _bulletsPool;

    /// <summary>
    /// Indica si debemos impedir el siguiente disparo
    /// </summary>
    private bool _desactivated = false;

    /// <summary>
    /// Randomizador
    /// </summary>
    private int _rnd;

    /// <summary>
    /// Contiene la información del componente Player
    /// </summary>
    private Movement_Player _player;

    /// <summary>
    /// Contiene la información del componente Jump
    /// </summary>
    private Jump _jump;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _ammo = MaxAmmo;
        _playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _player = gameObject.GetComponent<Movement_Player>();
        _jump = gameObject.GetComponent<Jump>();
    }

    /// <summary>
    /// Contiene la información del componente Animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _bulletsPool = new GameObject[NumBulletsPool]; // Reserva el espacio en el array
        for (int i = 0; i < NumBulletsPool; i++) // Por cada hueco genera una bala
        {
            GameObject bullet = Instantiate(Bullet);
            bullet.transform.SetParent(BulletsContainer, true);
            _bulletsPool[i] = bullet; // Guarda la bala en la pool
            bullet.SetActive(false);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // ---- DISPARO ----
        if(_animator)
        {
            _animator.SetBool("IsShootingAnim", false);
            _animator.SetBool("IsWalkingAndAttacking", false);
            _animator.SetBool("IsJumpingAndAttacking", false);
        }
        if (InputManager.Instance.FireWasPressedThisFrame() && !_desactivated && Time.time >= _timeToWait && _ammo > 0)
        {
            // -- Comprueba si hay balas usables o hay balas que recargar --
            int i = 0;
            while (i < NumBulletsPool && _bulletsPool[i].activeSelf)
            {
                i++;
            }
            if (i == NumBulletsPool) return; // No se puede disparar porque todas las balas estan en uso
           

            // -- Si hay balas usables --
            _rnd = Random.Range(0, 3);
            if (_animator && (_jump.IsJumping() || (_jump.IsJumping() && _player.IsWalking())) && _ammo > 0)
            {
                _animator.SetBool("IsJumpingAndAttacking", true);
            }
            else if (_animator && _player.IsWalking() && _ammo > 0)
            {
                _animator.SetBool("IsWalkingAndAttacking", true);
            }
            else if(_animator)
            {
                _animator.SetBool("IsShootingAnim", true);
            }
            int dir = _playerSpriteRenderer.flipX ? -1 : 1; // Para saber en que direccion mira el personjae principal

            Vector3 bulletOffset = gameObject.transform.position + (BulletPositionOffset * dir);

            _bulletsPool[i].transform.position = bulletOffset; // Coloca la bala enfrente del personaje
            _bulletsPool[i].GetComponent<SpriteRenderer>().flipX = _playerSpriteRenderer.flipX; // Rota la bala en la direccion del player
            _bulletsPool[i].SetActive(true);

            _timeToWait = Time.time + Cooldown;
            if(_animator)
            {
                _animator.SetBool("ReverseShooting", true);
            }
            switch (_rnd)
            {
                case 0: Scream1.Play(); break;
                case 1: Scream2.Play(); break;
                case 2: Scream3.Play(); break;
            }
            _ammo--;
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

    public int GetAmmo() // Devuelve la cantidad de disparos restantes
    {
        //Debug.Log("balas restantes: " + _ammo);
        return _ammo;
    }
    public void ReloadAmmo() // Recarga las balas al maximo
    {
        _ammo = MaxAmmo;
    }

    public void DesactivateShoot()
    {
        _desactivated = true;
    }
    public void ActivateShoot()
    {
        _desactivated = false;
    }
    public int GetMaxAmmo()
    {
        return MaxAmmo;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Shoot 
// namespace
