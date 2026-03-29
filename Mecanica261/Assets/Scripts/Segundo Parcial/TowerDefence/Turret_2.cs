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

    private void AutoAim()
    {
        Vector3 origin = _bulletSpawn.position;
        Vector3 targetPos = _target.position;

        // yaw
        Vector3 dir = targetPos - _yawPivot.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            float yaw = ClampAngle(targetRot.eulerAngles.y, _yawLimits.x, _yawLimits.y);
            _yawPivot.localEulerAngles = new Vector3(0f, yaw, 0f);
        }

        // PITCH (TIRO PARABOLICO)
        if (SolveBallisticAngleFull(origin, targetPos, _projectileSpeed, out float angle))
        {
            float angleDeg = angle * Mathf.Rad2Deg;
            float clampedPitch = Mathf.Clamp(angleDeg, _pitchLimits.x, _pitchLimits.y);
            Vector3 current = _yawPivot.localEulerAngles;
            _yawPivot.localEulerAngles = new Vector3(-clampedPitch, current.y, 0f);
        }
    }

    //ANGULO DE TIRO HORIZONTAL
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

    //TIRO PARABOLICO CON ALTURA DE OBJETIVO
    private bool SolveBallisticAngleFull(Vector3 origin, Vector3 target, float speed, out float angle)
    {
        float g = Physics.gravity.magnitude;

        Vector3 flat = new Vector3(target.x - origin.x, 0, target.z - origin.z);
        float x = flat.magnitude;

        float deltaY = target.y - origin.y;

        float v2 = speed * speed;
        float v4 = v2 * v2;

        float inside = v4 - g * (g * x * x + 2 * deltaY * v2);

        if (inside < 0f)
        {
            angle = 0f;
            return false;
        }

        float sqrt = Mathf.Sqrt(inside);

        float low = Mathf.Atan((v2 - sqrt) / (g * x));
        float high = Mathf.Atan((v2 + sqrt) / (g * x));

        angle = _useHighArc ? high : low;

        return true;
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
