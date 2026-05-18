using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Variables : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _playerLevel;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _attackRange;
    [SerializeField] private List<String> _inventary;
    [SerializeField] private AudioSource _au;
    protected float _baseDamage;
    private float _baseHealth;
    private bool _isAlive;
    internal Array Index;
    public string PlayerName;
    protected float _enemySpeed;
    private MeshRenderer _mesh;
    [Tooltip("Volumen")] public float MasterVolume;
    private bool _canAttack;
    // public static GameManager Instance;
    [FormerlySerializedAs("Speed")] public float _rawSpeed;
    private Vector3 playerPosition;
    public int MaxPlayer;
    protected float _distDetection;
    


    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
