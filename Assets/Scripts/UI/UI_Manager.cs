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
    /// <summary>
    /// Imagen encargada de representar las balas restantes
    /// </summary>
    [SerializeField]
    private Image Recarga;
    /// <summary>
    /// Barra encargada de repesentar la puntuación del Juego
    /// </summary>
    [SerializeField]
    private Image Barra_puntuacion;
    /// <summary>
    /// Referencia al jugador 
    /// </summary>
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
    // Variable privada encargada de almacenar el numero de balas
    private int balas;
    //Variable privada encargada de almacenar la puntuacion en cada momento
    private int puntuacion;
    //Variable que almacena la puntuacion inicial de el nivel
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
    {   //Guardamos la puntuación inicial  
        puntuacionInicial = LevelManager.Instance.GetcurrentScore();
        //Actualizamos el GUI de la bala para poner el valor inical
        Update_Bala();
        //Actualizamos la barra de puntuación para establecer el valor inical
        Update_Puntuacion();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {  //Actualizamos el GUI de la bala
        Update_Bala();
        //Actualizamos la barra de puntuación
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
    /// <summary>
    /// Metodo encargado de actualizar la Gui de las balas cuando el jugador dispare
    /// </summary>
    private void Update_Bala()
    { 
        //miramos las balas que tiene el jugador para representarlas  
        balas = Player.GetComponent<Shoot>().GetAmmo();
        // Y reducimos el grafico si ha bajado
        Reduce_Grito(balas);
    }
    /// <summary>
    /// Metodo que actualiza el GUI de la puntuacion cuando esta crece o se ve reducida
    /// </summary>
    private void Update_Puntuacion()
    {   //Obtenemos el valor de la puntuación 
        puntuacion = LevelManager.Instance.GetcurrentScore();
        //Debug para saber en las pruebas cuanto vale
        //Debug.Log(puntuacion);
        //Actualizamos la barra con el metodo conviertePorcent
        Barra_puntuacion.fillAmount = ConviertePorcent(puntuacionInicial,puntuacion);
    }
    /// <summary>
    /// Metodo que reduce la GUI de balas a la cantidad correspondiente
    /// </summary>
    /// <param name="numBalas"> Numero de balas que tiene el jugador </param>
    private void Reduce_Grito(int numBalas)
    { 
    //Mira en cada caso la cantidad de balas que tenenmos en el momento y asi podedemos
    // representar las balas correspondientes
        switch (numBalas)
        {
            case 4: Recarga.fillAmount = 1f;break;
            case 3: Recarga.fillAmount = 0.65f; break;
            case 2: Recarga.fillAmount = 0.46f; break;
            case 1: Recarga.fillAmount = 0.28f; break;
            case 0: Recarga.fillAmount = 0f; break;
        }

    }
    /// <summary>
    /// Metodo encargado de convertir el numero de puntos a porcentaje para poder ser representado
    /// </summary>
    /// <param name="max">Valor maximo de puntacion posible</param>
    /// <param name="act">Valor correspondiente al numero de puntos</param>
    /// <returns></returns>
    private float ConviertePorcent(float max ,float act)
    {
        //Variable encargada del porcentaje representada entre 0 y 1 en float
        float Diferencia;
        //Guardamos el diferencia en  la variable 
        Diferencia = max - act;
        //Miramos si esta es 0 entonces esta llena
        if(Diferencia == 0)
        {
            //Representamos la barra completa
            Diferencia = 1f;
        }
        //Si es distinto de 0 entonces ya no esta llena y calculamos su porcentaje
        else
        {
            //Representamos el porcentaje correspondiente
            Diferencia = act / max;
        }
        //Devolvemos la variable Diferencia
        return Diferencia;
    }
    #endregion   

} // class Balas 
// namespace
