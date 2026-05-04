using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class TurretBarco : MonoBehaviour
{
    [Header("Cannon parts")]
    [SerializeField] private Transform towerOrigin;
    [SerializeField] private Transform picth;
    [SerializeField] private Transform yaw;
    [SerializeField] private Transform bulletExit;

    [Header("Ammo configuration")]
    [SerializeField] private List<GameObject> bullets;
    [SerializeField] private float bulletSpeed;

    [Header("Turret movement")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationDegrees;

    [Header("Tracking")]
    [SerializeField] private float trackingRange = 50f;
    [SerializeField] private string enemyTag = "Enemy";

    private int currentBullet = 0;
    private bool enemyInRange = false;
    private Transform _target;

    void Start()
    {
        SelectWeapon();    
    }
    
    void Update()
    {
        ChangeBullet();
    }

    private void FixedUpdate()
    {
        Rotation();
    }

    private void Rotation()
    {
        if(_target != null)
        {
            float distance = Vector3.Distance(transform.position, _target.position);

            if(distance > trackingRange || !_target.gameObject.activeInHierarchy)
            {
                _target = null;
                enemyInRange = false;
            }
            else
            {
                enemyInRange = true;
            }
        }
    }

    private void FireBullet()
    {

    }

    private void MathShoot()
    {

    }

    private void ChangeBullet()
    {
        int previousBullet = currentBullet;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (currentBullet >= bullets.Count - 1)
            {
                currentBullet = 0;
            }
            else
            {
                currentBullet++;
            }
        }

        if(previousBullet != currentBullet)
        {
            SelectWeapon();
        }

    }

    private void SelectWeapon()
    {
        if(bullets.Count > 0 && bullets[currentBullet] != null)
        {
            bulletSpeed = bullets[currentBullet].GetComponent<IProjectile>().Speed;
            print("Bala cambiada" + bullets[currentBullet].name);
        }
    }

    private void OnDrawGizmos()
    {        
        if (bulletExit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(bulletExit.position, bulletExit.forward * 5f);
        }
        

        if (yaw != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(yaw.position, trackingRange);
        }
    }
}

//Para el cambio de balas use este video
//https://www.youtube.com/watch?v=Dn_BUIVdAPg
