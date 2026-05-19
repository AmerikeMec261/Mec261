using UnityEngine;


public class MetodosProgra : MonoBehaviour
{
    public int CurrentHealth = 100;
    public string PlayerName = "Diegux";

   
    
    
    
    
    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public bool IsAlive()
    {
        return false;
    }

    public float PlayerDistance(Vector3 positionA, Vector3 positionB)
    {
        return 0f;
    }
    
    public Vector3 GetDirection(Vector3 origin, Vector3 Destiny)
    {
        return (Destiny - origin);
    } 

    public string PlayerName2()
    {
        return PlayerName;
    }



}
