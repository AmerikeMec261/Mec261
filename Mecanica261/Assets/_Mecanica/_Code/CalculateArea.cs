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
    [SerializeField] private float _draf;
    float Draf { get => _draf; }


    private void Awake()
    {
        _rigbody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        FloatShip();
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
    private void Float()
    {
        float summersion = Mathf.Clamp01((_waterLevel - transform.position.y / 1f));

        if (summersion < 0f) return;

        float force = _waterDensity * summersion * Physics.gravity.magnitude;

        _rigbody.AddForce(Vector3.up * force, ForceMode.Force);
    }
}
