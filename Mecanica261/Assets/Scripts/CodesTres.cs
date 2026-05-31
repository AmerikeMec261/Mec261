using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CodesTres : MonoBehaviour
{/**
    [SerializeField] private float _stamina;
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _attackRange;
    [SerializeField] private Rigidbody _rigidbody;

    private int _currentHealth = 100;
    private int _maxHealth = 100;
    private float _movementSpeed;
    private int _damage;
    private float _volume;
    private int _maxInventorySize = 20;
    private List<Item> _inventoryItems = new List<Item>();
    private float _runnSpeedThreshold = 5f;

    public int Health { get; private set; } // 1.

    public bool IsDead => Health <= 0; //2.

    public string PlayerName { get; init; } //3.

    public int Coins { get; set; } //4.

    public float HealthPercent => _maxHealth > 0 ? (float)_currentHealth / _maxHealth * 100f : 0f; //5.

    public float MovementSpeed //6.
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }

    public int Damage //7.
    {
        get => _damage;
        set => _damage = Mathf.Clamp(value, 0, 100);
    }

    public static int TotalPlayers { get; private set; } //8.

    public int Experience { get; private set; } //9.

    public float Stamina //10.
    {
        get => _stamina;
        private set => _stamina = value
    }

    public bool CanAttack => _distanceToPlayer <= _attackRange; //11.

    public float Volume //12.
    {
        get => _volume;
        set => _volume = Mathf.Max(0f, value);
    }

    public DeltaTime CreationDate { get; } = DeltaTime.Now; //13.

    public bool IsInventoryFull => _inventoryItems.Count >= _maxInventorySize; //14.

    public int MaxLevel { get; in } //15.

    public float HorizontalSpeed => new Vector3(_rigidbody.velocity.x , 0f , _rigidbody.velocity.z).magnitude; //16.

    public float Energy {  get; private set; } //17.

    public Vector3 CurrentPosition => transform.position; //18.

    public List<Item> InventoryItems { get; private set; } //19.

    public List<Item> InventoryItems => new List<Item>(_inventoryItems);

    public IReadOnlyList<Item> InventoryItems => _inventoryItems;

    public bool IsRunning => _rigidbody.velocity.magnitude >= _runSpeedThreshold; //20.

    [Serializable]
    publuc class Item
    {
        public string Name;
    }  *///
}
  