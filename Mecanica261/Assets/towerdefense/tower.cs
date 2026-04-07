using UnityEngine;

public class tower : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Camera _camara;
    [SerializeField] private Transform _reticula;

    [Header("Configuracion Yaw")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Configuracion Pitch")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Reticula")]
    [SerializeField] private float _maxRange = 30f;

    public void FireProjectile()
    {
        GameObject bala = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        bala.GetComponent<IProjectile>()?.Fire();
    }

    public void CambiarPrefab(GameObject nuevoPrefab)
    {
        _bulletPrefab = nuevoPrefab;
    }

    private void Update()
    {
        SeguirMouse();
        ActualizarReticula();

        if (Input.GetKeyDown(KeyCode.Space))
            FireProjectile();
    }

    private void SeguirMouse()
    {
        Ray rayo = _camara.ScreenPointToRay(Input.mousePosition);
        Plane piso = new Plane(Vector3.up, Vector3.zero);

        if (!piso.Raycast(rayo, out float distancia))
            return;

        Vector3 puntoMouse = rayo.GetPoint(distancia);

        Vector3 direccion = puntoMouse - _yawPivot.position;
        direccion.y = 0f;

        float anguloYaw = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg;
        anguloYaw = Mathf.Clamp(anguloYaw, _yawLimits.x, _yawLimits.y);
        _yawPivot.localEulerAngles = new Vector3(0f, anguloYaw, 0f);

        float distanciaH = direccion.magnitude;
        float alturaY = _bulletSpawn.position.y - puntoMouse.y;

        float anguloPitch = Mathf.Atan2(alturaY, distanciaH) * Mathf.Rad2Deg;
        anguloPitch = Mathf.Clamp(anguloPitch, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(anguloPitch, 0f, 0f);
    }

    private void ActualizarReticula()
    {
        Ray rayo = new Ray(_bulletSpawn.position, _bulletSpawn.forward);

        if (Physics.Raycast(rayo, out RaycastHit golpe, _maxRange))
            _reticula.position = golpe.point;
        else
            _reticula.position = _bulletSpawn.position + _bulletSpawn.forward * _maxRange;
    }
}