using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]//hace que el objeto tenga un rigidbody
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f; // es la altura del agua
    [SerializeField] private float _waterDensity = 1000f; //es la dencidad del agua
    [SerializeField] private float _waterDrag = 0.1f; // es la resistencia del agua

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f; //ajusta el volumen del barco
    [SerializeField] private Transform _topPoint; //es el punto mas alto del barco
    [SerializeField] private Transform _bottomPoint; //es el punto mas bajo del barco
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>(); // son los puntos de flotabilidad del barco 

    private Rigidbody _rigidbody; // guarda un rigidbody para el barco

    private float _area; //es el area del barco
    private float _hullHeight; //la altura del barco
    private float _hullVolume; //volumen del barco
    private float _draft; // lo quye se hunde el barco

    private void Awake() // se ejcuta al iniciar el objeto
    {
        _rigidbody = GetComponent<Rigidbody>(); // esto detecta que el objeto tenga rigidbody
        CalculateHullData(); // calcula los datos del barco
    }

    private void FixedUpdate()// se ejecuta varias veces por segundo
    {
        ApplyBuoyancy();//llama a la funcion de flotar
    }

    private void ApplyBuoyancy()//la funcion de flotar
    {
        float gravityStrength = Physics.gravity.magnitude;//te da la fuerza de la gravedad
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count;// divide el volumen del barco

        for (int i = 0; i < _buoyancyPoints.Count; i++)//recorre todos los puntos del barco
        {
            Transform buoyancyPoint = _buoyancyPoints[i];// guarda el punto principal

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight);//calcula que el punto este en el agua
            if (submergedAmount <= 0f) { continue; }//si el primer punto no esta en el agua se pasa al que sigue

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount;//es la fuerza que empuja el barco hacia arriba

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force);//da la fuerza hacia arriba

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position);//te da la velocidad del punto del barco

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount;//esta calcula la resistencia del barco

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force);//y esta aplica la resistencia
        }
    }

    private void CalculateHullData()// calcula los datos del barco
    {
        _area = CalculateHullArea();// calcula el area del barco
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y;//calcula la altura del barco
        _hullVolume = _area * _hullHeight * _shapeFactor;//calcula el volumen del barco

        float requiredVolume = _rigidbody.mass / _waterDensity;//calcula el volumen para hacer flotar al barco
        _draft = requiredVolume / (_area * _shapeFactor);//calcula si esta hundido el barco
    }

    private float CalculateHullArea()// calcula el area del barco
    {
        float area = 0f;// aqui se guarda el area del barco 

        for (int i = 0; i < _buoyancyPoints.Count; i++)// recorre todos los puntos del barco
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position);//hace que el punto sean coordenadas
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position);//te da el siguente punto

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z);//calcula el area geometrica
        }

        return Mathf.Abs(area) * 0.5f;//hace que el area sea positiva
    }

    private void OnDrawGizmos()//hace lineas de color 
    {
        if (_buoyancyPoints == null || _buoyancyPoints.Count < 2) { return; }//revisa que existan los puntos del barco para flotar

        Gizmos.color = Color.green;//les pone una linea de color verde

        for (int i = 0; i < _buoyancyPoints.Count; i++)
        {
            Vector3 currentPoint = _buoyancyPoints[i].position;
            Vector3 nextPoint = _buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position;

            Gizmos.DrawLine(currentPoint, nextPoint);//hace las lineas entre los puntos
        }
    }
}