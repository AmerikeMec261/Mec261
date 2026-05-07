using UnityEngine;

public class Esqueletofloat : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] private float _waterLevel = 0f;
    [SerializeField] private float _waterDensity = 1000f;
    [SerializeField] private float _verticalWaterDrag = 1f;
    [SerializeField] private float _horizontalWaterDrag = 0.005f;

    [Header("Hull")]
    [SerializeField] private float _shapeFactor = 0.67f;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private Transform _floatPoints;

    private Rigidbody _rigidbody;

    private float _area;
    private float _hullHeight;
    private float _hullVolume;
    private float _draft;

    public float Area => _area;
    public float HullHeight => _hullHeight;
    public float HullVolume => _hullVolume;
    public float Draft => _draft;

    private void Awake()
    {
      
    }

    private void FixedUpdate()
    {
       
    }

    private void FloatShip()
    {
        
    }

    private void CalculateHullData()
    {
        
    }

    private float CalculateArea()
    {
       
    }
}
