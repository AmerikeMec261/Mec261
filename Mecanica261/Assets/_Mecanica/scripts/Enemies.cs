using System.ComponentModel;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public float health = 100f;
    public float speed = 10f;
     private float amount = 0;
    public float limit = 5f;

    public Transform target;

    Vector3 initialposition;

    void Start()
    {
        initialposition = transform.position;
    }

    void Update()
    {

        float movement = Mathf.PingPong(Time.time * speed, limit * 2) - limit;

        transform.position = new Vector3(
        initialposition.x,
         initialposition.y,
         initialposition.z + movement
        );
    }


    public void Damage( float damageToRecive)
    {
        health -= damageToRecive;

        if (health <= 0)
        {
          Destroy(gameObject);  
        }
    }

}
    

