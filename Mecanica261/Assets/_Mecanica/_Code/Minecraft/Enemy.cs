using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using UnityEngine.Events;

namespace Minecraft
{
    public class Enemy : MonoBehaviour, IDamagable, IArrivalReceiver
    {
        [Header("Target")]
        [SerializeField, Required] protected Transform _targetTransform;

        [Header("Life")]
        [SerializeField] private float _maxLife = 100f;
        [SerializeField] private float _currentLife = 100f;
        [SerializeField] private UnityEvent _onLifeChanged = new UnityEvent();

        [Header("Attack")]
        [SerializeField] private float _attackCooldown = 1f;

        private float _nextAttackTime;
        private bool _isAttacking;

        public float MaxLife { get { return _maxLife; } private set { _maxLife = value; } }
        public float CurrentLife { get { return _currentLife; } private set { SetCurrentLife(value); } }
        public UnityEvent OnLifeChanged { get { return _onLifeChanged; } }

        public virtual IEnumerator AttackMethod()
        {
            yield break;
        }

        public void OnArrival()
        {
            if (_isAttacking || Time.time < _nextAttackTime) { return; }

            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            _isAttacking = true;
            _nextAttackTime = Time.time + _attackCooldown;

            yield return AttackMethod();

            _isAttacking = false;
        }

        public void ReceiveDamage(float damage)
        {
            CurrentLife = Mathf.Max(CurrentLife - damage, 0f);

            if (CurrentLife <= 0f)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }

        private void SetCurrentLife(float currentLife)
        {
            if (Mathf.Approximately(_currentLife, currentLife)) { return; }

            _currentLife = currentLife;
            _onLifeChanged.Invoke();
        }
    }
}
