
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.Searcher;
using UnityEditor.PackageManager;

public class Ejercicio_Propiedades : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private int _volume;
    public float Health { get; private set; } //1

    public bool IsDead => Health <= 0; //2

   // public string PlayerName { get; init; } //3

    public int Coins { get; private set; } //4

    public float HealthPercent => (_currentHealth / _maxHealth) * 100;  //5

    public float MovementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } } //6

    public int Damage { get { return _damage; } set { _damage = Mathf.Clamp(value, 0, 100); }} //7

    public static int PlayerCount { get; private set; } //8

    public int Experience { get; private set; } //9

    [SerializeField] private int _stamina;
    public int Stamina { get { return _stamina; } set { _stamina = value; } } //10 ***

    [SerializeField] private Transform Player;
    public float _atackRange;
    public bool CanAtaack => Vector3.Distance(transform.position , Player.position) <= _atackRange; //11

    public int Volume { get { return _volume; } set { _volume = Mathf.Clamp(value, 0, 100); } } //12

    public int CreatePlayer { get; } //13***

    [SerializeField] List<string> _inventory = new List<string>();
    public int _maxInventtory;
    public bool InventoryFull => _inventory.Count >= _maxInventtory; //14

    // public int MaxLevel { get; init; } //15

    [SerializeField] private Rigidbody _rigidbody;
    public float HorizontalMovement => new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z).magnitude; //16

    public int Energy { get; private set; } //17

    public Vector3 CurrentPosition => transform.position; //18

    public List<string> Inventory = new List<string>(); //19

    public float _currentVelocity;
    public float _still = 0;
    public bool IsRunning => _still  <= (_currentVelocity = _rigidbody.linearVelocity.magnitude); //20


}
