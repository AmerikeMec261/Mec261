using UnityEngine;
using System.Collections;

namespace Minecraft
{
    public class CreeperEnemy : Enemy, IAttacker
    {
        [Header("Attack")]
        [SerializeField] private float _damage = 40f;
        [SerializeField] private float _explosionRadius = 3f;
        [SerializeField] private float _chargeTime = 3f;

        public float Damage { get { return _damage; } }

        public override IEnumerator AttackMethod()
        {
            GetComponent<IMovementController>()?.StopMovement();

            yield return new WaitForSeconds(_chargeTime);

            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (Collider collider in colliders)
            {
                IDamagable damagable = collider.GetComponentInParent<IDamagable>();

                if (damagable == this) { continue; }

                damagable?.ReceiveDamage(Damage);
            }

            ReceiveDamage(MaxLife);
        }
    }
}
