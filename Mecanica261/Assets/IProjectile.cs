public interface IProjectile
{
    float Damage { get; }
    float Speed { get; }

    void Shoot(UnityEngine.Vector3 direction);
    void DealDamage(UnityEngine.GameObject target);
}
