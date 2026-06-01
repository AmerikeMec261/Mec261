using UnityEngine;

namespace Minecraft
{
    public class PickupAnimator : MonoBehaviour
    {
        [SerializeField] private Vector3 _floatAmount = new Vector3(0f, 0.25f, 0f);
        [SerializeField] private float _floatSpeed = 2f;
        [SerializeField] private Vector3 _rotationSpeed = new Vector3(0f, 90f, 0f);

        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            Float();
            Rotate();
        }

        private void Float()
        {
            transform.position = _startPosition + _floatAmount * Mathf.Sin(Time.time * _floatSpeed);
        }

        private void Rotate()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
