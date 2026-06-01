using UnityEngine;

namespace Minecraft
{
    public class Fist : Weapon
    {
        [SerializeField] private float _range = 1f;
        [SerializeField] private float _radius = 0.25f;

        protected override void UseWeapon(Vector3 direction)
        {
            GetComponentInParent<IWeaponAnimationReceiver>()?.PlayPunch();

            if (Physics.SphereCast(AttackPoint.position, _radius, direction.normalized, out RaycastHit hit, _range))
            {
                DamageTarget(hit.collider);
            }
        }
    }
}
