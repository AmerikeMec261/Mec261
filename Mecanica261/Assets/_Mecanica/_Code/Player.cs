using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject Object;
    public Transform puntodesalidaA;
    [Header("ForceToAdd")]
    [SerializeField] private float _velocity = 10f;
    [SerializeField] private float _angle = 45f;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            Lanzar(Object);
        }
    }
    void Lanzar(GameObject obj)
    {
        
                GameObject nuevo = Instantiate(obj, puntodesalidaA.position, puntodesalidaA.rotation);
                Rigidbody rb = nuevo.GetComponent<Rigidbody>();
                float angleInRadians = _angle * Mathf.Deg2Rad;
                Vector3 velocityVector = new Vector3(_velocity * Mathf.Cos(angleInRadians), _velocity * Mathf.Sin(angleInRadians), 0f);
                rb.linearVelocity = velocityVector;
    }
}
