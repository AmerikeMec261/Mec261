using UnityEngine;
using System;
using System.Collections.Generic;

public class Properties : MonoBehaviour
{
    [SerializeField] private float _stamina;
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private Rigidbody _rigidbody;


    private int _currentHealth = 100;
    private int _maxHealth = 100;
    private float _movementSpeed;
    private int _damage;
    private float _volume;
    private int _maxInventorySize = 20;
    private List<GameObject> _inventoryItems = new List<GameObject>();
    private float _runSpeedThreshold = 5f;

    public int Health { get; set; }


    public bool IsDead => Health <= 0;


    //public string PlayerName { get; init; }

    public int Coin { get; set; }


    //public float HealthPercent => _maxHealth > 0 ? (float)_currentHealth / _maxHealth = 100f : 0f;

    public float MovementSpeed
    {
        get => _movementSpeed;
        private set => _movementSpeed = Mathf.Max(0, value);
    }


    public int Damage
    {
        get => _damage;
        set => _damage = Mathf.Clamp(value, 0, 100);
    }

    public static int TotalPlayers { get; private set; }

    public int Experience { get; private set; }

    public float Stamina
    {
        get => _stamina;
        private set => _stamina = value;
    }

    public bool CanAttack => _distanceToPlayer <= _attackRange;

    public float Volume
    {
        get => _volume;
        set => _volume = Mathf.Max(0, value);
    }

    public DateTime CreationDate { get; } = DateTime.Now;


    public bool IsInventoryFull => _inventoryItems.Count >= _maxInventorySize;


    // public int MaxLevel { get; innit; }


    public float HorizontalSpeed => new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z).magnitude;


    public float Energy { get; private set; }


    public Vector3 CurrentPosition => transform.position;


    public List<GameObject> InventoryItems { get; private set; }


    // public List<GameObject> InventoryItems => new List<GameObject>(_inventoryItems);


    // public IReadOnlyList<GameObject> InventoryItems => _inventoryItems;


    public bool IsRunning => _rigidbody.velocity.magnitude >= _runSpeedThreshold;


}

[Serializable]
public class Item
{
    public string Name;
}