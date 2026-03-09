using UnityEngine;

public class CestaController : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f;     
    [SerializeField] private float _range = 2.0f;     
    private Vector3 _startPosition;
    private bool _goingRight = true;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        float step = _speed * Time.deltaTime;

        if (_goingRight)
        {
            
            transform.position += Vector3.right * step;

           
            if (transform.position.x >= _startPosition.x + _range)
            {
                _goingRight = false;
            }
        }
        else
        {
            
            transform.position += Vector3.left * step;

            
            float minX = _startPosition.x; 
            if (transform.position.x <= minX)
            {
                _goingRight = true;
            }
        }
    }
}

