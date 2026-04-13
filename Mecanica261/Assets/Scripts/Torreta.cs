
using UnityEngine;

public class Torreta : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _normalBullet;
    [SerializeField] private GameObject _explosiveBullet;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);
    

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Reticle")]
    [SerializeField] private GameObject _reticle;
    [SerializeField] private float _maxRange = 50f;
    [SerializeField] private LayerMask _ground;

    private GameObject _currentBullet;
    private GameObject _reticleInstance;
    private float _currentPitch = 0f;
    private Vector3 _targetPoint;

    private void Start()
    {
        _currentBullet=_normalBullet;
        if (_reticle != null)
            _reticleInstance=Instantiate(_reticle); // porqué instancias la reticula?  Debería estar en la escena y solo moverlo.  
    }

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_currentBullet, _bulletSpawn.position, _bulletSpawn.rotation);
        IProjectile projectile = currentBullet.GetComponent<IProjectile>();

        if (projectile == null) return;

        Vector3 flat = new Vector3(_targetPoint.x - _bulletSpawn.position.x, 0f, _targetPoint.z - _bulletSpawn.position.z);
        float t = flat.magnitude / projectile.Speed; // No Usar abreaviaciones

        Vector3 velocity = flat.normalized * projectile.Speed;
        velocity.y = (_targetPoint.y - _bulletSpawn.position.y) / t + 0.5f * Mathf.Abs(Physics.gravity.y) * t; // No es la formula completa que vimos en clase

        projectile.Fire(velocity);



    }

    private void Update()
    {
        RotateMouse();
        UpdateReticle();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            _currentBullet=_currentBullet==_normalBullet?_explosiveBullet:_normalBullet;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void RotateYaw(float input)
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
    }


    private void RotateMouse()
    {
        Vector3 yawDirection = _targetPoint - _yawPivot.position;
        yawDirection.y = 0f;

        if (yawDirection != Vector3.zero)
        {
            Quaternion targetYaw = Quaternion.LookRotation(yawDirection);
            _yawPivot.rotation = Quaternion.Lerp(_yawPivot.rotation, targetYaw, _yawSpeed * Time.deltaTime);
        }

        Vector3 localTarget = _pitchPivot.InverseTransformPoint(_targetPoint); 
        float targetPitch = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
        _currentPitch = Mathf.Clamp(targetPitch, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localRotation = Quaternion.Lerp( // porqué los saltos de línea?  Esto suele ser residuo de GPT.
            _pitchPivot.localRotation,
            Quaternion.Euler(_currentPitch, 0f, 0f),
            _pitchSpeed * Time.deltaTime);
    }

    private void UpdateReticle()
    {
        if (_reticleInstance == null) return;

        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out RaycastHit hit, _maxRange,_ground))
        {
            _targetPoint = hit.point;
            _reticleInstance.transform.position = _targetPoint + Vector3.up * 0.02f;
            _reticleInstance.transform.rotation = Quaternion.Euler(90f,0f,0f);
        }
    }
}  //Trabajo en clase: Usar la formula completa que vimos en clase y usarla para el pitch de la torreta. Poner enemigos a diferentes alturas y hacer que repitan su ruta. 