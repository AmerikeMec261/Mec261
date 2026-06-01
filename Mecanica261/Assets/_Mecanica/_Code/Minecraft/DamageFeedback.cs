using UnityEngine;
using DG.Tweening;

namespace Minecraft
{
    public class DamageFeedback : MonoBehaviour
    {
        private const string BaseColorProperty = "_BaseColor";
        private const string ColorProperty = "_Color";

        [Header("Flash")]
        [Tooltip("Renderers that flash when this object loses life.")]
        [SerializeField] private Renderer[] _renderers;
        [Tooltip("Color applied during the damage flash.")]
        [SerializeField] private Color _damageColor = Color.red;
        [Tooltip("Time before flashed materials return to their original color.")]
        [SerializeField] private float _flashTime = 0.08f;

        [Header("Scale")]
        [Tooltip("Temporary scale offset applied when damage is received.")]
        [SerializeField] private Vector3 _scalePunch = new Vector3(0.08f, 0.08f, 0.08f);
        [Tooltip("Duration of the scale punch feedback.")]
        [SerializeField] private float _scalePunchTime = 0.12f;

        private IDamagable _damagable;
        private Material[][] _materials;
        private Color[][] _startColors;
        private string[][] _colorProperties;
        private Vector3 _startScale;
        private float _lastLife;

        private void Awake()
        {
            _damagable = GetComponent<IDamagable>();

            if (_damagable == null)
            {
                Debug.LogError($"{nameof(DamageFeedback)} requires an {nameof(IDamagable)} component on the same GameObject.", this);
                enabled = false;
                return;
            }

            _startScale = transform.localScale;
            if (_renderers.Length == 0) { _renderers = GetComponentsInChildren<Renderer>(); }
            StoreMaterials();
        }

        private void OnEnable()
        {
            if (_damagable == null) { return; }

            _lastLife = _damagable.CurrentLife;
            _damagable.OnLifeChanged.AddListener(OnLifeChanged);
        }

        private void OnDisable()
        {
            if (_damagable == null) { return; }

            _damagable.OnLifeChanged.RemoveListener(OnLifeChanged);
            KillTweens();
        }

        private void OnLifeChanged()
        {
            if (_damagable.CurrentLife < _lastLife)
            {
                PlayFeedback();
            }

            _lastLife = _damagable.CurrentLife;
        }

        private void PlayFeedback()
        {
            Flash();
            PunchScale();
        }

        private void StoreMaterials()
        {
            _materials = new Material[_renderers.Length][];
            _startColors = new Color[_renderers.Length][];
            _colorProperties = new string[_renderers.Length][];

            for (int i = 0; i < _renderers.Length; i++)
            {
                _materials[i] = _renderers[i].materials;
                _startColors[i] = new Color[_materials[i].Length];
                _colorProperties[i] = new string[_materials[i].Length];

                for (int j = 0; j < _materials[i].Length; j++)
                {
                    _colorProperties[i][j] = GetColorProperty(_materials[i][j]);

                    if (!string.IsNullOrEmpty(_colorProperties[i][j]))
                    {
                        _startColors[i][j] = _materials[i][j].GetColor(_colorProperties[i][j]);
                    }
                }
            }
        }

        private void Flash()
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                for (int j = 0; j < _materials[i].Length; j++)
                {
                    Material material = _materials[i][j];
                    Color startColor = _startColors[i][j];
                    string colorProperty = _colorProperties[i][j];

                    if (string.IsNullOrEmpty(colorProperty)) { continue; }

                    material.DOKill();
                    material.SetColor(colorProperty, _damageColor);
                    material.DOColor(startColor, colorProperty, _flashTime);
                }
            }
        }

        private void PunchScale()
        {
            transform.DOKill();
            transform.localScale = _startScale;
            transform.DOPunchScale(_scalePunch, _scalePunchTime);
        }

        private string GetColorProperty(Material material)
        {
            if (material.HasProperty(BaseColorProperty)) { return BaseColorProperty; }
            if (material.HasProperty(ColorProperty)) { return ColorProperty; }

            return string.Empty;
        }

        private void KillTweens()
        {
            transform.DOKill();

            for (int i = 0; i < _materials.Length; i++)
            {
                for (int j = 0; j < _materials[i].Length; j++)
                {
                    _materials[i][j].DOKill();
                }
            }
        }
    }
}
