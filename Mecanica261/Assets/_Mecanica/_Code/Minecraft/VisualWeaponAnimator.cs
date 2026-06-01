using UnityEngine;
using DG.Tweening;

namespace Minecraft
{
    public class VisualWeaponAnimator : MonoBehaviour
    {
        [Header("Use Offset")]
        [SerializeField] private Vector3 _usePositionOffset = Vector3.zero;
        [SerializeField] private Vector3 _useRotationOffset = new Vector3(35f, 0f, 0f);
        [SerializeField] private Vector3 _useScaleOffset = Vector3.zero;

        [Header("Timing")]
        [SerializeField] private float _useTime = 0.12f;
        [SerializeField] private float _returnTime = 0.12f;
        [SerializeField] private AnimationCurve _useCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private AnimationCurve _returnCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Weapon _weapon;
        private Vector3 _startLocalPosition;
        private Quaternion _startLocalRotation;
        private Vector3 _startLocalScale;
        private Tween _animationTween;
        private bool _hasStartPose;

        private void OnDisable()
        {
            _animationTween?.Kill();
            Unsubscribe();
        }

        public void ListenTo(Weapon weapon)
        {
            Unsubscribe();
            _weapon = weapon;

            if (_weapon != null)
            {
                _weapon.OnUsed += PlayAttack;
            }
        }

        private void PlayAttack()
        {
            SaveStartPose();
            _animationTween?.Kill();
            ResetPose();

            Vector3 targetPosition = _startLocalPosition + _usePositionOffset;
            Quaternion targetRotation = _startLocalRotation * Quaternion.Euler(_useRotationOffset);
            Vector3 targetScale = _startLocalScale + _useScaleOffset;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(DOVirtual.Float(0f, 1f, _useTime, value =>
            {
                ApplyPose(targetPosition, targetRotation, targetScale, _useCurve.Evaluate(value));
            }));
            sequence.Append(DOVirtual.Float(0f, 1f, _returnTime, value =>
            {
                ApplyPose(targetPosition, targetRotation, targetScale, 1f - _returnCurve.Evaluate(value));
            }));
            sequence.OnComplete(ResetPose);
            sequence.SetTarget(transform);

            _animationTween = sequence;
        }

        private void ApplyPose(Vector3 targetPosition, Quaternion targetRotation, Vector3 targetScale, float amount)
        {
            transform.localPosition = Vector3.LerpUnclamped(_startLocalPosition, targetPosition, amount);
            transform.localRotation = Quaternion.SlerpUnclamped(_startLocalRotation, targetRotation, amount);
            transform.localScale = Vector3.LerpUnclamped(_startLocalScale, targetScale, amount);
        }

        private void ResetPose()
        {
            transform.localPosition = _startLocalPosition;
            transform.localRotation = _startLocalRotation;
            transform.localScale = _startLocalScale;
        }

        private void SaveStartPose()
        {
            if (_hasStartPose) { return; }

            _startLocalPosition = transform.localPosition;
            _startLocalRotation = transform.localRotation;
            _startLocalScale = transform.localScale;
            _hasStartPose = true;
        }

        private void Unsubscribe()
        {
            if (_weapon == null) { return; }

            _weapon.OnUsed -= PlayAttack;
            _weapon = null;
        }
    }
}
