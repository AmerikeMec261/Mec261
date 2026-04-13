using UnityEngine;

public class PlayerNormalizado : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _groundCheck;

    [Header("Movement Settings")]
    [Tooltip("Horizontal Movement Speed")]
    [SerializeField] private float _moveSpeed = 5f;
    [Tooltip("Jump Force Applied To The Player")]
    [SerializeField] private float _JumpForce = 10f;

    [Header("Ground Settings")]
    [Tooltip("Distance Used To Detect The Ground.")]
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [Tooltip("Layer Considered As Ground")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Health Settings")]
    [Tooltip("Player Health Points")]
    [SerializeField] private int _health = 5;

    private float _horizontalInput;
    private bool _isGorunded;
    private bool _isDead;

    #endregion Variables

    #region Unity Methods

    private void Awake()
    {
        ValidateComponents();
    }

    private void Update()
    {
        if (_isDead) return;

        ReadInput();
        CheckGround();
        Jump();
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

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
        _rigidbody.linearVelocity = new Vector2 (_horizontalInput * _moveSpeed, _rigidbody.linearVelocity.y);

        if (_horizontalInput!=0f)
        {
            _spriteRenderer.flipX = _horizontalInput<0f;
        }
    }

    private void Jump()
    {
        if (!_isGorunded || !Input.GetKeyDown(KeyCode.Escape)) return;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _JumpForce);
    }

    private void CheckGround()
    {
        _isGorunded = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);
    }

    public void ReceiveDamage(int damage)
    {
        if (_isDead) return ;
        _health -= damage;

        if(_health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _isDead = true;
        _rigidbody.linearVelocity = Vector2.zero;
        _spriteRenderer.color= Color.yellow;
        gameObject.SetActive(false);
    }

    private void ValidateComponents()
    {
        if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
        if(_spriteRenderer==null) _spriteRenderer = GetComponent<SpriteRenderer>();
        if(_groundCheck==null)
        {
            Debug.LogError("PlayerController2D:GroundCheck Transform no asignado", this);
            enabled = false;
        }

        if (_rigidbody == null || _spriteRenderer==null)
        {
            Debug.LogError("PlayerController2D: Faltan componentes requeridos (Rigidbody2D/SpriteRenderer)!",this);
            enabled = false;
        }
    }

    #endregion Methods

    #region Unity Special Methods

    private void OnDrawGizmosSelected()
    {
        if(_groundCheck==null) return;

        Gizmos.color = Color.yellow;
    }

    #endregion Unity Special Methods
}
