using TMPro;
using UnityEngine;

public class Ejercicio2 : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]private float _playerLife = 100;
    [SerializeField]private float _damage = 10f;
    [SerializeField]private Vector3 _distance;
    [SerializeField] private bool _Alive;
    [SerializeField] private string _nametag;

    [Header("Rigidbody")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    public virtual void TakeDamage() //1
    {
        _playerLife -= _damage;
    }

    public virtual void PlayerAlive(bool IsAlive) //2
    {
        if (_playerLife > 0f)
        {
            IsAlive = true;
            _Alive = IsAlive;
        }
        else
        {
            IsAlive = false;
            _Alive = IsAlive;
        }     
    }
    public void Distance() //3
    {
        Vector3 distance = (_player.transform.position - _enemy.transform.position);
        _distance = distance;
    }

    public void Direction() //4 
    {
       _distance = _distance.normalized;
    }

    public virtual void PlayerName() //5
    {
        
    }
}
