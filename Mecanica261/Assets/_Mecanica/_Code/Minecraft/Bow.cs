using UnityEngine;
using NaughtyAttributes;

namespace Minecraft
{
    public class Bow : Weapon
    {
        [Header("Projectile")]
        [SerializeField, Required] private Projectile _projectilePrefab;
        [SerializeField] private float _projectileSpeed = 15f;

        public override void Use(Vector3 direction)
        {
            GetComponentInParent<IWeaponAnimationReceiver>()?.PlayBowRecoil();

            Projectile projectile = Instantiate(_projectilePrefab, AttackPoint.position, AttackPoint.rotation);
            projectile.Shoot(direction, _projectileSpeed, Damage);
        }
    }
}
