using UnityEngine;

public class IDamage : MonoBehaviour
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
    }

}
