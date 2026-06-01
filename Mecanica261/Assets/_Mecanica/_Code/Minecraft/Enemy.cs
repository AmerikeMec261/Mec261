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
        [SerializeField] private int _score = 10;
        [SerializeField] private UnityEvent _onLifeChanged = new UnityEvent();
        [SerializeField] private UnityEvent _onReceiveDamage = new UnityEvent();
        [SerializeField] private UnityEvent _onDeath = new UnityEvent();

        [Header("Attack")]
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private UnityEvent _onCharge = new UnityEvent();
        [SerializeField] private UnityEvent _onAttack = new UnityEvent();

        private float _nextAttackTime;
        private bool _isAttacking;
        private bool _isDead;

        public float MaxLife { get { return _maxLife; } private set { _maxLife = value; } }
        public float CurrentLife { get { return _currentLife; } private set { SetCurrentLife(value); } }
        public int Score { get { return _score; } }
        public UnityEvent OnLifeChanged { get { return _onLifeChanged; } }
        public UnityEvent OnReceiveDamage { get { return _onReceiveDamage; } }
        public UnityEvent OnDeath { get { return _onDeath; } }
        public UnityEvent OnCharge { get { return _onCharge; } }
        public UnityEvent OnAttack { get { return _onAttack; } }

        protected virtual void Awake()
        {
            AssignPlayerTargetIfNeeded();
        }

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
            _onAttack.Invoke();

            yield return AttackMethod();

            _isAttacking = false;
        }

        public void ReceiveDamage(float damage)
        {
            if (_isDead) { return; }
            if (damage <= 0f) { return; }

            CurrentLife = Mathf.Max(CurrentLife - damage, 0f);
            _onReceiveDamage.Invoke();

            if (CurrentLife <= 0f)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            if (_isDead) { return; }

            _isDead = true;
            GameManager.Instance?.AddScore(Score);
            _onDeath.Invoke();
            Destroy(gameObject);
        }

        protected void InvokeCharge()
        {
            _onCharge.Invoke();
        }

        private void SetCurrentLife(float currentLife)
        {
            if (Mathf.Approximately(_currentLife, currentLife)) { return; }

            _currentLife = currentLife;
            _onLifeChanged.Invoke();
        }

        private void AssignPlayerTargetIfNeeded()
        {
            if (_targetTransform != null) { return; }

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                _targetTransform = playerObject.transform;
            }
        }
    }
}
