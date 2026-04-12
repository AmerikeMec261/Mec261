
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 10;
    public int reward = 5;

    private int currentWaypoint = 0;
     GameObject[] waypoints;

  void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

  void Update()
  {
        if (waypoints.Length == 0) return;

        Vector3 target = waypoints[currentWaypoint].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed*Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypoint++;
            if(currentWaypoint>=waypoints.Length)
            {
                GameManager.Instance.LoseLife(1);
                Destroy(gameObject);
            }
        }
        
  }

    public void TakeDamage(int damage)
    {
        health-=damage;
        if(health <= 0)
        {
            GameManager.Instance.AddMoney(reward);
            Destroy(gameObject);
        }
    }
}
