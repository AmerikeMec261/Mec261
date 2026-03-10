using UnityEngine;

public class TargetGoal : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Goool!");
        }
    }
}