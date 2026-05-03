using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleFloat : MonoBehaviour
{
   [SerializeField] private float _waterlevel = 0;
   [SerializeField] private float _volume = 0;
   [SerializeField] private float _waterDensity = 0;

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
        float summersion = Mathf.Clamp01((_waterlevel - transform.position.y / 1f));
        if ( summersion < 0f) return;
        float force = _waterDensity * summersion * _volume * Physics.gravity.magnitude;
        _rigidbody.AddForce(Vector3.up * force, ForceMode.Force);
    }
}
