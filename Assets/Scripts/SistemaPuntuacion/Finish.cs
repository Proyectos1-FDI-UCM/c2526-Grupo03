//---------------------------------------------------------
// Script responsable de la presentacion de la puntuacion final del nivel con estrellas
// Tristan Sanchez Lopez && Colaboradores:
//      Gabriel Adrian Oltean
// Rodaje Rodante
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Finish : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints 
    /// <summary>
    /// Bola del dessierto 
    /// </summary>
    [SerializeField] private Dessert_Ball_Movement DesertBall;
    /// <summary>
    /// Veloccidad a la que viaja la camara cuando el jugador llegue a la meta 
    /// </summary>
    [SerializeField] private float SpeedBost = 1f;
    /// <summary>
    /// Objeto del player 
    /// </summary>
    [SerializeField] private GameObject Player;
    [SerializeField] private float PorcentajeVictoria = 0f;
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
    /// puntuaccion inicial del nivel
    /// </summary>
    private float _puntuacionInicial;
    /// <summary>
    /// Puntuacion actual del nivel
    /// </summary>
    private float _puntiacionActual;
    /// <summary>
    /// Dice si el jugador esta pausado o no
    /// </summary>
    private bool _jugadorPausado;
    /// <summary>
    /// Script del movement player
    /// </summary>
    private Movement_Player _movimientoJugador;
    /// <summary>
    /// Script del salto del jugador
    /// </summary>
    private Jump _saltoJugador;
    /// <summary>
    /// Script de la recarga del jugador
    /// </summary>
    private Scream_Reload _recargaJugador;
    /// <summary>
    /// Script del disparo del jugador
    /// </summary>
    private Shoot _disparoJugador;

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
        //Guardamos la puntuacion inicial
        _puntuacionInicial = LevelManager.Instance.GetcurrentScore();
        //hacemos el GetComponent de todos los scripts del jugador
        _movimientoJugador = Player.GetComponent<Movement_Player>();
        _saltoJugador = Player.GetComponent<Jump>();
        _disparoJugador = Player.GetComponent<Shoot>();
        _recargaJugador = Player.GetComponent<Scream_Reload>();
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
    /// Metodo publico que se puede usar para ver si el jugador ya ha llegado a la meta
    /// </summary>
    /// <returns>Devuelve true si has ganado y false si no </returns>
    public bool HasWin()
    {
        // Devuelve la variable que indica si ha llegado a la meta
        return _jugadorPausado;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Deteccion de la collision 
    /// </summary>
    /// <param name="collision">Collider del objeto collisionado</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement_Player>() != null)
        {
            //Pausar el movimiento del jugador
            Player = collision.gameObject;
            //Pausamos el jugador
            PausePlayer();
            //Hacer que ignore la deteccion del jugador
            _jugadorPausado = Player_ignore();
            //Accelerar el movimiento de la camara
            Accelera_Dolly();


        }
        //Si ya se ha realizado la collision del jugador buscamos que nos choque la camara también
        if (_jugadorPausado && collision.gameObject.GetComponent<Dolly_Movement>() != null)
        {
            // Guardamos el porcentaje final
            Stars_FillAmount();

        }
    }
    /// <summary>
    /// Metodo que pausa los componentes del jugador para que no se pueda mover 
    /// </summary>
    private void PausePlayer()
    {
        //desactivamos cada componente
        _disparoJugador.DesactivateShoot();
        _saltoJugador.DesactivateJump();
        _recargaJugador.DesactivateReload();
        _movimientoJugador.DesactivateMovement();
    }
    /// <summary>
    /// Metodo que modifica la veloccidad de la camara 
    /// </summary>
    private void Accelera_Dolly()
    {
        //cambiamos la velocidad de la bola del desierto para que viaje mas rapido y no aburrir al usuario
        DesertBall.SetSpeed(SpeedBost);
    }
    /// <summary>
    /// Metodo que hace que el jugador no sea detectable con la camara y por tanto al llegar a la meta no muera cuandp pase la camara
    /// </summary>
    /// <returns></returns>
    private bool Player_ignore()
    {
        bool jugador_pausado = true;
        //_player.gameObject.GetComponent<DetectableObject>().enabled = false;
        return jugador_pausado;
    }
    /// <summary>
    /// Metodo que establece el porcentaje de las estrellas que has conseguido
    /// </summary>
    private void Stars_FillAmount()
    {
        //Conseguimos la puntuacion actual
        _puntiacionActual = LevelManager.Instance.GetcurrentScore();
        //Establecemos el numero de estrellas que vas a tener 
        float fillAmount = Conviertefloat(_puntuacionInicial, _puntiacionActual);
        //Ponemos el rating final de las estrellas
        GameManager.Instance.SetFinalRating(fillAmount);
        //Cambiamos el botton para podernos mover por el menu de victoria
        LevelManager.Instance.ChangeButtonToVictory();
        Victoria(fillAmount);
    }
    /// <summary>
    /// Convierte a diferencia dos numeros 
    /// </summary>
    /// <param name="inicial">Valor inicial del nivel</param>
    /// <param name="final">Valor actual del nivel</param>
    private float Conviertefloat(float inicial, float final)
    {
        //Variable encargada del porcentaje representada entre 0 y 1 en float
        float Diferencia;
        //Representamos la diferencia en el flotante correspondiente
        Diferencia = final / inicial;
        //Devolvemos la variable porcentaje
        return Diferencia;
    }
    /// <summary>
    /// Metodo que se encarga de saber si has ganado o no la partida 
    /// </summary>
    /// <param name="puntuacion">puntuacion del jugador cuando acaba el nivel</param>
    /// <returns></returns>
    private bool Victoria(float puntuacion)
    {
        bool victoria = false;
        if (puntuacion >= PorcentajeVictoria)
        {
            victoria = true;   
        }
        return victoria;
    }

    #endregion

} // class Finish 
  // namespace