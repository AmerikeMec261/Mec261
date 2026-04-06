public interface IProjectile
{
    float Speed { get; }
    float Damage { get; }
    void Fire();
    void DoDamage();
}