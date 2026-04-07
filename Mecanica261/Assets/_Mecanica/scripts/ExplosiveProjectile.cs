using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveProjectile : MonoBehaviour, IProjectile
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 30f;
    [Tooltip("Radio de la explosión")]
    [SerializeField] private float _explosionRadius = 3f;

    private Rigidbody _rigidbody;
    private bool _hasExploded = false;

    public float Speed => _speed;

    #endregion Variables

    #region Unity Methods

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasExploded)
        {
            return;
        }

        _hasExploded = true;

        Explode();
    }

    #endregion Unity Methods

    #region Methods

    public void Fire()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            IDamageable target = colliders[i].GetComponent<IDamageable>();

            if (target != null)
            {
                target.DealDamage(_damage);
            }
        }

        Destroy(gameObject);
    }

    #endregion Methods
}