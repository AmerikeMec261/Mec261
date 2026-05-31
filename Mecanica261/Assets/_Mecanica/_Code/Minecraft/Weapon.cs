using UnityEngine;

namespace Minecraft
{
    public abstract class Weapon : MonoBehaviour, IAttacker
    {
        [Header("Attack")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] protected Transform _attackPoint;

        public float Damage { get { return _damage; } }
        protected Transform AttackPoint { get { return _attackPoint != null ? _attackPoint : transform; } }

        public abstract void Use(Vector3 direction);

        public virtual void Use(Transform targetTransform)
        {
            Use(targetTransform.position - AttackPoint.position);
        }

        protected void DamageTarget(Collider collider)
        {
            collider.GetComponentInParent<IDamagable>()?.ReceiveDamage(Damage);
        }
    }
}
