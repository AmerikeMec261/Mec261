using UnityEngine;

public class Turret : MonoBehaviour
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

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();
    }

    private void Update()
    {
        float yawInput = Input.GetKeyDown(KeyCode.A) ? -1f : Input.GetKeyDown(KeyCode.D) ? 1f : 0f;
        float pitchInput = Input.GetKeyDown(KeyCode.W) ? 1f : Input.GetKeyDown(KeyCode.S) ? -1f : 0f;

       AimWithMouse();

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

    private void AimWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 target = hit.point;
            Vector3 direction = target - _yawPivot.position;
            direction.y = 0;

            Quaternion yawRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
            _yawPivot.rotation = yawRotation;

            Vector3 fullDir = target - _pitchPivot.position;
            Quaternion pitchRotation = Quaternion.LookRotation(fullDir) * Quaternion.Euler(0, -90, 0);
            _pitchPivot.rotation = pitchRotation;
        }
        /*
        private void Formula()
    {
        public float initialvelocity = 20f;

        public float gravity = 9.81;

        public transform spawnturret;

        public transform objective;

     float distance x = vector3.Distance(new Vector3 (TurretSpawn.position.x, 0, turretSpawn.position.z));
        */

    }
    }
// Trabajo en clase: usar la formula que vimos en clase para determinar el ángulo de disaparo. Resolver los comentarios.