using UnityEngine;
using NaughtyAttributes;

namespace Minecraft
{
    public class EquipableWeapons : MonoBehaviour
    {
        [Header("Actual Weapon")]
        [SerializeField, Required] private Weapon _actualWeaponPrefab;
        [SerializeField, Tag, ValidateInput(nameof(HasActualSocketTag), "Actual weapon socket tag must be assigned.")]
        private string _actualWeaponSocketTag;
        [SerializeField] private Vector3 _actualWeaponLocalPosition;
        [SerializeField] private Vector3 _actualWeaponLocalRotation;
        [SerializeField, ValidateInput(nameof(IsActualWeaponScaleValid), "Actual weapon scale values must be greater than 0.")]
        private Vector3 _actualWeaponLocalScale = Vector3.one;
        [SerializeField, Layer] private int _actualWeaponLayer;

        [Header("Visual Weapon")]
        [SerializeField, Required] private GameObject _visualWeaponPrefab;
        [SerializeField, Tag, ValidateInput(nameof(HasVisualSocketTag), "Visual weapon socket tag must be assigned.")]
        private string _visualWeaponSocketTag;
        [SerializeField] private Vector3 _visualWeaponLocalPosition;
        [SerializeField] private Vector3 _visualWeaponLocalRotation;
        [SerializeField, ValidateInput(nameof(IsVisualWeaponScaleValid), "Visual weapon scale values must be greater than 0.")]
        private Vector3 _visualWeaponLocalScale = Vector3.one;
        [SerializeField, Layer] private int _ghostWeaponLayer;

        public Weapon ActualWeaponPrefab { get { return _actualWeaponPrefab; } }
        public string ActualWeaponSocketTag { get { return _actualWeaponSocketTag; } }
        public Vector3 ActualWeaponLocalPosition { get { return _actualWeaponLocalPosition; } }
        public Vector3 ActualWeaponLocalRotation { get { return _actualWeaponLocalRotation; } }
        public Vector3 ActualWeaponLocalScale { get { return _actualWeaponLocalScale; } }
        public int ActualWeaponLayer { get { return _actualWeaponLayer; } }

        public GameObject VisualWeaponPrefab { get { return _visualWeaponPrefab; } }
        public string VisualWeaponSocketTag { get { return _visualWeaponSocketTag; } }
        public Vector3 VisualWeaponLocalPosition { get { return _visualWeaponLocalPosition; } }
        public Vector3 VisualWeaponLocalRotation { get { return _visualWeaponLocalRotation; } }
        public Vector3 VisualWeaponLocalScale { get { return _visualWeaponLocalScale; } }
        public int GhostWeaponLayer { get { return _ghostWeaponLayer; } }

        private bool HasActualSocketTag()
        {
            return !string.IsNullOrEmpty(_actualWeaponSocketTag);
        }

        private bool HasVisualSocketTag()
        {
            return !string.IsNullOrEmpty(_visualWeaponSocketTag);
        }

        private bool IsActualWeaponScaleValid()
        {
            return _actualWeaponLocalScale.x > 0f && _actualWeaponLocalScale.y > 0f && _actualWeaponLocalScale.z > 0f;
        }

        private bool IsVisualWeaponScaleValid()
        {
            return _visualWeaponLocalScale.x > 0f && _visualWeaponLocalScale.y > 0f && _visualWeaponLocalScale.z > 0f;
        }
    }
}
