using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{

    // Esta sección de water se encarga de determinar las caracteristicas del agua como el nivel, la densidad y el arrastre de la misma
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 0.1f;


    // La seccion llamada Hull se encarga de detectar los puntos de flotación del barco para que la flotabilidad funcione
    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>();

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake()
    {
        //En el awake definimos el rigidbody y después llamamos el CalculateHullData
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()
    {
        ApplyBuoyancy();
    }

    private void ApplyBuoyancy()
    {
        //En el metodo de apply bouyancy, lo que se hace es aplicando la gravedad de Unity y tomando en cuenta los puntos de flotabilidad del barco, y aplicando la fuerza para la flotabilidad
        float gravityStrength = Physics.gravity.magnitude;
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Transform buoyancyPoint = _buoyancyPoints[i];

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);
            if (submergedAmount <= 0f) { continue; }

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force);

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force);
        }
    }

    private void CalculateHullData()
    {
        //Este metodo lo que hace es que se encarga de obtener el area y el volumen del barco
        _area = CalculateHullArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }

    private float CalculateHullArea()
    {
        //Este metodo se encarga de calcular el area del barco
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        //Este metodo se encarga de agregar color a los puntos de flotacion, para que sean facilmente aplicables

        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;

            Gizmos.DrawLine(currentPoint, nextPoint);
        }
    }
}