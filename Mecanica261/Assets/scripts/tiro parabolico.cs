using UnityEngine;

public class tiroparabolico : MonoBehaviour
{
    public Vector3 fuerza = new Vector3(14.14f, 14.14f, 0f);
    Rigidbody rb;

    private Vector3 Inicialposition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();
        Inicialposition = this.transform.position;
    }

    void Update()
    {
          if (Input.GetKeyDown(KeyCode.Space))
          {
            rb.AddForce(fuerza, ForceMode.VelocityChange);
          }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.linearVelocity = Vector3.zero;
            this.transform.position = Inicialposition;
        }
    }

   
}
