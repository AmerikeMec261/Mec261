using UnityEngine;
using static IDamage;

public class CestaController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _shield = 50f;
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _range = 5.0f;

    private Vector3 _startPosition;
    private bool _goingRight = true;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move() // Usa calculos con sin o puedes usar terniario. 
    {
        float step = _speed * Time.deltaTime;
        if (_goingRight)
        {
            transform.position += Vector3.right * step;
            if (transform.position.x >= _startPosition.x + _range) _goingRight = false;
        }
        else
        {
            transform.position += Vector3.left * step;
            if (transform.position.x <= _startPosition.x) _goingRight = true;
        }
    }

    // Metodo del daño recibido ante la bala
    public void TakeDamage(float amount)
    {
        if (_shield > 0)
        {
            _shield -= amount;
            if (_shield < 0)
            {
                _health += _shield;
                _shield = 0;
            }
        }
        else
        {
            _health -= amount;
        }

        Debug.Log($"{gameObject.name} - Vida: {_health} | Escudo: {_shield}"); // utiliza "this" para claridad
        
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemigo destruido");
        Destroy(gameObject);
    }
}

