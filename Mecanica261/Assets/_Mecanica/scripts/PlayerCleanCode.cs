using UnityEngine;

public class PlayerCleanCode : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _groundCheck;

    [Header("Movement Settings")]
    [Tooltip("Horizontal movement speed.")]
    [SerializeField] private float _moveSpeed = 5f;
    [Tooltip("Jump force applied to the player.")]
    [SerializeField] private float _jumpForce = 10f;

    [Header("Ground Settings")]
    [Tooltip("Distance used to detect the ground.")]
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [Tooltip("Layer considered as ground.")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Health Settings")]
    [Tooltip("Player health points.")]
    [SerializeField] private int _health = 5;

    private float _horizontalInput;
    private bool _isGrounded;
    private bool _isDead;

    #endregion Variables

    #region Unity Methods

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        ReadInput();

        CheckGround();

        Jump();
    }

    private void FixedUpdate()
    {
        if (_isDead)
        {
            return;
        }

        Move();
    }

    #endregion Unity Methods

    #region Methods

    private void ReadInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        _rigidbody.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rigidbody.linearVelocity.y);

        if (_horizontalInput > 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_horizontalInput < 0f)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void Jump()
    {
        if (!_isGrounded)
        {
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast
        (
            _groundCheck.position,
            Vector2.down,
            _groundCheckDistance,
            _groundLayer
        );

        if (hit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
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

        _spriteRenderer.color = Color.red;

        gameObject.SetActive(false);
    }

    #endregion Methods

    #region Unity Special Methods

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawLine
        (
            _groundCheck.position,
            _groundCheck.position + Vector3.down * _groundCheckDistance
        );
    }

    #endregion Unity Special Methods
}