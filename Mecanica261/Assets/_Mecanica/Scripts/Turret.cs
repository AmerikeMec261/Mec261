using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret References")]
    [Tooltip("Parte de la torreta que va a rotar en X")]
    public Transform barrel;

    [Tooltip("Transform de un Empty GameObject para indicar donde se va a disparar el proyectil")]
    public Transform firePoint;

    [Header("Target")]
    [Tooltip("Transform del objetivo o el enemigo")]
    public Transform target;

    [Header("Rotation")]
    [Tooltip("Velocidad horizontal")]
    public float rotationSpeed = 60f;

    [Tooltip("Velocidad de elevacion del cañon")]
    public float elevationSpeed = 40f;

    [Header("Shoot")]
    [Tooltip("Rotacion hacia abajo es con un valor negativo")]
    public float minElevationAngle = -5f;

    [Tooltip("Angulo maximo de elevacion")]
    public float maxElevationAngle = 45f;

    [Header("Shoot")]
    [Tooltip("Prefab del proyectil")]
    public GameObject projectilePrefab;

    [Tooltip("Angulo maximo de error permitido antes de disparar y no dispara hasta estar bien apuntado.")]
    public float aimTolerance = 5f;

    // Angulo de elevacion actual del cañon
    private float currentElevation = 0f;

    void Update()
    {
        RotateBase();
        RotateBarrel();

        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    private void RotateBase()
    {
        // Calculo de Vector desde la torreta al objetivo
        Vector3 directionToTarget = target.position - transform.position;

        // Se elimina el componente y para que solo rote horizontalmente
        directionToTarget.y = 0f;

        // Se crea la rotacion para que mire hacia el objetivo 
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // El Quaternion.RotateTowards respeta el limite que le establezcamos en la rotacion
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void RotateBarrel()
    {
        // Obtenemos la direccion en el espacio local del barrel
        Vector3 localDirection = barrel.InverseTransformPoint(target.position);

        // Usamos Atan2 para calcular el angulo vertical. Y es la diferencia vertical y Z es la distancia horizontal o profundidad
        float desireElevation = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;

        // Limitamos la rotacion con el maximo y minimo que se establecieron
        desireElevation = Mathf.Clamp(desireElevation, minElevationAngle, maxElevationAngle);

        // Rotamos suavemente al angulo deseado
        currentElevation = Mathf.MoveTowards(currentElevation, desireElevation, elevationSpeed * Time.deltaTime);

        // Aplicamos la rotación solo en el eje X local del barrel
        barrel.localRotation = Quaternion.Euler(-currentElevation, 0f, 0f);

        // Verificamos si el disparo es posible con el angulo actual de la torreta
        Vector3 flatToTarget = target.position - transform.position;
        flatToTarget.y = 0f;
        float horizontalAngle = Vector3.Angle(transform.forward, flatToTarget);

        float verticalAngle = Mathf.Abs(desireElevation - currentElevation);
    }

    private void Fire()
    {
        // Se instancia el prefab de la bala en la posicion de su punto de aparicion en la boca del cañon respetando la posicion y rotacion del cañon
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
