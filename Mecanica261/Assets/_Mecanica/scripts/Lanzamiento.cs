using System.Collections;
using UnityEngine;

public class Lanzamiento : MonoBehaviour
{
    public float fuerzaHorizontal = 1.0f;
    public float fuerzaVertical = 9.0f;
    public float limiteArrastre = 4.0f;

    public float minimoHorizontal = 0.2f;
    public float minimoVertical = 3.0f;

    public Transform manos;
    public float tiempoReaparicion = 3.0f;

    private Rigidbody rb;

    private bool arrastrando = false;
    private bool enManos = true;

    private Vector3 inicioArrastre;
    private Vector3 actualArrastre;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PonerEnManos();
    }

    void Update()
    {
        if (enManos && manos != null)
            transform.position = manos.position;

        if (enManos && Input.GetMouseButtonDown(0))
        {
            arrastrando = true;
            inicioArrastre = ObtenerPuntoMouse();
            actualArrastre = inicioArrastre;
        }

        if (enManos && Input.GetMouseButton(0) && arrastrando)
        {
            actualArrastre = ObtenerPuntoMouse();
        }

        if (enManos && Input.GetMouseButtonUp(0) && arrastrando)
        {
            arrastrando = false;
            Lanzar();
        }
    }

    void Lanzar()
    {
        enManos = false;

        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 vectorArrastre = inicioArrastre - actualArrastre;

        if (vectorArrastre.magnitude > limiteArrastre)
            vectorArrastre = vectorArrastre.normalized * limiteArrastre;

        float intensidad = vectorArrastre.magnitude;

        Vector3 dirH = new Vector3(vectorArrastre.x, 0f, vectorArrastre.z);
        if (dirH.magnitude > 0.001f)
            dirH = dirH.normalized;

        float velH = (intensidad * fuerzaHorizontal) + minimoHorizontal;
        float velV = (intensidad * fuerzaVertical) + minimoVertical;

        Vector3 velocidad = dirH * velH;
        velocidad.y = velV;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = velocidad;

        StartCoroutine(Reaparecer());
    }

    IEnumerator Reaparecer()
    {
        yield return new WaitForSeconds(tiempoReaparicion);
        PonerEnManos();
    }

    void PonerEnManos()
    {
        enManos = true;
        arrastrando = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;
        rb.useGravity = false;

        if (manos != null)
            transform.position = manos.position;
    }

    Vector3 ObtenerPuntoMouse()
    {
        Plane plano = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distancia;
        if (plano.Raycast(rayo, out distancia))
            return rayo.GetPoint(distancia);

        return transform.position;
    }

    public bool EstaArrastrando() { return arrastrando; }
    public Vector3 ObtenerInicio() { return inicioArrastre; }
    public Vector3 ObtenerActual() { return actualArrastre; }
}