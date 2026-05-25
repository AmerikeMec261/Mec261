using UnityEngine;
using System.Collections.Generic;

public class metodos : MonoBehaviour
{
    public int health = 100;

    public void ReceiveDamage(int damage)
    {
        health -= damage;
    } //1

    public int life = 100;

    public bool IsAlive()
    {
        return life > 0;
    } //2

    public float CalculateDistance(Vector3 pointA, Vector3 pointB)
    {
        return Vector3.Distance(pointA, pointB);
    } //3

    public Vector3 GetDirection(Vector3 origin, Vector3 destination)
    {
        return (destination - origin);
    } //4

    public string playerName = "Gael";

    public string GetPlayerName()
    {
        return playerName;
    } //5

    public List<GameObject> enemies = new List<GameObject>();

    public int CountEnemies()
    {
        return enemies.Count;
    } //6

    public GameObject FindClosestEnemy(List<GameObject> enemies)
    {
        GameObject closestEnemy = null;

        float minimumDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                closestEnemy = enemy;
            }
        } //7

        return closestEnemy;
    }

    public void MovePlayer(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    } //8

    public float ConvertToRadians(float degrees)
    {
        return degrees * Mathf.Deg2Rad;
    } //9

    public bool TryFindPlayer(out GameObject player)
    {
        player = GameObject.FindWithTag("Player");

        return player != null;
    } //10

    public bool ConvertText(string text, out int number)
    {
        return int.TryParse(text, out number);
    } //11

    public Quaternion GetRotation(float angle)
    {
        return Quaternion.Euler(0, angle, 0);
    } //12

    public List<Collider> GetEnemies(Vector3 center, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius);

        List<Collider> enemies = new List<Collider>();

        foreach (Collider col in colliders)
        {
            enemies.Add(col);
        }

        return enemies;
    } //13

    public Transform spawnPoint;

    public void ResetPosition()
    {
        transform.position = spawnPoint.position;
    } //14

    public class Character : MonoBehaviour
    {
        public virtual void Attack()
        {
            Debug.Log("The character attacks.");
        }
    } //15

    public class Warrior : Character
    {
        public override void Attack()
        {
            Debug.Log("The warrior uses a sword.");
        }
    } //16

    public class Archer : Character
    {
        public override void Attack()
        {
            base.Attack();

            Debug.Log("The archer shoots an arrow.");
        }
    } //17

    public float CalculateHealthPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    } //18

    public bool DodgeAttack(float probability)
    {
        return Random.value < probability;
    } //19

    public Rigidbody rb;

    public void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    } //20











    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
