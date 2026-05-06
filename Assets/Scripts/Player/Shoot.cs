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
        //Cachemos los componentes a los objetos
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
        //Creamos el array de balas
        _bulletsPool = new GameObject[NumBulletsPool]; // Reserva el espacio en el array
        for (int i = 0; i < NumBulletsPool; i++) // Por cada hueco genera una bala
        {
            //Istanciamos la bala
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
        bool soloDispara = false;
        // ---- DISPARO ----
        if (InputManager.Instance.FireWasPressedThisFrame() && !_desactivated && Time.time >= _timeToWait && _ammo > 0)
        {
            // -- Comprueba si hay balas usables o hay balas que recargar --
            int i = 0;
            while (i < NumBulletsPool && _bulletsPool[i].activeSelf)
            {
                i++;
            }
            if (i == NumBulletsPool) return; // No se puede disparar porque todas las balas estan en uso
           
            if(_animator)
            {
                ResetAttackTriggers();
            }

            // -- Si hay balas usables --
            _rnd = Random.Range(0, 3);
            if (_animator && (_jump.IsJumping() || (_jump.IsJumping() && _player.IsWalking())) && _ammo > 0)
            {//Animacion disparo y salto
                _animator.SetTrigger("JumpingAndAttacking");
            }
            else if (_animator && _player.IsWalking() && _ammo > 0)
            {
                //Animacion disparo y Andar
                _animator.SetTrigger("WalkingAndAttacking");
            }
            //Animacion disparo
            else if(_animator)
            {
                soloDispara = true;
                _animator.SetTrigger("ShootingAnim");
            }
            int dir = _playerSpriteRenderer.flipX ? -1 : 1; // Para saber en que direccion mira el personjae principal

            Vector3 bulletOffset = gameObject.transform.position + (BulletPositionOffset * dir);

            _bulletsPool[i].transform.position = bulletOffset; // Coloca la bala enfrente del personaje
            _bulletsPool[i].GetComponent<SpriteRenderer>().flipX = _playerSpriteRenderer.flipX; // Rota la bala en la direccion del player
            _bulletsPool[i].SetActive(true);

            _timeToWait = Time.time + Cooldown;
            //animacion de disparo
            if(_animator && soloDispara)
            {
                _animator.SetTrigger("ReverseShootingAnim");
            }
            //Random de sonidos de disparos
            switch (_rnd)
            {
                case 0: SoundManager.Instance.PlaySFXScreamOne(); break;
                case 1: SoundManager.Instance.PlaySFXScreamTwo(); break;
                case 2: SoundManager.Instance.PlaySFXScreamThree(); break;
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
    /// <summary>
    /// Devuelve la cantidad de disparos restantes
    /// </summary>
    public int GetAmmo() 
    {
        //Debug.Log("balas restantes: " + _ammo);
        return _ammo;
    }
    /// <summary>
    /// Metodo que recarga las balas al maximo
    /// </summary>
    public void ReloadAmmo()
    {
        _ammo = MaxAmmo;
    }
    /// <summary>
    /// Metodo que desactiva el disparo
    /// </summary>
    public void DesactivateShoot()
    {
        _desactivated = true;
    }
    /// <summary>
    /// Metodo que activa el disparo
    /// </summary>
    public void ActivateShoot()
    {
        _desactivated = false;
    }
    /// <summary>
    /// Metodo que devuelve el numero de balas maximo
    /// </summary>
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

    /// <summary>
    /// Resetea todas las animaciones de disparo
    /// </summary>
    private void ResetAttackTriggers()
    {
        _animator.ResetTrigger("JumpingAndAttacking");
        _animator.ResetTrigger("WalkingAndAttacking");
        _animator.ResetTrigger("ShootingAnim");
        _animator.ResetTrigger("ReverseShootingAnim");
    }

    #endregion

} // class Shoot 
// namespace
