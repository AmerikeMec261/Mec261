using System.Collections.Generic;
using UnityEngine;

public class Codigos : MonoBehaviour
{
    [SerializeField] private float _velocity = 5;

    public int actualLevel;

    protected int _damage = 10;

    private int _vida = 0;

    [SerializeField] private Rigidbody _rigidbody;

    private bool _isAlive;

    internal string _saveGame;

    [Range(0, 100)]
    [SerializeField] private int _attack;

    public string _playername;

    [SerializeField] private float _velocity_movement = 5;

    [HideInInspector] MeshRenderer _playerRenderer;

    [Tooltip("Ayuda")]
    [SerializeField] private int volumen;

    public bool _enemy_attack;

    public static // GameManager instance;

     List<Transform> _inventoryobjects;

    [SerializeField] private float _speed;

    private Vector3 _Player;

    private int _maxPlayers = 100;

    [SerializeField] internal string distance;

    [SerializeField] private AudioSource _audioSource;

}
