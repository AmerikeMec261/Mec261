using UnityEngine;

namespace Project.TurretSystem
{
    public class TurretInjector : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Turret _turret;
        [SerializeField] private Transform _target;

        #endregion Variables

        #region Unity Methods

        public void Fire(Vector3 targetPosition)
        {
            GameObject bullet = Instantiate(
                _bulletPrefab,
                _bulletSpawn.position,
                Quaternion.identity
            );

            if (!bullet.TryGetComponent<Rigidbody>(out Rigidbody rb)) return;

            rb.useGravity = true;

            Vector3 direction = targetPosition - _bulletSpawn.position;

            float height = direction.y;
            direction.y = 0;

            float distance = direction.magnitude;

            float gravity = Physics.gravity.y;

            float angle = 45f * Mathf.Deg2Rad;

            float velocity = Mathf.Sqrt(distance * -gravity / Mathf.Sin(2 * angle));

            Vector3 velocityY = Vector3.up * velocity * Mathf.Sin(angle);
            Vector3 velocityX = direction.normalized * velocity * Mathf.Cos(angle);

            rb.linearVelocity = velocityX + velocityY;
        }

        #endregion Unity Methods
    }
}
