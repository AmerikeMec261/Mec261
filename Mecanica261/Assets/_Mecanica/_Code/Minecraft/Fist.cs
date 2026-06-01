using UnityEngine;
using UnityEngine.Serialization;

namespace Minecraft
{
    public class Fist : Weapon
    {
        [Header("Hit Sphere")]
        [FormerlySerializedAs("_range")]
        [Tooltip("Distance the fist checks forward for a target.")]
        [SerializeField] private float _attackRange = 1f;
        [FormerlySerializedAs("_radius")]
        [Tooltip("Radius of the fist hit check.")]
        [SerializeField] private float _attackRadius = 0.25f;

        protected override void UseWeapon(Vector3 direction)
        {
            GetComponentInParent<IWeaponAnimationReceiver>()?.PlayPunch();

            if (Physics.SphereCast(AttackPoint.position, _attackRadius, direction.normalized, out RaycastHit hit, _attackRange))
            {
                DamageTarget(hit.collider);
            }
        }
    }
}
