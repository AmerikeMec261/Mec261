using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(CharacterController))]
public class BasicAI : MonoBehaviour, IMovementController
{
    [Header("Dependencies")]
    [Tooltip("Transform this AI will follow when it enters the activation distance. If empty, the AI only wanders.")]
    [SerializeField] private Transform _targetTransform;

    [Header("Movement")]
    [Tooltip("Movement speed used for wandering and following.")]
    [SerializeField] private float _speed = 5f;
    [Tooltip("Degrees per second the AI can rotate toward its movement direction.")]
    [SerializeField] private float _turnSpeed = 360f;
    [Tooltip("Downward acceleration applied through the CharacterController.")]
    [SerializeField] private float _gravity = -20f;

    [Header("Follow")]
    [Tooltip("Distance at which the AI stops wandering and starts following the target.")]
    [SerializeField] private float _activationDistance = 1f;
    [Tooltip("Total vision cone angle used to detect the target in front of the AI.")]
    [SerializeField] private float _visionAngle = 90f;
    [Tooltip("Distance where the AI detects the target from any direction, even outside the vision cone.")]
    [SerializeField] private float _passiveDetectionDistance = 2f;
    [Tooltip("Distance considered close enough to fire the arrival event.")]
    [SerializeField] private float _arrivalDistance = 0.1f;
    [Tooltip("Seconds the AI waits at the target's last known position before wandering again.")]
    [SerializeField] private float _investigationPauseTime = 1f;

    [Header("Wander")]
    [Tooltip("Maximum distance from the current position for the next random wander point.")]
    [SerializeField] private float _wanderRadius = 3f;
    [Tooltip("Distance considered close enough to a random wander point.")]
    [SerializeField] private float _wanderArrivalDistance = 0.25f;
    [Tooltip("Seconds the AI waits after reaching a wander point before choosing the next one.")]
    [SerializeField] private float _wanderPauseTime = 1f;
    [Tooltip("How many random points the AI tries before keeping its current position.")]
    [SerializeField] private int _wanderPointAttempts = 10;

    [Header("Idle Look")]
    [Tooltip("Radius used to pick random look positions while the AI is waiting.")]
    [SerializeField] private float _idleLookRadius = 4f;
    [Tooltip("Vertical offset used for random look positions while the AI is waiting.")]
    [SerializeField] private float _idleLookHeight = 1.5f;
    [Tooltip("Seconds between random look positions while the AI is waiting.")]
    [SerializeField] private float _idleLookInterval = 2f;

    [Header("Ground")]
    [Tooltip("Height above the random wander point where the ground raycast starts.")]
    [SerializeField] private float _groundCheckHeight = 5f;
    [Tooltip("Maximum distance the ground raycast checks downward.")]
    [SerializeField] private float _groundCheckDistance = 20f;
    [Tooltip("Layers considered valid ground for wander targets.")]
    [SerializeField] private LayerMask _groundLayer;
    [Tooltip("Tag considered valid ground for wander targets.")]
    [SerializeField, Tag] private string _groundTag = "Ground";

    [Header("Events")]
    [Tooltip("Invoked whenever the AI is within arrival distance. Enemy cooldowns usually decide whether an attack happens.")]
    [SerializeField] private UnityEvent _onArrival;

    private enum State { Wander, Investigate, Follow }

    private CharacterController _characterController;
    private ILooker _looker;
    private State _state;
    private Vector3 _wanderTarget;
    private Vector3 _lastKnownTargetPosition;
    private Vector3 _velocity;
    private float _nextWanderMoveTime;
    private float _investigationEndTime;
    private float _nextIdleLookTime;
    private bool _canMove = true;
    private bool _isInvestigating;

    public event UnityAction OnArrival
    {
        add => _onArrival.AddListener(value);
        remove => _onArrival.RemoveListener(value);
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _looker = GetComponent<ILooker>();
        if (_groundLayer.value == 0) { _groundLayer = LayerMask.GetMask(_groundTag); }

        PickWanderTarget();
    }

    private void Update()
    {
        ApplyGravity();

        if (!_canMove)
        {
            Move(Vector3.zero);
            return;
        }

        float distanceToTarget = GetDistanceToTarget();
        UpdateState(distanceToTarget, CanSeeTarget(distanceToTarget));

        if (_state == State.Follow)
        {
            Follow(distanceToTarget);
            return;
        }

        if (_state == State.Investigate)
        {
            Investigate();
            return;
        }

        Wander();
    }

    public void StopMovement()
    {
        _canMove = false;
    }

    public void ResumeMovement()
    {
        _canMove = true;
    }

    private float GetDistanceToTarget()
    {
        if (_targetTransform == null) { return Mathf.Infinity; }

        return Vector3.Distance(transform.position, _targetTransform.position);
    }

    private void Wander()
    {
        if (Vector3.Distance(transform.position, _wanderTarget) <= _wanderArrivalDistance)
        {
            PickWanderTarget();
            _nextWanderMoveTime = Time.time + _wanderPauseTime;
            _looker?.ClearLookTarget();
            Move(Vector3.zero);
            return;
        }

        if (Time.time < _nextWanderMoveTime)
        {
            LookAround();
            Move(Vector3.zero);
            return;
        }

        _looker?.SetLookPosition(_wanderTarget);
        MoveTowards(_wanderTarget);
    }

