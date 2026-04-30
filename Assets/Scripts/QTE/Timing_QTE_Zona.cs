//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Timing_QTE_Zona : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Variable que guarda el rect transform de la rayita.
    /// </summary>
    [SerializeField] private RectTransform Objetivo;
    /// <summary>
    /// Límite derecho de la zona.
    /// </summary>
    [SerializeField] private RectTransform MaxPos;
    /// <summary>
    /// Límite izquierdo de la zona.
    /// </summary>
    [SerializeField] private RectTransform MinPos;
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
    /// Booleano para comprobar cuando la rayita está en la zona
    /// </summary>
    private bool _acierto = false;
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
    /// Método público que devuelve true si la rayita está dentro de la zona y false en el caso contrario
    /// </summary>
    public bool CheckAcierto()
    {
        //Chequeamos si esta entre la posicion 
        if (Objetivo.anchoredPosition.x > MinPos.anchoredPosition.x && Objetivo.anchoredPosition.x < MaxPos.anchoredPosition.x)
        {
            _acierto = true;
        }
        else
        {
            _acierto = false;
        }
        //Devolvemos si hemos acertado
        return _acierto;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        Timing_QTE_Rayita colision = collision.GetComponent<Timing_QTE_Rayita>();
        Timing_QTE acierto = GetComponent <Timing_QTE>();
        if (colision)
        {
            Debug.Log("hola");
            acierto.Acierto(true);
        }
        else
        {
            acierto.Acierto(false);
        }
    }
    */
    
    #endregion   

} // class Timing_QTE_Zona 
// namespace
