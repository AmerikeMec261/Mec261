using UnityEngine;
using System.Collections.Generic;
//Comenté los 3 codigos conforme lo que entendí y como lo uba leyendo , consulte una libreria que adjunte y despues ya entendí como funcionaba , cualquier cosa o aclaración estoy al pendiente.
[RequireComponent(typeof(Rigidbody))]
public class ShipBuoyancy : MonoBehaviour
{
    [Header("Water")] //Valores editables en el editor , son el nivel del aguam , su densidad y su resistencia del agua.
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _waterDrag = 0.1f;

    [Header("Hull")] //Valores editables de Hull , el punto más alto del barco , el maás bajo , los puntos que lo hacen flotar y la forma del barco .
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private List<Transform> _buoyancyPoints = new List<Transform>();

    private Rigidbody _rigidbody; //El rigidbody.


    //Valores d¿editables del barco ( el casco creo ) 
    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    private void Awake() // Lo primero que se ejecuta 
    {
        _rigidbody = GetComponent<Rigidbody>();
        CalculateHullData(); //Clacula los datos del barco y con este los junta para ya tener digamos el barco con sus medidad .
    }

    private void FixedUpdate()
    {
        ApplyBuoyancy(); // Se aplica la flotabilidad 
    }

    private void ApplyBuoyancy() //Flotabilidad del barco .
    {
        float gravityStrength = Physics.gravity.magnitude; //Aquí dice que se use la gravedad normall o 9.81
        float hullVolumePerPoint = _hullVolume / _buoyancyPoints.Count; //Cada punto de flotabilidad del barco cuenta,.

        for (int i = 0; i < _buoyancyPoints.Count; i++) // Cuenta los puntos de flotación.
        {
            Transform buoyancyPoint = _buoyancyPoints[i]; //Posición de los puntos o de un punto de flotación .

            float submergedAmount = Mathf.Clamp01((_waterLevel - buoyancyPoint.position.y) / _hullHeight); 
            if (submergedAmount <= 0f) { continue; } //No comprendo el sumergedAmount , pero porla palabra entiendo que aquí se ven lo puntos de flotación y lo sumerge si es mennor a 0 .

            float buoyancyForce = _waterDensity * hullVolumePerPoint * gravityStrength * submergedAmount; // No la entiendo.

            _rigidbody.AddForceAtPosition(Vector3.up * buoyancyForce, buoyancyPoint.position, ForceMode.Force); //Aqui empuja a los puntos de flotación hacia arriba .los empuja con fuerza.

            Vector3 pointVelocity = _rigidbody.GetPointVelocity(buoyancyPoint.position); //Es un punto de velocidad en el rigidbody.

            Vector3 waterDragForce = -pointVelocity * pointVelocity.magnitude * _waterDrag * submergedAmount; //Esto ed de la fuerza de la resistencia del agua  que es igual al punto de velocidad por el drag por lo de submerged amount.

            _rigidbody.AddForceAtPosition(waterDragForce, buoyancyPoint.position, ForceMode.Force); // Aplica los calcilos anteriores al Rigidboduy.
        }
    }

    private void CalculateHullData() //Calcula los valores del barco.
    {
        _area = CalculateHullArea(); // Area del barco.
        _hullHeight = _topPoint.position.y - _bottomPoint.position.y; //Altura del barco , aquí van los emptys de el más alto y el bajo.
        _hullVolume = _area * _hullHeight * _shapeFactor; // El volumen del barco.

        float requiredVolume = _rigidbody.mass / _waterDensity; // Eñ volumen del agua entre la masa del rigidbody ? .
        _draft = requiredVolume / (_area * _shapeFactor); //No la entední.
    }

    private float CalculateHullArea() //Calcula el area del barco 
    { 
        float area = 0f; //Area inicial 0.

        for (int i = 0; i < _buoyancyPoints.Count; i++) //Aquí cuenta los puntos de flotabilidad. 
        {
            Vector3 currentPoint = transform.InverseTransformPoint(_buoyancyPoints[i].position); //El punto actual es igual a rel espacio del barco .
            Vector3 nextPoint = transform.InverseTransformPoint(_buoyancyPoints[(i + 1) % _buoyancyPoints.Count].position); //Aqui se sigue al siguiente punto cerrando el area.

            area += (currentPoint.x * nextPoint.z) - (nextPoint.x * currentPoint.z); //Aquí es de que el area es el calculo de los puntos ya cerrando el "rectangulo" del barco por decir algo.
        }

        return Mathf.Abs(area) * 0.5f; //Regresa el valor del area y lo multiplica por 0.5.
    }

    private void OnDrawGizmos() // Bueno aquí es para ver como están conectados los puntos de flotabilidad del barco y que esten correctos , se traza una línea entre ellos en orden.
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