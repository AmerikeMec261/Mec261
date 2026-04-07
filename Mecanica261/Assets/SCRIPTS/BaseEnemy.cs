using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _shield = 0f;
    [SerializeField] private float _movementSpeed = 3f;

    [Header("Patrol Points")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private float _currentHealth;
    private Transform _currentTarget;
    public float Health => _currentHealth;
    public float Shield => _shield;
    public float MovementSpeed => _movementSpeed;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _currentTarget = _pointB;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _movementSpeed * Time.deltaTime);

        bool reachedTarget = Vector3.Distance(transform.position, _currentTarget.position) < 0.1f;

        if (reachedTarget) { _currentTarget = _currentTarget == _pointB ? _pointA : _pointB; }
    }

    public void ReceiveDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0f);

        Debug.Log($"{gameObject.name} recibio {damage} de daño. Vida restante: {_currentHealth}");

        if (_currentHealth <= 0f) { Die(); }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} fue destruido.");
        Destroy(gameObject);
    }
}