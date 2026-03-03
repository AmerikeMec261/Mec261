using UnityEngine;

public class CestaController : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;     
    [SerializeField] private float range = 2.0f;     
    private Vector3 startPosition;
    private bool goingRight = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;

        if (goingRight)
        {
            
            transform.position += Vector3.right * step;

           
            if (transform.position.x >= startPosition.x + range)
            {
                goingRight = false;
            }
        }
        else
        {
            
            transform.position += Vector3.left * step;

            
            float minX = startPosition.x; 
            if (transform.position.x <= minX)
            {
                goingRight = true;
            }
        }
    }
}

