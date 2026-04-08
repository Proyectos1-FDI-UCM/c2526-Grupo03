//---------------------------------------------------------
// Encargado del disparo del jugador y el pool de balas
// Alejandro Garcia y Víctor Román y Gabriel Adrian
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
    [SerializeField] private GameObject Bullet;
    [SerializeField] private int NumBulletsPool = 7;
    [SerializeField] private Transform BulletsContainer;
    [SerializeField] private int MaxAmmo = 4;

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
    /// Controla el tiempo que debe pasar entre disparos.
    /// </summary>
    private float _cooldown = .8f; // Tiempo de espera entre disparo y disparo

    /// <summary>
    /// Tiempo que ha pasado entre disparos.
    /// </summary>
    private float _timeToWait = .0f; // Tiempo en el que se podra volver a disparar

    /// <summary>
    ///  Posicion de la bala en relacion al personaje.
    /// </summary>
    private Vector3 _bulletPositionOffset = new Vector3(0.8f, 0); // Posicion de la bala en relacion al personaje

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
        }
        if (InputManager.Instance.FireWasPressedThisFrame() && !_desactivated && Time.time >= _timeToWait && _ammo > 0)
        {
            // -- Comprueba si hay balas usables --
            int i = 0;
            while (i < NumBulletsPool && _bulletsPool[i].activeSelf)
            {
                i++;
                Debug.Log("try " + i);
            }
            if (i == NumBulletsPool) return; // No se puede disparar porque todas las balas estan en uso

            // -- Si hay balas usables --
            if(_animator)
            {
                _animator.SetBool("IsShootingAnim", true);
            }
            int dir = _playerSpriteRenderer.flipX ? -1 : 1; // Para saber en que direccion mira el personjae principal

            Vector3 bulletOffset = gameObject.transform.position + (_bulletPositionOffset * dir);

            _bulletsPool[i].transform.position = bulletOffset; // Coloca la bala enfrente del personaje
            _bulletsPool[i].GetComponent<SpriteRenderer>().flipX = _playerSpriteRenderer.flipX; // Rota la bala en la direccion del player
            _bulletsPool[i].SetActive(true);

            Debug.Log("balas restantes: " + _ammo);
            _ammo--;

            _timeToWait = Time.time + _cooldown;
            if(_animator)
            {
                _animator.SetBool("ReverseShooting", true);
            }
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
