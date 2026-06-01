using UnityEngine;

namespace Minecraft
{
    public class PickupAnimator : MonoBehaviour
    {
        [Tooltip("World position offset used for the floating motion.")]
        [SerializeField] private Vector3 _floatAmount = new Vector3(0f, 0.25f, 0f);
        [Tooltip("Speed of the floating motion.")]
        [SerializeField] private float _floatSpeed = 2f;
        [Tooltip("Local rotation speed in degrees per second.")]
        [SerializeField] private Vector3 _rotationSpeed = new Vector3(0f, 90f, 0f);

        private Vector3 _startingPosition;

        private void Awake()
        {
            _startingPosition = transform.position;
        }

        private void Update()
        {
            Float();
            Rotate();
        }

        private void Float()
        {
            transform.position = _startingPosition + _floatAmount * Mathf.Sin(Time.time * _floatSpeed);
        }

        private void Rotate()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
