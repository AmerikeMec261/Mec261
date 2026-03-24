using UnityEngine;

public class FireDirector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Turret _turret;

    private void Update()
    {
        _turret.AimAtTarget(_enemy.transform.position);
    }
}
