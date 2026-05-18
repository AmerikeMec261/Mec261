using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

public class variables : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Rigidbody _playerRigidbody;
    [SerializeField, Range(0, 100)] private float _attackRange;
    [Header("Configuracion de audio")]
    [SerializeField] private float _gameVolume;
    [SerializeField] private float _movementspeed;
    [SerializeField] protected float _detectionEnemy;
    [SerializeField] private AudioSource _audioSource;




    public int _playerLevel1;
    protected int _baseDamage = 10;
    public int _CurrentHealth { get; private set; }

    private bool _isAlive;
    internal int _saveIndex;

    public string _PlayerName;
    protected float _moveSpeed;
    private MeshRenderer meshRenderer;

    private bool _canAttack;
    public static GameManager Instance;
    private List<GameObject> _inventoryItems = new List<GameObject>();

    private Vector3 _PlayerPosition;
    public int _maxPlayers;


    void Start()
    {

    }

    void Update()
    {

    }
}
