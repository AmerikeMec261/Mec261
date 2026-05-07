using UnityEngine;

public class TurretBarquito : MonoBehaviour
{
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private float _bulletForce = 50f;
    [SerializeField] private float _maxYaw = 90f;

    void Update()
    
    {
        Aim();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Aim()
    
    {
        if (_target == null) return;
        Vector3 direccion = _target.position - _yawPivot.position;
        float yaw = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg;
        yaw = Mathf.Clamp(yaw, - _maxYaw,_maxYaw);
        _yawPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
        float distance = Vector3.Distance(_bulletSpawn.position,_target.position);
        float pitch = distance * 1f;
        _pitchPivot.localRotation = Quaternion.Euler(- pitch, 0f, 0f);
    }
    
    private void Shoot()
    
    {
        if (_bulletPrefab == null) return;
        if (_target == null) return;
        GameObject bullet = Instantiate(_bulletPrefab,_bulletSpawn.position,Quaternion.identity);
        Rigidbody _rigidbody = bullet.GetComponent<Rigidbody>();

        if (_rigidbody != null)
        {
            Vector3 direccion = _target.position - _bulletSpawn.position;
            _rigidbody.AddForce(direccion.normalized * _bulletForce,ForceMode.Impulse);
        }
    }
}
