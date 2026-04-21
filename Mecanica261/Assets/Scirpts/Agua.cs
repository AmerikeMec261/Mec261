using UnityEngine;

public class Agua : MonoBehaviour
{
    [Header("Water configuration")]
    [SerializeField] public float _waterLevel = 1.0f;
    [SerializeField] public float _waterDensity = 1.0f;
    [SerializeField] public float _waterDrag = 1.0f;

    [Header("Barco")]
    [SerializeField] public float _shapeFactor = 1.0f;
    [SerializeField] public Transform _;

    private void Awake()
    {
        
    }
    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        
    }

    private void FloatShip()
    {

    }

    private void CalculateHullData()
    {

    }

    //private float CalculateArea()
    //{
    //    float area = 0f;

    //    for (int i = 0; i < _floatPoints.Count; i++)
    //    {
    //        Vector3 current = transform.InverseTransformPoint(_floatPoints[i].position);
    //        Vector3 next = transform.InverseTransformPoint(_floatPoints[(i + 1) % _floatPoints.Count].position);

    //        area += (current.x * next.z) - (next.x * current.z);
    //    }

    //    return Mathf.Abs(area) * 0.5f;
    //}
}
