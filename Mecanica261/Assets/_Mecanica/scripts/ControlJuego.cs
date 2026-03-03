using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControlJuego : MonoBehaviour
{
   
    public TMP_Text textoPuntos;
    private int puntos = 0;
    

    void Start()
    {
      
        puntos = 0;
        ActualizarUI();
    }


    public void SumarPunto()
    {

        puntos++;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        
        if (textoPuntos != null)
            textoPuntos.text = "Puntos: " + puntos;
    }

   
}