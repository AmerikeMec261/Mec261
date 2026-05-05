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
    [SerializeField] private float maxElevation = 45f;
    [SerializeField] private float maxDown = 5f;

    [Header("Tracking")]
    [SerializeField] private float trackingRange = 50f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float visionAngle = 90f;

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
        RotationYaw();
    }

    private void RotationYaw()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag(enemyTag);
        enemyInRange = false;

        if (enemy != null)
        {
            Vector3 dirToEnemy = enemy.transform.position - yaw.position;
            float distance = dirToEnemy.magnitude;
            float angleToEnemy = Vector3.Angle(yaw.forward, dirToEnemy);

            if (distance <= trackingRange && angleToEnemy <= visionAngle)
            {
                _target = enemy.transform;
                enemyInRange = true;
            }
        }

        if (enemyInRange && _target != null)
        {
            Vector3 dirForRotation = _target.position - yaw.position;
            dirForRotation = Vector3.ProjectOnPlane(dirForRotation, towerOrigin.up);

            float signedAngle = Vector3.SignedAngle(towerOrigin.forward, dirForRotation, towerOrigin.up);
            float clampedAngle = Mathf.Clamp(signedAngle, -rotationDegrees, rotationDegrees);

            Quaternion targetRotation = Quaternion.Euler(0f, clampedAngle, 0f);
            yaw.localRotation = Quaternion.RotateTowards(yaw.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _target = null;
            Quaternion idleRotation = Quaternion.Euler(0f, 0f, 0f);
            yaw.localRotation = Quaternion.RotateTowards(yaw.localRotation, idleRotation, rotationSpeed * Time.fixedDeltaTime);
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
        if (yaw != null)
        {            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(yaw.position, trackingRange);
            
            Gizmos.color = Color.red;
            Vector3 rightLimit = Quaternion.AngleAxis(visionAngle / 2f, Vector3.up) * yaw.forward;
            Vector3 leftLimit = Quaternion.AngleAxis(-visionAngle / 2f, Vector3.up) * yaw.forward;

            Gizmos.DrawRay(yaw.position, rightLimit * trackingRange);
            Gizmos.DrawRay(yaw.position, leftLimit * trackingRange);
        }

        if (enemyInRange && _target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(bulletExit != null ? bulletExit.position : picth.position, _target.position);
            Gizmos.DrawWireSphere(_target.position, 1f);
        }
    }
}

//Para el cambio de balas use este video
//https://www.youtube.com/watch?v=Dn_BUIVdAPg

//Para el traqueo del enemigo 
//https://www.youtube.com/watch?v=lV47ED8h61k