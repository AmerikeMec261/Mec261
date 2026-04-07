using UnityEngine;
using System.Collections.Generic;
public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yamPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _reticle;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("AimSettings")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _maxAimDistance = 40f;

    private float _currentYaw;
    private float _currentPitch;
    private Vector3 _aimPoint;
    private bool _hasBallisticSolution;


    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        IProjectile projectile=currentBullet.GetComponent<IProjectile>();

        if (projectile == null)
        {
            return;
        }

        Vector3 launchVelocity = _bulletSpawn.forward * projectile.Speed;
        projectile.Fire(launchVelocity);
    }

    private void Update()
    {
        UpdateAimPointFromMouse();
        

        if (Input.GetKeyDown(KeyCode.Space) && _hasBallisticSolution)
        {
            FireProjectile();
        }
    }

    private void UpdateAimPointFromMouse()
    {
        if (_camera==null)
        {
            return;
        }

        Ray ray=_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500f, _groundMask))
        {
            _aimPoint = hit.point;
        }
        else
        {
            _aimPoint = _bulletSpawn.position + _yamPivot.forward * _maxAimDistance;
        }
    }

    private void RotateYaw(float input)
    {
        float yawChange=input*_yawSpeed*Time.deltaTime;
        float newYaw = Mathf.Clamp(_yamPivot.localEulerAngles.y + yawChange, _yawLimits.x, _yawLimits.y);
        _yamPivot.localEulerAngles = new Vector3(_yamPivot.localEulerAngles.x, newYaw, _yamPivot.localEulerAngles.z);
    }

    private void RotatePitch(float input)
    {
       float pitchChange=input*_pitchSpeed*Time.deltaTime;
       float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.z + pitchChange, _pitchLimits.x, _pitchLimits.y);
       _pitchPivot.localEulerAngles=new Vector3(_pitchPivot.localEulerAngles.x,_pitchPivot.localEulerAngles.y,newPitch); 


    }
}