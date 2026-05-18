using System.Collections.Generic;
using UnityEngine;

public class PrograVariables : MonoBehaviour
{
    [SerializeField] private float _movespeed = 5f;
    public int PlayerLevel = 10;
    protected float DañoBase = 10f;
    [SerializeField] private int _playerVida = 100;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private bool _muerte = false;
    internal int _index;
    [SerializeField, Range(0f, 100f)] private float _range;
    public string PlayerName;
    protected internal float Velocidad;
    private MeshRenderer _meshRenderer;
    [Tooltip("Sonido Sonoramico")]
    [SerializeField] private float Sonido;
    private bool _enemyataca = false;
    //public static GameManager Instance;
    private List<GameObject> _inventario;
    [SerializeField] private float _speed2 = 1f;
    private Vector3 _position;
    public int MaximodeJugadores = 100;
    [SerializeField] protected float EnemyDetection = 15;
    [SerializeField] private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
