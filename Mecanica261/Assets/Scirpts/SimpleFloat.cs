using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class SimpleFloat : MonoBehaviour
{
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterDensity = 1000;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Float();
    }

    private void Float()
    {
        float submersion = Mathf.Clamp01((_waterLevel - transform.position.y) / 1f);

        if (submersion < 0f) return;

        float force = _waterDensity * submersion * _volume * Physics.gravity.magnitude;

        _rigidbody.AddForce(Vector3.up * force, ForceMode.Force);
    }
}
