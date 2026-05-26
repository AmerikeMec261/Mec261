using System;
using System.Collections.Generic;
using UnityEngine;

public class Codigo_Tres : MonoBehaviour
{
    //1
    [SerializeField] private float _life = 100f;
    public bool Life { get; private set; }
    
    //2
    public bool IsDead => _life <= 0;

    /*   3
    public string PlayerName { get; init; }  
    */

    //4
    public int Coins { get; set; }

    //5
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    public float Healt=> _currentHealth / _maxHealth;

    // 6
    [SerializeField] private float _movementSpeed;
    
    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }

    // 7
    [SerializeField] private int _damage;
    public int Damage
    {
        get { return _damage; }
        set { _damage = Mathf.Clamp(value, 0, 100); }
    }

    // 8
    public static int PlayersOnline { get; set; }

    // 9
    [SerializeField] private int _experience;

    public int Experience { get; private set; }

    // 10.
    [SerializeField] private float _stamina = 100f;
    public float Stamina
    { get { return _stamina; } set { _stamina = value; } }

    // 11
    [SerializeField] private Transform _player;
    [SerializeField] private float _attackDistance = 3f;
    [SerializeField] private float _distanceToPlayer;

    public bool CanAttak => _distanceToPlayer <= _attackDistance;

    // 12
    [SerializeField] private float _volume;
    public float Volume
    { get { return _volume; } set { _volume = Mathf.Clamp(value, 0, 100);}}

    // 13
    public readonly DateTime Time = DateTime.Now;

    // 14
    [SerializeField] private int _currentItems;
    [SerializeField] private int _maxItems;
    public bool InventoryFull => _currentItems >= _maxItems;

    /*   15
    public int MaxLevel { get; init; }
    */


    // 16
    [SerializeField] private Rigidbody _rb;

    public float HorizontalSpeed =>
    new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;

    // 17

    public float Energy { get; private set;}

    // 18
    public Vector3 CurrentPosition => transform.position;

    // 19
    [SerializeField] private List<string> _inventoryItems = new List<string>();

    public IReadOnlyList<string> InventoryItems => _inventoryItems;

    // 20
    public bool IsRunning => HorizontalSpeed > 10f;
}



