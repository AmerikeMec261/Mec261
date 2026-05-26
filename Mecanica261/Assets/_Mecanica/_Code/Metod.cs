using Unity.VisualScripting;
using UnityEngine;

public class Metod : MonoBehaviour
{
    [SerializeField] private float _currentHealth = 100f;
    void DamagePLayer(int Damage = -1)
    {
        float currentHealth = _currentHealth;
        currentHealth = -Damage;
    }

    void IsDead(bool IsDead)
    {
        if(_currentHealth > 0)
        {
            IsDead = false;
        }
        else if (_currentHealth <= 0) 
        {
            IsDead = true;
        }
    }

    
}
