using UnityEngine;

namespace Minecraft
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IAttacker
    {
        [SerializeField] private float _damage = 10f;

        private Rigidbody _rigidbody;

        public float Damage { get { return _damage; } }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Shoot(Vector3 direction, float speed, float damage)
        {
            _damage = damage;
            _rigidbody.linearVelocity = direction.normalized * speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamagable damagable = other.GetComponentInParent<IDamagable>();

            damagable?.ReceiveDamage(Damage);

            Destroy(gameObject);
        }
    }
}
