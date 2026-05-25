using JetBrains.Annotations;
using System;
using UnityEngine;

public class Ejercicios_Propiedades : MonoBehaviour
{
    private float _currentHealth;
    private float _maxHealth;
    private float _damage;
    private int _volume;
    public float Distance;
    public float Things;
    public float Velocity;
    public float Speed; 

    public int Health {  get; set; } //Ej.1

    public bool IsDead => Health <= 0;//Ej.2

    public string PlayerName { get; init; }//Ej.3

    public int Coins { get; set; }//Ej.4

    public float HealthPercent => (float)_currentHealth / _maxHealth * 100f;//Ej.5

    private float _movementSpeed;
    public float MovementSpeed=>_movementSpeed;//Ej.6

    public float Damage { get { return _damage; }set { _damage = Mathf.Clamp(value, 0, 100); } }//Ej.7

    public static int PlayersConnected {  get; private set; }//Ej.8

    public int Items {  get; private set; }//Ej.9

    [SerializeField] private float _stamina;
    public float Stamina => _stamina;//Ej.10

    public bool CanAttack => Distance <= 50f;//Ej.11

    public int Volume { get { return _volume; } set { _volume = Mathf.Clamp(value, 0, 100); } }//Ej.12

    public string Date { get; } = DateTime.Now.ToString();//Ej.13

    public bool IsFull => Things <= 5;//Ej.14

    public int MaxLevel { get; init; }//Ej.15

    public bool IsHorizontalSpeed => Velocity <= 0;//Ej.16

    public int Energy {  get; private set; }//Ej.17

    public Vector3 CurrentPosition=>transform.position;//Ej.18

    [SerializeField] private int _iventoryItems;
    public int InventoryItems => _iventoryItems;//Ej.19

    public bool IsRunning => Speed <= 1f;//Ej.20
}
