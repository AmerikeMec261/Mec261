using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EjercicioMetodos : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private int _currentHealth = 100;
    private int _maxHealth = 100;
    private string _playerName = "Player";

    //Reducir vida del jugador
    public void TakeDamage(int damageAmount)
    {
        _currentHealth=Mathf.Max(0,_currentHealth-damageAmount);
    }

    //El jugador sigue vivo
    public bool IsAlive()
    {
        return _currentHealth>0;
    }

    //Distancia entre dos posiciones
    public float GetDistance(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Vector3.Distance(firstPosition, secondPosition);
    }

    //Dirección normalizada
    public Vector3 GetDirection (Vector3 originPosition, Vector3 targetPosition)
    {
        return (targetPosition - originPosition).normalized;
    }

    //Nombre actual del jugador
    public string GetPlayerName()
    {
        return _playerName;
    }

    //Lista de enemigos
    public int GetEnemyCount(List<Enemy> enemies)
    {
        return enemies.Count;
    }

    //Enemigo más cercano
    public Enemy GetClosestEnemy(List<Enemy> enemies, Vector3 playerPosition)
    {
        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;
        
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition,enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    //Movimiento del jugador
    public void Move (Vector3 direction, float speed)
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    //Grados a Radianes
    public float DegreesToRadians(float degrees)
    {
        return degrees*Mathf.Deg2Rad;
    }

    //Jugador más cercano
    public bool TryGetClosestPlayer(float searchRange, out Player closestPlayer)
    {
        closestPlayer = null;

        float closestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach (Player player in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(currentPosition, player.transform.position);

            if (distance <= searchRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }

        }
        return closestPlayer != null;
    }

   //Texto a entero
   public bool TryParseInt(string text, out int resultValue)
    {
        return int.TryParse(text, out resultValue);
    }

    //Angulo a rotacion
    public Quaternion GetRotation(float angleInDegrees)
    {
        return Quaternion.Euler(0f, angleInDegrees, 0f);
    }

    //Enemigos en un área
    public void GetEnemiesInArea(Vector3 centerPosition, float radius, List<Enemy> enemiesInArea)
    {
        enemiesInArea.Clear();

        Collider[] hits = Physics.OverlapSphere(centerPosition, radius);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemiesInArea.Add(enemy);
            }
        }
    }

    //Reiniciar posición del jugador
    public void ResetPosition(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
        transform.rotation = Quaternion.identity;

        if(_rigidbody != null)
        {
          _rigidbody.velocity = Vector3.zero;
          _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    //Metodo virtual en clase base
    public virtual void PerformAction()
    {
        Debug.Log("Accion base del personaje.");
    }

    //Porcentaje de vida
    public float GetHealthPercent()
    {
        return (float)_currentHealth / _maxHealth * 100f;
    }

    //Esquivar
    public bool CanDodge(float dodgeChance)
    {
        return Random.value <= Mathf.Clamp01(dodgeChance);
    }

    //Fuerza al Rigidbody
    public void ApplyForce(Vector3 direction, float forceMagnitude)
    {
        if (_rigidbody == null) { return; }

        _rigidbody.AddForce(direction.normalized * forceMagnitude, ForceMode.Impulse);
    }

    //Override del metodo virtual
    public class Warrior:EjercicioMetodos
    {
        public override void PerformAction()
        {
            Debug.Log("El guerrero realiza un ataque poderoso");
        }
    }

    //Override usando base.PerformAction
    public class WarriorWithBaseCall:EjercicioMetodos
    {
        public override void PerformAction()
        {
            base.PerformAction();
            Debug.Log("El guerrero realiza un ataque poderoso");
        }
    }


    public class Player : MonoBehaviour
    {

    }

    public class Enemy : MonoBehaviour
    { }
}
