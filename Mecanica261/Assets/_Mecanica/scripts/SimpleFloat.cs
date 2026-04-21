using System;
using UnityEngine;

public class SimpleFloat : MonoBehaviour
{
    [SerializeField] private float _waterlevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterdensity = 1000;
    [SerializeField] private float _ShapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private Transform[] _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float Area;
    [SerializeField] private float HullHeight;
    [SerializeField] private float HullVolume;
    [SerializeField] private float Draft;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Float();
    }

  

    private void Float()
    {
        float summersion = Mathf.Clamp01((_waterlevel - transform.position.y / 1f));

        if (summersion < 0f) return;
        
        float force = _waterdensity * summersion * _volume * Physics.gravity.magnitude;

        _rigidbody.AddForce(Vector3.up * force, ForceMode.Force);
    }

  

    private void FloatShip()
    {

    }

    private void CalculateHullData()
    {

    }

    [SerializeField] private float CalculateArea;
}
