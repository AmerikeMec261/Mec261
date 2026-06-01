using UnityEngine;
using NaughtyAttributes;
using System.Collections;

namespace Minecraft
{
    public class ArcherEnemy : Enemy
    {
        [Header("Attack")]
        [Tooltip("Weapon used when the archer fires.")]
        [SerializeField, Required] private Weapon _weapon;
        [Tooltip("Time spent aiming before the weapon is used.")]
        [SerializeField] private float _aimTime = 0.75f;
        [Tooltip("Time the archer waits after shooting before moving again.")]
        [SerializeField] private float _postShotWaitTime = 0.25f;
        [Tooltip("Degrees per second used to turn toward the target while aiming.")]
        [SerializeField] private float _turnSpeed = 360f;

        private IWeaponAnimationReceiver _weaponAnimationReceiver;

        protected override void Awake()
        {
            base.Awake();
            _weaponAnimationReceiver = GetComponent<IWeaponAnimationReceiver>();
        }

        public override IEnumerator Attack()
        {
            IMovementController movementController = GetComponent<IMovementController>();
            movementController?.StopMovement();
            OnChargeStarted();
            SetAiming(true);

            yield return WaitWhileTurning(_aimTime);

            _weapon.Use(_targetTransform);

            yield return WaitWhileTurning(_postShotWaitTime);

            SetAiming(false);
            movementController?.ResumeMovement();
        }

        private void SetAiming(bool isAiming)
        {
            if (_weaponAnimationReceiver == null) { return; }

            _weaponAnimationReceiver.SetAiming(isAiming);
        }

        private IEnumerator WaitWhileTurning(float duration)
        {
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                TurnTowardsTarget();
                yield return null;
            }
        }

        private void TurnTowardsTarget()
        {
            if (_targetTransform == null) { return; }

            Vector3 direction = _targetTransform.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0f) { return; }

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
        }
    }
}
