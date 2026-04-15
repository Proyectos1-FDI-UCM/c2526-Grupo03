//---------------------------------------------------------
// Script encargado de activar y desactivar/activar todos los scripts del mismo elemento 
// Tristan Sanchez Lopez
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

//----------------------------------------------------------
//                   ¿ COMO SE USA ?
//Crear una entidad,hacer hijos a todo lo que quieras pausar.
//Añadir a la entidad el script
//Añadir a todos los QTE's el script y activar la opción de QTE
//----------------------------------------------------------

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
    [SerializeField] private bool QTE = false;
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
    /// Array de MonoBehaviour que contiene todos los scrips del objeto
    /// </summary>
    private MonoBehaviour[] Scripts;
    /// <summary>
    /// Array de MonoBehaviour que contiene todos los scripts del objeto que estan activados
    /// </summary>
    private MonoBehaviour[] ScriptsActivados;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    void Start()
    {
        //Inicializamos el array de ScriptsActivados
        InicializarArrayActivos();
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
    /// Metodo que desactiva todos los componentes del objeto que lo contiene 
    /// </summary>
    public void Pause()
    {
        //Revisamos si es un QTE para volver a inicializar el array de ScriptsActivados
        if (QTE)
        {
            //Inicializamos el array de ScriptsActivados
            InicializarArrayActivos();
        }
        //Desactivamos uno a uno cada script que estaban activados
        foreach (MonoBehaviour s in ScriptsActivados)
        {
            //Miramos que no sea null
            if (s != null)
            {
                //Lo desactivamos
                s.enabled = false;
                //Debug.Log("activando");
            }
        }
    }
    /// <summary>
    /// Metodo que activa todos los componentes del objeto que lo contiene
    /// </summary>
    public void UnPause()
    {
        //Activamos uno a uno cada script pero solo los que estaban desactivados antes
        foreach (MonoBehaviour s in ScriptsActivados)
        {
            ////Miramos que no sea null
            if (s != null)
            {
                //Lo activamos
                s.enabled = true;
            }
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Metodo encargado de inicializar el array de scripts activados 
    /// </summary>
    private void InicializarArrayActivos()
    {
        //variable local del start que va a llevar la cuenta del numero scripts que estan activados 
        int i = 0;
        //Hacemos un array de MonoBehaviour todos los componentes que estan en el objeto o son hijos de este
        Scripts = GetComponentsInChildren<MonoBehaviour>(true);
        //Recorremos el array por cada uno de los scripts
        foreach (MonoBehaviour s in Scripts)
        {
            //Miramos si estan activados
            if (s.enabled == true)
            {
                //Si lo estan sumamos uno al contador
                //Debug.Log("Un componete estaba desactivado");
                i++;

            }
        }
        //Declaramos el array de scripts activados con el numero correspondiente
        ScriptsActivados = new MonoBehaviour[i];
        //hacemos una variable auxiliar para recorrer el array
        int j = 0;
        //Recorremos el array por cada uno de los scripts
        foreach (MonoBehaviour s in Scripts)
        {
            //Miramos si estan activados
            if (s.enabled == true)
            {
                //Lo guardamos en el array
                ScriptsActivados[j] = s;
                //Incrementamos la variable para que no se sobre escriba
                j++;
            }
        }
    }
    #endregion

} // class Activate_Deactivate 
// namespace
