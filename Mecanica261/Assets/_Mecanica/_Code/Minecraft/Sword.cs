using UnityEngine;

namespace Minecraft
{
    public class Sword : Weapon
    {
        [SerializeField] private float _range = 1.75f;
        [SerializeField] private float _radius = 0.45f;

        public override void Use(Vector3 direction)
        {
            GetComponentInParent<IWeaponAnimationReceiver>()?.PlaySwing();

            if (Physics.SphereCast(AttackPoint.position, _radius, direction.normalized, out RaycastHit hit, _range))
            {
                DamageTarget(hit.collider);
            }
        }
    }
}
