using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _target;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f); //si ya no se usa para qué sigue aqui? 

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Modes")]
    [SerializeField] private bool _autoAimEnabled = false; // Presiona 'T' para cambiar de modo

    [Header("Reticle")]
    [SerializeField] private Transform _reticle; //inglés

    [Header("Auto Settings")]
    [SerializeField] private float _autoRotationSpeed = 5f;

    [Header("Bullet")]
    [SerializeField] private GameObject _explosiveBullet;
    private bool _usingExplosive = false;
    private float _currentYaw;
    private float _currentPitch;

    private void Update()
    {
        // Alternar modo con la tecla T
        if (Input.GetKeyDown(KeyCode.T)) _autoAimEnabled = !_autoAimEnabled;

        if (_autoAimEnabled && _target != null)
        {
        
        }
        else
        {
            RotateMouse();
           
            if (Input.GetKeyDown(KeyCode.X))
            {
                _usingExplosive = !_usingExplosive;
                Debug.Log(_usingExplosive ? "Bala Explosiva" : "Bala Normal");
            }

        }

        // El disparo manual
        if (Input.GetKeyDown(KeyCode.Space)) FireProjectile();
    }  

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        IProjectile projectile = currentBullet.GetComponent<IProjectile>(); //la torreta no debe ser responsable de poner el daño, eso lo hace la bala, la torreta solo dispara, si quieres cambiar el daño de la bala hazlo en la bala no en la torreta.
        if (projectile != null)
        {
            projectile.SetDamage(20f);
            projectile.Fire();
        }
    }

    private void RotateMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        int layerMask = LayerMask.GetMask("Default");
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = hit.point;

            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion correctedRotation = targetRotation * Quaternion.Euler(0f, 100f, 0f);
                _yawPivot.rotation = Quaternion.Lerp(
                _yawPivot.rotation, correctedRotation, _yawSpeed * Time.deltaTime);

            }


            float horizontalDistance = direction.magnitude;
            float heightDifference = hit.point.y - _pitchPivot.position.y;
            float targetPitch = Mathf.Atan2(heightDifference, horizontalDistance) * Mathf.Rad2Deg;
            _currentPitch = Mathf.Clamp(targetPitch, _pitchLimits.x, _pitchLimits.y);
            _pitchPivot.localEulerAngles = new Vector3(_currentPitch, 0f, 0f);
         
            if (_reticle != null)
                _reticle.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
        }


    }

} //Trabajo en clase: usar la formula que vimos en clase.
