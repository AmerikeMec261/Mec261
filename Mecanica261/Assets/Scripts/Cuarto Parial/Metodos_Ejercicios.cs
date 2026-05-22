using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Collections.Generic;

public class Metodos_Ejercicios : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody; //Para el ejercicio 20

    private void Awake() //Para el ejercicio 20
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float _damage) //1
    {
        Debug.Log("Recibe Daño");
    }

    public bool StayinAlive() //2
    {
        return true;
    }

    public float CalculateDistance(Vector3 _distance1, Vector3 _distance2) //3
    {
        return Vector3.Distance(_distance1, _distance2);
    }

    public Vector3 NormalizedDirection(Vector3 _origin, Vector3 _destiny) //4
    {
        return (_origin - _destiny).normalized;
    }

    public void TryNamePlayer(string PlayerName) //5 
    {
        Debug.Log("Busca nombre");
    }

    public int CounterEnemies(List<GameObject> _listEnemies) //6
    {
        return _listEnemies.Count;
    }

    public GameObject FindTarget(List<GameObject> Enemies) //7
    {
        GameObject _closestEnemy = null;
        float _detectionRadius = 24f;

        foreach (GameObject _enemy in Enemies)
        {
            float _distance = Vector3.Distance(transform.position, _enemy.transform.position);

            if (_distance <= _detectionRadius)
            {
                _closestEnemy = _enemy;
            }
        }

       return _closestEnemy;
    }

    public void MovementPlayer(float _velocity, Vector3 _direction) //8
    {
        transform.Translate(_velocity * _direction * Time.deltaTime);
    }

    public void ConvertToRadius(float _degreesCelcius) //9
    {
        float _radius = _degreesCelcius * Mathf.Deg2Rad;
    }

    public bool ClosestPlayer(List<GameObject> Player, float _range, out GameObject PlayerFound) //10
    {
        PlayerFound = null;

        foreach (GameObject _player in Player)
        {
            float _distance = Vector3.Distance(transform.position, _player.transform.position);

            if (_distance <= _range)
            {
                PlayerFound = _player;
                return true;
            }
        }

        return false;
    }
    public bool Text(string _text, out int _result) //11
    {
        return int.TryParse(_text, out _result);
    }

    public Quaternion ReturnToQuaternion(float _angle) //12
    {
        return Quaternion.Euler(0, _angle, 0);
    }

    public List<GameObject> GetEnemies (float _radius) //13
    {
        List<GameObject> EnemiesInside = new List<GameObject>();

        foreach (GameObject Enemy in EnemiesInside)
        {
            float _distance = Vector3.Distance(transform.position, Enemy.transform.position);

            if (_distance <= _radius)
            {
                EnemiesInside.Add(Enemy);
            }
        }

        return EnemiesInside;
    }
    public void RestartPosition(Vector3 _position) //14
    {
        transform.position = _position;
    }

    public class Character //15
    {
        public virtual void Attack()
        {
            Debug.Log("La persona ataca");
        }
    }

    public class Warrior : Character //16 y 17
    {
        public override void Attack()
        {
            Debug.Log("La persona ataca");
        }
    }
    public float PercentageOfHealth(float _currentHealth, float _maxHealth = 100f) //18
    {
        return (_currentHealth / _maxHealth) * 100f;
    }

    public bool DodgeAttack(float _probability) //19
    {
        return Random.value <= _probability * 100f;
    }

    public void AddForce(Vector3 _direction, float _force) //20
    {
       _rigidbody.AddForce(_direction * _force);
    }



}
