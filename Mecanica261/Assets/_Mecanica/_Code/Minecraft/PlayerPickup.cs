using UnityEngine;

namespace Minecraft
{
    [RequireComponent(typeof(FirstPersonPlayer))]
    public class PlayerPickup : MonoBehaviour
    {
        private FirstPersonPlayer _player;
        private Weapon _spawnedWeapon;
        private GameObject _spawnedVisualWeapon;

        private void Awake()
        {
            _player = GetComponent<FirstPersonPlayer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_player.IsDead) { return; }

            IPickable pickable = other.GetComponentInParent<IPickable>();

            if (pickable == null) { return; }

            pickable.Pick(this);
        }

        public void EquipWeapon(EquipableWeapons equipableWeapons)
        {
            if (equipableWeapons == null) { return; }

            Transform actualWeaponSocket = GetSocket(equipableWeapons.ActualWeaponSocketTag);
            Transform visualWeaponSocket = GetSocket(equipableWeapons.VisualWeaponSocketTag);

            if (actualWeaponSocket == null) { return; }
            if (visualWeaponSocket == null) { return; }

            if (_spawnedWeapon != null)
            {
                Destroy(_spawnedWeapon.gameObject);
            }

            if (_spawnedVisualWeapon != null)
            {
                Destroy(_spawnedVisualWeapon);
            }

            Weapon weapon = Instantiate(equipableWeapons.ActualWeaponPrefab, actualWeaponSocket);
            ApplyLocalTransform(weapon.transform, equipableWeapons.ActualWeaponLocalPosition, equipableWeapons.ActualWeaponLocalRotation, equipableWeapons.ActualWeaponLocalScale);
            SetLayerRecursively(weapon.transform, equipableWeapons.ActualWeaponLayer);

            GameObject visualWeapon = Instantiate(equipableWeapons.VisualWeaponPrefab, visualWeaponSocket);
            ApplyLocalTransform(visualWeapon.transform, equipableWeapons.VisualWeaponLocalPosition, equipableWeapons.VisualWeaponLocalRotation, equipableWeapons.VisualWeaponLocalScale);
            SetLayerRecursively(visualWeapon.transform, equipableWeapons.GhostWeaponLayer);
            visualWeapon.GetComponentInChildren<VisualWeaponAnimator>()?.ListenTo(weapon);

            _spawnedWeapon = weapon;
            _spawnedVisualWeapon = visualWeapon;
            _player.SetWeapon(weapon);
            equipableWeapons.InvokeEquiped();
        }

        private Transform GetSocket(string socketTag)
        {
            if (string.IsNullOrEmpty(socketTag)) { return null; }

            Transform[] transforms = GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(socketTag)) { return transforms[i]; }
            }

            return null;
        }

        private void ApplyLocalTransform(Transform targetTransform, Vector3 localPosition, Vector3 localRotation, Vector3 localScale)
        {
            targetTransform.localPosition = localPosition;
            targetTransform.localEulerAngles = localRotation;
            targetTransform.localScale = localScale;
        }

        private void SetLayerRecursively(Transform targetTransform, int layer)
        {
            targetTransform.gameObject.layer = layer;

            for (int i = 0; i < targetTransform.childCount; i++)
            {
                SetLayerRecursively(targetTransform.GetChild(i), layer);
            }
        }
    }
}
