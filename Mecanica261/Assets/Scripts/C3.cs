using UnityEngine;
// CorregidoEnClase
public class ParabolicShot : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f; // Vo = 20 m/s
    [SerializeField] private float _angle = 45f;    // Ángulo = 45°

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        Debug.Log("Touhspace");
            PresionaLanzar();
        }
    }
    public void PresionaLanzar()
    {
        // Convertimos el ángulo de grados a radianes para las funciones de Math
        float angleInRadians = _angle * Mathf.Deg2Rad;

        // Calculamos las componentes de la velocidad (Vox y Voy)
        // Vox = Vo * cos(θ)
        // Voy = Vo * sin(θ)
        Vector3 velocityVector = new Vector3(
            _velocity * Mathf.Cos(angleInRadians),
            _velocity * Mathf.Sin(angleInRadians),
            0f
        );

        // Aplicamos la velocidad inicial al Rigidbody
        // Nota: En versiones recientes de Unity se usa linearVelocity 
        // En versiones anteriores usa: _rigidbody.velocity = velocityVector;
        _rigidbody.linearVelocity = velocityVector;
    }

 
    
}
