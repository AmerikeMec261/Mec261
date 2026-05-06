using UnityEngine;

public class FloatBehaviour : MonoBehaviour
{
    [Header("Water Level")]
    [Tooltip("Pon la posición de Y del plano que estas utilizando como referencia del agua")]
    public float waterLevel = 0;

    [Header("Float points")]
    [Tooltip("A los costados del barco se deben poner puntos de flotación y deben agregarse en este array")]
    public Transform[] floatPoints;

    [Header("Buoyancy Force")]
    [Tooltip("Cuanta fuerza hacía arriba se aplica por metro de profundidad")]
    // Para obtener el valor aproximado utiliza la siguiente formula = (masa * 9.81) / profundidad que quieres / cantidad de float points
    public float buoyancyForce = 15000f;

    [Header("Water Damping")]
    [Tooltip("Fuerza de frena el movimiento en el agua. Los valores 0.99 o 0.995 es mejor")]
    [Range(0.9f, 1)]
    public float waterDamping = 0.99f;

    [Tooltip("Frena la rotación dentro del agua para que no gire infinitamente")]
    [Range(0.9f, 1)]
    public float angularDamping = 0.97f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        Bouyancy();
        WaterDrag();
    }

    private void Bouyancy()
    {
        foreach (Transform point in floatPoints)
        {
            float submersionDepth = waterLevel - point.position.y;

            if (submersionDepth > 0)
            {
                float force = buoyancyForce * submersionDepth / floatPoints.Length;
                rb.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);
            }
        }
    }

    private void WaterDrag()
    {
        if (transform.position.y < waterLevel)
        {
            rb.linearVelocity *= waterDamping;
            rb.angularVelocity *= angularDamping;
        }
    }
}
