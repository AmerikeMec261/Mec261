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

        private Vector3[] _startRotations;

        private void Awake()
        {
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

        private void PlayAnimation(Vector3 rotation, float outTime, float backTime)
        {
            for (int i = 0; i < _armPivots.Length; i++)
            {
                if (_armPivots[i] == null) { continue; }

                Transform armPivot = _armPivots[i];
                Vector3 startRotation = _startRotations[i];
                Vector3 targetRotation = startRotation + rotation;

                armPivot.DOKill();
                armPivot.DOLocalRotate(targetRotation, outTime).OnComplete(() =>
                {
                    armPivot.DOLocalRotate(startRotation, backTime);
                });
            }
        }
    }
}
