using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform yawPivot;
    [SerializeField] private Transform pitchPivot;
    [SerializeField] private Transform bulletSpawn;

    [Header("Proyectil")]
    [SerializeField] private GameObject currentBulletPrefab;

    [Header("Disparo")]
    [SerializeField] private float shootForce = 20f;

    
    public void AimAt(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - yawPivot.position;

        
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);

        if (flatDirection != Vector3.zero)
        {
            yawPivot.rotation = Quaternion.LookRotation(flatDirection);
        }

        
        float distance = flatDirection.magnitude;
        float height = direction.y;

        float angle = Mathf.Atan2(height, distance) * Mathf.Rad2Deg;

        pitchPivot.localRotation = Quaternion.Euler(-angle, 0f, 0f);
    }

    
    public void Fire(Vector3 targetPosition)
    {
        GameObject bulletObj = Instantiate(
            currentBulletPrefab,
            bulletSpawn.position,
            Quaternion.identity
        );

        
        IProjectile projectile = bulletObj.GetComponent<IProjectile>();

        if (projectile == null)
        {
            Debug.LogError("El prefab no tiene IProjectile");
            return;
        }

        
        Vector3 direction = targetPosition - bulletSpawn.position;

        float height = direction.y;
        direction.y = 0;

        float distance = direction.magnitude;

        float gravity = Physics.gravity.y;

        
        float angle = 45f * Mathf.Deg2Rad;

        float velocity = Mathf.Sqrt(distance * -gravity / Mathf.Sin(2 * angle));

        Vector3 velocityY = Vector3.up * velocity * Mathf.Sin(angle);
        Vector3 velocityX = direction.normalized * velocity * Mathf.Cos(angle);

        Vector3 finalDirection = (velocityX + velocityY).normalized;

        
        projectile.Shoot(finalDirection);
    }
}
