//---------------------------------------------------------
// Script encargado del manejo del movimiento del personaje jugable
// Alejandro Garcia
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Movement_Player : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] private float MaxVelocity = 5.0f;
    [SerializeField] private float Acceleration = 1.0f;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _velocity = .0f;

    SpriteRenderer _spriteRenderer;
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
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        //  ---- Movimiento del Personaje ----

        // direccion de movimiento
        Vector2 dir = InputManager.Instance.MovementVector;

        // en caso de moverse a la derecha
        if (dir.x > 0)
        {
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_velocity, 0.0f) * Time.deltaTime;
            _spriteRenderer.flipX = false;

            // Acelera al personaje o fija su velocidad si ha llegado al maximo de velocidad
            _velocity += Acceleration;
            if (_velocity <= MaxVelocity)
            {
                _velocity += Acceleration;
            }
            else _velocity = MaxVelocity;

        }
        // en caso de moverse a la izquierda
        else if (dir.x < 0)
        {
            // mueve al personaje en base a la velocidad (_velocity) en el eje x
            this.transform.position += new Vector3(_velocity, 0.0f) * Time.deltaTime;
            _spriteRenderer.flipX = true;

            // Acelera al personaje o fija su velocidad si ha llegado al maximo de velocidad
            if (_velocity >= -MaxVelocity)
            {
                _velocity -= Acceleration;
            }
            else _velocity = -MaxVelocity;
        }
        else _velocity = .0f;
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

} // class Movement_Player 
// namespace
