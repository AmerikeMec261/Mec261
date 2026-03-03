using UnityEngine;

public class Trayectoria : MonoBehaviour
{
    public Lanzamiento lanzamiento;
    public Transform balon;
    public int puntos = 25;
    public float pasoTiempo = 0.1f;
    private LineRenderer linea;

    void Start()
    {
        linea = GetComponent<LineRenderer>();
        linea.positionCount = 0;
    }

    void Update()
    {
        if (lanzamiento == null || balon == null) return;

        if (lanzamiento.EstaArrastrando())
            DibujarLinea();
        else
            linea.positionCount = 0;
    }

    void DibujarLinea()
    {
        Vector3 inicio = balon.position;

        Vector3 vectorArrastre = lanzamiento.ObtenerInicio() - lanzamiento.ObtenerActual();

        if (vectorArrastre.magnitude > lanzamiento.limiteArrastre)
            vectorArrastre = vectorArrastre.normalized * lanzamiento.limiteArrastre;

        float intensidad = vectorArrastre.magnitude;

        Vector3 dirH = new Vector3(vectorArrastre.x, 0f, vectorArrastre.z);
        if (dirH.magnitude > 0.001f)
            dirH = dirH.normalized;

        float velH = (intensidad * lanzamiento.fuerzaHorizontal) + lanzamiento.minimoHorizontal;
        float velV = (intensidad * lanzamiento.fuerzaVertical) + lanzamiento.minimoVertical;

        Vector3 velocidadInicial = dirH * velH;
        velocidadInicial.y = velV;

        Vector3 g = Physics.gravity;

        linea.positionCount = puntos;

        for (int i = 0; i < puntos; i++)
        {
            float t = i * pasoTiempo;
            Vector3 pos = inicio + velocidadInicial * t + 0.5f * g * t * t;
            linea.SetPosition(i, pos);
        }
    }
}