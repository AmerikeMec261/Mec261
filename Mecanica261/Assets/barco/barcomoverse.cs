using UnityEngine;

public class barcomoverse : MonoBehaviour //Recuerda cambiar el código a inglés y estandarizarlo
{
    [Header("Puntos")]
    [SerializeField] private Transform[] _motorPoints;
    [SerializeField] private Transform[] _turnPoints;

    [Header("Fuerzas")]
    [SerializeField] private float _motorForce = 50f;
    [SerializeField] private float _turnForce = 20f; 

    private Rigidbody _rigiBody;

    private float _currentspeed = 0f;
    private float _currentturn = 0f;

    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _smoothingTurn = 2f;

    private float _inputForward;
    private float _inputTurn;

    void Update()
    {
        float inputForward = Input.GetAxis("Vertical"); //El input va en el Update, pero el movimiento se aplica en el FixedUpdate. 
        float inputTurn = Input.GetAxis("Horizontal");
    }

    private void Awake()
    {
        _rigiBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {


        _currentspeed = Mathf.Lerp(_currentspeed, _inputForward, Time.fixedDeltaTime * _acceleration);
        _currentturn = Mathf.Lerp(_currentturn, _inputTurn, Time.fixedDeltaTime * _smoothingTurn);

        for (int i = 0; i < _motorPoints.Length; i++)
        {
            Vector3 fuerza = transform.forward * _currentspeed * _motorForce;
            _rigiBody.AddForceAtPosition(fuerza, _motorPoints[i].position);
        }

        for (int i = 0; i < _turnPoints.Length; i++)
        {
            Vector3 fuerza = transform.right * _currentturn * _turnForce;
            _rigiBody.AddForceAtPosition(fuerza, _turnPoints[i].position);
        }
    }
}
