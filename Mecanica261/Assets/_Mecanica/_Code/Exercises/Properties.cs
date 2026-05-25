using UnityEngine;
using System;
using System.Collections.Generic;

public class Properties : MonoBehaviour
{
    [SerializeField] private int _currentHealth = 100;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private int _damage;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Health { get; set; }


    public bool IsDead => Health <= 0;


    //public string PlayerName { get; init; }

    public int Coin { get; set; }


    public int MinHealth
    {
        get => _currentHealth;
        private set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
    }


    public float MovementSpeed
    {
        get => _movementSpeed;
        private set => _movementSpeed = Mathf.Max(0, value);
    }


    public int Damage { get { return _damage;}
        set { _damage = Mathf.Clamp(value, 0, 100); } }


}
