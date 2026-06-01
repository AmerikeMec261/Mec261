using UnityEngine;
using DG.Tweening;

namespace Minecraft
{
    public class WeaponArmAnimator : MonoBehaviour, IWeaponAnimationReceiver
    {
        [Header("Dependencies")]
        [SerializeField] private Transform[] _armPivots;

        [Header("Punch")]
        [SerializeField] private Vector3 _punchRotation = new Vector3(45f, 0f, 0f);
        [SerializeField] private float _punchOutTime = 0.08f;
        [SerializeField] private float _punchBackTime = 0.12f;

        [Header("Swing")]
        [SerializeField] private Vector3 _swingRotation = new Vector3(70f, 0f, 0f);
        [SerializeField] private float _swingOutTime = 0.12f;
        [SerializeField] private float _swingBackTime = 0.18f;

        [Header("Bow")]
        [SerializeField] private Vector3 _bowRecoilRotation = new Vector3(-15f, 0f, 0f);
        [SerializeField] private float _bowRecoilOutTime = 0.08f;
        [SerializeField] private float _bowRecoilBackTime = 0.16f;

        [Header("Aim")]
        [SerializeField] private Vector3 _aimRotation = new Vector3(90f, 0f, 0f);
        [SerializeField] private float _aimInTime = 0.2f;
        [SerializeField] private float _aimOutTime = 0.2f;

        private Vector3[] _startRotations;
        private ArmAnimator _armAnimator;
        private Tween _enableArmAnimatorTween;
        private bool _isAiming;

        private void Awake()
        {
            _armAnimator = GetComponent<ArmAnimator>();
            _startRotations = new Vector3[_armPivots.Length];

            for (int i = 0; i < _armPivots.Length; i++)
            {
                if (_armPivots[i] == null) { continue; }

                _startRotations[i] = _armPivots[i].localEulerAngles;
            }
        }

        public void PlayPunch()
        {
            PlayAnimation(_punchRotation, _punchOutTime, _punchBackTime);
        }

        public void PlaySwing()
        {
            PlayAnimation(_swingRotation, _swingOutTime, _swingBackTime);
        }

        public void PlayBowRecoil()
        {
            PlayAnimation(_bowRecoilRotation, _bowRecoilOutTime, _bowRecoilBackTime);
        }

        public void SetAiming(bool isAiming)
        {
            _isAiming = isAiming;

            if (_enableArmAnimatorTween != null)
            {
                _enableArmAnimatorTween.Kill();
                _enableArmAnimatorTween = null;
            }

            if (isAiming && _armAnimator != null) { _armAnimator.enabled = false; }

            MoveArmsToRotation(isAiming ? _aimRotation : Vector3.zero, isAiming ? _aimInTime : _aimOutTime);

            if (!isAiming && _armAnimator != null)
            {
                _enableArmAnimatorTween = DOVirtual.DelayedCall(_aimOutTime, () => _armAnimator.enabled = true);
            }
        }

        private void PlayAnimation(Vector3 rotation, float outTime, float backTime)
        {
            for (int i = 0; i < _armPivots.Length; i++)
            {
                if (_armPivots[i] == null) { continue; }

                int armIndex = i;
                Transform armPivot = _armPivots[i];
                Vector3 baseRotation = GetBaseRotation(armIndex);
                Vector3 targetRotation = baseRotation + rotation;

                armPivot.DOKill();
                armPivot.DOLocalRotate(targetRotation, outTime).OnComplete(() =>
                {
                    armPivot.DOLocalRotate(GetBaseRotation(armIndex), backTime);
                });
            }
        }

        private void MoveArmsToRotation(Vector3 rotation, float duration)
        {
            for (int i = 0; i < _armPivots.Length; i++)
            {
                if (_armPivots[i] == null) { continue; }

                _armPivots[i].DOKill();
                _armPivots[i].DOLocalRotate(_startRotations[i] + rotation, duration);
            }
        }

        private Vector3 GetBaseRotation(int index)
        {
            return _startRotations[index] + (_isAiming ? _aimRotation : Vector3.zero);
        }
    }
}
