using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yamPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;

    [Header("Balas")]
    [SerializeField] private GameObject balaSimple;
    [SerializeField] private GameObject balaExplosiva;

    private GameObject balaActual;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    private void Start()
    {
        balaActual = balaSimple;
    }

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(balaActual, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();
    }

    private void Update()
    {
        float yawInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float pitchInput = Input.GetKey(KeyCode.W) ? -1f : Input.GetKey(KeyCode.S) ? 1f : 0f;

        RotateYaw(yawInput);
        RotatePitch(pitchInput);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            balaActual = balaSimple;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            balaActual = balaExplosiva;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void RotateYaw(float input)
    {
        float yawChange = input * _yawSpeed * Time.deltaTime;
        float newYaw = Mathf.Clamp(_yamPivot.localEulerAngles.y + yawChange, _yawLimits.x, _yawLimits.y);
        _yamPivot.localEulerAngles = new Vector3(_yamPivot.localEulerAngles.x, newYaw, _yamPivot.localEulerAngles.z);
    }

    private void RotatePitch(float input)
    {
        float pitchActual = _pitchPivot.localEulerAngles.z;

        if (pitchActual > 180f)
        {
            pitchActual -= 360f;
        }

        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float newPitch = Mathf.Clamp(pitchActual + pitchChange, _pitchLimits.x, _pitchLimits.y);

        if (newPitch < 0f)
        {
            newPitch += 360f;
        }

        _pitchPivot.localEulerAngles = new Vector3(
            _pitchPivot.localEulerAngles.x,
            _pitchPivot.localEulerAngles.y,
            newPitch
        );
    }
}