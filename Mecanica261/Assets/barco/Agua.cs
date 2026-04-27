using UnityEngine;
using System.Collections.Generic;

public class Agua : MonoBehaviour
{
    [Header("Water configutation")]
    [SerializeField] private float _watterlevel = 1.0f;
    [SerializeField] private float _watterDensity = 1.0f;
    [SerializeField] private float _watterDraft = 1.0f;

    [Header("Barco")]
    [SerializeField] private float _shapeFactor = 1.0f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List <Transform> _floatPoints;

    private float _area;
    private float _HullHeight;
    private float _HullVolume;

    private Rigidbody _rigidBody;

        public float Area { get; }
    public float HullHeight { get; }
    public float HullVolume { get; }
    public float Draft { get; }


   private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        CalculateData();
       
    }

    private void FixedUpdate()
    {
        FloatShip();
    }


    private void FloatShip()
    {

        float Gravity = Physics.gravity.magnitude;

        float volumenperpoint = HullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
           Transform point = _floatPoints[i];
            float sumergtion = Mathf.Clamp01(_watterlevel - point.position.y) / HullHeight;
            if (sumergtion <=0)
            {
                continue;
            }

            float Force = _watterDensity * volumenperpoint * Gravity * sumergtion;
            _rigidBody.AddForceAtPosition(Vector3.up * Force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidBody.GetPointVelocity(point.position);
            _rigidBody.AddForceAtPosition(-velocity * _watterDraft * sumergtion, point.position, ForceMode.Force);
        }

    }


    private void CalculateData()
    {
        _area = CalculateArea();
        _HullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _HullVolume = _area * _HullHeight * _shapeFactor;

        float RiquiereVolume = _rigidBody.mass / _watterDensity;

        _watterDraft = RiquiereVolume    / (_area * _shapeFactor);
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
}
