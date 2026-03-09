using UnityEngine;

public class tiroparabolico : MonoBehaviour
{
    [SerializeField] private Vector3 _force = new Vector3(14.14f, 14.14f, 0f);
   [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _initialPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = this.transform.position;
    }

    void Update()
    {
        SettingsControls();
    }

    private void SettingsControls()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(_force, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _rigidbody.linearVelocity = Vector3.zero;
            this.transform.position = _initialPosition;
        }
    }

}
