using UnityEngine;

public class Fuerza : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float _fuerzaAplicada = 2147f;
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidBody.AddRelativeForce(Vector2.right * _fuerzaAplicada);
    }
}
