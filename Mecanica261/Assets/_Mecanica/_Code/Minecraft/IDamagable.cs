using UnityEngine.Events;

namespace Minecraft
{
    public interface IDamagable
    {
        float MaxLife { get; }
        float CurrentLife { get; }
        UnityEvent OnLifeChanged { get; }

        void ReceiveDamage(float damage);
    }
}
