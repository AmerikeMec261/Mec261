
using UnityEngine;


public class Enemys : MonoBehaviour ,IEnemy , IDamageable
{
    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _shield = 1f;
    [SerializeField] private float _health = 70f;

    [Header("MOVEMENT")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    public float Speed => _speed;
    public float Shield => _shield;
    public float Health => _health;


    private float _currentHealth;
    private Transform _currentTarget;

    private void Start()
    {
        _currentHealth = _health;
        _currentTarget = _pointB;

    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, _currentTarget.position, _speed * Time.deltaTime );

        if ( Vector3.Distance(transform.position, _currentTarget.position) <0.1f )
        {
            _currentTarget = _currentTarget == _pointB  ? _pointA : _pointB;
        }
        
    }

    public void ReciveDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log($"{gameObject.name}.Le hicieron{damage}de daño , Le quedan {_currentHealth}");
        if (_currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
