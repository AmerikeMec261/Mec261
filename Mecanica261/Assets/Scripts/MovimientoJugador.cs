using UnityEngine;

public class MovimientoJugador : MonoBehaviour

{
    public float Velocity = 5f;
    public float Jump = 7f;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(_rigidbody.linearVelocity.y) < 0.01f)
        {
            _rigidbody.AddForce(Vector3.up * Jump, ForceMode.Impulse);
        }
    }
}
