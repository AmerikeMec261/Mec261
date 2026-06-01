using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 0.1f;

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>();

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData();
    }

    private void FixedUpdate()
    {
        ApplyBuoyancy();
    }

    private void ApplyBuoyancy()
    {
        float gravityStrength = Physics.gravity.magnitude;//Aqui se aplican las fisicas de la fuerza de gravedad
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count;//Aqui es el volumen del barco con los puntos del barco

        for (int i = 0; i < _buoyancyPoints.Count; i++)//Aqui es para cada posicion de os puntos del barco
        {
            Transform buoyancyPoint = _buoyancyPoints[i];

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);//El nivel del agua que interactua con la punto de los barco
            if (submergedAmount <= 0f) { continue; }

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;// Aqui se multiplican la densidad del agua con el volumen de los puntos, la gravedad y el submerge 

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force);// Aqui se agrega la fuerza de los puntos del barco 

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);// Se calcula la velocidad de cada uno de los puntos 

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;// Aqui es la fuerza del agua con los puntos de velocidad que va tener 

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force);// Aqui se le agrega al rigidbody los elementos de los puntos del barco y la fuerza del agua
        }
    }

    private void CalculateHullData()//CAlcula los datos del barco
    {
        _area = CalculateHullArea();//Area del barco
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;// Lo alto del barco
        _hullVolume = _area * _hullHeight * _shapeFactor;// El volumen del barco

        float requiredVolume = _rigidbody.mass / _waterDensity;//El requerimiento del volumen con el rigidbody y con la densidad del agua
        _draft = requiredVolume / (_area * _shapeFactor);//Draft del barco
    }

    private float CalculateHullArea()// CAlcula el area del barco
    {
        float area = 0f;

        for (int i = 0; i < _buoyancyPoints.Count; i++)// Cuenta los puntos del barco
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);//En la posicion del punto que se encuntre ahora
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);//La sgiuente posicion del siguiente punto

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);//Metodo para calcular el area 
        }

        return Mathf.Abs(area) * 0.5f;
    }

    private void OnDrawGizmos()//Los puntos del barco para que sean visibles y esten bien estructurados
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }

        Gizmos.color = Color.green;

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;

            Gizmos.DrawLine(currentPoint, nextPoint);
        }
    }
}