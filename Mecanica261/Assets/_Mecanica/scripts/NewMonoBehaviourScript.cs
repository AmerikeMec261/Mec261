using UnityEngine;

using UnityEngine;

public class MoverPlano : MonoBehaviour
{
    public float force = 2146f;
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }
}