using UnityEngine;

public class Tower : MonoBehaviour //El codigo no está estandarizado.
{
    public float range = 5f;
    public float firerate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer < 1f / firerate) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float nearestDist = float.MaxValue;

        foreach (GameObject e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position); //No uses abreviaciones
            if (d < nearestDist && d <= range)
            {
                nearest = e;
                nearestDist = d;
            }
        }

        if (nearest == null) return;

        if (projectilePrefab == null)
        {
            Debug.LogError("Tower: projectilePrefab no asignado.");
            return;
        }

        GameObject p = Instantiate(projectilePrefab,firePoint.position,Quaternion.identity);
        Projectile proj = p.GetComponent<Projectile>();
        if(proj==null)
        {
            Debug.LogError("Tower: El prefab del proyectil no tiene componente Projectile");
            return;
        }

        proj.target = nearest;
        fireTimer = 0f;
    }
}
