using UnityEngine;

public class tower : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _reticula;

    [Header("Municion")]
    [SerializeField] private GameObject[] _bulletPrefabs;

    [Header("Configuracion Mortero")]
    [SerializeField] private float _fuerzaMortero = 15f;
    [SerializeField] private float _maxRange = 1000f;
    [SerializeField] private LayerMask _capasApuntado;

    private void Update()
    {
        MoverReticulaAlMouse();
        ApuntarTorreta();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        int indiceAleatorio = Random.Range(0, _bulletPrefabs.Length);

        GameObject go = Instantiate(_bulletPrefabs[indiceAleatorio], _bulletSpawn.position, _bulletSpawn.rotation);

        Rigidbody rb = go.GetComponent<Rigidbody>();
        IProjectile proyectil = go.GetComponent<IProjectile>();

        if (rb != null && proyectil != null)
        {
            rb.useGravity = true;

            Vector3 velocidadFinal = CalcularMortero(_bulletSpawn.position, _reticula.position, _fuerzaMortero);

            rb.linearVelocity = velocidadFinal;

            proyectil.Fire();
        }
    }

    private Vector3 CalcularMortero(Vector3 origen, Vector3 destino, float v)
    {
        Vector3 diff = destino - origen;
        float x = new Vector2(diff.x, diff.z).magnitude;
        float y = diff.y;
        float g = Physics.gravity.magnitude;

        float v2 = v * v;
        float raiz = v2 * v2 - g * (g * x * x + 2 * y * v2);

        if (raiz < 0) return diff.normalized * v;

        float angulo = Mathf.Atan((v2 + Mathf.Sqrt(raiz)) / (g * x));

        Vector3 dir = diff;
        dir.y = 0;
        Vector3 velFinal = dir.normalized * v * Mathf.Cos(angulo);
        velFinal.y = v * Mathf.Sin(angulo);

        return velFinal;
    }

    private void MoverReticulaAlMouse()
    {
        Ray rayoMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayoMouse, out RaycastHit hit, _maxRange, _capasApuntado))
            _reticula.position = hit.point;
        else
            _reticula.position = rayoMouse.GetPoint(_maxRange / 10);
    }

    private void ApuntarTorreta()
    {
        Vector3 targetYaw = _reticula.position;
        targetYaw.y = _yawPivot.position.y;
        _yawPivot.LookAt(targetYaw);

        _pitchPivot.LookAt(_reticula.position);
    }
}