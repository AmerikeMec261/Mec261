
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ejercicio_en_Clase : MonoBehaviour
{
    //1 
    [SerializeField] private float _velocity = 5f;

    //2
    public int _playeLevel;

    //3 
    protected int _enemyHealt = 10;

    //4
    private int _playerHealth;

    //5
    [SerializeField] private Rigidbody _rigidbody; 

    //6 
    private bool _isAlive;

    //7
    internal int _spawPoint;

    //8 
    [Range(0f, 100f)]
    [SerializeField] private float _detectionRadius = 0f;

    //9
    public string _playerName;

    //10 
    protected float _velocities;

    //11
    private MeshRenderer _meshRenderer;

    //12
    [Tooltip("Volume")] private float _volume;

    //13
    private bool _canAtack;

    //14
    // public static GameManager.Instance

    //15
    private List<Transform> _inventory;

    //16
    [FormerlySerializedAs("_speed")] 
    [SerializeField] private float _velocidad;

    //17
    private Vector3 _position = new Vector3(0f,0f,0f);

    //18
    public int _maxPlayers = 10;

    //19 *
    [SerializeField] internal float _detectionEnemy;

    //20 
    [SerializeField] private AudioSource AudioSource;
}
