using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f; // Nivel del agua
    [SerializeField] private float _waterDensity = 1000f; // Densidad del agua
    [SerializeField] private float _waterDrag = 0.1f; // Drag del agua

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f; // forma del Factor
    [SerializeField] private Transform _topPoint; // Punto superior
    [SerializeField] private Transform _bottomPoint; // Punto inferior
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>(); // La referencia a los puntos de flotacion

    private Rigidbody _rigidbody; // Referencia al Rigidbody

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake() // Metodo para iniciar los calculos del rigidbody antes de inicializar el juego
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate() // Metodo para iniciar constantemente los puntos de flotacion suavemente
    {
        ApplyBuoyancy();
    }

    private void ApplyBuoyancy() // Metodo de los puntos de flotacion calculado por las mismas posiciones entre vectores en el espacio del barco
    {
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

    private void CalculateHullData() // Calculos de los datos de coraza
    {
        _area = CalculateHullArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }

    private float CalculateHullArea() // Calculos del area de la coraza
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos() // OnDrawGizmos para ver como deben de ir acomodados
    {
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