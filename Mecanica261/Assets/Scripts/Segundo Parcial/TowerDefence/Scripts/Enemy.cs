using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _shield = 0f;
    public void TakeDamage(float amount)
    {
        float damageLeft = amount;

        // escudo
        if (_shield > 0f)
        {
            float absorbed = Mathf.Min(_shield, damageLeft);
            _shield -= absorbed;
            damageLeft -= absorbed;

            Debug.Log($"{gameObject.name} absorbió {absorbed} de daño. Escudo restante: {_shield}");
        }

        // vida
        if (damageLeft > 0f)
        {
            _health -= damageLeft;
            _health = Mathf.Max(_health, 0f);

            Debug.Log($"{gameObject.name} recibió {damageLeft} de daño. Vida restante: {_health}");
        }

        if (_health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} murió");
        Destroy(gameObject);
    }
}