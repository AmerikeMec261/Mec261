using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClusterMissile : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 10f;

    [Header("Explosion")]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private LayerMask _damageLayers;

    [Header("Submunition")]
    [SerializeField] private GameObject _subMunition;
    [SerializeField] private int _subMunitionCount = 5;
    [SerializeField] private float _spawnRadius = 1.5f;
    [SerializeField] private float _clusterHeight = 5f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _hasClustered;

    public float Speed => _speed;
    public float Damage => _damage;

    private void Update()
    {
        Vector3 velocity = GetComponent<Rigidbody>().linearVelocity;

        if (velocity.sqrMagnitude > 0.001f) { transform.rotation = Quaternion.LookRotation(velocity); }
        if (_hasClustered) { return; }
        if (velocity.y >= 0f) { return; }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, _groundLayer))
        {
            if (hitInfo.distance <= _clusterHeight) { Cluster(); }
        }
    }

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }

    private void Cluster()
    {
        _hasClustered = true;

        Vector3 center = transform.position;
        Vector3 moveDirection = GetComponent<Rigidbody>().linearVelocity.normalized;

        if (moveDirection.sqrMagnitude <= 0.001f) { moveDirection = transform.forward; }

        Vector3 ringAxis = Vector3.Cross(moveDirection, Vector3.up);
        if (ringAxis.sqrMagnitude <= 0.001f) { ringAxis = Vector3.Cross(moveDirection, Vector3.right); }

        ringAxis.Normalize();

        for (int i = 0; i < _subMunitionCount; i++)
        {
            float angle = (360f / _subMunitionCount) * i;
            Quaternion ringRotation = Quaternion.AngleAxis(angle, moveDirection);
            Vector3 offsetDirection = ringRotation * ringAxis;
            Vector3 spawnPosition = center + offsetDirection * _spawnRadius;
            Vector3 fireDirection = (moveDirection + offsetDirection).normalized;

            GameObject subMunitionInstance = Instantiate(_subMunition, spawnPosition, Quaternion.LookRotation(fireDirection));
            subMunitionInstance.GetComponent<IProjectile>()?.Fire();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DoDamage();
        Destroy(gameObject);
    }

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