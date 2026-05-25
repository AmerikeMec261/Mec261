using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Ejercicios3 : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damage;
    [SerializeField] private float _stamina;
    [SerializeField] private int _volume;
    [SerializeField] private float _distance;
    [SerializeField]private  Rigidbody _rigidBody;
    [SerializeField] private float _runSpeed;
    
    public int Health { get; private set; } //1
    public bool IsDead => Health <= 0; //2
    /*public string PlayerName { get; init; }*/ //3
    public int Coins { get; set; } //4
    public int  HealthPorcent => (int) Health / _maxHealth * 100; //5
    private int _movementSpeed;
    public int Speed => _movementSpeed; //6
    public float Damage { get { return _damage; } set { _damage = Mathf.Clamp(value, 0, 100); } } //7
    public static int PlayerOnline {  get; private set; } //8
    public float Xp { get; private set; } //9
    public float Stamina => _stamina;//10
    public bool IsNear => _distance <= 0;//11
    public int Volume { get { return _volume; } set { _volume = Mathf.Clamp(value, 0, 100); } }//12
    public int CreationDate { get; } = 0;//13
    /*public bool FullInventory => Inventory <= 0;*///14
    /*public int MaxLevel { get; init; }*/  //15
    public float HorizontalSpeed => new Vector3(_rigidBody.linearVelocity.x, 0f, _rigidBody.linearVelocity.z).magnitude;  //16
    public int Energy { get; private set; }//17
    public Vector3 CurrentPosition => transform.position; //18
    /*public ReadInventory<Item> => _inventory;*/ //19
    public bool Running => _rigidBody.linearVelocity.magnitude > _runSpeed; //20

}
