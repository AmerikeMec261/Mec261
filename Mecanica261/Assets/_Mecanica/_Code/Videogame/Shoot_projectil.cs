using UnityEngine;
using UnityEngine.InputSystem;


public class Shoot_projectil : MonoBehaviour
{

    public GameObject projectil;
    public Transform shootPoint;        // empty GameObject in front of player
    public float shootForce = 15f;      // initial force

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectil, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply force at an angle (parabolic trajectory)
        Vector3 direction = new Vector3(-6, 1, 0).normalized;
        rb.AddForce(direction * shootForce, ForceMode.Impulse);
    }
}


