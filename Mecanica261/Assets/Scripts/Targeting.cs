using UnityEngine;
using UnityEngine.InputSystem;

public class Targeting : MonoBehaviour
{
    [Header("References")] //Falta estandarización
    public GameObject targetPrefab; 
    public Collider floorCollider;
    public Camera gameCamera;

    [Header("Turret Setting")] //Falta estandarización
    public Transform turretBarrel; 
    public float rotationSpeed = 10f;

    private GameObject activeTarget;

    void Start()
    {
        if (targetPrefab != null)
        {
            activeTarget = Instantiate (targetPrefab);
        }
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = gameCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (floorCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            activeTarget.transform.position = hit.point + Vector3.up * 0.1f;

            AimAtTarget(hit.point);
        }
    }

    void AimAtTarget(Vector3 targetPosition)
    {
        if (turretBarrel != null)
        {
            Vector3 direction = targetPosition - turretBarrel.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            turretBarrel.rotation = Quaternion.Slerp(turretBarrel.rotation, targetRotation, rotationSpeed * Time.deltaTime); //no modifiques el valor de la torreta desde otro script, debes hablar con la torreta misma. 
        }
    }
}

//Use Slerp en vez de Lerp para la línea 48 para hacer el movimiento de mi torreta más fluido y no tan robótico, porque Slerp hace que el giro sea más constante, con Lerp en cmabio, mi torreta haría cambios más rapidos o se movería de forma mas lineal entonces no habría tenido tanta fluidez
