using UnityEngine;
using NaughtyAttributes;

namespace Minecraft
{
    public class Bow : Weapon
    {
        [Header("Projectile")]
        [Tooltip("Projectile prefab spawned when the bow fires.")]
        [SerializeField, Required, ValidateInput(nameof(HasProjectile), "Prefab must have a Projectile component.")]
        private GameObject _projectilePrefab;
        [Tooltip("Initial movement speed applied to fired projectiles.")]
        [SerializeField] private float _projectileSpeed = 15f;

        protected override void UseWeapon(Vector3 direction)
        {
            GetComponentInParent<IWeaponAnimationReceiver>()?.PlayBowRecoil();

            GameObject projectileObject = Instantiate(_projectilePrefab, AttackPoint.position, AttackPoint.rotation);
            Projectile projectile = projectileObject.GetComponentInChildren<Projectile>();
            projectile.Shoot(AttackPoint.forward, _projectileSpeed, Damage);
        }

        private bool HasProjectile()
        {
            return _projectilePrefab != null && _projectilePrefab.GetComponentInChildren<Projectile>() != null;
        }
    }
}
