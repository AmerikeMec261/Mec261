using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private float _maxDistance = 20f;

    [Header("Crosshair")]
    [SerializeField] private Transform _crosshair;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 target = hit.point;

            // mover retícula
            _crosshair.position = target;

            // YAW (horizontal)
            Vector3 flatDirection = target - _yawPivot.position;
            flatDirection.y = 0;

            _yawPivot.rotation = Quaternion.LookRotation(flatDirection);

            // PITCH (vertical)
            Vector3 fullDirection = target - _pitchPivot.position;
            float distance = flatDirection.magnitude;
            float height = fullDirection.y;

            float angle = CalculateHighAngle(distance, height, 20f);

            Vector3 angles = _pitchPivot.localEulerAngles;
            angles.x = -angle;
            _pitchPivot.localEulerAngles = angles;
        }
    }
    private float CalculateHighAngle(float distance, float height, float speed)
    {
        float g = Physics.gravity.magnitude;

        float speedSquared = speed * speed;
        float discriminant = (speedSquared * speedSquared) -
                             g * (g * distance * distance + 2 * height * speedSquared);

        if (discriminant < 0)
        {
            return 45f; // no hay solución, ángulo por defecto
        }

        float angle = Mathf.Atan(
            (speedSquared + Mathf.Sqrt(discriminant)) /
            (g * distance)
        );

        return angle * Mathf.Rad2Deg;
    }
}
