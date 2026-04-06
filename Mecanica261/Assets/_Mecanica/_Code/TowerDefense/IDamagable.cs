public interface IDamagable 
{
    float MaxHealth { get; }
    float CurrentHealth { get; set; }

    void TakeDamage(float damage);
    void UpdateHealthBar();
    void Death();
}
