using UnityEngine;

public class TiroParabolico : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f; // Vo = 20 m/s
    [SerializeField] private float _angle = 45f;    // Ángulo = 45°

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
