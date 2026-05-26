using System.Collections.Generic;
using UnityEngine;

public class Codigos_Dos : MonoBehaviour
{
    [SerializeField] public float _playerLife = 100f;

    [SerializeField] public float _damage = 100f;

    public string _playerName = "Player";

    public Rigidbody _rigidbody;


    // 1
    public void Damage(float damageQuantity)
    {
        _playerLife -= _damage;
    }


    // 2
    public bool PlayerAlive()
    {
        if (_playerLife < 0)
        { return false; } else { return true; }
    }


    // 3
    private float CalculateDistance(Vector3 origin, Vector3 end)
    {
        return Vector3.Distance(origin, end);
    }

    // 4
    private Vector3 GetDirection(Vector3 origin, Vector3 end)
    {
        return (origin - end);
    }

    //5
    private string PlayerName()
    {
        return _playerName;
    }

    //6
    public int _numberEnemys(List<GameObject> enemyList)
    {
        return enemyList.Count;
    }

    //7
    private GameObject EnemyClose(List<GameObject> enemyList)
    {
        GameObject enemigoCercano = null;
        return enemigoCercano;
    }


    //8
    public void PlayerMovement(float velocity, Vector3 direccion)
    {
        transform.position += direccion * velocity;
    }

    //9
    private float GradeToRadians(float grades)
    {
        return grades * Mathf.Deg2Rad;
    }


    //10

    //private GameObject PlayerClose(List<GameObject> playerList)
    //{return grades* Mathf.Deg2Rad;}

    //11  

    public bool ConvertToInt(string Text, out int Value)
    {
        return int.TryParse(Text, out Value);
    }

    //12
    public Quaternion Rotation(float angle)
    {
        return Quaternion.Euler(0, angle, 0);
    }

    //13
    public void EnemiesRange(Vector3 Center, float Radius, List<GameObject> Enemys)
    {
        return;
    }

    //14
    public void ResetPosition(Vector3 Spawn)
    {
        transform.position = Spawn;
    }

    //15
    public virtual void Attack()
    {
     Debug.Log("Enemy ATTACK");
    }



    /*   16 Y 17
     
    public override void Attack()
    {
    Debug.Log("Enemy ATTACK");
    }


    public override void Attack()
    {
    base.Attack();
    Debug.Log("ATTACK");
    }

    */


    //18
    public float HealthPorcentaje(float Health, float MaxHealth)
    {
     return Health / MaxHealth;
    }


    //19
    public bool TryDodge(float Dodge, float Random)
    {
    return Random <= Dodge;
    }


    //20
    public void ApplyForce(Vector3 Direction, float Force)
    {
    //Rigidbody.AddForce(Direction * Force);
        return;
    }

    // LE PUSE RETUTN ES PARA QUE NO ME MARQUE ERROR :)
}
