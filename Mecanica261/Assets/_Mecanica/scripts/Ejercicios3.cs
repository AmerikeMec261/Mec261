using System;
using UnityEngine;

public class Ejercicios3 : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private int _stamina;
    [SerializeField] public int PlayerDistance;
    [SerializeField] public int AttackDistance;
    [SerializeField] private int _volume;
    [SerializeField] private float _speed;
    public int Health {get; private set;}

    public bool IsDead => Health <= 0; 

    public string PlayerName { get; }

    public int Coins { get; set; }

    public float HealthPercent => (float)_currentHealth / _maxHealth * 100f;


    private int _movementSpeed;
    public int MovementSpeed => _movementSpeed;

    public int Damage
    {
        get { return _damage; }
        set { _damage = Mathf.Clamp(value, 0, 100); }
    }

    public static int TotalPlayerCount { get; private set; }

    public int Experience { get; private set; }

    public float Stamina => _stamina;

    public bool CanAttack ()
    {
        return PlayerDistance <= AttackDistance;
    }

    public int Volume
    {
        get { return _volume; }
        set { _volume = Mathf.Clamp(value, 0, 100); }
    }

    public DateTime CharacterCreationDate { get; } = DateTime.Now;

    //public bool IsInventoryFull => _inventoryItems.Count >= _maxInventory;

    

}
