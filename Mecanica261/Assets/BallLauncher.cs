using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;

    public Vector3 hoopCenter = new Vector3(0f, 3f, 15f);

    public float launchForce = 25f;
    public float extraHeight = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        GameObject newBall = Instantiate(
            ballPrefab,
            transform.position,
            Quaternion.identity
        );

        Rigidbody rb = newBall.GetComponent<Rigidbody>();

        Vector3 direction = hoopCenter - transform.position;

        direction.y += extraHeight;

        direction = direction.normalized;

        rb.AddForce(direction * launchForce, ForceMode.Impulse);
    }
}
