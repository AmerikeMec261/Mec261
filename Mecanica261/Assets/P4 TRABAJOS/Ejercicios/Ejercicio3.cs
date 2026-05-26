using System;
using UnityEngine;
using System.Collections.Generic;


public class Ejercicio3 : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int _stamina;
    [SerializeField] private List<GameObject> _Inventory = new();
    [SerializeField] private float _speed;
    [SerializeField] private Transform _enemyPosition;

    private int _currentHealth;
    private int _MaxHealth;
    private int _movementSpeed;
    private int _damage;
    private int _volume;
    private Rigidbody _rigidbodyP;


    public int Health { get; private set; } //1

    public bool IsDead => Health <= 0; //2

    public string Nametag { get; init; } //3

    public int Coins { get; set; } //4

    public int Percentage => _currentHealth / _MaxHealth * 100; //5

    public int MovementSpeed => _movementSpeed; //6

    public int TakeDamage { get{ return _damage;} //7
    set {_damage = Mathf.Clamp(value, 0, 100);}}

    public static int TotalPlayers { get; private set; }//8

    public int Exp { get; set; } //9

    public int Stamina => _stamina; //10

    public bool Attack => (_rigidbodyP.transform.position - _enemyPosition.transform.position).magnitude <= 5; //11

    public int Volume { get { return _volume; } //12
    set { _volume = Mathf.Clamp(value, 0, 100); }}
    public DateTime PlayerCreation { get; } = DateTime.Now; //13

    public int MaxLevel { get; set; } //15

    public float HorizontalSpeed => _rigidbodyP.linearVelocity.magnitude; //16

    public int Energy { get; private set ; } //17

    public Vector3 CurrentPosition => _rigidbodyP.transform.position; //18

    public IReadOnlyList<GameObject> InventoryItems => _Inventory; //19

    public bool IsRunning => _rigidbodyP.linearVelocity.magnitude > 0.5; //20


}
