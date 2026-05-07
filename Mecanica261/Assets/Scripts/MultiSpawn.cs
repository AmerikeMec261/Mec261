using UnityEngine;

public class MultiSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] _allSpawns;
    [SerializeField] private GameObject _bulletPrefab;

    public void Fire()
    {
        foreach (Transform s in _allSpawns)
        {
            GameObject bullet = Instantiate(_bulletPrefab, s.position, s.rotation);

            bullet.GetComponent<IProjectile>()?.Fire();
        }
    }
}
