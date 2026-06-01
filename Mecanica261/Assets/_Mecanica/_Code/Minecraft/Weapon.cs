using UnityEngine;
using UnityEngine.Events;

namespace Minecraft
{
    public abstract class Weapon : MonoBehaviour, IAttacker
    {
        [Header("Attack")]
        [Tooltip("Damage this weapon applies when it hits a target.")]
        [SerializeField] private float _damage = 10f;
        [Tooltip("Origin used for aiming, spawning, or hit checks.")]
        [SerializeField] protected Transform _attackPoint;
        [Tooltip("Minimum time between successful weapon uses.")]
        [SerializeField] private float _useCooldown = 0.75f;

        private float _nextUseTime;
        [Tooltip("Invoked whenever this weapon is used successfully.")]
        [SerializeField] private UnityEvent _onUsed = new UnityEvent();

        public float Damage { get { return _damage; } }
        protected Transform AttackPoint { get { return _attackPoint != null ? _attackPoint : transform; } }
        public event UnityAction OnUsed
        {
            add { _onUsed.AddListener(value); }
            remove { _onUsed.RemoveListener(value); }
        }

        public void Use(Vector3 direction)
        {
            UseWeapon(direction);
            _onUsed.Invoke();
        }

        public bool TryUse(Vector3 direction)
        {
            if (Time.time < _nextUseTime) { return false; }

            Use(direction);
            _nextUseTime = Time.time + _useCooldown;
            return true;
        }

        public virtual void Use(Transform targetTransform)
        {
            Use(targetTransform.position - AttackPoint.position);
        }

        protected abstract void UseWeapon(Vector3 direction);

        protected void DamageTarget(Collider collider)
        {
            collider.GetComponentInParent<IDamagable>()?.ReceiveDamage(Damage);
        }
    }
}
