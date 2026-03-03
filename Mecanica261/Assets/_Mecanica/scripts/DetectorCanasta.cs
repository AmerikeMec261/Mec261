using UnityEngine;

public class DetectorCanasta : MonoBehaviour
{
    public ControlJuego controlJuego;
    public bool esArriba = false; 

    private static bool pasoPorArriba = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Balon"))
            return;

        if (esArriba)
        {
            pasoPorArriba = true;
        }
        else
        {
            if (pasoPorArriba)
            {
                if (controlJuego != null)
                    controlJuego.SumarPunto();

                pasoPorArriba = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Balon"))
            return;

        if (!esArriba)
            pasoPorArriba = false;
    }
}