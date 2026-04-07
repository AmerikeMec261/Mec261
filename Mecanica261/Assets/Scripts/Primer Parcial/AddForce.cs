using UnityEngine;

public class AddForce : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f; 
    [SerializeField] private float _angle = 45f;   

    private void Start()
    {
        
        float angleInRadians = _angle * Mathf.Deg2Rad;
        Vector3 velocityVector = new Vector3(
            _velocity * Mathf.Cos(angleInRadians),
            _velocity * Mathf.Sin(angleInRadians),
            0f
    );
        _rigidbody.linearVelocity = velocityVector;
    }
}
