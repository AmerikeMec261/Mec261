using UnityEngine;

public class BasicTurret : MonoBehaviour
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
        if (_bulletPrefab == null || _bulletSpawn == null)
        {
            return;
        }

        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IBasicProjectile>()?.Fire();
    }

    private void Update()
    {
        float yawInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float pitchInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        RotateYaw(yawInput);
        RotatePitch(pitchInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void RotateYaw(float input)
    {
        float yawChange = input * _yawSpeed * Time.deltaTime;
        float currentYaw = NormalizeAngle(_yawPivot.localEulerAngles.y);
        float newYaw = Mathf.Clamp(currentYaw + yawChange, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3(
            _yawPivot.localEulerAngles.x,
            newYaw,
            _yawPivot.localEulerAngles.z
        );
    }

    private void RotatePitch(float input)
    {

        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float currentPitch = NormalizeAngle(_pitchPivot.localEulerAngles.x);
        float newPitch = Mathf.Clamp(currentPitch - pitchChange, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(
            newPitch,
            _pitchPivot.localEulerAngles.y,
            _pitchPivot.localEulerAngles.z
        );
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}
