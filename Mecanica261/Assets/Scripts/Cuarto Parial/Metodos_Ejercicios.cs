
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Metodos_Ejercicios : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody; //Para el ejercicio 20
     private int _currentHealth = 100;
     private int _maxHealth = 100;
  

    private void Awake() //Para el ejercicio 20
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int damage) //1
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
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

    public string TryNamePlayer(string _playerName) //5 
    {
        return _playerName;
    }

    public int CounterEnemies(List<GameObject> _listEnemies) //6
    {
        return _listEnemies.Count;
    }

    public Enemy GetClosestEnemy(List<Enemy> enemies, Vector3 playerPosition)
    {
        Enemy _closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                _closestEnemy = enemy;
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

    /*
    public bool TryGetClosestPlayer(float searchRange, out Player closestPlayer ) //10
    {
        closestPlayer = null;

        float closestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach(Player player in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(currentPosition, player.transform.position);

            if (distance <= searchRange && distance < closestDistance)
            {
                closestDistance= distance;
                closestPlayer = player;
            }
        }

        return closestPlayer != null;
    }
    */

    public bool Text(string _text, out int _result) //11
    {
        return int.TryParse(_text, out _result);
    }

    public Quaternion ReturnToQuaternion(float _angle) //12
    {
        return Quaternion.Euler(0, _angle, 0);
    }

    
    public void GetEnemiesInArea(Vector3 centerPosition, float radius, List<Enemy> enemiesInArea) //13
    {
        enemiesInArea.Clear();
        Collider[] hits = Physics.OverlapSphere(centerPosition, radius);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemiesInArea.Add(enemy);
            }
        }
    }
    
    public void RestartPosition(Vector3 _position) //14
    {
        transform.position = _position;
        transform.rotation = Quaternion.identity;

        if(_rigidbody != null)
        {
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
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
