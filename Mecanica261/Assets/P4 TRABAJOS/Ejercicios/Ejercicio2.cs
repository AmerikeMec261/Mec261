using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Ejercicio2 : MonoBehaviour
{
    [Header("Variables en Inspector Player")]
    [SerializeField] private float _playerLife = 500;
    [SerializeField] private float _speedPlayer = 10;
    [SerializeField] private float _force = 10;
    [SerializeField] private string _nametag;
    [SerializeField] private string _conversion;

    [Header("Lista")]
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private List<GameObject> _players;
    private List<GameObject> _enemyListFound = new List<GameObject>();

   [Header("Area")]
    [SerializeField]private Vector3 _min;
    [SerializeField] private Vector3 _max;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _rigidBodyPlayer;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Transform _emptyForce;
    [SerializeField] private Transform _playerRotation;
    [SerializeField] private Transform _reset;

    private Vector3 _angle = new Vector3(0f, 90f, 0f);
    private Vector3 _playerPosition;
    private Vector3 _distance;
    private int _totalEnemies;
    private bool _IsAlive;
    private bool _dodge;
    private bool _playerfound;
    private bool _enemyfound;


    public void TakeDamage(float damage) //1
    {
        _playerLife -= damage;
        Debug.Log($"El jugador recibio {damage} de daño");
    }

    public void PlayerAlive() //2
    {
        if (_playerLife == 0f)
        {
            _IsAlive = false;
        }
        else
        {
            _IsAlive = true;
        }
    }
    public void Distance() //3
    {
        Vector3 distance = (_rigidBodyPlayer.transform.position - _enemy.transform.position);
        _distance = distance;
    }

    public void Direction() //4 
    {
        _distance = _distance.normalized;
    }

    public void PlayerName() //5
    {
        Debug.Log($"Nombre del jugador:{_nametag}");
    }

    public void EnemyList() //6
    {
        _totalEnemies = _enemies.Count;
        Debug.Log($"Hay un total de {_totalEnemies} enemigos");
    }

    public void NearEnemy() //7
    {
        int nearEnemy = 0;
        GameObject ie = _enemies[nearEnemy];

        for (int i = 0; i < _enemies.Count; i++)
        {
            GameObject enemies = _enemies[i];

            Vector3 position = enemies.transform.position - _rigidBodyPlayer.transform.position; 

            Vector3 nearPosition = ie.transform.position - _rigidBodyPlayer.transform.position;

            if (position.magnitude < nearPosition.magnitude) //Link de .magnitude: https://docs.unity3d.com/ScriptReference/Vector3-magnitude.html
            {
                nearEnemy = i;
                ie = _enemies[nearEnemy];
                Debug.Log($" El enemigo mas cercano es {nearEnemy}");
            }
        
        }
    }

    public void MovePlayer(Vector3 movement) //8
    {
        movement = _distance;
        _rigidBodyPlayer.MovePosition(transform.position * movement.z * _speedPlayer * Time.deltaTime);
    }

    public void Rad() //9
    {
        _playerPosition.y = (_playerPosition.y * 90f) * Mathf.Deg2Rad;
    }
    public void NearPlayer(out GameObject ip) //10
    {
        int nearPlayer = 0;
        ip = _players[nearPlayer];

        for (int i = 0; i < _players.Count; i++)
        {
            GameObject players = _players[i];

            Vector3 playerposition = players.transform.position;

            Vector3 position = players.transform.position - _rigidBodyPlayer.transform.position;

            Vector3 nearPosition = ip.transform.position - _rigidBodyPlayer.transform.position;

            if (playerposition.magnitude >= _min.magnitude && playerposition.magnitude <= _max.magnitude)
            {
                _playerfound = true;
                Debug.Log($" Se encontro el jugador {players} de la lista");

                if (position.magnitude < nearPosition.magnitude) //Link de .magnitude: https://docs.unity3d.com/ScriptReference/Vector3-magnitude.html
            {
                nearPlayer = i;
                ip = _players[nearPlayer];
                Debug.Log($" El jugador mas cercano es {nearPlayer}");
            }
            }
        }
    }
    public void StringToInt () //11
    {

        if (int.TryParse(_conversion, out int result))
        {
            Debug.Log($"La conversacion fue exitosa {result} ");
        }        
        else
        {
            Debug.Log($"La opcion {result} no es una opcion valida");
        }
    }
    public void Angles() //12
    { 
        _playerRotation.localRotation = Quaternion.Euler(_angle);
    }
    public void FoundEnemy() //13 Link de List Class 1: https://discussions.unity.com/t/cant-find-list-manual-scripting-api/773528 y 2: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=netframework-4.6
    {
        Vector3 positionEnemy = _enemy.transform.position;

        if (positionEnemy.magnitude >= _min.magnitude && positionEnemy.magnitude <= _max.magnitude)
        {
            _enemyfound = true;
            _enemyListFound.Add(_enemy); 
            Debug.Log($" Se encontro el jugador {_players} de la lista");

        }
    }
    public void ResetPoosition() //14
    {
        if (_IsAlive == false)
        {
            _rigidBodyPlayer.transform.position = _reset.transform.position;
        }
        
    }
    public virtual void Attack() //15
    {
        Debug.Log("El jugador dispara");
    }

    public class Enemy : Ejercicio2 //16
    {
        public override void Attack() 
        {
            base.Attack();//17
            Debug.Log("El enemigo dispara");
        }

    }
    public void CurrentLife() //18
    {
        float MaxLife = 500f; 

        float percentage = _playerLife/ MaxLife * 100f;
        Debug.Log($"El jugador tiene el {percentage} procentaje de su vida");

    }
    public void Dodge() //19 //Link de Random.Range:https://docs.unity3d.com/ScriptReference/Random.Range.html  
    {
        int probability = Random.Range(0,2);

        if ( probability == 1)
        {
            _dodge = true;
            Debug.Log("El enemigo esquivo el ataque");
        }
        else 
        {
            _dodge = false;
            Debug.Log("El enemigo no esquivo el ataque");
        }
    }
    private void ApplyForce() //20
    {
        Vector3 directionForce = transform.forward;
        _rigidBodyPlayer.AddForceAtPosition(directionForce, _emptyForce.position, ForceMode.Force);
    }


}
