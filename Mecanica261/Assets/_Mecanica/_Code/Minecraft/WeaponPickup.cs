using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Minecraft
{
    public class WeaponPickup : MonoBehaviour, IPickable
    {
        [Tooltip("Weapon setup given to the player when collected.")]
        [SerializeField, Required] private EquipableWeapons _equipableWeapons;
        [Tooltip("Destroys this pickup object after a successful pickup.")]
        [SerializeField] private bool _destroyAfterPickup = true;
        [Tooltip("Invoked after this pickup is collected.")]
        [SerializeField] private UnityEvent _onPicked = new UnityEvent();

        public UnityEvent OnPicked { get { return _onPicked; } }

        private void Awake()
        {
            if (_equipableWeapons == null) { _equipableWeapons = GetComponent<EquipableWeapons>(); }
        }

        public void Pick(PlayerPickup playerPickup)
        {
            if (playerPickup == null) { return; }

            playerPickup.EquipWeapon(_equipableWeapons);
            _onPicked.Invoke();

            if (_destroyAfterPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}
