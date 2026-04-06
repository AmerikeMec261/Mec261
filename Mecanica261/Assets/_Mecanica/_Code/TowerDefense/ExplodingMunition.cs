using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplodingMunition : MonoBehaviour, IProjectile
{
    [Header("Dependencies")]
    [SerializeField] private GameObject _explosionVFXPrefab;
    [SerializeField] private Vector3 _explosionScale = Vector3.one;

    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 10f;

    [Header("Explosion")]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private LayerMask _damageLayers;

    public float Speed => _speed;
    public float Damage => _damage;

    public void DoDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _damageLayers);

        for (int i = 0; i < colliders.Length; i++)
        {
            IDamagable damagable = colliders[i].GetComponent<IDamagable>();
            if (damagable == null) { continue; }

            Vector3 closestPoint = colliders[i].ClosestPoint(transform.position);
            float distanceToTarget = Vector3.Distance(transform.position, closestPoint);
            float normalizedDistance = Mathf.Clamp01(distanceToTarget / _explosionRadius);
            float finalDamage = _damage * (1f - normalizedDistance);

            if (finalDamage <= 0f) { continue; }

            damagable.TakeDamage(finalDamage);
        }

        DrawDebugSphere(transform.position, _explosionRadius, 5f);
    }

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SpawnExplosionVFX(collision);
        DoDamage();
        Destroy(gameObject);
    }

    private void SpawnExplosionVFX(Collision collision)
    {
        if (_explosionVFXPrefab == null) { return; }

        ContactPoint contactPoint = collision.GetContact(0);
        Quaternion effectRotation = Quaternion.LookRotation(contactPoint.normal);
        GameObject explosionVFXInstance = Instantiate(_explosionVFXPrefab, contactPoint.point, effectRotation);
        explosionVFXInstance.transform.localScale = _explosionScale;

        ParticleSystem particleSystem = explosionVFXInstance.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(explosionVFXInstance, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
            return;
        }

        Destroy(explosionVFXInstance, 5f);
    }

    private void DrawDebugSphere(Vector3 center, float radius, float duration)
    {
        int segments = 24;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = (i / (float)segments) * Mathf.PI * 2f;
            float angle2 = ((i + 1) / (float)segments) * Mathf.PI * 2f;

            Vector3 xy1 = center + new Vector3(Mathf.Cos(angle1), Mathf.Sin(angle1), 0f) * radius;
            Vector3 xy2 = center + new Vector3(Mathf.Cos(angle2), Mathf.Sin(angle2), 0f) * radius;

            Vector3 xz1 = center + new Vector3(Mathf.Cos(angle1), 0f, Mathf.Sin(angle1)) * radius;
            Vector3 xz2 = center + new Vector3(Mathf.Cos(angle2), 0f, Mathf.Sin(angle2)) * radius;

            Vector3 yz1 = center + new Vector3(0f, Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius;
            Vector3 yz2 = center + new Vector3(0f, Mathf.Cos(angle2), Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(xy1, xy2, Color.red, duration);
            Debug.DrawLine(xz1, xz2, Color.red, duration);
            Debug.DrawLine(yz1, yz2, Color.red, duration);
        }
    }
}