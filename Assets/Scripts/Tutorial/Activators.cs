//---------------------------------------------------------
// Script encargado de activar ciertos componentes del jugador de manera progresiva
// Tristan Sanchez
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
public class Activators : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Booleano que indica si queremos activar el salto
    /// </summary>
    [SerializeField] private bool salto = false;
    /// <summary>
    /// Booleano que indica si queremos activar el disparo
    /// </summary>
    [SerializeField] private bool disparo = false;
    /// <summary>
    /// Booleano que indica si queremos activar el GUI
    /// </summary>
    [SerializeField] private bool GUI = false;
    /// <summary>
    /// Booleano que indica si queremos activar la recarga
    /// </summary>
    [SerializeField] private bool recarga = false;
    /// <summary>
    /// Booleano que indica si queremos activar a Dolly
    /// </summary>
    [SerializeField] private bool Dolly = false;
    /// <summary>
    /// Objeto del hud de la puntuacion
    /// </summary>
    [SerializeField] private GameObject Puntuacion = null;
    /// <summary>
    /// GameObject del hud de balas
    /// </summary>
    [SerializeField] private GameObject Balas = null;
   
    #endregion
    
    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

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
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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
        //Chequear que es el jugador 
        if(collision.gameObject.GetComponent<Movement_Player>()!= null)
        {

            //activamos el componente que se nos ha dicho en el editor
            Componente_Activar(collision.gameObject);

        }
    }
   /// <summary>
   /// Metodo encargado de activar los componentes desados
   /// </summary>
   /// <param name="Player"> GameObject de Player con el que activaremos las caracteristicas</param>
    private void Componente_Activar(GameObject Player)
    {
        //Si el salto es true lo activamos
        if(salto == true)
        {
            //Activamos componente
            Player.gameObject.GetComponent<Jump>().ActivateJump();
        }
        //Si el disparo es true lo activamos
        if (disparo == true)
        {
            //Activamos componente
            Player.gameObject.GetComponent<Shoot>().ActivateShoot();
            Balas.SetActive(true);
        }
        //Si la recarga es true lo activamos
        if (recarga == true)
        {
            //Activamos componente
            Player.gameObject.GetComponent<Scream_Reload>().ActivateReload();
        }
        if(Dolly == true)
        {
            Tutorial_Manager.Instance.Activa_Dolly();
        }
        //Si el GUI es true lo activamos
        if (GUI == true)
        {
            //Revisamos que no sea null el objeto GUI_objeto
            if (Puntuacion != null)
            {
                //Activamos componente
                Puntuacion.SetActive(true);
            }
            else
            {
                Debug.Log("GameObjet Puntuacion no encontrado");
            }
        }


    }
    #endregion   

} // class Activators 
// namespace
