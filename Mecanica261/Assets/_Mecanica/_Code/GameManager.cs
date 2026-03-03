using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TiroParabolico tiro;
    public Transform PuntodeRotacion;
    public InputField angulo;
    public InputField Velocidad;
    public GameObject PaneldeReinicio;
    public GameObject GrupoLata1;
    public GameObject GrupoLata2;
    public GameObject GrupoLata3;
 

    public int MaximoTiros = 3;
    public float VelocidadDeRotacion = 50f;

    private int TirosGastados = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PaneldeReinicio.SetActive(false);
        angulo.text = "45";
        Velocidad.text = "20";

      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            tiro.transform.Rotate(Vector3.up * -VelocidadDeRotacion * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            tiro.transform.Rotate(Vector3.up * VelocidadDeRotacion * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && TirosGastados < MaximoTiros)
            Disparar();
    }
    void Disparar()
    {
        float ang = float.Parse(angulo.text);
        float vel = float.Parse(Velocidad.text);

        tiro.Parametros(vel, ang);
        tiro.Disparo();

        TirosGastados++;
        if(TirosGastados >= MaximoTiros )
            PaneldeReinicio?.SetActive(true);
    
    }

    public void ReinicioJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        TirosGastados = 0;
        tiro.Reinicio();
        PaneldeReinicio.SetActive(false);
        angulo.text = "45";
        Velocidad.text = "20";
    }
}
