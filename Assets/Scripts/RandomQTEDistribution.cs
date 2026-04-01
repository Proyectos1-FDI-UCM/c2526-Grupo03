//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Víctor Román Román
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class RandomQTEDistribution : MonoBehaviour
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

    /// <summary>
    /// Número que dirá quñe QTE cargar.
    /// </summary>
    private int _selectedQTE = 0;

    /// <summary>
    /// Número de cactus que hay en el nivel.
    /// </summary>
    GameObject[] cactusReparables;

    /// <summary>
    /// Número de grafitis que hay en el nivel.
    /// </summary>
    GameObject[] grafitisReparables;

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
        SearchRepairables();
        RandomRepairable();
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

    /// <summary>
    /// Elige un número aleatorio que repartirá los QTE entre reparables.
    /// </summary>
    private void RandomRepairable()
    {
        int n1 = Random.Range(0, 3);
        for (int i = 0; i < cactusReparables.Length - 1; i++)
        {
            cactusReparables[i].GetComponent<Repair>().ValorQTE(n1);
        }
        int n2 = Random.Range(0, 3);
        while(n2 == n1)
        {
            n2 = Random.Range(0, 3);
        }
        for (int i = 0;i < grafitisReparables.Length - 1; i++)
        {
            grafitisReparables[i].GetComponent<Repair>().ValorQTE(n2);
        }
    }

    private void SearchRepairables()
    {
        cactusReparables = GameObject.FindGameObjectsWithTag("Cactus");
        grafitisReparables = GameObject.FindGameObjectsWithTag("Grafiti");
    }

    #endregion   

} // class RandomQTEDistribution 
// namespace
