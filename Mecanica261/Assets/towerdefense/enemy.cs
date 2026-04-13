using UnityEngine;

public class enemy : MonoBehaviour, IDamageable, Ienemy //Todo debe estar en inglés.
{
    [Header("Dependencias")]
    [SerializeField] private Transform _puntoA;
    [SerializeField] private Transform _puntoB;

    [Header("Stats")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _shield = 0f;
    [SerializeField] private float _health = 100f;

    public float Speed => _speed;
    public float Shield => _shield;
    public float Health => _health;

    private float _healthActual;
    private bool _yendoAB = true;

    private void Start()
    {
        _healthActual = _health;
    }

    private void Update()
    {
        Moverse();
    }

    public void TakeDamage(float damage)
    {
        _healthActual -= damage;
        Debug.Log(gameObject.name + " recibio " + damage + " de danio. Vida restante: " + _healthActual);

        if (_healthActual <= 0)
            Destroy(gameObject);
    }

    private void Moverse()
    {
        Transform objetivo = _yendoAB ? _puntoB : _puntoA; // porque necesitas el transform del objetivo? no puedes usar directamente el vector3 de la posición del objetivo?

        transform.position = Vector3.MoveTowards(transform.position, objetivo.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, objetivo.position) < 0.1f)
            _yendoAB = !_yendoAB;
    }
}
