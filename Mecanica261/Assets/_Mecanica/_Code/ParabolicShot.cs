using UnityEngine;

public class ParabolicShot : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f;
    [SerializeField] private float _angle=45f;

    private void Start()
    {
        //Convertimos el angulo de grados a radianes para las funciones de Math
        float AngelinRadians = _angle * Mathf.Rad2Deg;

        // Calculamos las componentes de la velocidad (Vox y Voy)
        // Vox= Vo * cos(0)
        // Voy= Vo * cos(0)
        Vector3 velocityVector = new Vector3(_velocity * Mathf.Cos(AngelinRadians, _velocity) * Mathf.Sin(AngelinRadians), 0f);

        // Aplicamos la velocidad inicial al Rigidbody
        _rigidbody.linearVelocity = velocityVector;

    }
}
