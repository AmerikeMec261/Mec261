using UnityEngine;

public class TurretAiming : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _barrel;

    [Header("Settings")]
    [Tooltip("Velocidad inicial de la bala")]
    [SerializeField] private float _bulletSpeed = 50f;

    [Tooltip("Usar ángulo alto o bajo")]
    [SerializeField] private bool _useHighAngle = false;

    [Tooltip("Velocidad de rotación vertical")]
    [SerializeField] private float _elevationSpeed = 5f;

    private float _currentAngle;

    #endregion Variables

    #region Unity Methods

    private void Update()
    {
        AimBarrel();
    }

    #endregion Unity Methods

    #region Methods

    private void AimBarrel()
    {
        if (_target == null || _barrel == null) { return; }

        Vector3 direction = _target.position - _barrel.position;

        float x = new Vector2(direction.x, direction.z).magnitude;
        float y = direction.y;

        float g = Mathf.Abs(Physics.gravity.y);
        float v2 = _bulletSpeed * _bulletSpeed;

        float discriminant = (v2 * v2) - g * (g * x * x + 2 * y * v2);

        if (discriminant < 0f) { return; } // no hay solución

        float sqrt = Mathf.Sqrt(discriminant);

        float angle;

        if (_useHighAngle)
        {
            angle = Mathf.Atan((v2 + sqrt) / (g * x));
        }
        else
        {
            angle = Mathf.Atan((v2 - sqrt) / (g * x));
        }

        float angleDegrees = angle * Mathf.Rad2Deg;

        _currentAngle = Mathf.Lerp(
            _currentAngle,
            angleDegrees,
            Time.deltaTime * _elevationSpeed
        );

        _barrel.localRotation = Quaternion.Euler(-_currentAngle, 0f, 0f);
    }

    #endregion Methods
}
