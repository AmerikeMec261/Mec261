using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ejercicio : MonoBehaviour
{
    [SerializeField] private float _speed = 5f; //1

    public float CurrentPlayerLevel; //2

    protected float Basedamege = 10f; //3

    private protected float _currentHealth; //4

    [SerializeField] private Rigidbody _rigidbody; //5

    private bool _life; //6

    internal float _index; //7

    [Range(0f, 100f)]
    [SerializeField] private float _rangeAttack; //8

    public string PlayerName; //9

    public float _movementSpeed; //10

    private MeshRenderer meshRenderer; //11

    [SerializeField][Tooltip("ayuda")] private float _volume = 0f; //12

    private bool _isAlive => _currentHealth > 0f; //13

    //public static GameManager Instance; //14

    private List<GameObject> _inventory = new List<GameObject>(); //15

    [FormerlySerializedAs("_speed")] private float _speed2; //16

    private Vector3 _playerPosition; //17

    public float MaxPlayers; //18

    [Range(0f, 50f)]
    [SerializeField] protected float _enemyRadar; //19

    [SerializeField] private AudioSource _audioSource; //20


}
