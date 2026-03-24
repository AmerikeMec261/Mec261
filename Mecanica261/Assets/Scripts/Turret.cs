using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header ("YawSettings")]
    [SerializeField ] private float _yawSpeed = 90f;
    [SerializeField ] private Vector2 _yawLimits = new Vector2 (-90f, 90f);

    [Header("PichSettings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();

    }

    private void Update()
    {
        float yawInput = Input.GetKey(KeyCode.A) ? -1f :
                 Input.GetKey(KeyCode.D) ? 1f : 0f;

        float pitchInput = Input.GetKey(KeyCode.W) ? 1f :
                           Input.GetKey(KeyCode.S) ? -1f : 0f;

        RotateYaw(yawInput);
        RotationPitch(pitchInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private float currentYaw = 0f;

    private void RotateYaw(float input)
    {
        currentYaw += input * _yawSpeed * Time.deltaTime;
        currentYaw = Mathf.Clamp(currentYaw, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3(0f, currentYaw, 0f);
    }

    private void RotationPitch(float input)
    {
        float pitchChange = input* _pitchSpeed * Time.deltaTime;
        float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.z + pitchChange, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);

    }

}

