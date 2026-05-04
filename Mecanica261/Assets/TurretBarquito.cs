using UnityEngine;

public class TurretBarquito : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _turretBase;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private float _maxRotationAngle = 90f;

    #endregion Variables


    #region Unity Methods

    private void Update()
    {
        RotateTurret();
    }

    #endregion Unity Methods


    #region Methods

    private void RotateTurret()
    {
        if (_target == null || _turretBase == null) { return; }

        Vector3 direction = _target.position - _turretBase.position;
        direction.y = 0f;

        if (direction == Vector3.zero) { return; }

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float currentAngle = _turretBase.localEulerAngles.y;

        float smoothAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            _rotationSpeed * Time.deltaTime
        );

        float normalizedAngle = NormalizeAngle(smoothAngle);

        float clampedAngle = Mathf.Clamp(
            normalizedAngle,
            -_maxRotationAngle,
            _maxRotationAngle
        );

        _turretBase.localRotation = Quaternion.Euler(0f, clampedAngle, 0f);
    }


    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }

    #endregion Methods
}
