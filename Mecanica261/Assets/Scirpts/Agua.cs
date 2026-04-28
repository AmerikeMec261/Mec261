using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Agua : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDraft = 10f;

    [Header("BattleShip")]
    [SerializeField] private float _shapeFactor = 1.0f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;

    private float _area;
    private float _HullHeight;
    private float _HullVolume;    

    private Rigidbody _rigidBody;

    public float Area { get; }
    public float HullHeight { get; }
    public float HullVolume { get; }
    public float Graft { get; }


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        FloatShip();
    }

    private void FloatShip()
    {
        float gravity = Physics.gravity.magnitude;
        float volumePerPoint = HullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];
            float submersion = Mathf.Clamp01(_waterLevel - point.position.y) / _HullHeight;

            if (submersion <= 0)
            {
                continue;
            }
            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidBody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidBody.GetPointVelocity(point.position);

            _rigidBody.AddForceAtPosition(- velocity * _waterDraft * submersion, point.position, ForceMode.Force);
        }
    }

    private void CalculateHullData()
    {
        _area = CalculateArea();
        _HullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _HullVolume = _area * _HullHeight * _shapeFactor;

        float requireVolume = _rigidBody.mass / _waterDensity;
        _waterDraft = requireVolume / (_area * _shapeFactor);
    }


    private float CalculateArea()
    {
        float area = 0f;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Vector3 current = transform.InverseTransformPoint(_floatPoints[i].position);
            Vector3 next = transform.InverseTransformPoint(_floatPoints[(i + 1) % _floatPoints.Count].position);

            area += (current.x * next.z) - (next.x * current.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        if (_floatPoints == null || _floatPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Vector3 current = _floatPoints[i].position;
            Vector3 next = _floatPoints[(i + 1) % _floatPoints.Count].position;

            Gizmos.DrawLine(current, next);
        }
    }
}