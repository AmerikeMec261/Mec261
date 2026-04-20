using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class SimpleFloat : MonoBehaviour
{
    [SerializeField] private float _waterLevel = 0;
    [SerializeField] private float _volume = 1;
    [SerializeField] private float _waterDensity = 1000;

    private Rigidbody _rigbody;

    private void Awake()
    {
        _rigbody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        Float();
    }

    private void Float()
    {
        float summersion = Mathf.Clamp01((_waterLevel - transform.position.y / 1f));

        if (summersion < 0f) return;

        float force = _waterDensity * summersion * Physics.gravity.magnitude;

        _rigbody.AddForce(Vector3.up * force * _volume, ForceMode.Force);
    }
}
