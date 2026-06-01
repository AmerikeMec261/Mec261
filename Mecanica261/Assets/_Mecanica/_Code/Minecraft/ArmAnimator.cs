using UnityEngine;

namespace Minecraft
{
    public class ArmAnimator : MonoBehaviour
    {
        [Header("Dependencies")]
        [Tooltip("Arm pivots to rotate. Even and odd indexes move in opposite directions.")]
        [SerializeField] private Transform[] _armPivots;

        [Header("Idle")]
        [Tooltip("Base rotation added while idle so arms sit slightly away from the body.")]
        [SerializeField] private Vector3 _idleRotation = new Vector3(0f, 0f, 3f);
        [Tooltip("Extra breathing motion added over time while idle.")]
        [SerializeField] private Vector3 _breathingRotation = new Vector3(1f, 0f, 0f);
        [Tooltip("How fast the idle breathing loop plays.")]
        [SerializeField] private float _breathingSpeed = 1.5f;

        [Header("Walk")]
        [Tooltip("Arm swing used while walking.")]
        [SerializeField] private Vector3 _walkSwingRotation = new Vector3(25f, 0f, 0f);
        [Tooltip("How fast the walking arm swing loops.")]
        [SerializeField] private float _walkSwingSpeed = 7f;

        [Header("Run")]
        [Tooltip("Horizontal speed where the animator switches from walking to running.")]
        [SerializeField] private float _runSpeedThreshold = 6f;
        [Tooltip("Arm swing used while running.")]
        [SerializeField] private Vector3 _runSwingRotation = new Vector3(45f, 0f, 0f);
        [Tooltip("How fast the running arm swing loops.")]
        [SerializeField] private float _runSwingSpeed = 11f;

        [Header("Movement Detection")]
        [Tooltip("Minimum horizontal speed needed before walking animation starts.")]
        [SerializeField] private float _minimumMoveSpeed = 0.05f;
        [Tooltip("How quickly measured movement speed changes are smoothed.")]
        [SerializeField] private float _speedSmoothTime = 12f;
        [Tooltip("How quickly arms blend toward the current procedural pose.")]
        [SerializeField] private float _rotationSmoothTime = 12f;

        private Quaternion[] _startingRotations;
        private Vector3 _lastPosition;
        private float _animationTime;
        private float _horizontalSpeed;

        private void Awake()
        {
            _startingRotations = new Quaternion[_armPivots.Length];

            for (int i = 0; i < _armPivots.Length; i++)
            {
                _startingRotations[i] = _armPivots[i].localRotation;
            }

            _lastPosition = transform.position;
        }

        private void Update()
        {
            if (_armPivots.Length == 0) { return; }

            float horizontalSpeed = GetSmoothedHorizontalSpeed();

            if (horizontalSpeed < _minimumMoveSpeed)
            {
                AnimateIdle();
                return;
            }

            AnimateMovement(horizontalSpeed);
        }

        private float GetSmoothedHorizontalSpeed()
        {
            Vector3 movement = transform.position - _lastPosition;
            movement.y = 0f;
            _lastPosition = transform.position;

            float targetSpeed = Time.deltaTime <= 0f ? 0f : movement.magnitude / Time.deltaTime;
            _horizontalSpeed = Mathf.Lerp(_horizontalSpeed, targetSpeed, _speedSmoothTime * Time.deltaTime);

            return _horizontalSpeed;
        }

        private void AnimateIdle()
        {
            _animationTime += Time.deltaTime * _breathingSpeed;
            Vector3 breathingRotation = _breathingRotation * Mathf.Sin(_animationTime);

            for (int i = 0; i < _armPivots.Length; i++)
            {
                ApplyArmRotation(i, MirrorRotation(_idleRotation + breathingRotation, i));
            }
        }

        private void AnimateMovement(float horizontalSpeed)
        {
            bool isRunning = horizontalSpeed >= _runSpeedThreshold;
            Vector3 swingRotation = isRunning ? _runSwingRotation : _walkSwingRotation;
            float swingSpeed = isRunning ? _runSwingSpeed : _walkSwingSpeed;

            _animationTime += Time.deltaTime * swingSpeed;
            float swingAmount = Mathf.Sin(_animationTime);

            for (int i = 0; i < _armPivots.Length; i++)
            {
                float swingDirection = i % 2 == 0 ? 1f : -1f;
                ApplyArmRotation(i, swingRotation * swingAmount * swingDirection);
            }
        }

        private void ApplyArmRotation(int armIndex, Vector3 rotationOffset)
        {
            Quaternion targetRotation = _startingRotations[armIndex] * Quaternion.Euler(rotationOffset);
            _armPivots[armIndex].localRotation = Quaternion.Lerp(_armPivots[armIndex].localRotation, targetRotation, _rotationSmoothTime * Time.deltaTime);
        }

        private Vector3 MirrorRotation(Vector3 rotation, int armIndex)
        {
            float mirrorDirection = armIndex % 2 == 0 ? 1f : -1f;

            return new Vector3(rotation.x, rotation.y * mirrorDirection, rotation.z * mirrorDirection);
        }
    }
}
