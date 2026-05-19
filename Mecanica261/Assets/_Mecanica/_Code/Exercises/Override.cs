using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Override : MonoBehaviour
{
    [Header("Player Systems")]
    [SerializeField] private int _playerHealth = 100;
    [SerializeField] private int _damage = 10;
    private bool _isAlive;

    [Header("Distance")]
    private float _distanceX;
    private float _distanceY;

    void Start()
    {

    }


    void Update()
    {

    }


    void TakeDamage()
    {
        _playerHealth -= _damage;

        if (_playerHealth <= 0)
        {
            _isAlive = false;
            Debug.Log("Murio el player.");
        }
        else
        {
            _isAlive = true;
        }
    }

    void CalculateDistance(Vector2 playerPosition, Vector2 enemyPosition)
    {
        Vector3.Distance(playerPosition, enemyPosition);
        Vector3.Normalize(playerPosition - enemyPosition);
    }

    void PlayerName()
    {
        
    }
}
