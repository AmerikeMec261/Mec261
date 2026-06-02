using UnityEngine;
using System;
using System.Collections;
using NUnit.Framework;

public class EjercicioPropiedades : MonoBehaviour
{
    [Header("Serialized Fields")]
    [SerializeField] private float _stamina;
    [SerializeField] private float _distanceToPlayer;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Backing Fields")]
    private int _currentHealth = 100;
    private int _maxHealth = 100;
    private float _movementSpeed;
    private int _damage;
    private float _volume;
    private int _maxInventorySize = 20;
    private List<Item> _inventoryItems=new List<Item>();
    private float _runSpeedThreshold = 5f;

    //Una propiedad de vida que cualquier sistema puede leer pero solo el objeto puede modificar
    public int Health { get; private set; }

    //El jugador esta muerto
    public bool IsDead => Health <= 0;

    //Nombre del jugador
    public string PlayerName { get; init; }

    //Monedas
    public int Coins { get; set; }

    //Porcentaje de vida
    public float HealthPercent = _maxHealth > 0 ? (float)_currentHealth / _maxHealth * 100f : 0f;

    //Backing Field
    public float MovementSpeed
    {
        get => _movementSpeed;
        set => _movementSpeed = value;
    }

    //Limite de daño
    public int Damage
    {
        get => _damage;
        set => _damage = Mathf.Clamp(value, 0, 100);
    }

    //Jugadores conectados
    public static int TotalPlayers { get; private set; }

    //Experiencia
    public int Experience { get; private set; }

    //Estamina
    public float Stamina
    {
        get => _stamina;
        private set => _stamina = value;
    }

    //Puede atacar
    public bool CanAttack => _distanceToPlayer <= _attackRange;

    //Volumen
    public float Volume
    {
        get => _volume;
        set => _volume =Mathf.Max(0f,value);
    }

    //Fecha de creacion de personaje
    public DateTime CreationDate { get; } = DateTime.Now;

    //Inventario lleno
    public bool IsInventoryFull => _inventoryItems.Count >= _maxInventorySize;

    //Nivel maximo
    public int MaxLevel { get; init; }

  
}

[Serializable]
public class Item
{
    public string Name;
}
