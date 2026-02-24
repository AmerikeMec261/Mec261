using UnityEngine;

public class tiroparabolico : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>()
        .AddForce(new Vector3(14.14f, 14.14f, 0f), ForceMode.VelocityChange);
    }

   
}
