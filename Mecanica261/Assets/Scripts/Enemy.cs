using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Ajustes del Enemigo")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _speed = 2f;

    [Header("Los puntos donde se mueve el enemigo")]
    [SerializeField] private Transform _puntotuno; //falta estandarización
    [SerializeField] private Transform _puntodos; //falta estandarización
    [SerializeField] private TextMeshPro _Text; //texto de qué? 

    private Transform _Target;

    private void Start()
    {
        _Target = _puntodos;
    }
    private void Update()
    {
        MoveEnemy();
    }
    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position,_Target.position,_speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _Target.position) < 0.1f)
        {
            _Target = _Target == _puntotuno ? _puntodos : _puntotuno;
        }
    }
    public void DamageEnemy(float damage)
    {
        _health -= damage;
        _Text.text = _health.ToString();
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}