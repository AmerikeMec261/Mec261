using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Ajustes del Enemigo")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _speed = 2f;

    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private TextMeshPro _healthText;

    private Transform _target;

    private void Start()
    {
        _target = _pointB;
    }
    private void Update()
    {
        MoveEnemy();
    }
    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position,_target.position,_speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            _target = _target == _pointA ? _pointB : _pointA;
        }
    }
    public void DamageEnemy(float damage)
    {
        _health -= damage;
        _healthText.text = _health.ToString();
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}