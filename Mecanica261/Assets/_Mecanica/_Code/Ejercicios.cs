using UnityEngine;
using System.Collections.Generic;
using System;

public class Ejercicios : MonoBehaviour
{
    public void TakeDamage(float damage) // 1 
    {
        float CurrentHealth = 100;
        CurrentHealth -= damage;
    }

    public bool IsAlive(float Life)// 2 
    {
        return Life > 0;
    }

    public float Distance(Vector3 Point_A, Vector3 Point_B) // 3 
    {
        return Vector3.Distance(Point_A, Point_B);
    }

    public Vector3 DirectionNormalize(Vector3 origin, Vector3 Destiny) // 4 
    {
        return (origin - Destiny).normalized;
    }

    public string PlayerName(string PlayerName) // 5
    {
        return PlayerName;
    }

    public int CountEnemies(List<GameObject> enemies) // 6 
    {
        return enemies.Count;
    }

    /*public Enemys GetCloseEnemy(Vector3 PostionPlayer, List<Enemys> Enemies)// 7
    {
        Enemys closeEnemy = null;
        float DistancecloseEnemy = float.MaxValue;
        foreach (GameObject enemy in Enemies)
        {
            float distance = Vector3.Distance(PostionPlayer, enemy.transform.position);
            if (distance > DistancecloseEnemy)
            {
                DistancecloseEnemy = distance;
                closeEnemy = enemy;
            }
        }
        return closeEnemy;
    }*/

    public void MovePlayer(Transform Player, float Velocity, Vector3 Direction)// 8
    {
        Player.transform.position += Direction * Velocity * Time.deltaTime;
    }

    public float Radiales(float grades) // 9
    {
        return grades * Mathf.Deg2Rad;
    }

    public bool TryGetClosetsPlayer(float searchRange, out Player clostsPlayer)// 10_2
    {
        clostsPlayer = null;

        float closetsDistance = float.MaxValue;
        Vector3 currenPosition = transform.position;
        foreach(Player player in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(currenPosition, player.transform.position);
            if(distance <= searchRange && distance < closetsDistance)
            {
                closetsDistance = distance;
                clostsPlayer = player;
            }
        }
        return clostsPlayer != null;
    }
    /*public bool TryGetPlayer(out GameObject player)// 10 
    {
        player = GameObject.FindWithTag("PlayerName");
        return player != null;
    }*/

    public bool Text(string text, out int valor)// 11
    {
        return int.TryParse(text, out valor);
    }

    public Quaternion Rotation(float angle)// 12
    {
        return Quaternion.Euler(0, angle, 0);
    }

    public void GetEnemies(List<GameObject> Enemies, List<GameObject> Result)// 13
    {
        foreach (GameObject Enemy in Enemies)
        {
            Result.Add(Enemy);
        }
    }

    public void ResetPosition(Vector3 Spawn)// 14
    {
        transform.position = Spawn;
    }

    public class Character : MonoBehaviour // 15
    {
        public virtual void Attack()
        {
            Debug.Log("Character Attack");
        }
    }

    public class Warrior : Character // 16
    {
        public override void Attack()
        {
            Debug.Log("Warrior attacks");
        }
    }

    public class Warriors : Character // 17
    {
        public override void Attack()
        {
            base.Attack();
            Debug.Log("Warrior attacks");
        }
    }

    public float PercentengLife(float HealtMax, float CurrentHealt)// 18
    {

        return CurrentHealt / HealtMax * 100;
    }

    public bool CanDodge(float probability)// 19
    {
        return Random.value < probability;
    }

    public void ApplyForce(Rigidbody _rigidBody, float Force, Vector3 direction)// 20
    {
        _rigidBody.AddForce(direction * Force);
    }
}
