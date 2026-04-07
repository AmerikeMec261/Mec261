using UnityEngine;

public class ExplosiveBullet : MonoBehaviour, IProjectile

{
    [SerializeField] private float velocidad = 15f;
    [SerializeField] private float damagee = 40f;
    [SerializeField] private float radio = 5f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        _rigidbody.AddForce(transform.forward * velocidad, ForceMode.Impulse);
    }

    public void Impact()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider hit in hits)
        {
            IDamageable enemy = hit.GetComponent<IDamageable>();

            if (enemy != null)
            {
                enemy.DamageEnemy(damagee);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Impact();
        Destroy(gameObject);
    }
}