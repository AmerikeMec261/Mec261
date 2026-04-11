using UnityEngine;

public class EnemyCleanCode : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [Header("Movement Settings")]
    [Tooltip("Speed used while the enemy patrols.")]
    [SerializeField] private float _patrolSpeed = 2f;
    [Tooltip("Speed used while the enemy chases the player.")]
    [SerializeField] private float _chaseSpeed = 3.5f;

    [Header("Detection Settings")]
    [Tooltip("Distance to detect the player.")]
    [SerializeField] private float _detectionRange = 5f;
    [Tooltip("Distance to stop chasing the player.")]
    [SerializeField] private float _losePlayerRange = 8f;

    [Header("Combat Settings")]
    [Tooltip("Enemy health points.")]
    [SerializeField] private int _health = 3;

    private bool _isDead;
    private bool _seesPlayer;
    private bool _isGoingToPointB = true;

    #endregion Variables

    #region Unity Methods

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        CheckPlayerDistance();

        if (_seesPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    #endregion Unity Methods

    #region Methods

    private void CheckPlayerDistance()
    {
        if (_player == null)
        {
            _seesPlayer = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _detectionRange)
        {
            _seesPlayer = true;
        }

        if (distanceToPlayer >= _losePlayerRange)
        {
            _seesPlayer = false;
        }
    }

    private void Patrol()
    {
        if (_pointA == null || _pointB == null)
        {
            return;
        }

        if (_isGoingToPointB)
        {
            Vector2 directionToPointB = (_pointB.position - transform.position).normalized;

            _rigidbody.linearVelocity = new Vector2(directionToPointB.x * _patrolSpeed, _rigidbody.linearVelocity.y);

            float distanceToPointB = Vector2.Distance(transform.position, _pointB.position);

            if (distanceToPointB <= 0.2f)
            {
                _isGoingToPointB = false;
            }
        }
        else
        {
            Vector2 directionToPointA = (_pointA.position - transform.position).normalized;

            _rigidbody.linearVelocity = new Vector2(directionToPointA.x * _patrolSpeed, _rigidbody.linearVelocity.y);

            float distanceToPointA = Vector2.Distance(transform.position, _pointA.position);

            if (distanceToPointA <= 0.2f)
            {
                _isGoingToPointB = true;
            }
        }

        if (_rigidbody.linearVelocity.x > 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_rigidbody.linearVelocity.x < 0f)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void ChasePlayer()
    {
        if (_player == null)
        {
            return;
        }

        Vector2 directionToPlayer = (_player.position - transform.position).normalized;

        _rigidbody.linearVelocity = new Vector2(directionToPlayer.x * _chaseSpeed, _rigidbody.linearVelocity.y);

        if (_rigidbody.linearVelocity.x > 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_rigidbody.linearVelocity.x < 0f)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        _rigidbody.linearVelocity = Vector2.zero;
        _spriteRenderer.color = Color.gray;
        Destroy(gameObject, 1.5f);
    }

    #endregion Methods

    #region Unity Special Methods

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _losePlayerRange);
    }

    #endregion Unity Special Methods
}