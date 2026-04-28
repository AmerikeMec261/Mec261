using System.Collections.Generic;
using UnityEngine;

public class CalculateArea : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _waterDrag = 1;
    [SerializeField] private float _waterDensity = 1000;

    [Header("Points")]
    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoint;
    [SerializeField] private Rigidbody _rigbody;

    [Header("Area")]
    private float _area;
    float Area { get => _area; }

    [Header("Altura")]
    [SerializeField] private float _hullheight;
    float Hullheight { get =>  _hullheight; }

    [Header("Volumen")]
    [SerializeField] private float _hullvolumen;
    float Volumen { get => _hullvolumen; }

    [Header("Movimiento")]
    [SerializeField] private float _draft;
    float Draf { get => _draft; }


    private void Awake()
    {
        _rigbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()
    {
        FloatShip();
        OnDrawGizmos();
    }
    void FloatShip()
    {
        float gravity = Physics.gravity.magnitude;
        float Volumpropoin = _hullvolumen / _floatPoint.Count;
        for (int i = 0;  i < _floatPoint.Count; i++)
        {
            Transform Point = _floatPoint[i];
            float summersion = Mathf.Clamp01(_waterLevel - Point.position.y) / Hullheight;
            if (summersion <= 0)
            {
                continue;
            }
            float force = _waterDensity * summersion * Volumpropoin * gravity;
            _rigbody.AddForceAtPosition(Vector3.up * force, Point.position, ForceMode.Force);
            Vector3 velocity = _rigbody.GetPointVelocity(Point.position);
            _rigbody.AddForceAtPosition(-velocity * _waterDrag * summersion, Point.position, ForceMode.Force);
        }
    }
    private void CalculateHullData()
    {
        _area = CalculateArea1();
        _hullheight = _topPoint.position.y - _bottomPoint.position.y;
        _hullvolumen = _area * _hullheight * _shapeFactor;

        float requiredVolume = _rigbody.mass / _waterDensity;
        _draft = requiredVolume / (_area * _shapeFactor);
    }

    private float CalculateArea1()
    {
        float area = 0f;

        for (int i = 0; i < _floatPoint.Count; i++)
        {
            Vector3 current = transform.InverseTransformPoint(_floatPoint[i].position);
            Vector3 next = transform.InverseTransformPoint(_floatPoint[(i + 1) % _floatPoint.Count].position);

            area += (current.x * next.z) - (next.x * current.z);
        }
        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos()
    {
        if (_floatPoint == null || _floatPoint.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _floatPoint.Count; i++)
        {
            Vector3 current = _floatPoint[i].position;
            Vector3 next = _floatPoint[(i + 1) % _floatPoint.Count].position;

            Gizmos.DrawLine(current, next);
        }
    }
}
