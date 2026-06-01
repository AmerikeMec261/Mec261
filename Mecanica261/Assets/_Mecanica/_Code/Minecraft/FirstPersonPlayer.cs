using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections;
using UnityEngine.Serialization;

namespace Minecraft
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonPlayer : MonoBehaviour, IDamagable
    {
        [Header("Dependencies")]
        [Tooltip("Camera used for first-person view and weapon aiming.")]
        [SerializeField, Required] private Camera _playerCamera;
        [Tooltip("Head pivot rotated by mouse look.")]
        [SerializeField, Required] private Transform _headPivot;
        [Tooltip("Parent transform used for held weapons.")]
        [SerializeField, Required] private Transform _handTransform;
        [Tooltip("Weapon currently used by player input.")]
        [SerializeField] private Weapon _currentWeapon;

        [Header("Movement")]
        [Tooltip("Movement speed used without sprinting.")]
        [SerializeField] private float _moveSpeed = 5f;
        [Tooltip("Movement speed used while holding sprint.")]
        [SerializeField] private float _sprintSpeed = 8f;
        [Tooltip("Degrees per second the body turns toward the look direction while moving.")]
        [SerializeField] private float _bodyTurnSpeed = 360f;

        [Header("Look")]
        [Tooltip("Multiplier applied to mouse movement.")]
        [SerializeField] private float _lookSensitivity = 2f;
        [Tooltip("Maximum head yaw before the body begins turning.")]
        [SerializeField] private float _headYawLimit = 65f;

        [Header("Jump")]
        [Tooltip("Height reached by a jump.")]
        [SerializeField] private float _jumpHeight = 1.5f;
        [Tooltip("Downward acceleration applied while airborne.")]
        [SerializeField] private float _gravity = -20f;

        [Header("Ground Check")]
        [Tooltip("Radius of the overlap sphere used to detect ground.")]
        [SerializeField] private float _groundCheckRadius = 0.3f;
        [Tooltip("Distance below the controller used for the ground check.")]
        [SerializeField] private float _groundCheckDistance = 0.2f;
        [Tooltip("Layers accepted as ground.")]
        [SerializeField] private LayerMask _groundLayer;
        [Tooltip("Tag accepted as ground.")]
        [SerializeField, Tag] private string _groundTag = "Ground";

        [Header("Life")]
        [Tooltip("Maximum life the player can have.")]
        [SerializeField] private float _maxLife = 100f;
        [Tooltip("Current life the player starts with.")]
        [SerializeField] private float _currentLife = 100f;
        [FormerlySerializedAs("_debugNoDeath")]
        [Tooltip("Prevents player death at zero life for testing.")]
        [SerializeField] private bool _isDeathDisabledForDebug;
        [Tooltip("Invoked whenever current life changes.")]
        [SerializeField] private UnityEvent _onLifeChanged = new UnityEvent();

        [Header("Damage Feedback")]
        [Tooltip("Duration of the camera shake after damage.")]
        [SerializeField] private float _cameraShakeDuration = 0.15f;
        [Tooltip("Camera shake strength added per point of damage.")]
        [SerializeField] private float _cameraShakeStrengthPerDamage = 0.015f;
        [Tooltip("Maximum camera shake strength after damage.")]
        [SerializeField] private float _maximumCameraShakeStrength = 0.35f;

        private CharacterController _characterController;
        private Coroutine _cameraShakeCoroutine;
        private Vector3 _cameraShakeStartPosition;
        private Vector3 _verticalVelocity;
        private float _cameraPitch;
        private float _headYaw;
        private bool _isGrounded;
        private bool _isDead;

        public Transform Hand { get { return _handTransform; } }
        public Weapon CurrentWeapon { get { return _currentWeapon; } private set { _currentWeapon = value; } }
        public bool IsDead { get { return _isDead; } }
        public float MaxLife { get { return _maxLife; } private set { _maxLife = value; } }
        public float CurrentLife { get { return _currentLife; } private set { SetCurrentLife(value); } }
        public UnityEvent OnLifeChanged { get { return _onLifeChanged; } }

        private void Awake()
        {
            LockCursor();
            _characterController = GetComponent<CharacterController>();
            if (_groundLayer.value == 0) { _groundLayer = LayerMask.GetMask(_groundTag); }
            if (_currentWeapon == null) { _currentWeapon = _handTransform.GetComponentInChildren<Weapon>(); }
        }

        private void Update()
        {
            if (_isDead) { return; }

            Look();
            Move();
            UseWeapon();
        }

        public void ReceiveDamage(float damage)
        {
            if (_isDead) { return; }

            float previousLife = CurrentLife;
            CurrentLife = Mathf.Max(CurrentLife - damage, 0f);

            ShakeCamera(previousLife - CurrentLife);

            if (CurrentLife <= 0f && !_isDeathDisabledForDebug)
            {
                Die();
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
        }

        private void SetCurrentLife(float currentLife)
        {
            if (Mathf.Approximately(_currentLife, currentLife)) { return; }

            _currentLife = currentLife;
            _onLifeChanged.Invoke();
        }

        private void Die()
        {
            if (_isDead) { return; }

            _isDead = true;
            _verticalVelocity = Vector3.zero;
            UnlockCursor();
            GameManager.Instance?.ShowDeathScreen();
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void UseWeapon()
        {
            if (!Input.GetMouseButtonDown(0)) { return; }
            if (_currentWeapon == null) { return; }

            _currentWeapon.TryUse(_playerCamera.transform.forward);
        }

        private void ShakeCamera(float damage)
        {
            if (damage <= 0f) { return; }

            Camera mainCamera = Camera.main;
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

            RotateBodyWhenHeadPassesLimit();
            ApplyHeadRotation();
        }

        private void RotateBodyWhenHeadPassesLimit()
        {
            if (_headYaw > _headYawLimit)
            {
                float extraYaw = _headYaw - _headYawLimit;
                transform.Rotate(Vector3.up * extraYaw);
                _headYaw = _headYawLimit;
                return;
            }

            if (_headYaw < -_headYawLimit)
            {
                float extraYaw = _headYaw + _headYawLimit;
                transform.Rotate(Vector3.up * extraYaw);
                _headYaw = -_headYawLimit;
            }
        }

        private void ApplyHeadRotation()
        {
            _headPivot.localRotation = Quaternion.Euler(_cameraPitch, _headYaw, 0f);
            _playerCamera.transform.localRotation = Quaternion.identity;
        }

        private void Move()
        {
            _isGrounded = IsGrounded();

            if (_isGrounded && _verticalVelocity.y < 0f)
            {
                _verticalVelocity.y = -2f;
            }

            Vector3 cameraForward = _playerCamera.transform.forward;
            Vector3 cameraRight = _playerCamera.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            Vector3 moveDirection = GetMoveDirection(cameraForward, cameraRight);
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _moveSpeed;

            TurnBodyTowardsLookDirection(cameraForward, moveDirection);
            _characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

            Jump();
            ApplyGravity();
        }

        private Vector3 GetMoveDirection(Vector3 cameraForward, Vector3 cameraRight)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            return cameraRight.normalized * horizontalInput + cameraForward.normalized * verticalInput;
        }

        private void Jump()
        {
            if (!_isGrounded) { return; }
            if (!Input.GetKeyDown(KeyCode.Space)) { return; }

            _verticalVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        private void ApplyGravity()
        {
            _verticalVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_verticalVelocity * Time.deltaTime);
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
            ApplyHeadRotation();
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
            bool hasGroundTag = collider.CompareTag(_groundTag);

            return isOnGroundLayer || hasGroundTag;
        }
    }
}
