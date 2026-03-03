using UnityEngine;

public class Balon : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _Fuerza = 20f;  
    [SerializeField] private float _Angulo = 30f;
    private bool lanzamiento = false;
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !lanzamiento)
        {
            LanzarBalon();
        }
    }

    private void LanzarBalon()
    {
        lanzamiento = true;
        float angleInRadians = _Angulo * Mathf.Deg2Rad;
        Vector3 Direccion = new Vector3(0f, Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));
        Vector3 VelocidadFinal = transform.TransformDirection(Direccion) * _Fuerza;
        _rigidbody.linearVelocity = VelocidadFinal;
    }
}
