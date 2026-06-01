using UnityEngine;
using System.Collections;

namespace Minecraft
{
    public class ZombieEnemy : Enemy, IAttacker
    {
        [Header("Attack")]
        [Tooltip("Damage dealt when the melee attack connects.")]
        [SerializeField] private float _damage = 10f;
        [Tooltip("Maximum distance from the target required to apply melee damage.")]
        [SerializeField] private float _attackRange = 1.5f;
        [Tooltip("Delay between starting the swing and applying damage.")]
        [SerializeField] private float _attackWindupTime = 0.2f;

        public float Damage { get { return _damage; } }

        public override IEnumerator Attack()
        {
            GetComponent<IWeaponAnimationReceiver>()?.PlayPunch();
            yield return new WaitForSeconds(_attackWindupTime);

            if (Vector3.Distance(transform.position, _targetTransform.position) <= _attackRange)
            {
                _targetTransform.GetComponent<IDamagable>()?.ReceiveDamage(Damage);
            }
        }
    }
}
