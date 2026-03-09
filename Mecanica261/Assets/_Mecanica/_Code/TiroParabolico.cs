using UnityEngine;

public class TiroParabolico : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f; // Vo = 20 m/s
    [SerializeField] private float _angle = 45f;    // �ngulo = 45�

    public int MaximoTiros = 3;
    private int TirosGastados = 0;

    private Vector3 IPosicion;
    private Quaternion IRotacion;

    void Start()
    {
        IPosicion = transform.position;
        IRotacion = transform.rotation;
       
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reinicio();
        }
    }

    public void Parametros(float vel, float ang)
    { 
    _velocity = vel;
    _angle = ang;
    }

    public void Disparo ()
    {
    float angleInRadians = _angle * Mathf.Deg2Rad;

    Vector3 velocityVector = new Vector3(
        _velocity * Mathf.Cos(angleInRadians),
        _velocity * Mathf.Sin(angleInRadians),
        0f
        );
    _rigidbody.linearVelocity = transform.TransformDirection(velocityVector);
   

        TirosGastados++;

    }

    public void Reinicio()
    {
        _rigidbody.linearVelocity = Vector3.zero;

        transform.position = IPosicion;
        transform.rotation = IRotacion;
        
        TirosGastados = 0; 
    }
}
