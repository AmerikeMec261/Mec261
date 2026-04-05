using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _shield = 0f;

    [Header("Patrol settings")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private Transform _currentTarget;

    public float Health
    {
        get => _health;
        set => _health = value;
    }
    public float Speed
    {
        get => _health;
        set => _health = value;
    }
    public float Shield
    {
        get => _health;
        set => _health = value;
    }

    private void Start()
    {        
        if (_pointA != null)
        {
            transform.position = _pointA.position;
            _currentTarget = _pointB;
        }
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (_pointA == null || _pointB == null) return;
        transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _currentTarget.position) < 0.1f)
        {
            _currentTarget = _currentTarget = _pointA ? _pointB : _pointA;
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        Debug.Log($"[{gameObject.name}] Recibió {amount} de daño. Vida restante: {_health}");

        if (_health <= 0)
        {
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        if (_pointA != null && _pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_pointA.position, _pointB.position);
        }
    }

    private void Die()
    {
        Debug.Log($"[{gameObject.name}] ha sido destruido.");
        Destroy(gameObject);
    }
}
