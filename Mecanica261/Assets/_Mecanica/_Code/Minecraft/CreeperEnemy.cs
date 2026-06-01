using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Minecraft
{
    public class CreeperEnemy : Enemy, IAttacker
    {
        [Header("Attack")]
        [Tooltip("Maximum explosion damage at the center of the blast.")]
        [SerializeField] private float _damage = 40f;
        [Tooltip("Radius used to find targets damaged by the explosion.")]
        [SerializeField] private float _explosionRadius = 3f;
        [Tooltip("Time the creeper must stay in range before exploding.")]
        [SerializeField] private float _chargeTime = 3f;
        [FormerlySerializedAs("_chargedScale")]
        [Tooltip("Scale multiplier reached at the end of the charge.")]
        [SerializeField] private float _chargedScaleMultiplier = 1.5f;
        [FormerlySerializedAs("_cancelScaleTime")]
        [Tooltip("Time used to return to normal scale when the charge is cancelled.")]
        [SerializeField] private float _cancelChargeScaleTime = 0.4f;
        [FormerlySerializedAs("_damageByDistance")]
        [Tooltip("Damage dealt by distance from the explosion center.")]
        [SerializeField] private AnimationCurve _damageByDistanceCurve = new AnimationCurve();
        [FormerlySerializedAs("_lastCurveDamage")]
        [Tooltip("Stored curve damage value used to preserve curve shape after edits.")]
        [SerializeField, HideInInspector] private float _lastCurveMaximumDamage;
        [FormerlySerializedAs("_lastCurveDistance")]
        [Tooltip("Stored curve distance value used to preserve curve shape after edits.")]
        [SerializeField, HideInInspector] private float _lastCurveMaximumDistance;

        [Header("Events")]
        [Tooltip("Invoked when the explosion is applied.")]
        [SerializeField] private UnityEvent _onExplode = new UnityEvent();

        public float Damage { get { return _damage; } }
        public UnityEvent OnExplode { get { return _onExplode; } }

        private void OnValidate()
        {
            _damage = Mathf.Max(0f, _damage);
            _explosionRadius = Mathf.Max(0.01f, _explosionRadius);

            if (_damageByDistanceCurve == null || _damageByDistanceCurve.length < 2)
            {
                _damageByDistanceCurve = AnimationCurve.Linear(0f, _damage, _explosionRadius, 0f);
                _lastCurveMaximumDamage = _damage;
                _lastCurveMaximumDistance = _explosionRadius;
                return;
            }

            if (_lastCurveMaximumDamage <= 0f || _lastCurveMaximumDistance <= 0f)
            {
                _lastCurveMaximumDamage = GetHighestCurveDamage();
                _lastCurveMaximumDistance = GetHighestCurveDistance();
            }

            if (Mathf.Approximately(_lastCurveMaximumDamage, _damage) && Mathf.Approximately(_lastCurveMaximumDistance, _explosionRadius)) { return; }

            ScaleDamageCurveToInspectorValues();
            _lastCurveMaximumDamage = _damage;
            _lastCurveMaximumDistance = _explosionRadius;
        }

        public override IEnumerator Attack()
        {
            IMovementController movementController = GetComponent<IMovementController>();
            Vector3 startScale = transform.localScale;
            Vector3 chargedScale = startScale * _chargedScaleMultiplier;
            float chargeTimer = 0f;

            movementController?.StopMovement();
            OnChargeStarted();

            while (chargeTimer < _chargeTime)
            {
                if (!IsTargetInExplosionRange())
                {
                    yield return ScaleToSize(transform.localScale, startScale, _cancelChargeScaleTime);
                    movementController?.ResumeMovement();
                    yield break;
                }

                chargeTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, chargedScale, chargeTimer / _chargeTime);
                yield return null;
            }

            if (!IsTargetInExplosionRange())
            {
                yield return ScaleToSize(transform.localScale, startScale, _cancelChargeScaleTime);
                movementController?.ResumeMovement();
                yield break;
            }

            OnExploded();

            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

            foreach (Collider collider in colliders)
            {
                IDamagable damagable = collider.GetComponentInParent<IDamagable>();

                if (ReferenceEquals(damagable, this)) { continue; }

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

            return Mathf.Clamp(_damageByDistanceCurve.Evaluate(distance), 0f, Damage);
        }

        private void ScaleDamageCurveToInspectorValues()
        {
            float distanceScale = _explosionRadius / _lastCurveMaximumDistance;
            float damageScale = _damage / _lastCurveMaximumDamage;
            Keyframe[] keys = _damageByDistanceCurve.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time *= distanceScale;
                keys[i].value *= damageScale;
                keys[i].inTangent *= damageScale / distanceScale;
                keys[i].outTangent *= damageScale / distanceScale;
            }

            _damageByDistanceCurve.keys = keys;
        }

        private float GetHighestCurveDamage()
        {
            float highestValue = 0.01f;
            Keyframe[] keys = _damageByDistanceCurve.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                highestValue = Mathf.Max(highestValue, keys[i].value);
            }

            return highestValue;
        }

        private float GetHighestCurveDistance()
        {
            float highestTime = 0.01f;
            Keyframe[] keys = _damageByDistanceCurve.keys;

            for (int i = 0; i < keys.Length; i++)
            {
                highestTime = Mathf.Max(highestTime, keys[i].time);
            }

            return highestTime;
        }

        private IEnumerator ScaleToSize(Vector3 startScale, Vector3 targetScale, float duration)
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

        private void OnExploded()
        {
            _onExplode.Invoke();
        }
    }
}
