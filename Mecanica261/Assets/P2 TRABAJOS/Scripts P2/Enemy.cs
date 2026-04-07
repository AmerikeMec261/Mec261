using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour, IDamage, IEnemyInfo
{
    [Header("Estadistics")]
    [SerializeField] private float _life = 100f;
    [SerializeField] private float _shield = 0f;
    [SerializeField] private float _speed = 2f;

    //Estos seran los puntos los cuales se moveran los enemigos
    [Header("Points")]
    [SerializeField] private Transform _pointA; 
    [SerializeField] private Transform _pointB; 

    private Transform _currentPoint; //Es el punto actual en el que se esta moviendo
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _currentPoint = _pointB; //Primero comenzara dirigiendose al puntoB
        Data(); //Muestra la informacion del enemigo en la consola
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyMove(); //Llama a la funcion del enemigo para que se mueva el enemigo hacia el punto actual
    }

    private void EnemyMove()
    {
        if (_pointA == null || _pointB == null) //Si no esta asignado ninguno de los puntos este no se mueve
        {
            return;
        }
        //El enemigo avanza hacia el punto actual usando la velocidad y el tiempo del frame
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, _speed * Time.deltaTime);
        //Si el enemigo se encuentra cerca del punto actual cambia su direccion al otro punto
        if (Vector3.Distance(transform.position, _currentPoint.position) < 0.1f)
        {
            //En resumen en esas condiciones si esta en el punto a pasa o se dirige al B y viceversa
            if (_currentPoint == _pointA)
            {
                _currentPoint = _pointB;
            }
            else
            {
                _currentPoint = _pointA;
            }
        }

    }

    public void TakeDamage(float damage)
    {
        _life -= damage; //Aqui disminuye la vida segune el daño que reciba y en la consola se muestra cuanta vida le queda y cuanto dao recibio

        Debug.Log(gameObject.name + "Daño Recibido" + damage);
        Debug.Log(gameObject.name + "Vida Faltante" + _life);

        if (_life <= 0f) //Si la vida del enemigo llega a cero este se destruye
        {
            Destroy(gameObject);
        }
    }

    //En resumen estas 3 devuelven el valor actual de cada estadistica del enemigo
    public float Life()
    { 
        return _life; 
    }
    public float Shield()
    {
        return _shield;
    }
    public float Speed()
    {
        return _speed;
    }
    public void Data() //Muestra las estadisticas del enemigo en un solo mensaje
    {
        Debug.Log(gameObject.name + "Vida:" + Life() + "Escudo:" + Shield() + "Velocidad:" + Speed());
    }    
}
