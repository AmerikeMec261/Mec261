using UnityEngine;

public class AddForce : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;
    [Header("ForceToAdd")]
    [SerializeField] private float _forceToAdd = 10f;

    private void FixedUpdate()
    {
        Vector3 rayOrigin = _rigidbody.worldCenterOfMass;

        if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 50)) { return; }

        Vector3 groundNormal = hit.normal;

        Vector3 downhill = Vector3.ProjectOnPlane(Physics.gravity, groundNormal).normalized;
        Vector3 uphill = -downhill;

        _rigidbody.AddForce(uphill * _forceToAdd, ForceMode.Force);

        Debug.DrawRay(rayOrigin, Vector3.down * 50, Color.yellow);
        Debug.DrawRay(hit.point, groundNormal, Color.blue);
        Debug.DrawRay(rayOrigin, uphill * _forceToAdd, Color.green);
    }
}
