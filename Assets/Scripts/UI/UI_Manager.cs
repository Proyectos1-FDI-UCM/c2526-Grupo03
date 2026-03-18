//---------------------------------------------------------
// Se encargara de renderizar los datos que son necesarios para el bucle de juego
// Tristan Sanchez Lopez
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
public class Balas : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField]
    private Image Recarga;
    [SerializeField]
    private Image Barra_puntuacion;
    [SerializeField] private GameObject Player;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private int balas;
    private int puntuacion;
    private int puntuacionInicial;
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
        puntuacionInicial = GameManager.Instance.GetcurrentScore();
        Update_Bala();
        Update_Puntuacion();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Update_Bala();
        Update_Puntuacion();
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
    private void Update_Bala()
    { //miramos las balas que tiene el jugador para representarlas  
        balas = Player.GetComponent<Movement_Player>().getAmmo();
        // Y reducimos el grafico si ha bajado
        Reduce_Grito(balas);
    }
    private void Update_Puntuacion()
    {   
        puntuacion = GameManager.Instance.GetcurrentScore();
        Debug.Log(puntuacion);
        Barra_puntuacion.fillAmount = ConviertePorcent(puntuacionInicial,puntuacion);
    }
     private void Reduce_Grito(int bala)
    { //Mira en cada caso la cantidad de balas que tenenmos en el momento y asi podedemos
      // representar las balas correspondientes
        switch (bala)
        {
            case 4: Recarga.fillAmount = 1f;break;
            case 3: Recarga.fillAmount = 0.65f; break;
            case 2: Recarga.fillAmount = 0.46f; break;
            case 1: Recarga.fillAmount = 0.28f; break;
            case 0: Recarga.fillAmount = 0f; break;
        }

    }
    private float ConviertePorcent(float max ,float min)
    {
        float Porcentaje;
        Porcentaje = max - min;
        if(Porcentaje == 0)
        {
            Porcentaje = 1f;
        }
        else
        {
            Porcentaje = min / max;
        }
        return Porcentaje;
    }
    #endregion   

} // class Balas 
// namespace
