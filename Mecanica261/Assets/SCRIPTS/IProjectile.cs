public interface IProjectile
{
    float Damage { get; }
    float Speed { get; }

    void Fire();
    void DealDamage(IDamageable target);
}