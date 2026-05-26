using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class propiedades : MonoBehaviour
{
    [SerializeField] private float _stamina;
    //[SerializeField] private List<Item> _inventoryItems = new List<Item>();
    [SerializeField] private int _maxInventorySlots = 10;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private List<Item> _inventoryItems = new List<Item>();
    private float _currentHealth;
    private float _maxHealth;
    private float _movementSpeed;
    private int _damage;
    private float _distanceToPlayer;
    private float _attackRange;
    private float _volume;
    

    public int Health { get; private set; } //1
    public bool IsDead => Health <= 0;//2
    public string PlayerName { get; private set; } //3
    public int Coins { get; set; } //4
    public float HealthPercent => (float)_currentHealth / _maxHealth * 100f; //5
    public float MovementSpeed => _movementSpeed; //6
    public int Damage
    { 
        get { return _damage; }
        set { _damage = Mathf.Clamp(value, 0, 100);}
    } //7
    public static int TotalPlayers { get; private set; } //8
    public int Experience { get; private set; } //9
    public float Stamina => _stamina; //10
    public bool CanAttack => _distanceToPlayer <= _attackRange; //11
    public float Volume
    {
        get { return _volume; }
        set { _volume = Mathf.Clamp(value, 0f, 100f); }
    } //12
    public DateTime CreationDate { get; } = DateTime.Now; //13
    public bool IsInventoryFull => _inventoryItems.Count >= _maxInventorySlots; //14
    public int MaxLevel { get; private set; } //15
    public float HorizontalSpeed => _rigidbody.linearVelocity.x;//16
    public float Energy { get; private set; }//17
    public Vector3 CurrentPosition => transform.position; //18
    public IReadOnlyList<Item> InventoryItems => _inventoryItems;//19
    public bool IsRunning => HorizontalSpeed > 5f;//20



















    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
