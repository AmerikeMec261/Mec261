using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Burst.CompilerServices;

public class Ejercicios : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5f; //ejercicio 1

    public int playerLevel = 1;   //ejercicio 2

    protected float damage = 10f;  //ejercicio 3

    [SerializeField] private int _playerHealth;  //ejercicio 4

    [SerializeField] private Rigidbody _rigidbody; //ejercicio 5

    [SerializeField] private bool _playeralive; //ejercicio 6

    internal int  _save; //ejercicio 7

     [Range(0, 100)] //ejercicio 8

    public string zeravlax;  //ejercicio 9

    public float movementSpeed; //ejercicio 10

    private MeshRenderer _meshRenderer; //ejercicio 11

    [Tooltip("Volume")]
    [SerializeField] private float _volumeGame ; //ejercicio 12

    private bool _enemyattack;    //ejercicio 13

    // public static GameManager.Instance;  //ejercicio 14

    private List<GameObject> _Inventory = new List<GameObject>();   //ejercicio 15

    [FormerlySerializedAs("_speed")]
    [SerializeField] private float _movementspeed;  //ejercicio 16

    private Vector3 _Playermovement; //ejercicio 17

    public int  MaxPlayers; //ejercicio 18

    [SerializeField] protected float _enemyDetection; //ejercicio 19

    [SerializeField] private AudioSource _audioSource; //ejercicio 20






















}
