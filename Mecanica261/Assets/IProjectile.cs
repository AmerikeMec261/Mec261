using UnityEngine;

public interface IProjectile
{
    float Damage { get; }
    float Speed { get; }

    void DealDamage(GameObject target);
}
