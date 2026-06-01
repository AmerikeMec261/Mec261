using UnityEngine;
using DG.Tweening;

namespace Minecraft
{
    public class VisualWeaponAnimator : MonoBehaviour
    {
        [Header("Use Offset")]
        [Tooltip("Local position offset reached at the peak of the use animation.")]
        [SerializeField] private Vector3 _usePositionOffset = Vector3.zero;
        [Tooltip("Local rotation offset reached at the peak of the use animation.")]
        [SerializeField] private Vector3 _useRotationOffset = new Vector3(35f, 0f, 0f);
        [Tooltip("Local scale offset reached at the peak of the use animation.")]
        [SerializeField] private Vector3 _useScaleOffset = Vector3.zero;

        [Header("Timing")]
        [Tooltip("Time to move from the starting pose to the use pose.")]
        [SerializeField] private float _useTime = 0.12f;
        [Tooltip("Time to return from the use pose to the starting pose.")]
        [SerializeField] private float _returnTime = 0.12f;
        [Tooltip("Curve controlling movement toward the use pose.")]
        [SerializeField] private AnimationCurve _useCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [Tooltip("Curve controlling movement back to the starting pose.")]
        [SerializeField] private AnimationCurve _returnCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Weapon _weapon;
        private Vector3 _startingLocalPosition;
        private Quaternion _startingLocalRotation;
        private Vector3 _startingLocalScale;
        private Tween _animationTween;
        private bool _hasStartingPose;

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
                _weapon.OnUsed += PlayUseAnimation;
            }
        }

        private void PlayUseAnimation()
        {
            SaveStartingPose();
            _animationTween?.Kill();
            ResetPose();

            Vector3 targetPosition = _startingLocalPosition + _usePositionOffset;
            Quaternion targetRotation = _startingLocalRotation * Quaternion.Euler(_useRotationOffset);
            Vector3 targetScale = _startingLocalScale + _useScaleOffset;

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
            transform.localPosition = Vector3.LerpUnclamped(_startingLocalPosition, targetPosition, amount);
            transform.localRotation = Quaternion.SlerpUnclamped(_startingLocalRotation, targetRotation, amount);
            transform.localScale = Vector3.LerpUnclamped(_startingLocalScale, targetScale, amount);
        }

        private void ResetPose()
        {
            transform.localPosition = _startingLocalPosition;
            transform.localRotation = _startingLocalRotation;
            transform.localScale = _startingLocalScale;
        }

        private void SaveStartingPose()
        {
            if (_hasStartingPose) { return; }

            _startingLocalPosition = transform.localPosition;
            _startingLocalRotation = transform.localRotation;
            _startingLocalScale = transform.localScale;
            _hasStartingPose = true;
        }

        private void Unsubscribe()
        {
            if (_weapon == null) { return; }

            _weapon.OnUsed -= PlayUseAnimation;
            _weapon = null;
        }
    }
}
