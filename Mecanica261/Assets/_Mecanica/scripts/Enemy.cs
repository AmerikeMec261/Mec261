using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    #region Variables

    [Header("Stats")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _shield = 0f;
    [SerializeField] private float _speed = 2f;

    [Header("Movement")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private Transform _currentTarget;
    private Rigidbody _rigidbody;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentTarget = _pointB;
    }

    private void FixedUpdate()
    {
        MoveEnemy();
        CheckTargetReached();
    }

    #endregion Unity Methods

    #region Methods

    private void MoveEnemy()
    {
        Vector3 targetPositionOnPlane = new Vector3(
            _currentTarget.position.x,
            _rigidbody.position.y,
            _currentTarget.position.z
        );

        Vector3 nextPosition = Vector3.MoveTowards(
            _rigidbody.position,
            targetPositionOnPlane,
            _speed * Time.fixedDeltaTime
        );

        _rigidbody.MovePosition(nextPosition);
    }

    private void CheckTargetReached()
    {
        Vector3 targetPositionOnPlane = new Vector3(
            _currentTarget.position.x,
            _rigidbody.position.y,
            _currentTarget.position.z
        );

        if (Vector3.Distance(_rigidbody.position, targetPositionOnPlane) >= 0.2f)
        {
            return;
        }

        if (_currentTarget == _pointA)
        {
            _currentTarget = _pointB;
            return;
        }

        _currentTarget = _pointA;
    }

    public void DealDamage(float damage)
    {
        if (_shield > 0f)
        {
            _shield -= damage;

            if (_shield < 0f)
            {
                _health += _shield;
                _shield = 0f;
            }

            Debug.Log(gameObject.name + " remaining health: " + _health);
            return;
        }

        _health -= damage;

        Debug.Log(gameObject.name + " remaining health: " + _health);

        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    #endregion Methods
}