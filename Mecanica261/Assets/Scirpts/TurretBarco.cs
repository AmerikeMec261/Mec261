using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;

public class TurretBarco : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _turret;

    [Header("Cannon parts")]
    [SerializeField] private Transform picth;
    [SerializeField] private Transform yaw;
    [SerializeField] private Transform bulletExit;

    [Header("Ammo configuration")]
    [SerializeField] private List<GameObject> bullets;
    [SerializeField] private float bulletSpeed;

    [Header("Turret movement")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationDegrees;

    void Start()
    {
        
    }
    
    void Update()
    {
        Rotation(); 
    }

    private void Rotation()
    {
        Vector3 direction = _target.position - _turret.position;
        direction.y = 0f;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
