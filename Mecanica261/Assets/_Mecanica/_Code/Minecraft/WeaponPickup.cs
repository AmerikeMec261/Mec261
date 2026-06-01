using UnityEngine;
using NaughtyAttributes;

namespace Minecraft
{
    public class WeaponPickup : MonoBehaviour, IPickable
    {
        [SerializeField, Required] private EquipableWeapons _equipableWeapons;
        [SerializeField] private bool _destroyAfterPickup = true;

        private void Awake()
        {
            if (_equipableWeapons == null) { _equipableWeapons = GetComponent<EquipableWeapons>(); }
        }

        public void Pick(PlayerPickup playerPickup)
        {
            if (playerPickup == null) { return; }

            playerPickup.EquipWeapon(_equipableWeapons);

            if (_destroyAfterPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}
