using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float health = 100f;
    public float speed = 10f;

    public Transform target;

    void Update()
    {
        Move();
    }

    void Move
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed* Time.deltaTime);
    }

    public void Damage
    {
        health -= amount;

        if (health <= 0)
        {
          Destroy(gameObject);  
        }
    }

}

