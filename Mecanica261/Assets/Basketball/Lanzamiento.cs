using UnityEngine;

public class Lanzamiento : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject _balaPrefab;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f;
    [SerializeField] private float _angle = 45f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Lanzar();
        }

    }
    private void Lanzar()
    {
     
        GameObject newCube = Instantiate(_balaPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = newCube.GetComponent<Rigidbody>();

        float angleInRadians = _angle * Mathf.Deg2Rad;
        Vector3 velocityVector = new Vector3(
            _velocity * Mathf.Cos(angleInRadians),
            _velocity * Mathf.Sin(angleInRadians),
            0f
    );
        rb.linearVelocity = velocityVector;
    }
}
