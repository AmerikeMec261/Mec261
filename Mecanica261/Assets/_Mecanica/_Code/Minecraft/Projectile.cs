using UnityEngine;
using UnityEngine.Events;

namespace Minecraft
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IAttacker
    {
        [Header("Attack")]
        [SerializeField] private float _damage = 10f;

        [Header("Events")]
        [SerializeField] private UnityEvent _onFired = new UnityEvent();
        [SerializeField] private UnityEvent _onHit = new UnityEvent();

        private Rigidbody _rigidbody;

        public float Damage { get { return _damage; } }
        public UnityEvent OnFired { get { return _onFired; } }
        public UnityEvent OnHit { get { return _onHit; } }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Shoot(Vector3 direction, float speed, float damage)
        {
            _damage = damage;
            _rigidbody.linearVelocity = direction.normalized * speed;
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
