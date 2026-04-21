using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFloat : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterDensity = 1000;

    [Header("Settings")]
    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private Transform[] _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float Area;
    [SerializeField] private float hulllHeight;
    [SerializeField] private float hullVolume;
    [SerializeField] private float draft;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()
    {
        FloatShip();
    }

    private void FloatShip()
    {
        float summersion = Mathf.Clamp01((_waterLevel - transform.position.y / 1f));

        if (summersion < 0f) return;

        float force = _waterDensity * summersion * Physics.gravity.magnitude;

        _rigidbody.AddForce(Vector3.up * force * _volume, ForceMode.Force);
    }

    private void CalculateHullData()
    {
        
    }

    private float CalculateArea()
    {
        float area = 0f;

        for (int i = 0; i < _floatPoints.Length; i++)
        {
            Vector3 current = transform.InverseTransformPoint(_floatPoints[i].position);
            Vector3 next = transform.InverseTransformPoint(_floatPoints[(i + 1) % _floatPoints.Length].position);

            area += (current.x * next.z) - (next.x * current.z);
        }

        return Mathf.Abs(area) * 0.5f;
    }
}