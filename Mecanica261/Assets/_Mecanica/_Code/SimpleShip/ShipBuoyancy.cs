using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f; // valor para el nivel del agua
    [SerializeField] private float _waterDensity = 1000f; // valor para la densidad del agua
    [SerializeField] private float _waterDrag = 0.1f; // el valor del jalado del agua

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f; // calcula el volumen como si fuera un cubo omitiendo las partes que no son del barco
    [SerializeField] private Transform _topPoint; // punto de arriba
    [SerializeField] private Transform _bottomPoint; // punto de hasta abajo
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>(); //pa poner varios puntos para que simule bien el movimiento del agua (entre mas mejor)

    private Rigidbody _rigidbody;

    private float _area; // valor del area
    private float _hullHeight; // valor dde la altura del casco
    private float _hullVolume; // valor del volumen del casco
    private float _draft; // que tanto derrapa

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // llama al rigid para que se use si o si al inicio de todo
        CalculateHullData(); // llama al ese coso pa que se ejecute de primeras
    }

    private void FixedUpdate()
    {
        ApplyBuoyancy(); // va actualizando los valores del ese constantemente
    }

    private void ApplyBuoyancy()
    {
        float gravityStrength = Physics.gravity.magnitude; // es la gravedad
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count; // volumen del casco por todos los puntos que le pusiste

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Transform buoyancyPoint = _buoyancyPoints[i];

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);
            if (submergedAmount <= 0f) { continue; }

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force);

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force); // calcula el rebote entre todos los puntos del casco y lo distribuye
        }
    }

    private void CalculateHullData()
    {
        _area = CalculateHullArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor); // hace que se guarden los valores dle barco
    }

    private float CalculateHullArea()
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);
        }

        return Mathf.Abs(area) * 0.5f; //calcula todos los puntos alrededor del casco
    }

    private void OnDrawGizmos()
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;

            Gizmos.DrawLine(currentPoint, nextPoint);   // pa ver una linea verde  y asi ver si etsan bien puestos los puntos
        }
    }
}