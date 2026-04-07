public interface IProjectile
{
    void Fire();
    void SetSpeed(float speed);
    public interface IDamageable
    {
        void TakeDamage(float damage);
    }
}