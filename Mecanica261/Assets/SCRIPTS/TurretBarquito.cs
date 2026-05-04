using UnityEngine;

public class TurretBarquito : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _turretBase;

    [Header("Settings")]
    [Tooltip("Velocidad de rotación de la torreta")]
    [SerializeField] private float _rotationSpeed = 5f;

    [Tooltip("Ángulo máximo hacia la derecha")]
    [SerializeField] private float _maxRightAngle = 60f;

    [Tooltip("Ángulo máximo hacia la izquierda")]
    [SerializeField] private float _maxLeftAngle = -60f;

    private float _currentAngle;

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

        float smoothAngle = Mathf.LerpAngle(
            _turretBase.eulerAngles.y,
            targetAngle,
            Time.deltaTime * _rotationSpeed
        );

        float clampedAngle = ClampAngle(smoothAngle);

        _turretBase.rotation = Quaternion.Euler(0f, clampedAngle, 0f);
    }

    private float ClampAngle(float angle)
    {
        float normalizedAngle = NormalizeAngle(angle);
        return Mathf.Clamp(normalizedAngle, _maxLeftAngle, _maxRightAngle);
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