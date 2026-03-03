using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControlJuego : MonoBehaviour
{
    public TMP_Text textoTiempo;
    public TMP_Text textoPuntos;
    public TMP_Text textoRecord;
    public TMP_Text textoFinal;
    public GameObject botonReiniciar;
    public Lanzamiento lanzamiento; 
    private float tiempo = 60f;
    private int puntos = 0;
    private int record = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        tiempo = 60f;
        puntos = 0;
        juegoTerminado = false;

        record = PlayerPrefs.GetInt("Record", 0);

        if (textoFinal != null) textoFinal.text = "";

        if (botonReiniciar != null)
            botonReiniciar.SetActive(false);

        if (lanzamiento != null)
            lanzamiento.enabled = true;

        ActualizarUI();
    }

    void Update()
    {
        if (juegoTerminado) return;

        tiempo -= Time.deltaTime;

        if (tiempo <= 0f)
        {
            tiempo = 0f;
            TerminarJuego();
        }

        ActualizarUI();
    }

    public void SumarPunto()
    {
        if (juegoTerminado) return;

        puntos++;
        ActualizarUI();
    }

    void TerminarJuego()
    {
        juegoTerminado = true;

        if (lanzamiento != null)
            lanzamiento.enabled = false;

        if (puntos > record)
        {
            record = puntos;
            PlayerPrefs.SetInt("Record", record);
            PlayerPrefs.Save();

            if (textoFinal != null)
                textoFinal.text = "¡Nuevo récord! Score: " + puntos;
        }
        else
        {
            if (textoFinal != null)
                textoFinal.text = "Score: " + puntos + " | Récord: " + record;
        }

        if (botonReiniciar != null)
            botonReiniciar.SetActive(true);
    }

    void ActualizarUI()
    {
        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(tiempo);

        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntos;

        if (textoRecord != null)
            textoRecord.text = "Récord: " + record;
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}