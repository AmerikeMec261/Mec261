using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _Object;
    public Transform _puntoSalida;
    [Header("ForceToAdd")]
    [SerializeField] private float _velocity = 10f;
    [SerializeField] private float _angle = 45f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lanzar(_Object);
        }
    }
    void Lanzar(GameObject _obj)
    {
        
                GameObject _position = Instantiate(_obj, _puntoSalida.position, _puntoSalida.rotation);
                Rigidbody _rigidbody = _position.GetComponent<Rigidbody>();
                float _angleInRadians = _angle * Mathf.Deg2Rad;
                Vector3 _velocityVector = new Vector3(_velocity * Mathf.Cos(_angleInRadians), _velocity * Mathf.Sin(_angleInRadians), 0f);
                _rigidbody.linearVelocity = _velocityVector;
    }
}
