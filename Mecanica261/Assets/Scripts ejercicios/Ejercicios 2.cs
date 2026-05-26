using UnityEngine;

public class Ejercicios2 : MonoBehaviour
{
    
    [SerializeField] private int _playerHealth = 100;
    [SerializeField] private int _damage = 5;
    private string _playername 


    private void PlayerDamage() // ejercicio 1
    {
        _playerHealth -= _damage;
    }

    private void PlayerAlive(bool player Alive) // Ejercicio 2
    {
        return _playerHealth > 0;
    
    }


    private void CalculateDistance()  //3
    { 
     return Vector3.Distance(origin, destiny);
    }

    private Vector3 GetDirection() //4
    {
    return (destiny - origin).normalized;
    }

     private string PlayerName()   //5

    {
       return _playername 
    }

    private void  Enemies(List<GameObject> Enemylist) //6
    {
        return Enemylist.count;
    }

    public Enemy GetClosestEnemy(List<Enemy> enemies, Vector3 playerPosition) //7
    {
        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach(Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    private void PlayerMove(Vector3 direction, float celocity) //8
    {
    transform.position +=direction.normalized * velocity * Time.deltaTime;
    }

    private float GradeToRadians(float grades) //9
    {
     return grades * Mathf.Deg2Rad;
    }

    public bool TryGetClosestPlayer(float searchRange, out Player closestPlayer)//10
    {
        closestPlayer = null;

        float closestDistance = float.MaxValue;
        Vector3 currentPosition = transform.position;

        foreach(Player player in FindObjectsByTypre)
    }    

    private Quaternion AngleRotation() //12

    {
    return Quaternion.Euler(0, gradesangles, 0);
    }



    */
}
