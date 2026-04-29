using UnityEngine;
using UnityEngine.InputSystem;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]

    [SerializeField] private Transform _yawPivot;

    [SerializeField] private Transform _pitchPivot;

    [SerializeField] private Transform _bulletSpawn;

    [SerializeField] private GameObject _bulletPrefab;

    [Header("Ballistics Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _projectileSpeed = 20f;
    [SerializeField] private bool _useHighArc = false;


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
        if (_target != null)
        {
            if (SolveBallisticAngle(_bulletSpawn.position, _target.position, _projectileSpeed, out float angle))
            {
                float targetPitch = -angle * Mathf.Rad2Deg;

                Vector3 currentRotation = _pitchPivot.localEulerAngles;
                _pitchPivot.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, targetPitch);
            }
        }

        float yawInput = 0f;
        if (Keyboard.current.aKey.isPressed) yawInput = -1f;
        else if (Keyboard.current.dKey.isPressed) yawInput = 1f;

        float pitchInput = 0f;
        if (Keyboard.current.wKey.isPressed) pitchInput = 1f;
        else if (Keyboard.current.sKey.isPressed) pitchInput = -1f;

        RotateYaw(yawInput);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireProjectile();
        }
    }

    private bool SolveBallisticAngle(Vector3 originPosition, Vector3 targetPosition, float _projectileSpeed, out float launchAngle)
    {
        float gravity = Physics.gravity.magnitude;

        Vector3 diff = targetPosition - originPosition;
        Vector3 horizontalDiff = new Vector3(diff.x, 0f, diff.z);
        float x = horizontalDiff.magnitude;

        float y = diff.y;

        float v = _projectileSpeed;
        float v2 = v * v;
        float v4 = v2 * v2;

        float discriminant = v4 - gravity * (gravity * (x * x) + 2 * y * v2);

        if (discriminant < 0f )
        {
            launchAngle = 0f;
            return false;
        }

        float root = Mathf.Sqrt(discriminant);

        float lowAngle = Mathf.Atan((v2 - root) / (gravity * x));
        float highAngle = Mathf.Atan((v2 + root) / (gravity * x));

        launchAngle = _useHighArc ? highAngle : lowAngle;
        return true;
    }

    private void RotateYaw(float input)

    {

        float yawChange = input * _yawSpeed * Time.deltaTime;

        float newYaw = Mathf.Clamp(_yawPivot.localEulerAngles.y + yawChange, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles. z);

    }


    private void RotatePitch(float input)

    {

        float pitchChange = input * _pitchSpeed * Time.deltaTime;

     float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.z + pitchChange, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);

    }   
}// Trabajo en clase: usar la formula que vimos en clase para calcular el ángulo de disparo. 