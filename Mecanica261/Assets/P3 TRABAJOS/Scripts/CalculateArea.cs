using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CalculateArea : MonoBehaviour
{

    [SerializeField] private float _waterLevel;
    [SerializeField] private float _waterDensity;
    [SerializeField] private float _waterDrag;

    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _area;
    [SerializeField] private float _hullHeight;
    [SerializeField] private float _hullVolume;
    [SerializeField] private float _draft;

    float Area { get => _area; }
    float HullHeight { get => _hullHeight; }
    float HullVolume { get => _hullVolume; }
    float Draft { get => _draft; }

    //hullheight = toppoint.position.y - bottomPoint.poisition.y
    //VOLUMNE ARE * HULLhEIGHT
    //FLOAT REQUIERED VOLUMEN = _RIGIDBODY.MASS/ WATERDENSITY
    //_DRAFT = VOLUMEN REQUERIDO/ AREA * shapefactor
    void Awake()
    {
        CalculateHullData();
        FloatShip();

    }
    void FixedUpdate()
    {
        
    }
    private void FloatShip()
    {
        float gravity = Physics.gravity.magnitude;
        float volumePerPoint = _hullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float submersion = Mathf.Clamp01((_waterLevel - point.position.y) / _hullHeight);
            if (submersion <= 0f) { continue; }

            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);
        }
    }
    private void CalculateHullData()
    {
        _area = CalculatedArea();
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;
        _hullVolume = _area * _hullHeight * _shapeFactor;

        float requiredVolume = _rigidbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);


    }
    private float CalculatedArea()
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
