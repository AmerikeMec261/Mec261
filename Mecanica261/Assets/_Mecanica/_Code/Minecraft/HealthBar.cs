using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;

namespace Minecraft
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField, Required, ValidateInput(nameof(IsDamagableScriptValid), "Script must implement IDamagable.")]
        private MonoBehaviour _damagableScript;
        [SerializeField] private Image[] _emptyHeartImages = new Image[10];
        [SerializeField] private Image[] _fullHeartImages = new Image[10];

        [Header("Settings")]
        [SerializeField] private float _halfHeartFillAmount = 0.55f;

        [Header("Animation")]
        [SerializeField] private float _jumpHeight = 10f;
        [SerializeField] private float _jumpDuration = 0.09f;
        [SerializeField] private float _heartDelay = 0.025f;
        [SerializeField] private float _dangerShakeHeight = 4f;
        [SerializeField] private float _dangerShakeDuration = 0.06f;
        [SerializeField] private float _dangerShakeStartDelay = 0.055f;
        [SerializeField] private float _dangerLoopDelay = 0.3f;

        private IDamagable _damagable;
        private Vector3[] _emptyStartingPositions;
        private Vector3[] _fullStartingPositions;
        private float[] _jumpOffsets;
        private float[] _shakeOffsets;
        private Sequence _waveSequence;
        private Sequence _dangerSequence;
        private bool _isDangerAnimating;

        private void OnEnable()
        {
            StoreStartingPositions();
            _damagable = _damagableScript as IDamagable;
            _damagable.OnLifeChanged.AddListener(UpdateHealthBar);
            UpdateHealthBar();
        }

        private void OnDisable()
        {
            _damagable.OnLifeChanged.RemoveListener(UpdateHealthBar);
            StopWaveAnimation();
            StopDangerAnimation();
        }

        private bool IsDamagableScriptValid()
        {
            return _damagableScript is IDamagable;
        }

        private void UpdateHealthBar()
        {
            float lifePercentage = Mathf.Clamp01(_damagable.CurrentLife / Mathf.Max(_damagable.MaxLife, 1f));
            float heartValue = lifePercentage * _fullHeartImages.Length;

            for (int i = 0; i < _fullHeartImages.Length; i++)
            {
                _fullHeartImages[i].fillAmount = GetHeartFillAmount(heartValue - i);
            }

            if (heartValue <= 1f)
            {
                StartDangerAnimation();
            }
            else
            {
                StopDangerAnimation();
                PlayWaveAnimation();
            }
        }

        private float GetHeartFillAmount(float heartValue)
        {
            if (heartValue >= 1f) { return 1f; }
            if (heartValue >= 0.5f) { return _halfHeartFillAmount; }

            return 0f;
        }

        private void StoreStartingPositions()
        {
            _emptyStartingPositions = new Vector3[_emptyHeartImages.Length];
            _fullStartingPositions = new Vector3[_fullHeartImages.Length];
            _jumpOffsets = new float[_fullHeartImages.Length];
            _shakeOffsets = new float[_fullHeartImages.Length];

            for (int i = 0; i < _emptyHeartImages.Length; i++)
            {
                _emptyStartingPositions[i] = _emptyHeartImages[i].rectTransform.localPosition;
            }

            for (int i = 0; i < _fullHeartImages.Length; i++)
            {
                _fullStartingPositions[i] = _fullHeartImages[i].rectTransform.localPosition;
            }
        }

        private void Update()
        {
            if (_isDangerAnimating)
            {
                UpdateDangerShake();
            }
        }

        private void PlayWaveAnimation()
        {
            StopWaveAnimation();

            _waveSequence = DOTween.Sequence();

            for (int i = 0; i < _fullHeartImages.Length; i++)
            {
                int heartIndex = i;
                float delay = i * _heartDelay;

                _waveSequence.Insert(delay, CreateHeartJumpTween(heartIndex));
            }
        }

        private Tween CreateHeartJumpTween(int heartIndex)
        {
            return DOTween.To(() => _jumpOffsets[heartIndex], value =>
                {
                    _jumpOffsets[heartIndex] = value;
                    ApplyHeartPosition(heartIndex);
                }, _jumpHeight, _jumpDuration)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _jumpOffsets[heartIndex] = 0f;
                    ApplyHeartPosition(heartIndex);
                });
        }

        private void StartDangerAnimation()
        {
            if (_dangerSequence != null && _dangerSequence.IsActive()) { return; }

            StopWaveAnimation();
            _dangerSequence = DOTween.Sequence();

            for (int i = 0; i < _fullHeartImages.Length; i++)
            {
                int heartIndex = i;
                float delay = i * _heartDelay;

                _dangerSequence.Insert(delay, CreateHeartJumpTween(heartIndex));
            }

            _dangerSequence.AppendInterval(_dangerLoopDelay);
            _dangerSequence.SetLoops(-1, LoopType.Restart);
            _isDangerAnimating = true;
        }

        private void UpdateDangerShake()
        {
            for (int i = 0; i < _fullHeartImages.Length; i++)
            {
                float timingOffset = GetShakeDelay(i);
                float shakeTime = (Time.time + timingOffset) / _dangerShakeDuration;
                _shakeOffsets[i] = Mathf.Sin(shakeTime * Mathf.PI * 2f) * _dangerShakeHeight;
                ApplyHeartPosition(i);
            }
        }

        private float GetShakeDelay(int heartIndex)
        {
            return ((heartIndex * 3) % _fullHeartImages.Length) * _dangerShakeStartDelay;
        }

        private void StopWaveAnimation()
        {
            if (_waveSequence != null)
            {
                _waveSequence.Kill();
                _waveSequence = null;
            }

            ResetHeartPositions();
        }

        private void StopDangerAnimation()
        {
            if (_dangerSequence != null)
            {
                _dangerSequence.Kill();
                _dangerSequence = null;
            }

            _isDangerAnimating = false;
            ResetHeartPositions();
        }

        private void ResetHeartPositions()
        {
            for (int i = 0; i < _fullHeartImages.Length; i++)
            {
                _emptyHeartImages[i].rectTransform.DOKill();
                _fullHeartImages[i].rectTransform.DOKill();
                _jumpOffsets[i] = 0f;
                _shakeOffsets[i] = 0f;
                ApplyHeartPosition(i);
            }
        }

        private void ApplyHeartPosition(int heartIndex)
        {
            Vector3 offset = Vector3.up * (_jumpOffsets[heartIndex] + _shakeOffsets[heartIndex]);

            _emptyHeartImages[heartIndex].rectTransform.localPosition = _emptyStartingPositions[heartIndex] + offset;
            _fullHeartImages[heartIndex].rectTransform.localPosition = _fullStartingPositions[heartIndex] + offset;
        }
    }
}
