using Unity.Mathematics;
using UnityEngine;

public class TurretBarco : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _turret;
    void Start()
    {

    }


    void Update()
    {
        Rotacion();
    }

    private void Rotacion()
    {
        Vector3 direction = _target.position - _turret.position;
        direction.y = 0f;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
