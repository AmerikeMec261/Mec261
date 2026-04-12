using UnityEngine;
using UnityEngine.Rendering;

public class Tower : MonoBehaviour // La torreta ya no debe de funcionar con A y W. Debe seguir a la posición del mouse. 
{
    [Header("Dependences")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-90f, 90f);

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<SimpleBullet>()?.Fire(_bulletSpawn);
    }

    private void Update()
    {
        float yawInput = Input.GetKeyDown(KeyCode.A) ? -5f : Input.GetKeyDown(KeyCode.D) ? 5f : 0f;
        float pitchInput = Input.GetKeyDown(KeyCode.W) ? 5f : Input.GetKeyDown(KeyCode.S) ? -5f : 0f;

        RotateYaw(yawInput);
        RotatePitch(pitchInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
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
} //Trabajo en clase: Agregar lo solicitado para el exámen. 