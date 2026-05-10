using UnityEngine;

public class ShipController : MonoBehaviour //Recuerda estandarizar el código
{
    [Header("Force Points")]
    [SerializeField] private Transform[] engineForcePoints;
    [SerializeField] private Transform[] rudderForcePoints;

    [Header("Richelieu Battleship Values")]
    [SerializeField] private float maximumSpeed = 16.46f;
    [SerializeField] private float engineForce = 155000f;
    [SerializeField] private float rudderForce = 45000f;
    [SerializeField] private float acceleration = 0.25f;
    [SerializeField] private float deceleration = 0.12f;
    [SerializeField] private float rudderSmoothness = 0.35f;
    [SerializeField] private float maximumRudderAngle = 35f;

    private Rigidbody rigidbodyComponent;

    private float currentEnginePower;
    private float currentRudderInput;

    private void Awake()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() //El input va en el update. La física va en el fixed update.
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rudderInput = Input.GetAxis("Horizontal");

        UpdateEnginePower(forwardInput);
        UpdateRudderInput(rudderInput);
        ApplyEngineForce();
        ApplyRudderForce();
    }

    private void UpdateEnginePower(float forwardInput)
    {
        float speedChange = acceleration;

        if (Mathf.Abs(forwardInput) < 0.01f)
        {
            speedChange = deceleration;
        }

        currentEnginePower = Mathf.Lerp(currentEnginePower, forwardInput, Time.fixedDeltaTime * speedChange);
    }

    private void UpdateRudderInput(float rudderInput)
    {
        currentRudderInput = Mathf.Lerp(currentRudderInput, rudderInput, Time.fixedDeltaTime * rudderSmoothness);
    }

    private void ApplyEngineForce()
    {
        if (rigidbodyComponent.linearVelocity.magnitude >= maximumSpeed && currentEnginePower > 0f) //Utiliza clamp en su lugar para limitar la velocidad máxima
        {
            return;
        }

        for (int i = 0; i < engineForcePoints.Length; i++)
        {
            Vector3 force = transform.right * currentEnginePower * engineForce;
            rigidbodyComponent.AddForceAtPosition(force, engineForcePoints[i].position);
        }
    }

    private void ApplyRudderForce()
    {
        float rudderAngle = currentRudderInput * maximumRudderAngle;

        for (int i = 0; i < rudderForcePoints.Length; i++)
        {
            Vector3 force = transform.forward * rudderAngle * rudderForce;
            rigidbodyComponent.AddForceAtPosition(force, rudderForcePoints[i].position);
        }
    }
}