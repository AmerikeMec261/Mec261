using UnityEngine;

public class TurretInjector : MonoBehaviour
{
    [SerializeField] private Turret _turret;
    [SerializeField] private Transform _target;

    void Update()
    {
        if (_turret == null || _target == null) return;

        
        _turret.AimAt(_target.position);

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _turret.Fire(_target.position);
        }
    }
}
