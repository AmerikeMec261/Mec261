using UnityEngine;


public class Tower2 : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;


    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);


    [Header("Bullet")]
    [SerializeField] private GameObject _explosivePrefab;
    private bool _usingExplosive = false;
    private float _currentYaw = 0f;
    private float _currentPitch = 0f;

    [Header("Turret")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _turret;



    public void FireProjectile()
    {
        GameObject prefabToSpawn = _usingExplosive ? _explosivePrefab : _bulletPrefab;
        if (prefabToSpawn == null)
        {
            Debug.Log("Prefab No Agsinado");
            return;
        }
        GameObject currentBullet = Instantiate(prefabToSpawn, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red, 2f);
    }

    private void Rotation()
    {
        Vector3 direction = _target.position - _turret.position;
        direction.y = 0f;

        float angle = Mathf.Atan2(  direction.x, direction.z) * Mathf.Rad2Deg;

        _yawPivot.localEulerAngles = new Vector3(0f, angle - 90f , 0f);
    }
    private void Update()
    {
        //RotateMouse();
        Rotation();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _usingExplosive = !_usingExplosive;
            Debug.Log(_usingExplosive ? "Bala Explosiva" : "Bala Normal");
        }
    }

    /*private void RotateYaw(float input)
    {
        float yawChange = input * _yawSpeed * Time.deltaTime;
        float newYaw = Mathf.Clamp(_yawPivot.localEulerAngles.y + yawChange, _yawLimits.x, _yawLimits.y);
        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }

    private void RotatePitch(float input)
    {
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.z + pitchChange, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);
    }*/


}