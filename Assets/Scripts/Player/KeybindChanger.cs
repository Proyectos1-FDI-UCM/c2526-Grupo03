//---------------------------------------------------------
// Componente que cambia el sprite de un GameObject entre teclas de teclado y botones de mando
// Gabriel Adrian Oltean
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

// Instrucciones de uso:
// Aplicar el componente al objeto de la keybind que queremos que cambie
// Meter en los serializedfields los sprites de la carpeta de sprites en assets

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class KeybindChanger : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Sprite de la tecla para reparar
    /// </summary>
    [SerializeField] private Sprite Tecla = null;

    /// <summary>
    /// Sprite del boton para reparar
    /// </summary>
    [SerializeField] private Sprite Boton = null;

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
    /// Indica si se ha cambiado la tecla de raparación
    /// y esta estaba activa
    /// </summary>
    private bool _keyChangedActive = false;

    /// <summary>
    /// Componente spriterenderer cacheado
    /// </summary>
    private SpriteRenderer _component;

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
        // Cacheamos el componente
        _component = GetComponent<SpriteRenderer>();
        if (Boton != null && Tecla != null)
        {
            // Iniciamos la primera tecla
            if (GameManager.Instance.GetMando())
            {
                _component.sprite = Boton;
            }
            else
            {
                _component.sprite = Tecla;
            }
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Detección de cambio de dispositivo
        if (Boton != null && Tecla != null && GameManager.Instance.DeviceChanged())
        {
            // Si es mando
            if (GameManager.Instance.GetMando())
            {
                // Si la tecla estaba activa
                if (this.gameObject.activeSelf)
                {
                    // Cambiamos el estado activo de la tecla anterior
                    this.gameObject.SetActive(!this.gameObject.activeSelf);
                    _keyChangedActive = true;
                }

                // Cambiamos el objeto de la tecla
                _component.sprite = Boton;

                // Si la tecla estaba activa y ha cambiado
                if (_keyChangedActive)
                {
                    // Volvemos a cambiar el estado activo de la tecla
                    this.gameObject.SetActive(!this.gameObject.activeSelf);
                    _keyChangedActive = false;
                }
            }
            // Si es teclado
            else
            {
                // Si la tecla estaba activa
                if (this.gameObject.activeSelf)
                {
                    // Cambiamos el estado activo de la tecla anterior
                    this.gameObject.SetActive(!this.gameObject.activeSelf);
                    _keyChangedActive = true;
                }

                // Cambiamos el objeto de la tecla
                _component.sprite = Tecla;

                // Si la tecla estaba activa y ha cambiado
                if (_keyChangedActive)
                {
                    // Volvemos a cambiar el estado activo de la tecla
                    this.gameObject.SetActive(!this.gameObject.activeSelf);
                    _keyChangedActive = false;
                }
            }
            GameManager.Instance.SetDeviceChanged(false);
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

    #endregion

} // class KeybindChanger 
// namespace
