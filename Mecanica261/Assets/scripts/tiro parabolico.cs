using UnityEngine;

public class tiroparabolico : MonoBehaviour
{
    public Vector3 fuerza = new Vector3(14.14f, 14.14f, 0f);
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
          if (Input.GetKeyDown(KeyCode.Space))
          {
            rb.AddForce(fuerza, ForceMode.VelocityChange);
          }
    }

   
}
