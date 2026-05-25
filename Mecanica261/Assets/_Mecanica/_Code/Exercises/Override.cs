using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Override : MonoBehaviour
{
    int _playerHealth;
    string _playerName;
    private bool _isAlive;


    void Start()
    {

    }


    void Update()
    {

    }


    void TakeDamage(int damage)
    {
        _playerHealth -= damage;  
    }

    bool IsAlive()
    {
        return _playerHealth > 0;
    }

    Vector3 Direction(Vector3 playerPosition, Vector3 enemyPosition)
    {
        return (playerPosition - enemyPosition).normalized;

    }

    string PlayerName()
    {
        return _playerName;
    }

    int EnemyCount(List<GameObject> _enemyList)
    {
        return _enemyList.Count;
    }

    GameObject NearestEnemy(List<GameObject> _enemies, Vector3 _playerPosition)
    {
        GameObject nearestEnemy = null;
        float _minDist = Mathf.Infinity;
        foreach (GameObject e in _enemies)
        {
            float dist = Vector3.Distance(_playerPosition, e.transform.position);
            if (dist < _minDist)
            {
                _minDist = dist;
                nearestEnemy = e;
            }
            return nearestEnemy;
        }
    }

    void MovePlayer(Vector3 _direction, float _speed)
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    float _deegresToRadians(float _deegres)
    {
        return _deegres * Mathf.Deg2Rad;
    }

    bool _playerInRange(float _range, out GameObject _playerFound)
    {
        GameObject player;
        float dist = Vector3.Distance(transform.position, player.transform.position);
        _playerFound = (dist < _range) ? player : null;
        return _playerFound != null;
    }

    bool _convertText(string _text, out int _result)
    {
        return int.TryParse(_text, out _result);
    }

    Quaternion _deegresToQuatenion(float _deegres)
    {
        return Quaternion.Euler(0, _deegres, 0);
    }

    void ObtainEnemiesArea(Vector3 _center, float _radius, List<GameObject> _list)
    {
        Collider[] _colliders = Physics.OverlapBox(transform.position, _center);
        foreach (var c in _colliders)
        {
            if (c.CompareTag("Enemigo"))
            {
                _list.Add(c.gameObject);
            }
        }
    }

    void RestarPosition(Vector3 _spawnPoint)
    {
        transform.position = _spawnPoint;
    }

    public class BaseClass
    {
        public virtual void Action()
        {
            // Acciones de base
        }
    }

    public class InheritanceClass : BaseClass
    {
        public override void Action()
        {
            base.Action();
        }
    }

    float _lifePercentage(float _actualLife, float _maxLife)
    {
        return (_actualLife / _maxLife) * 100f;
    }

    bool IsDodgeable(float _probability)
    {
        return Random.Range(0, 100f) == _probability;
    }

    void ApplyForce(Rigidbody _rigidbody, Vector3 _direction, float _force)
    {
        _rigidbody.AddForce(_direction * _force, ForceMode.Impulse);
    }
}
