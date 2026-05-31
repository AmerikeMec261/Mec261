using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Override : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    private int _playerHealth;
    private int maxHealth;
    private string _playerName = "Player";
    

    public void TakeDamage(int damage)
    {
        _playerHealth = Mathf.Max(0, _playerHealth - damage);  
    }

    public bool IsAlive()
    {
        return _playerHealth > 0;
    }

    public float GetDistance(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Vector3.Distance(firstPosition, secondPosition);
    }
    Vector3 Direction(Vector3 playerPosition, Vector3 enemyPosition)
    {
        return (playerPosition - enemyPosition).normalized;

    }

    public string PlayerName()
    {
        return _playerName;
    }

    public int EnemyCount(List<GameObject> _enemyList)
    {
        return _enemyList.Count;
    }

    public GameObject NearestEnemy(List<GameObject> enemies, Vector3 _playerPosition)
    {
        GameObject nearestEnemy = null;
        float closestDist = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(_playerPosition, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public void MovePlayer(Vector3 _direction, float _speed)
    {
        transform.position += _direction.normalized * _speed * Time.deltaTime;
    }

    public float _deegresToRadians(float _deegres)
    {
        return _deegres * Mathf.Deg2Rad;
    }

    public bool PlayerInRange(float searchRange, out GameObject closestPlayer)
    {
        closestPlayer = null;

        float closestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach (GameObject player in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(currentPosition, player.transform.position);

            if (distance < closestDistance && distance <= searchRange)
            {
                closestDistance = distance;
                closestPlayer = player;
            }

            return closestPlayer != null; 
        }
    }

    public bool _convertText(string text, out int result)
    {
        return int.TryParse(text, out result);
    }

    public Quaternion _deegresToQuatenion(float _deegres)
    {
        return Quaternion.Euler(0, _deegres, 0);
    }

    public void ObtainEnemiesArea(Vector3 center, float radius, List<GameObject> enemiesInArea)
    {
       enemiesInArea.Clear();
        Collider[] hits = Physics.OverlapSphere(center, radius);
        foreach (Collider hit in hits)
        {
            GameObject enemy = hit.GetComponent<GameObject>();

            if (enemy != null)
            {
                enemiesInArea.Add(enemy);
            }
        }
    }

    public void RestartPosition(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        transform.rotation = Quaternion.identity;

        if(_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public virtual void PerformAction()
    {
        Debug.Log("Acci�n base realizada. ");
    }

    public class Warrior : Override
    {
        public override void PerformAction()
        {
            Debug.Log("Guerrero realiza un ataque poderoso. ");
        }
    }

    public class InheritanceClass : Warrior
    {
        public override void PerformAction()
        {
            base.PerformAction();
            Debug.Log("El guerrero realiza un ataque poderoso. ");
        }
    }

    public float LifePercentage()
    {
        return (float)_playerHealth / maxHealth * 100f;
    }

    public bool IsDodgeable(float probability)
    {
        return Random.Range(0, 100f) == probability;
    }

    void ApplyForce(Vector3 direction, float force)
    {
        if (_rigidbody == null) return;
        _rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
