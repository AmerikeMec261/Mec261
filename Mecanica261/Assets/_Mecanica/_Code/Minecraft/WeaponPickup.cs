using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Minecraft
{
    public class WeaponPickup : MonoBehaviour, IPickable
    {
        [SerializeField, Required] private EquipableWeapons _equipableWeapons;
        [SerializeField] private bool _destroyAfterPickup = true;
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
