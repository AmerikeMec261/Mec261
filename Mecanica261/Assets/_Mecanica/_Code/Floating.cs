using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleFloat : MonoBehaviour
{
    [Header("Water Settings")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterDensity = 1000;
    [SerializeField] private float _waterDrag = 1;

    private Rigidbody _rigidbody;

    [Header("Debug Settings")]
    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;


    [Header("Area")]
    float Area { get => _area; }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();      
    }

    public float HullHeight => _hullHeight;
    public float HullVolume => _hullVolume;
    public float Draft => _draft;



    private void FixedUpdate()
    {
        Float();
    }

    private void Float()
    {
        float summersion = Mathf.Clamp01((_waterLevel - transform.position.y / 1f));

        if (summersion < 0f) return;

        float force = _waterDensity * summersion * Physics.gravity.magnitude;

        _rigidbody.AddForce(Vector3.up * force * _volume, ForceMode.Force);
    }


}