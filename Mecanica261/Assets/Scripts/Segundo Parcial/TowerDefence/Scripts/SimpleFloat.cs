using UnityEngine;
using System.Collections.Generic;



[RequireComponent(typeof(Rigidbody))]
public class SimpleFloat : MonoBehaviour
{
    [Header("Dependencias")]
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 10f;

    [Header("================================================================")]
    [Header("Settings")]
    [SerializeField] private float _shapeFactor;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _floatPoints;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("================================================================")]
    [Header("Motores")]
    [SerializeField] private List<Transform> _motors;
    [SerializeField] private List<Transform> _rightmotors;
    [SerializeField] private List<Transform> _leftmotors;

    [Header("================================================================")]
    [Header("Settings Motores")]
    [SerializeField] private float _motorForce = 20f;
    [SerializeField] private float _maxmotorForce = 24f;
    //Información de mi barco: ~ Velocidad Máxima: aproximadamente 18-24 km/h
    [SerializeField] private float _rotation = 13f;
    //Infromación de mi barco: ~ Velocidad de crucero: 10 y 13 nudos

    [Header("================================================================")]
    [SerializeField] private float _area;
    [SerializeField] private float hulllHeight;
    [SerializeField] private float hullVolume;
    [SerializeField] private float draft;
    private float HullHeight => hulllHeight;

    private float Area => _area;
    private float HullVolume  => hullVolume;
    private float Draft => draft;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()
    {
        FloatShip();

        if (Input.GetKey(KeyCode.W))
        {
            MoverAdelante();
        }
        if (Input.GetKey(KeyCode.D))
        {
            GirarDerecha();
        }
        if (Input.GetKey(KeyCode.A))
        {
            GirarIzquierda();
        }
    }
   
    private void FloatShip()
    {
        float gravity = Physics.gravity.magnitude;
        float volumePerPoint = hullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float submersion = Mathf.Clamp01((_waterLevel - point.position.y) / HullHeight);
            if (submersion <= 0f) { continue; }

            float force = _waterDensity * volumePerPoint * gravity * submersion;
            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            //ADD FORCE POSITION IMPORTANTE PARA QUE EL AVANCE EL BOTE

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);
            _rigidbody.AddForceAtPosition(-velocity * _waterDrag * submersion, point.position, ForceMode.Force);
        }
    }

    private void MoverAdelante()
    {
        Vector3 direction = transform.right;

        foreach (Transform motor in _motors)
        {
            _rigidbody.AddForceAtPosition(direction * _motorForce, motor.position, ForceMode.Force); 
        }
    }

    private void GirarDerecha()
    {
        Vector3 direction = transform.forward;
        float _rotationAplication =  _motorForce * _rotation * _maxmotorForce;

        foreach (Transform motor in _leftmotors)
        {
            _rigidbody.AddForceAtPosition(direction * _rotationAplication, motor.position, ForceMode.Force);
        }

        foreach (Transform motor in _rightmotors)
        {
            _rigidbody.AddForceAtPosition(-direction * _rotationAplication, motor.position,ForceMode.Force);
        }
    }

    private void GirarIzquierda()
    {
        Vector3 direction = transform.forward;
        float _rotationAplication = _motorForce * _rotation * _maxmotorForce;

        foreach (Transform motor in _rightmotors)
        {
            _rigidbody.AddForceAtPosition(direction * _rotationAplication, motor.position, ForceMode.Force);
        }

        foreach (Transform motor in _leftmotors)
        {
            _rigidbody.AddForceAtPosition(-direction * _rotationAplication, motor.position, ForceMode.Force);
        }
    }
    private void CalculateHullData()
    {
        _area = CalculateArea();
        //Calcular altura del top y button point
        hulllHeight = (_topPoint.position.y - _bottomPoint.position.y);
        //Calcular volumen 
        hullVolume = _area * hulllHeight * _shapeFactor;
        // calcular el volumen requerido para flotar
        float requiredVolume = _rigidbody.mass / _waterDensity;
        draft = requiredVolume / (_area * _shapeFactor);

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