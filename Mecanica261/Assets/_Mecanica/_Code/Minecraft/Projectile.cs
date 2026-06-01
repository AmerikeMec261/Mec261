using UnityEngine;
using UnityEngine.Events;

namespace Minecraft
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IAttacker
    {
        [Header("Attack")]
        [Tooltip("Damage dealt when this projectile hits a damageable target.")]
        [SerializeField] private float _damage = 10f;

        [Header("Events")]
        [Tooltip("Invoked after the projectile receives its launch velocity.")]
        [SerializeField] private UnityEvent _onFired = new UnityEvent();
        [Tooltip("Invoked when the projectile collides with something.")]
        [SerializeField] private UnityEvent _onHit = new UnityEvent();

        private Rigidbody _rigidbody;

        public float Damage { get { return _damage; } }
        public UnityEvent OnFired { get { return _onFired; } }
        public UnityEvent OnHit { get { return _onHit; } }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Shoot(Vector3 direction, float projectileSpeed, float damage)
        {
            _damage = damage;
            _rigidbody.linearVelocity = direction.normalized * projectileSpeed;
            _onFired.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamagable damagable = collision.collider.GetComponentInParent<IDamagable>();

            damagable?.ReceiveDamage(Damage);
            _onHit.Invoke();

            Destroy(gameObject);
        }
    }
}
