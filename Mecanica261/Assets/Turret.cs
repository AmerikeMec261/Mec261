using UnityEngine;

namespace Project.TurretSystem
{
    public class Turret : MonoBehaviour
    {
        #region Variables

        [Header("Dependencies")]
        [SerializeField] private Transform _yawPivot;
        [SerializeField] private Transform _pitchPivot;
        [SerializeField] private Transform _bulletSpawn;
        [SerializeField] private GameObject _bulletPrefab;

        [Header("Settings")]
        [SerializeField] private float _bulletSpeed = 20f;

        #endregion Variables

        #region Methods

       
        public void SetRotation(float yaw, float pitch)
        {
            _yawPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
            _pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        
        public float CalculateLaunchAngle(float distance)
        {
            float gravity = Physics.gravity.magnitude;

            float value = (distance * gravity) / (_bulletSpeed * _bulletSpeed);

            
            if (value > 1f)
            {
                Debug.LogWarning("No hay solución para ese disparo");
                return 45f;
            }

            float angle = Mathf.Asin(value) * Mathf.Rad2Deg * 0.5f;

            return angle;
        }

        
        public void AimAt(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _yawPivot.position;

            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float distance = new Vector2(direction.x, direction.z).magnitude;

            float pitch = CalculateLaunchAngle(distance);

            SetRotation(yaw, pitch);
        }

        public void Fire(Vector3 targetPosition)
        {
            GameObject bullet = Instantiate(
                _bulletPrefab,
                _bulletSpawn.position,
                Quaternion.identity
            );

            if (!bullet.TryGetComponent<Rigidbody>(out Rigidbody rb)) return;

            
            Vector3 direction = (targetPosition - _bulletSpawn.position).normalized;

            rb.linearVelocity = direction * _bulletSpeed;
        }

        #endregion Methods
    }
}
