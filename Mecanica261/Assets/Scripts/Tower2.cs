using UnityEngine;

public class Tower1 : MonoBehaviour
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

    [Header("Reticula")]
    [SerializeField] private Transform _reticula;

    [Header("Bullet")]
    [SerializeField] private GameObject _explosivePrefab;
    private bool _usingExplosive = false;
    //private float _currentYaw = 0f;
    private float _currentPitch = 0f;

    public void FireProjectile()
    {
        GameObject prefabToSpawn = _usingExplosive ? _explosivePrefab : _bulletPrefab;
        if (prefabToSpawn == null)
        {
            Debug.Log("Prefab No Agsinado");
            return;
        }
        GameObject currentBullet = Instantiate(prefabToSpawn, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red, 2f);
    }

    private void Update()
    {
        RotateMouse();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _usingExplosive = !_usingExplosive;
            Debug.Log(_usingExplosive ? "Bala Explosiva" : "Bala Normal");
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


    private void RotateMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        int layerMask = LayerMask.GetMask("Default");
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = hit.point;

            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion correctedRotation = targetRotation * Quaternion.Euler(0f, 180f, 0f);
                _yawPivot.rotation = Quaternion.Lerp(
                    _yawPivot.rotation, correctedRotation, _yawSpeed * Time.deltaTime);



            }


            float horizontalDistance = direction.magnitude;
            float heightDifference = hit.point.y - _pitchPivot.position.y;
            float targetPitch = Mathf.Atan2(heightDifference, horizontalDistance) * Mathf.Rad2Deg;
            _currentPitch = Mathf.Clamp(targetPitch, _pitchLimits.x, _pitchLimits.y);
            _pitchPivot.localEulerAngles = new Vector3(_currentPitch, 0f, 0f);


            //int layerMask = -LayerMask.GetMask("Ignorado");
            // if (Physics.Raycast(ray, out hit , Mathf.Infinity , layerMask))
            // {
            //    Debug.Log($"Raycast golpeó : {hit.collider.name}");

            //  }
            // else
            //{
            //       Debug.Log("No golpeo nada");
            // }


            if (_reticula != null)
                _reticula.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
        }


    }
}