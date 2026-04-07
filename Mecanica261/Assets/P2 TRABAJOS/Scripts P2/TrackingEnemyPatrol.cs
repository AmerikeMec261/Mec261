using UnityEngine;

public class TrackingEnemyPatrol : MonoBehaviour // De aqui tambien utilice de codigo base el enemigo del examen de este parcial 
{
    [Header("Points")]
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [Header("Movement")]
    [SerializeField] private float _speed = 3f;

    private Transform _currentPoint;

    private void Start()
    {
        if (_pointA != null)
        {
            transform.position = _pointA.position;
        }

        _currentPoint = _pointB;
    }

    private void Update()
    {
        EnemyMove(); 
    }

    private void EnemyMove()
    {
        if (_pointA == null || _pointB == null || _currentPoint == null)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, _speed * Time.deltaTime);
       
        if (Vector3.Distance(transform.position, _currentPoint.position) < 0.1f)
        {
            
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

}
