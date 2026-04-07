using UnityEngine;
using UnityEngine.InputSystem;

public class Targeting : MonoBehaviour
{
    [Header("References")]
    public GameObject targetPrefab;
    public Collider floorCollider;
    public Camera gameCamera;

    [Header("Turret Setting")]
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

            turretBarrel.rotation = Quaternion.Slerp(turretBarrel.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
