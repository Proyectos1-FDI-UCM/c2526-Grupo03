//---------------------------------------------------------
// Un entero de vida que va restando cuando lo golpea la exclamación o grito
// Gabriel Adrian Oltean
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
public class Enemies_Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Cantidad de vida que le queda al enemigo
    /// </summary>
    [SerializeField] private int Health = 1;

    /// <summary>
    /// Cantidad de vida que le quita al enemigo cada golpe
    /// </summary>
    [SerializeField] private int DamagePerHit = 1;

    /// <summary>
    /// Objeto de barra de vida de la escena
    /// </summary>
    [SerializeField] private GameObject BarraVida;

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
    /// Componente de la barra de vida
    /// </summary>
    private Slider _componenteBarraVida;

    /// <summary>
    /// Variable de si está muerto o no
    /// </summary>
    private bool _dead;

    /// <summary>
    /// Componente renderer
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Componente Collider
    /// </summary>
    private BoxCollider2D _boxCollider;

    /// <summary>
    /// Componente Detectable
    /// </summary>
    private DetectableObject _detectableObject;



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
        // Si el objeto tiene el componente lo cacheamos y le ponemos las variables necesarias
        if (BarraVida.GetComponent<Slider>() != null)
        { 
            //Ponemos todos los componentes a las variables
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _boxCollider = this.GetComponent <BoxCollider2D>();
            _detectableObject = this.GetComponent<DetectableObject>();
            _componenteBarraVida = BarraVida.GetComponent<Slider>();
            //Establecemos el maximo, minimo y valor actual de la barra de vida
            _componenteBarraVida.maxValue = Health;
            _componenteBarraVida.minValue = 0;
            _componenteBarraVida.value = Health;
            // Desactivamos el pbjeto para que aparezca en el primer golpe
            BarraVida.SetActive(false);
        }
    }

    private void Update()
    {
        //Miramos si ha muerto y ya no hay sonidos activos
        if (_dead && !SoundManager.Instance.GetDmgPlaying())
        {
            //Desactivamos este objeto
            this.gameObject.SetActive(false);
            //Y lo destruimos
            Destroy(this.gameObject);
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
        // Solo baja vida si esta en el layer de exclamación
        if (collision.gameObject.GetComponent<Bullet_Exclamation>() != null)
        {
            //Bajamos la vida y ponemos el sonido
            Health -= DamagePerHit;
            SoundManager.Instance.PlaySFXDamage();
            
            // Si el objeto de barra de vida tiene el componente le modificamos el valor para mostrar la vida
            if (_componenteBarraVida != null)
            {
                // Si es el primer golpe activamos el objeto
                if (_componenteBarraVida.value == _componenteBarraVida.maxValue)
                {
                    BarraVida.SetActive(true);
                }
                // Restamos el valor del componente
                _componenteBarraVida.value -= DamagePerHit;
            }
            // Destruimos el enemigo si ya no le queda vida
            if (Health < 1)
            {
                _dead = true;
                BarraVida.SetActive(false);
                _detectableObject.enabled = false;
                _boxCollider.enabled = false;
                _spriteRenderer.enabled = false;
            }
            //Debug.Log($"Me ha golpeado {collision.gameObject.name} y me queda {Health} vida");
        }
    }
    #endregion

} // class Enemies_Health 
// namespace
