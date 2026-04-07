using UnityEngine;
public class SimpleBullet : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private Rigidbody _rigidbody;
    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Fire(Transform origen)
    {
        _rigidbody.AddForce(origen.forward * _speed, ForceMode.Impulse);
    }
}
