using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UIElements;

public class Ejercicios2 : MonoBehaviour
{
    private string _playerName = "hola";
    private void ReducePlayerHealth(int damageAmount) //1
    {        
        print($"Se redujo la vida en {damageAmount}");
    }

    private bool IsAlive (int health) //2
    {
        return health > 0;
    }

    private float DistanceAtoB(Vector3 startPosition, Vector3 targetPosition) //3
    {
        return Vector3.Distance(startPosition, targetPosition);
    }

    private Vector3 GetNormalized (Vector3 origin, Vector3 destination) //4
    {
        return (destination - origin).normalized;
    }

    private string GetPlayerName() //5
    {
        return _playerName;
    }

    public int CountEnemies(List<GameObject> enemyList) //6
    {
        return enemyList.Count;
    }

    private GameObject FindNearestEnemy (Vector3 playerPosition, List<GameObject> enemyList) //7
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

    private void VelocityNDirection (Vector3 moveDirection, float speed) //8
    {
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
    }

    public float ConvertDegreesToRadians(float angleInDegrees) //9
    {
        return angleInDegrees * Mathf.Deg2Rad;
    }
    
    public bool TryGetClosestPlayer(Vector3 currentPosition, float searchRange, out GameObject foundPlayer) //10
    {        
        foundPlayer = null;

        Collider[] colliders = Physics.OverlapSphere(currentPosition, searchRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                foundPlayer = col.gameObject;
                return true;
            }
        }
        return false;
    }
    
    public bool TryConvertTextToInt(string textValue, out int resultValue) //11
    {
        return int.TryParse(textValue, out resultValue);
    }
    
    public Quaternion GetRotationFromAngle(float angleInDegrees) //12
    {        
        return Quaternion.Euler(0f, 0f, angleInDegrees);
    }
    
    public void GetEnemiesInArea(Vector3 areaCenter, float areaRadius, List<GameObject> enemiesInArea) //13
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
    
    public void ResetPlayerPosition(Vector3 spawnPoint) //14
    {
        transform.position = spawnPoint;
    }

    public class BaseCharacter : MonoBehaviour //15
    {        
        public virtual void TakeDamage(int damageAmount)
        {
            Debug.Log($"El personaje recibe {damageAmount} de daño.");
        }
    }

    public class PlayerCharacter : BaseCharacter //16
    {        
        public override void TakeDamage(int damageAmount)
        {            
            base.TakeDamage(damageAmount);            
            Debug.Log("Además, el jugador actualiza su barra de vida en la pantalla.");
        }
    }

    public float GetHealthPercentage(float currentHealth, float maxHealth) //18
    {        
        if (maxHealth <= 0) return 0f;
        return currentHealth / maxHealth;
    }
    
    public bool CanDodgeAttack(float dodgeProbability) //19
    {
        float randomRoll = Random.Range(0f, 100f);
        return randomRoll <= dodgeProbability;
    }
    
    public void ApplyForceToRigidbody(Rigidbody targetRigidbody, Vector3 forceDirection, float forceAmount) //20
    {
        targetRigidbody.AddForce(forceDirection.normalized * forceAmount, ForceMode.Impulse);
    }
}
