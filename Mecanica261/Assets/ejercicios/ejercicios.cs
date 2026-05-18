
using System;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Serialization;

public class ejercicios : MonoBehaviour
{
    [SerializeField] private float _movementvelocity = 5f; //1
    [SerializeField] private int _levelplayer = 1; //2
    protected int _daño = 1; //3
    protected int _vidaplayer = 1; //4
    [SerializeField] private Rigidbody _rigiBody;//5
    [SerializeField] protected bool _muerto = false;//6
    internal int _saveIndex;//7
    [SerializeField, Range(0f, 100f)] private float _range;//8
    public string PlayerName;//9
    protected internal float Velocity = 1f;//10
    internal MeshRenderer _render;//11
    [Tooltip("Ayuda")] public float Volume = 1f;//12
     protected bool _atacaEnemy = false;//13
    //public static GameManager instance; 14
     private List <GameObject> _inventario = new List<GameObject>();//15
    [FormerlySerializedAs("_speed")] public float Speed = 1f;//16
    private Vector3 _position; //17
    public int Maxplayers = 10; //18
    [SerializeField] protected float _enemyDetection; //19
    [SerializeField]  private AudioSource _audio;//20

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
