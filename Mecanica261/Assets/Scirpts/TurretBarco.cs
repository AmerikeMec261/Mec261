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
        
        
        if (Input.GetKeyDown(KeyCode.Space) && enemyInRange)
        {
            FireBullet();
        }
    }

    private void FixedUpdate()
    {
        RotationYaw();
        RotationPitch();
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

    private void RotationPitch()
    {
        if (picth == null || _target == null || !enemyInRange)
        {            
            picth.localRotation = Quaternion.RotateTowards(picth.localRotation, Quaternion.identity, rotationSpeed * Time.fixedDeltaTime);
            return;
        }
        
        Vector3 dirToTarget = _target.position - picth.position;
        
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget, yaw.up);
        Quaternion relativeRotation = Quaternion.Inverse(yaw.rotation) * lookRotation;

        float targetPitchAngle = relativeRotation.eulerAngles.x;
        
        if (targetPitchAngle > 180f) targetPitchAngle -= 360f;


        float clampedPitch = Mathf.Clamp(targetPitchAngle, -maxElevation, maxDown);
        
        Quaternion targetRotation = Quaternion.Euler(clampedPitch, 0f, 0f);
        picth.localRotation = Quaternion.RotateTowards(picth.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }


    private void FireBullet()
    {
        if (bullets.Count > 0 && bullets[currentBullet] != null && bulletExit != null)
        {
            GameObject bullet = Instantiate(bullets[currentBullet], bulletExit.position, bulletExit.rotation);

            IProjectile projectileScript = bullet.GetComponent<IProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Fire();
            }
            else
            {
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = bulletExit.forward * bulletSpeed;
                }
            }
        }
    }

    private void MathShoot()
    {
        if (bulletExit == null || picth == null || _target == null || !enemyInRange) return;

        Vector3 dir = _target.position - bulletExit.position;
        float x = new Vector2(dir.x, dir.z).magnitude;
        float y = _target.position.y - bulletExit.position.y;
        float g = 9.81f;
        float v = bulletSpeed;
        float targetPitchAngle = 0f;

        if (v > 0)
        {
            float root = (v * v * v * v) - g * (g * (x * x) + 2 * y * (v * v));

            if (root >= 0)
            {
                float angleRadians = Mathf.Atan(((v * v) - Mathf.Sqrt(root)) / (g * x));
                targetPitchAngle = -(angleRadians * Mathf.Rad2Deg);
            }
            else
            {
                Vector3 localDir = picth.parent.InverseTransformPoint(_target.position);
                targetPitchAngle = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
            }
        }
        else
        {
            Vector3 localDir = picth.parent.InverseTransformPoint(_target.position);
            targetPitchAngle = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
        }

        float clampedPitch = Mathf.Clamp(targetPitchAngle, -maxElevation, maxDown);
        Quaternion targetRotation = Quaternion.Euler(clampedPitch, 0f, 0f);
        picth.localRotation = Quaternion.RotateTowards(picth.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void ChangeBullet()
    {
        if (bullets.Count <= 1) return;

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