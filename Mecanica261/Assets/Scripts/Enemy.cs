using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _speed = 2f;

    [Header("Movement")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;


    [SerializeField] private TextMeshPro _lifeText;

    private Transform _currentTarget;

    private void Start()
    {
        _currentTarget = _pointB;
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _currentTarget.position,
            _speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, _currentTarget.position) < 0.1f)
        {
            _currentTarget = _currentTarget == _pointA ? _pointB : _pointA;
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _lifeText .text = _health.ToString();

        Debug.Log(gameObject.name + " vida restante: " + _health);

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}