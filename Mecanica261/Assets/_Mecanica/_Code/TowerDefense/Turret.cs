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

    private void Update()
    {
        float yawInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float pitchInput = Input.GetKey(KeyCode.W) ? -1f : Input.GetKey(KeyCode.S) ? 1f : 0f;

        RotateYaw(yawInput);
        RotatePitch(pitchInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    public void AimAtTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - _yawPivot.position;
        float targetYaw = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        YawTurret(targetYaw);
        PitchTurret(CalculateFireAngle(Vector3.Distance(transform.position, targetPosition)));
    }

    private void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();
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

    private void YawTurret(float angleToRotateTo)
    {
        float currentYaw = _yawPivot.localEulerAngles.y;
        float targetYaw = Mathf.Clamp(angleToRotateTo, _yawLimits.x, _yawLimits.y);
        float newYaw = Mathf.MoveTowardsAngle(currentYaw, targetYaw, _yawSpeed * Time.deltaTime);
        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }

    private void PitchTurret(float angleToRotateTo)
    {
        float currentPitch = _pitchPivot.localEulerAngles.x;
        float targetPitch = Mathf.Clamp(angleToRotateTo, _pitchLimits.x, _pitchLimits.y);
        float newPitch = Mathf.MoveTowardsAngle(currentPitch, targetPitch, _pitchSpeed * Time.deltaTime);
        _pitchPivot.localEulerAngles = new Vector3(newPitch, _pitchPivot.localEulerAngles.y, _pitchPivot.localEulerAngles.z);
    }

    private float CalculateFireAngle(float distanceToTarget)
    {
        float angleOne = (Mathf.Asin( ( (9.81f * distanceToTarget) / Mathf.Pow(_bulletPrefab.GetComponent<IProjectile>().GetSpeed(), 2))) * Mathf.Rad2Deg)/ 2;
        float angleTwo = (180 - Mathf.Asin(((9.81f * distanceToTarget) / Mathf.Pow(_bulletPrefab.GetComponent<IProjectile>().GetSpeed(), 2))) * Mathf.Rad2Deg) / 2;

        Debug.Log($"Angle One: {angleOne}, Angle Two: {angleTwo}");

        if (angleOne > angleTwo)
        {
            return angleOne;
        }
        else
        {
            return angleTwo;
        }
    }

}
