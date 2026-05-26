using System;
using System.Collections.Generic;
using UnityEngine;

public class Ejercicios_2 : MonoBehaviour
{
    private float _currentHealth, _maxHealth;
    private int _damage;
    [SerializeField] private int _stamina;
    private int _distancePlayer;
    private int _rangeAttack;
    private List<GameObject> _slots;
    private int _maxSlots;
    private Rigidbody _rigidBody;
    private float _runSpeed;

    public int Health { get; private set; }// 1

    public bool IsDead => Health <= 0;// 2

    //public string PlayerName { get; init; }// 3 

    public int Coins { get; set; }// 4

    public float PorcentHealth =>(float)_currentHealth / _maxHealth * 100; // 5

    private float _speed;
    public float Speed => _speed;// 6

    public int Damage { get { return _damage; } set { _damage = Mathf.Clamp(value, 0, 100); } }// 7

    public static int PlayersCount { get; private set; }// 8

    public int Gameobject { get; private set; }// 9

    public int Stamina { get => _stamina; set => _stamina = value; }// 10

    public bool CanAttack => _distancePlayer <= _rangeAttack;// 11

    public float _volumen;
    public float Volumen { get { return _volumen; } set { _volumen = Mathf.Clamp(value, 0f, 100f); } }// 12

    public string CreationCharacter { get; } = "25/05/2026";// 13

    public bool InventoryFull => _slots.Count >= _maxSlots;// 14

    //public int MexLevel { get; init; }// 15

    public float HorizontalSpeed => _rigidBody.linearVelocity.x;// 16

    public int Energy { get; private set; }// 17

    public Vector3 CurrentPosition => transform.position;// 18

    public List <GameObject> Slots => _slots;// 19

    public bool IsRunning => _speed > _runSpeed;// 20
}
