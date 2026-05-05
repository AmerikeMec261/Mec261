using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _normalBulletPrefab;
    [SerializeField] private GameObject _explosiveBulletPrefab;

    [Header("Settings")]
    [Tooltip("Fuerza de disparo")]
    [SerializeField] private float _shootForce = 2000f;

    [Tooltip("Tiempo entre disparos")]
    [SerializeField] private float _fireRate = 1f;

    private float _currentCooldown;

    #endregion Variables

    #region Unity Methods

    private void Update()
    {
        HandleShooting();
    }

    #endregion Unity Methods

    #region Methods

    private void HandleShooting()
    {
        if (_shootPoint == null) { return; }

        _currentCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            TryShoot(_normalBulletPrefab);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            TryShoot(_explosiveBulletPrefab);
        }
    }

    private void TryShoot(GameObject bulletPrefab)
    {
        if (_currentCooldown > 0f) { return; }
        if (bulletPrefab == null) { return; }

        Shoot(bulletPrefab);
        _currentCooldown = _fireRate;
    }

    private void Shoot(GameObject bulletPrefab)
    {
        GameObject bulletInstance = Instantiate(
            bulletPrefab,
            _shootPoint.position,
            _shootPoint.rotation
        );

        Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();

        if (bulletRigidbody == null) { return; }

        bulletRigidbody.AddForce(
            _shootPoint.forward * _shootForce,
            ForceMode.Impulse
        );
    }

    #endregion Methods
}