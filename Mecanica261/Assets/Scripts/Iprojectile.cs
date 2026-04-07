using UnityEngine;

public interface IProjectile
{
    float Damage { get; }
    float Speed { get; }

    void Fire(Vector3 launchVelocity);
    void DealDamage(Collider other);
}