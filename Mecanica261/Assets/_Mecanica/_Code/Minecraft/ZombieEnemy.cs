using UnityEngine;
using System.Collections;

namespace Minecraft
{
    public class ZombieEnemy : Enemy, IAttacker
    {
        [Header("Attack")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private float _attackWindupTime = 0.2f;

        public float Damage { get { return _damage; } }

        public override IEnumerator AttackMethod()
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
