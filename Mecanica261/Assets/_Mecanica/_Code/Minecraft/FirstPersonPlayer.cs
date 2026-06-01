using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonPlayer : MonoBehaviour, Minecraft.IDamagable
{
    [Header("Dependencies")]
    [SerializeField, Required] private Camera _playerCamera;
    [SerializeField, Required] private Transform _headPivot;
    [SerializeField, Required] private Transform _handTransform;
    [SerializeField] private Minecraft.Weapon _currentWeapon;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _bodyTurnSpeed = 360f;

    [Header("Look")]
    [SerializeField] private float _lookSensitivity = 2f;
    [SerializeField] private float _headYawLimit = 65f;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -20f;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckRadius = 0.3f;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField, Tag] private string _groundTag = "Ground";

    [Header("Life")]
    [SerializeField] private float _maxLife = 100f;
    [SerializeField] private float _currentLife = 100f;
    [SerializeField] private UnityEvent _onLifeChanged = new UnityEvent();

    [Header("Damage Feedback")]
    [SerializeField] private float _cameraShakeDuration = 0.15f;
    [SerializeField] private float _cameraShakeStrengthPerDamage = 0.015f;
    [SerializeField] private float _maximumCameraShakeStrength = 0.35f;

    private CharacterController _characterController;
    private Vector3 _velocity;
    private float _cameraPitch;
    private float _headYaw;
    private bool _isGrounded;
    private Coroutine _cameraShakeCoroutine;
    private Vector3 _cameraShakeStartPosition;

    public Transform Hand { get { return _handTransform; } private set { } }
    public Minecraft.Weapon CurrentWeapon { get { return _currentWeapon; } private set { _currentWeapon = value; } }
    public float MaxLife { get { return _maxLife; } private set { _maxLife = value; } }
    public float CurrentLife { get { return _currentLife; } private set { SetCurrentLife(value); } }
    public UnityEvent OnLifeChanged { get { return _onLifeChanged; } }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _characterController = GetComponent<CharacterController>();
        if (_groundLayer.value == 0) { _groundLayer = LayerMask.GetMask(_groundTag); }
        if (_currentWeapon == null) { _currentWeapon = _handTransform.GetComponentInChildren<Minecraft.Weapon>(); }
    }

    private void Update()
    {
        Look();
        Move();
        UseWeapon();
    }

    public void ReceiveDamage(float damage)
    {
        float previousLife = CurrentLife;

        CurrentLife = Mathf.Max(CurrentLife - damage, 0f);
        ShakeCamera(previousLife - CurrentLife);
    }

    public void SetWeapon(Minecraft.Weapon weapon)
    {
        CurrentWeapon = weapon;
    }

    private void SetCurrentLife(float currentLife)
    {
        if (Mathf.Approximately(_currentLife, currentLife)) { return; }

        _currentLife = currentLife;
        _onLifeChanged.Invoke();
    }

    private void UseWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentWeapon?.TryUse(_playerCamera.transform.forward);
        }
    }

    private void ShakeCamera(float damage)
    {
        Camera mainCamera = Camera.main;

        if (damage <= 0f) { return; }
        if (mainCamera == null) { return; }

        if (_cameraShakeCoroutine != null)
        {
            StopCoroutine(_cameraShakeCoroutine);
            mainCamera.transform.localPosition = _cameraShakeStartPosition;
        }

        _cameraShakeCoroutine = StartCoroutine(CameraShakeRoutine(mainCamera.transform, damage));
    }

    private IEnumerator CameraShakeRoutine(Transform cameraTransform, float damage)
    {
        _cameraShakeStartPosition = cameraTransform.localPosition;
        float strength = Mathf.Min(damage * _cameraShakeStrengthPerDamage, _maximumCameraShakeStrength);
        float timer = 0f;

        while (timer < _cameraShakeDuration)
        {
            timer += Time.deltaTime;
            float remainingStrength = Mathf.Lerp(strength, 0f, timer / _cameraShakeDuration);
            cameraTransform.localPosition = _cameraShakeStartPosition + Random.insideUnitSphere * remainingStrength;
            yield return null;
        }

        cameraTransform.localPosition = _cameraShakeStartPosition;
        _cameraShakeCoroutine = null;
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * _lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _lookSensitivity;

        _cameraPitch = Mathf.Clamp(_cameraPitch - mouseY, -90f, 90f);
        _headYaw += mouseX;

        if (_headYaw > _headYawLimit)
        {
            float extraYaw = _headYaw - _headYawLimit;
            transform.Rotate(Vector3.up * extraYaw);
            _headYaw = _headYawLimit;
        }
        else if (_headYaw < -_headYawLimit)
        {
            float extraYaw = _headYaw + _headYawLimit;
            transform.Rotate(Vector3.up * extraYaw);
            _headYaw = -_headYawLimit;
        }

        _headPivot.localRotation = Quaternion.Euler(_cameraPitch, _headYaw, 0f);
        _playerCamera.transform.localRotation = Quaternion.identity;
    }

    private void Move()
    {
        _isGrounded = IsGrounded();

        if (_isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 cameraForward = _playerCamera.transform.forward;
        Vector3 cameraRight = _playerCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 moveDirection = cameraRight.normalized * horizontalInput + cameraForward.normalized * verticalInput;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _moveSpeed;

        TurnBodyTowardsLookDirection(cameraForward, moveDirection);

        _characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void TurnBodyTowardsLookDirection(Vector3 lookDirection, Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude <= 0f) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        Quaternion previousRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _bodyTurnSpeed * Time.deltaTime);

        float bodyYawDelta = Mathf.DeltaAngle(previousRotation.eulerAngles.y, transform.eulerAngles.y);
        _headYaw -= bodyYawDelta;
        _headYaw = Mathf.Clamp(_headYaw, -_headYawLimit, _headYawLimit);
        _headPivot.localRotation = Quaternion.Euler(_cameraPitch, _headYaw, 0f);
    }

    private bool IsGrounded()
    {
        Vector3 spherePosition = transform.position + Vector3.down * (_characterController.height * 0.5f - _characterController.radius + _groundCheckDistance);
        Collider[] colliders = Physics.OverlapSphere(spherePosition, _groundCheckRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.transform == transform) { continue; }
            if (IsGround(collider)) { return true; }
        }

        return false;
    }

    private bool IsGround(Collider collider)
    {
        bool isOnGroundLayer = (_groundLayer.value & (1 << collider.gameObject.layer)) != 0;
        bool hasGroundTag = collider.gameObject.tag == _groundTag;

        return isOnGroundLayer || hasGroundTag;
    }
}
