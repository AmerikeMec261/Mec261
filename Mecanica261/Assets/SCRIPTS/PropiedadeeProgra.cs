using System;
using UnityEngine;

public class PropiedadeeProgra : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private float _stamina;
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _rangeAttak;
    [SerializeField] private float _volume;
    [SerializeField] private float _inventoryItems;
    [SerializeField] private float _inventorySize;
    public int Health {  get; private set; }
    public bool IsDead => Health <= 0;
    //public string PlayerName { get; init; }
    public int Coins {  get; set; }

    public float HealthPercent => (_currentHealth / _maxHealth);
    private float _movementSpeed;
    public float Speed => _movementSpeed;
    public int Damage { get { return _damage; } set { _damage = Mathf.Clamp(value, 0, 100); } }
    public static int PlayersTotal { get; private set; }
    public int Experience { get; private set; }
    public float Stamina => _stamina;
    public bool CanAttak => _distanceToPlayer <= _rangeAttak;
    public float Volume { get { return _volume; } set { _volume = Mathf.Clamp(value, 0, 100); } }
    public DateTime Date { get; } = DateTime.Now;
    public bool InventoryFull => _inventoryItems >= _inventorySize;
    //public int LevelMax { get; init; }
    public float SpeedHorizontal;
    




   
}
