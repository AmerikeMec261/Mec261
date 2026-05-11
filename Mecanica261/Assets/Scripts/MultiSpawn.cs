using UnityEngine;

public class MultiSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] _allSpawns;
    [SerializeField] private GameObject _bulletPrefab;

    public void Fire()
    {
        Quaternion baseRotation = transform.rotation;

        Quaternion correctedRotation = baseRotation * Quaternion.AngleAxis(90, Vector3.up);

        foreach (Transform s in _allSpawns)
        {
            GameObject bullet = Instantiate(_bulletPrefab, s.position, correctedRotation);

            bullet.GetComponent<IProjectile>()?.Fire();
        }
    }

    public Transform GetFirstSpawn()
    {
        return (_allSpawns != null && _allSpawns.Length > 0) ? _allSpawns[0] : transform;
    }
}
