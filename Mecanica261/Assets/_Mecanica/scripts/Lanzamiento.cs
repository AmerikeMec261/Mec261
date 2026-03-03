using UnityEngine;

public class Lanzamiento : MonoBehaviour
{
    public Transform manos;
    public float fuerzaHorizontal = 2f;
    public float fuerzaVertical = 6f;
    public float tiempoReaparicion = 2f;
    private Rigidbody rb;
    private bool enManos = true;
    private bool arrastrando = false;
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
            inicioArrastre = PuntoEnSuelo();
            actualArrastre = inicioArrastre;
        }

       
        if (enManos && arrastrando && Input.GetMouseButton(0))
        {
            actualArrastre = PuntoEnSuelo();
        }

       
        if (enManos && arrastrando && Input.GetMouseButtonUp(0))
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

      
        Vector3 arrastre = inicioArrastre - actualArrastre;

    
        float velX = arrastre.x * fuerzaHorizontal;
        float velZ = arrastre.z * fuerzaHorizontal;
        float velY = Mathf.Abs(arrastre.z) * fuerzaVertical; 

        Vector3 velocidad = new Vector3(velX, velY, velZ);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = velocidad;

        Invoke("PonerEnManos", tiempoReaparicion);
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


    Vector3 PuntoEnSuelo()
    {
        Plane plano = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distancia;
        if (plano.Raycast(rayo, out distancia))
            return rayo.GetPoint(distancia);

        return transform.position;
    }

    
    public bool EstaArrastrando()
    {
        return arrastrando && enManos;
    }

    public Vector3 ObtenerInicio()
    {
        return inicioArrastre;
    }

    public Vector3 ObtenerActual()
    {
        return actualArrastre;
    }
}