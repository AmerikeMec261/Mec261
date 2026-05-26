using System.Collections.Generic;
using UnityEngine;

public class Ejercicio_Metodos : MonoBehaviour
{
    private string _playerName;

    public void ReceiveDamage(int Damage)//Ej.1
    {

    }

    public bool IsPlayerAlive(int PlayerHealth)//Ej.2
    {
        return PlayerHealth > 0;
    }

    public float Distance(Vector3 Position1, Vector3 Position2) //Ej.3
    {
        return Vector3.Distance(Position1,Position2);
    }

    private Vector3 GetNormalized(Vector3 origin, Vector3 destination) //Ej.4
    {
        return (destination - origin).normalized;
    }

    private string GetPlayerName() //Ej.5
    {
        return _playerName;
    }

    public int Enemies(List<GameObject> enemyList) //Ej.6
    {
        return enemyList.Count;
    }

    private GameObject FindEnemy(Vector3 playerPosition, List<GameObject> enemyList) //Ej.7
    {
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemyList)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    private void VelocityAndDirection(Vector3 moveDirection, float speed) //Ej.8
    {
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
    }

    public float ConvertDegreesToRadians(float angleInDegrees) //Ej.9
    {

        return angleInDegrees * Mathf.Deg2Rad;
    }

    public bool ClosestPlayer(Vector3 currentPosition, float searchRange, out GameObject player) //Ej.10
    {
        player = null;

        Collider[] colliders = Physics.OverlapSphere(currentPosition, searchRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                player = col.gameObject;
                return true;
            }
        }
        return false;
    }

    public bool ConvertText(string textValue, out int resultValue) //Ej.11
    {
        return int.TryParse(textValue, out resultValue);
    }

    public Quaternion RotationAngle(float angleInDegrees) //Ej.12
    {
        return Quaternion.Euler(0f, 0f, angleInDegrees);
    }

    public void EnemiesInArea(Vector3 areaCenter, float areaRadius, List<GameObject> enemiesInArea) //Ej.13
    {
        enemiesInArea.Clear();
        Collider[] colliders = Physics.OverlapSphere(areaCenter, areaRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemiesInArea.Add(col.gameObject);
            }
        }
    }

    public void ResetPlayerPosition(Vector3 spawnPoint) //Ej.14
    {

        transform.position = spawnPoint;
    }

    public class BaseCharacter : MonoBehaviour //Ej.15
    {
        public virtual void TakeDamage(int damageAmount)
        {
            Debug.Log($"Tu personaje ha recibido {damageAmount} de daño.");
        }
    }

    public class PlayerCharacter : BaseCharacter //Ej.16
    {
        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            Debug.Log("Jugador actualiza su barra de vida.");
        }
    }

    public float HealthPercentage(float currentHealth, float maxHealth) //Ej.18
    {
        
        if (maxHealth <= 0) return 0f;
        return currentHealth / maxHealth;
    }

    public bool CanDodgeAttack(float dodgeProbability) //Ej.19
    {
        float randomRoll = Random.Range(0f, 100f);
        return randomRoll <= dodgeProbability;
    }

    public void ApplyForce(Rigidbody targetRigidbody, Vector3 forceDirection, float forceAmount) //Ej.20
    {
        targetRigidbody.AddForce(forceDirection.normalized * forceAmount, ForceMode.Impulse);
    }
}
