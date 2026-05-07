using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

//Referencias
//https://filehostingbucket.web.app/Unity/Instructables/index.html
//https://github.com/ditzel/UnityOceanWavesAndShip/blob/master/Waves/Assets/WaterBoat.cs 
//https://www.youtube.com/watch?v=gdW_rXFE1Gk&t=1s
//https://www.youtube.com/watch?v=eL_zHQEju8s&t=6s
//https://codefinity.com/es/courses/v2/9284ee3b-35da-4063-b243-6f8e8cbd4412/0838fd67-05d4-4253-a981-5fb1b74c885e/823bce7d-7f39-43d1-aa07-c3d05bf3e61d
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

    [SerializeField] private Transform _motorPoint;
    [SerializeField] private float _steerPower = 500f;

    [SerializeField] private float _maxThrustForce = 50f;
    [SerializeField] private float _thrustAcceleration = 5f;
    [SerializeField] private float _targetThrust = 0f;
    [SerializeField] private float _currentThrust = 0f;

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
        
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();


    }
    void FixedUpdate()
    {
        FloatShip();
        ShipController(); 
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

    private void ShipController()
    {
        int steer = 0; 

        if (Input.GetKey(KeyCode.A))
            steer = 1;
        if (Input.GetKey(KeyCode.D))
            steer = -1;

        if (_motorPoint != null)
            _rigidbody.AddForceAtPosition(steer * transform.right * _steerPower / 100f, _motorPoint.position);

        if (Input.GetKey(KeyCode.W))
            _targetThrust = Mathf.Clamp(_targetThrust + 4225f * Time.deltaTime, 0f, _maxThrustForce);
        else
            _targetThrust = 0f;

        _currentThrust = Mathf.MoveTowards(_currentThrust, _targetThrust, _thrustAcceleration * Time.fixedDeltaTime);
        _rigidbody.AddForce(transform.forward * _currentThrust);
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
