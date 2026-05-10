
using System.Data.Common;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public interface IDamageable //IDamagaeable sirve para crear un metodo común para todos los objetos dentro del juego que puedan recibir daño
    {
        int Health { get; }
        void TakeDamage(int damage);
        bool IsAlive { get; }  //El {get;} en esta línea y 2 arriba en el int Heakth, sirven para que cuando sean referenciados abajo se pueda leer el valor
    }

    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [Header("Health & Rewards")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int reward = 5;

    [Header("Waypoints")]
    private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private const float arrivalThreshold = 0.1f;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        if (waypoints.Length == 0)
        {
            Debug.LogWarning($"{name}: No waypoints found with tag 'Waypoint'. Destroying.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0 || !IsAlive) return;

        Vector3 targetPosition = waypoints[currentWaypointIndex].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);





        if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                GameManager.Instance.LoseLife(1); //Esta línea se utiliza para marcar que cuando el enemigo es alcanzado por el proyectil a la vida que tiene se le debe restar 1
                Destroy(gameObject);
            }
        }
    }

    
    public int Health => currentHealth;

    public bool IsAlive => currentHealth > 0;

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (!IsAlive)
        {
            
            Destroy(gameObject);
        }
    }
}
