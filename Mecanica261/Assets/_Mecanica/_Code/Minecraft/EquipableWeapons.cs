using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Minecraft
{
    public class EquipableWeapons : MonoBehaviour
    {
        [Header("Actual Weapon")]
        [Tooltip("Functional weapon prefab spawned on the player.")]
        [SerializeField, Required] private Weapon _actualWeaponPrefab;
        [Tooltip("Socket tag used to place the functional weapon.")]
        [SerializeField, Tag, ValidateInput(nameof(HasActualSocketTag), "Actual weapon socket tag must be assigned.")]
        private string _actualWeaponSocketTag;
        [Tooltip("Local position applied to the functional weapon.")]
        [SerializeField] private Vector3 _actualWeaponLocalPosition;
        [Tooltip("Local rotation applied to the functional weapon.")]
        [SerializeField] private Vector3 _actualWeaponLocalRotation;
        [Tooltip("Local scale applied to the functional weapon.")]
        [SerializeField, ValidateInput(nameof(IsActualWeaponScaleValid), "Actual weapon scale values must be greater than 0.")]
        private Vector3 _actualWeaponLocalScale = Vector3.one;
        [Tooltip("Layer applied to the functional weapon and its children.")]
        [SerializeField, Layer] private int _actualWeaponLayer;

        [Header("Visual Weapon")]
        [Tooltip("Visual-only weapon prefab spawned on the player.")]
        [SerializeField, Required] private GameObject _visualWeaponPrefab;
        [Tooltip("Socket tag used to place the visual weapon.")]
        [SerializeField, Tag, ValidateInput(nameof(HasVisualSocketTag), "Visual weapon socket tag must be assigned.")]
        private string _visualWeaponSocketTag;
        [Tooltip("Local position applied to the visual weapon.")]
        [SerializeField] private Vector3 _visualWeaponLocalPosition;
        [Tooltip("Local rotation applied to the visual weapon.")]
        [SerializeField] private Vector3 _visualWeaponLocalRotation;
        [Tooltip("Local scale applied to the visual weapon.")]
        [SerializeField, ValidateInput(nameof(IsVisualWeaponScaleValid), "Visual weapon scale values must be greater than 0.")]
        private Vector3 _visualWeaponLocalScale = Vector3.one;
        [FormerlySerializedAs("_ghostWeaponLayer")]
        [Tooltip("Layer applied to the visual weapon and its children.")]
        [SerializeField, Layer] private int _visualWeaponLayer;

        [Header("Events")]
        [FormerlySerializedAs("_onEquiped")]
        [Tooltip("Invoked after this weapon setup is equipped.")]
        [SerializeField] private UnityEvent _onEquipped = new UnityEvent();

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
        public int VisualWeaponLayer { get { return _visualWeaponLayer; } }
        public UnityEvent OnEquipped { get { return _onEquipped; } }

        public void InvokeEquipped()
        {
            _onEquipped.Invoke();
        }

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
