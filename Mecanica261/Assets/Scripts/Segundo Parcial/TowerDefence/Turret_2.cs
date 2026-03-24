using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Turret_2 : MonoBehaviour
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

    [Header("Auto Aim")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _projectileSpeed = 20f;
    [SerializeField] private bool _useHighArc = false;

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();
    }

    private void Update()
    {
        if (_target != null)
        {
            AutoAim();
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

        _yawPivot.localEulerAngles = new Vector3(0f, newYaw, 0f);
    }

    private void RotatePitch(float input)
    {
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.x + pitchChange, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(newPitch, 0f, 0f);
    }

    private bool SolveBallisticAngle(Vector3 origin, Vector3 target, float speed, out float angle)
    {
        float g = Physics.gravity.magnitude;

        Vector3 flat = new Vector3(target.x - origin.x, 0, target.z - origin.z);
        float R = flat.magnitude;

        float inside = (R * g) / (speed * speed);

        if (inside > 1f)
        {
            angle = 0f;
            return false;
        }

        float asin = Mathf.Asin(inside);

        float low = asin * 0.5f;
        float high = (Mathf.PI - asin) * 0.5f;

        angle = _useHighArc ? high : low;

        return true;
    }

    private void AutoAim()
    {
        Vector3 targetPos = _target.position;

        // YAW
        Vector3 dir = targetPos - _yawPivot.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            float clampedYaw = ClampAngle(targetRot.eulerAngles.y, _yawLimits.x, _yawLimits.y);

            _yawPivot.localEulerAngles = new Vector3(0f, clampedYaw, 0f);
        }

        // PITCH
        if (SolveBallisticAngle(_bulletSpawn.position, targetPos, _projectileSpeed, out float angle))
        {
            float pitchDeg = angle * Mathf.Rad2Deg;
            float clampedPitch = Mathf.Clamp(pitchDeg, _pitchLimits.x, _pitchLimits.y);
            _pitchPivot.localEulerAngles = new Vector3(-clampedPitch, 0f, 0f);
        }
    }
    private float ClampAngle(float angle, float min, float max)
    {
        angle = NormalizeAngle(angle);
        return Mathf.Clamp(angle, min, max);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
