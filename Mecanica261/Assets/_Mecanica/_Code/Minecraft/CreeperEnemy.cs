using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minecraft
{
    public class CreeperEnemy : Enemy, IAttacker
    {
        [Header("Attack")]
        [SerializeField] private float _damage = 40f;
        [SerializeField] private float _explosionRadius = 3f;
        [SerializeField] private float _chargeTime = 3f;
        [SerializeField] private float _chargedScale = 1.5f;
        [SerializeField] private float _cancelScaleTime = 0.4f;
        [SerializeField] private AnimationCurve _damageByDistance = new AnimationCurve();
        [SerializeField, HideInInspector] private float _lastCurveDamage;
        [SerializeField, HideInInspector] private float _lastCurveDistance;

        public float Damage { get { return _damage; } }

        private void OnValidate()
        {
            _damage = Mathf.Max(0f, _damage);
            _explosionRadius = Mathf.Max(0.01f, _explosionRadius);

            if (_damageByDistance == null || _damageByDistance.length < 2)
            {
                _damageByDistance = AnimationCurve.Linear(0f, _damage, _explosionRadius, 0f);
                _lastCurveDamage = _damage;
                _lastCurveDistance = _explosionRadius;
                return;
            }

            if (_lastCurveDamage <= 0f || _lastCurveDistance <= 0f)
            {
                _lastCurveDamage = GetHighestCurveValue();
                _lastCurveDistance = GetHighestCurveTime();
            }

            if (Mathf.Approximately(_lastCurveDamage, _damage) && Mathf.Approximately(_lastCurveDistance, _explosionRadius)) { return; }

            ScaleDamageCurve();
            _lastCurveDamage = _damage;
            _lastCurveDistance = _explosionRadius;
        }

        public override IEnumerator AttackMethod()
        {
            IMovementController movementController = GetComponent<IMovementController>();
            Vector3 startScale = transform.localScale;
            Vector3 chargedScale = startScale * _chargedScale;
            float chargeTimer = 0f;

            movementController?.StopMovement();
            InvokeCharge();

            while (chargeTimer < _chargeTime)
            {
                if (!IsTargetInExplosionRange())
                {
                    yield return ScaleOverTime(transform.localScale, startScale, _cancelScaleTime);
                    movementController?.ResumeMovement();
                    yield break;
                }

                chargeTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, chargedScale, chargeTimer / _chargeTime);
                yield return null;
            }

            if (!IsTargetInExplosionRange())
            {
                yield return ScaleOverTime(transform.localScale, startScale, _cancelScaleTime);
                movementController?.ResumeMovement();
                yield break;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

            foreach (Collider collider in colliders)
            {
                IDamagable damagable = collider.GetComponentInParent<IDamagable>();

                if (damagable == this) { continue; }

                if (damagable != null && damagedTargets.Add(damagable))
                {
                    damagable.ReceiveDamage(GetDamageAtPosition(collider.ClosestPoint(transform.position)));
                }
            }

            ReceiveDamage(MaxLife);
        }

        private bool IsTargetInExplosionRange()
        {
            return Vector3.Distance(transform.position, _targetTransform.position) <= _explosionRadius;
        }

        private float GetDamageAtPosition(Vector3 position)
        {
            float distance = Vector3.Distance(transform.position, position);

            return Mathf.Clamp(_damageByDistance.Evaluate(distance), 0f, Damage);
        }

        private void ScaleDamageCurve()
        {
            float distanceScale = _explosionRadius / _lastCurveDistance;
            float damageScale = _damage / _lastCurveDamage;
            Keyframe[] keys = _damageByDistance.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time *= distanceScale;
                keys[i].value *= damageScale;
                keys[i].inTangent *= damageScale / distanceScale;
                keys[i].outTangent *= damageScale / distanceScale;
            }

            _damageByDistance.keys = keys;
        }

        private float GetHighestCurveValue()
        {
            float highestValue = 0.01f;
            Keyframe[] keys = _damageByDistance.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                highestValue = Mathf.Max(highestValue, keys[i].value);
            }

            return highestValue;
        }

        private float GetHighestCurveTime()
        {
            float highestTime = 0.01f;
            Keyframe[] keys = _damageByDistance.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                highestTime = Mathf.Max(highestTime, keys[i].time);
            }

            return highestTime;
        }

        private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 targetScale, float duration)
        {
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}
