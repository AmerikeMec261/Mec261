using UnityEngine;
using NaughtyAttributes;

namespace Minecraft
{
    public class HeadLookController : MonoBehaviour, ILooker
    {
        [Header("Dependencies")]
        [Tooltip("Transform rotated to aim the head.")]
        [SerializeField, Required] private Transform _head;
        [Tooltip("Body transform used as the local look reference.")]
        [SerializeField, Required] private Transform _body;

        [Header("Settings")]
        [Tooltip("How quickly the head rotates toward its target.")]
        [SerializeField] private float _lookSpeed = 8f;
        [Tooltip("Maximum left and right head rotation in degrees.")]
        [SerializeField] private float _maximumHorizontalLookAngle = 70f;
        [Tooltip("Maximum up and down head rotation in degrees.")]
        [SerializeField] private float _maximumVerticalLookAngle = 35f;

        private Quaternion _initialLocalHeadRotation;
        private Transform _lookTarget;
        private Vector3 _lookPosition;
        private bool _hasLookPosition;

        private void Awake()
        {
            _initialLocalHeadRotation = _head.localRotation;
        }

        private void LateUpdate()
        {
            if (_lookTarget != null)
            {
                LookAtPosition(_lookTarget.position);
                return;
            }

            if (_hasLookPosition)
            {
                LookAtPosition(_lookPosition);
                return;
            }

            ReturnToInitialRotation();
        }

        public void SetLookTarget(Transform lookTarget)
        {
            _lookTarget = lookTarget;
            _hasLookPosition = false;
        }

        public void SetLookPosition(Vector3 lookPosition)
        {
            _lookPosition = lookPosition;
            _lookTarget = null;
            _hasLookPosition = true;
        }

        public void ClearLookTarget()
        {
            _lookTarget = null;
            _hasLookPosition = false;
        }

        private void LookAtPosition(Vector3 lookPosition)
        {
            Vector3 directionToLookPosition = lookPosition - _head.position;
            Quaternion worldLookRotation = Quaternion.LookRotation(directionToLookPosition, Vector3.up);
            Quaternion localLookRotation = Quaternion.Inverse(_body.rotation) * worldLookRotation;
            Vector3 localEulerAngles = localLookRotation.eulerAngles;

            float horizontalAngle = Mathf.Clamp(Mathf.DeltaAngle(0f, localEulerAngles.y), -_maximumHorizontalLookAngle, _maximumHorizontalLookAngle);
            float verticalAngle = Mathf.Clamp(Mathf.DeltaAngle(0f, localEulerAngles.x), -_maximumVerticalLookAngle, _maximumVerticalLookAngle);
            Quaternion targetLocalRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);

            _head.localRotation = Quaternion.Slerp(_head.localRotation, targetLocalRotation, _lookSpeed * Time.deltaTime);
        }

        private void ReturnToInitialRotation()
        {
            _head.localRotation = Quaternion.Slerp(_head.localRotation, _initialLocalHeadRotation, _lookSpeed * Time.deltaTime);
        }
    }
}