    private void Investigate()
    {
        _looker?.SetLookPosition(_lastKnownTargetPosition);

        if (Vector3.Distance(transform.position, _lastKnownTargetPosition) > _wanderArrivalDistance)
        {
            MoveTowards(_lastKnownTargetPosition);
            return;
        }

        if (_investigationEndTime <= 0f)
        {
            _investigationEndTime = Time.time + _investigationPauseTime;
        }

        if (Time.time < _investigationEndTime)
        {
            Move(Vector3.zero);
            return;
        }

        _isInvestigating = false;
        _investigationEndTime = 0f;
        PickWanderTarget();
    }

    private void Follow(float distanceToTarget)
    {
        _looker?.SetLookTarget(_targetTransform);
        _lastKnownTargetPosition = _targetTransform.position;
        _isInvestigating = true;
        _investigationEndTime = 0f;

        if (distanceToTarget <= _arrivalDistance)
        {
            Arrive();
            return;
        }

        MoveTowards(_targetTransform.position);
    }

    private void Arrive()
    {
        _onArrival.Invoke();

        foreach (IArrivalReceiver arrivalReceiver in GetComponents<IArrivalReceiver>())
        {
            arrivalReceiver.OnArrival();
        }
    }

    private bool CanSeeTarget(float distanceToTarget)
    {
        if (_targetTransform == null) { return false; }
        if (distanceToTarget <= _passiveDetectionDistance) { return true; }
        if (distanceToTarget > _activationDistance) { return false; }

        Vector3 directionToTarget = _targetTransform.position - transform.position;
        directionToTarget.y = 0f;

        return Vector3.Angle(transform.forward, directionToTarget) <= _visionAngle * 0.5f;
    }

    private void UpdateState(float distanceToTarget, bool canSeeTarget)
    {
        if (canSeeTarget)
        {
            _state = State.Follow;
            return;
        }

        _state = _isInvestigating ? State.Investigate : State.Wander;
    }

    private void PickWanderTarget()
    {
        for (int i = 0; i < _wanderPointAttempts; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * _wanderRadius;
            Vector3 randomPosition = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            Vector3 rayOrigin = randomPosition + Vector3.up * _groundCheckHeight;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, _groundCheckDistance) && IsGround(hit.collider))
            {
                _wanderTarget = hit.point;
                return;
            }
        }

        _wanderTarget = transform.position;
    }

    private void LookAround()
    {
        if (Time.time < _nextIdleLookTime) { return; }

        Vector2 randomPoint = Random.insideUnitCircle * _idleLookRadius;
        Vector3 lookPosition = transform.position + new Vector3(randomPoint.x, _idleLookHeight, randomPoint.y);

        _looker?.SetLookPosition(lookPosition);
        _nextIdleLookTime = Time.time + _idleLookInterval;
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        Move(direction.normalized * _speed);
    }

    private void Move(Vector3 horizontalVelocity)
    {
        FaceMoveDirection(horizontalVelocity);

        Vector3 movement = horizontalVelocity + Vector3.up * _velocity.y;
        _characterController.Move(movement * Time.deltaTime);
    }

    private void FaceMoveDirection(Vector3 horizontalVelocity)
    {
        if (horizontalVelocity.sqrMagnitude <= 0f) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity.normalized, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
    }

    private bool IsGround(Collider collider)
    {
        bool isOnGroundLayer = (_groundLayer.value & (1 << collider.gameObject.layer)) != 0;
        bool hasGroundTag = collider.gameObject.tag == _groundTag;

        return isOnGroundLayer || hasGroundTag;
    }

    #region DebugVisuals
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _activationDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _arrivalDistance);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, _passiveDetectionDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _wanderRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_wanderTarget, _wanderArrivalDistance);

        Gizmos.color = Color.Lerp(Color.blue, Color.white, 0.5f);
        Gizmos.DrawWireSphere(transform.position, _idleLookRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_lastKnownTargetPosition, _arrivalDistance);

        DrawVisionCone();

        Vector3 groundRayStart = transform.position + Vector3.up * _groundCheckHeight;
        Vector3 groundRayEnd = groundRayStart + Vector3.down * _groundCheckDistance;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundRayStart, groundRayEnd);
        Gizmos.DrawWireSphere(groundRayStart, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundRayEnd, 0.25f);
    }

    private void DrawVisionCone()
    {
        Vector3 leftDirection = Quaternion.Euler(0f, -_visionAngle * 0.5f, 0f) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0f, _visionAngle * 0.5f, 0f) * transform.forward;
        Vector3 forwardEnd = transform.position + transform.forward * _activationDistance;
        Vector3 leftEnd = transform.position + leftDirection * _activationDistance;
        Vector3 rightEnd = transform.position + rightDirection * _activationDistance;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, leftEnd);
        Gizmos.DrawLine(transform.position, rightEnd);
        Gizmos.DrawLine(transform.position, forwardEnd);
        Gizmos.DrawLine(leftEnd, rightEnd);
    }
    #endregion DebugVisuals

}
