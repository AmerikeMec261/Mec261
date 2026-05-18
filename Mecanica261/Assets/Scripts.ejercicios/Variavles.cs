using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Variavles : MonoBehaviour
{

    [SerializeField] private float _speed = 5.0f;//Ej.1
    [SerializeField] private Rigidbody _rigidbody;//Ej.5
    [Range(0, 100)]
    [SerializeField] private float _range;//Ej.8
    [HideInInspector] MeshRenderer _meshRenderer;//Ej.11
    [Tooltip("Volumen del Juego")]
    [SerializeField]float _music = 0f;//Ej.12
    [FormerlySerializedAs("_speed")]//Ej.16
    [Range(0, 100)]
    [SerializeField] protected float _detectionRange;
    [SerializeField] AudioSource _audioSource;//Ej.20

    public int LevelPlayer;//Ej.2
    protected float _damage = 10f;//Ej.3
    private int _playerHealth;//Ej.4
    private bool _isAlive;//Ej.6
    internal int _save;//Ej.7
    public string CharacterName;//Ej.9
    protected internal int _velocity;//Ej.10
    private bool _canAttack;//Ej.13
    //public static GameManager instiate;//Ej.14
    private List<Transform> _objects;//Ej.15
    private Vector3 _position;//Ej.17
    public int MaxPlayers = 100;//Ej.18


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
