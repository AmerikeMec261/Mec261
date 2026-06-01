using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using UnityEngine.Events;

namespace Minecraft
{
    public class Enemy : MonoBehaviour, IDamagable, IArrivalReceiver
    {
        [Header("Target")]
        [Tooltip("Target this enemy attacks when the AI reaches it.")]
        [SerializeField, Required] protected Transform _targetTransform;

        [Header("Life")]
        [Tooltip("Maximum life this enemy can have.")]
        [SerializeField] private float _maxLife = 100f;
        [Tooltip("Current life this enemy starts with.")]
        [SerializeField] private float _currentLife = 100f;
        [Tooltip("Score added when this enemy dies.")]
        [SerializeField] private int _score = 10;
        [Tooltip("Invoked whenever current life changes.")]
        [SerializeField] private UnityEvent _onLifeChanged = new UnityEvent();
        [Tooltip("Invoked after this enemy receives damage.")]
        [SerializeField] private UnityEvent _onReceiveDamage = new UnityEvent();
        [Tooltip("Invoked before this enemy is destroyed by death.")]
        [SerializeField] private UnityEvent _onDeath = new UnityEvent();

        [Header("Attack")]
        [Tooltip("Minimum time between attack starts.")]
        [SerializeField] private float _attackCooldown = 1f;
        [Tooltip("Invoked when this enemy begins a charge or aim phase.")]
        [SerializeField] private UnityEvent _onCharge = new UnityEvent();
        [Tooltip("Invoked when this enemy starts an attack routine.")]
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

        public virtual IEnumerator Attack()
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
            OnAttackStarted();

            yield return Attack();

            _isAttacking = false;
        }

        public void ReceiveDamage(float damage)
        {
            if (_isDead) { return; }
            if (damage <= 0f) { return; }

            CurrentLife = Mathf.Max(CurrentLife - damage, 0f);
            OnReceivedDamage();

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
            OnDied();
            Destroy(gameObject);
        }

        protected void OnChargeStarted()
        {
            _onCharge.Invoke();
        }

        protected void OnAttackStarted()
        {
            _onAttack.Invoke();
        }

        protected void OnReceivedDamage()
        {
            _onReceiveDamage.Invoke();
        }

        protected void OnDied()
        {
            _onDeath.Invoke();
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
