using System;
using System.Collections.Generic;
using UnityEngine;

public class Ejercicios_2 : MonoBehaviour
{
    private float _currentHealth, _maxHealth;
    private int _damage;
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


}
