using UnityEngine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("EnemyStats")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _shield = 0f;

    [Header("Patol")]
    [SerializeField] private Transform _ponitA;
    [SerializeField] private Transform _ponitB;

    private float _currentHP;
    private Transform _currentTarget;

    private void Start()
    {
        _currentHP = _maxHealth;
        _currentTarget = _ponitB;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (_ponitA == null || _ponitB == null) return;

        transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTarget.position) < 0.1f)
        {
            _currentTarget = (_currentTarget == _ponitB) ? _ponitB : _ponitA;
        }
    }

    public void ReceiveDamage(float damage)
    {
        _currentHP-=damage;
        Debug.Log($"{gameObject.name}recibió{damage}de daño. Vida:{_currentHP}/{_maxHealth}");

        if (_maxHealth <= 0f)
        {
            Debug.Log($"{gameObject.name}ha sido eliminado");
            Destroy(gameObject);
        }
    }
}
