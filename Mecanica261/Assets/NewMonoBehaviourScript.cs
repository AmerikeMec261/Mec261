using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("References")]
    public Rigidbody targetCube;
    public Transform inclinePlane;

    [Header("Force Settings")]
    public float mass = 200f;
    public float frictionCoeff = 0.8f;
    public float inclineAngleDegrees = 20f;
    public bool moveUp = true;

    private Vector3 directionAlongPlane;

    void Start()
    {

        if (targetCube == null)
        {
            Debug.LogError("No se asign� el Rigidbody del cubo.");
            enabled = false;
            return;
        }


        Vector3 upDirection = inclinePlane.up;
        Vector3 planeNormal = inclinePlane.up;


        Vector3 right = Vector3.Cross(planeNormal, Vector3.up).normalized;
        directionAlongPlane = Vector3.Cross(planeNormal, right).normalized;

        if (!moveUp)
        {
            directionAlongPlane = -directionAlongPlane;
        }
    }

    void Update()
    {

        float g = 9.8f;
        float angleRad = Mathf.Deg2Rad * inclineAngleDegrees;


        float sinAngle = Mathf.Sin(angleRad);
        float cosAngle = Mathf.Cos(angleRad);


        float forceMagnitude = mass * g * (sinAngle + frictionCoeff * cosAngle);


        Vector3 force = directionAlongPlane * forceMagnitude;
        targetCube.AddForce(force, ForceMode.Force);


        Debug.DrawRay(targetCube.position, directionAlongPlane * 2f, Color.red);
        Debug.Log("Fuerza aplicada: " + forceMagnitude.ToString("F2") + " N");
    }
}
