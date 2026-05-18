using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class Ejercicios : MonoBehaviour
{    
    [SerializeField] private float _movementSpeed = 5f; //1      
    [SerializeField] private Rigidbody _rigidbody; //5
    [SerializeField, Range(0f, 100f)] private float _range; //8 
    [Tooltip("Este ajusta el volumen")]
    [SerializeField] private float _volume; //12
    [FormerlySerializedAs("_speed")][SerializeField] private float _velocity;//16    
    [SerializeField] private AudioSource _audioSource; //20
    [SerializeField] protected float _enemyDetection; //19



    public string PlayerNAme; //9
    //public static GameManager instance;  //14
    public int MaxPlayers; //18    
    protected int damage = 10; //3
    internal int _saveIndex; //7
    protected internal float _enemyVelocity; //10
    private int _playerLevel; //2
    private int _playerHealth; //4
    private bool _isAlive; //6
    private MeshRenderer meshRenderer; //11
    private bool _canAttack; //13
    private List<GameObject> _items; //15
    private Vector3 _enemyPos; //17

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
