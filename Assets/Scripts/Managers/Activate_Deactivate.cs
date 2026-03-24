//---------------------------------------------------------
// Script encargado de activar y desactivar/activar todos los scripts del mismo elemento 
// Tristan Sanchez Lopez
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Activate_Deactivate : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
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
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    /// <summary>
    /// Metodo que desactiva todos los componentes del objeto que lo contiene 
    /// </summary>
    public void Pause()
    {
        //Hacemos un array de MonoBehaviour todos los componentes que estan en el objeto o son hijos de este
        MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>(true);
        //Desactivamos uno a uno cada script
        foreach (MonoBehaviour s in scripts)
        {
            s.enabled = false;
        }
    }
    /// <summary>
    /// Metodo que activa todos los componentes del objeto que lo contiene
    /// </summary>
    public void UnPause()
    {
        //Hacemos un array de MonoBehaviour todos los componentes que estan en el objeto o son hijos de este
        MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>(true);
        //Activamos uno a uno cada script
        foreach (MonoBehaviour s in scripts)
        {
                s.enabled = true;
        }
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Activate_Deactivate 
// namespace
