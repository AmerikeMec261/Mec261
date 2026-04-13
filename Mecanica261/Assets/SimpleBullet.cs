using UnityEngine;

public class SimpleBullet : MonoBehaviour, IProjectile
{
    private Rigidbody rigidbody; //no usar abreviaciones

    [SerializeField] private float damage = 20f;
    [SerializeField] private float speed = 25f;

    public float Damage => damage;
    public float Speed => speed;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
        Destroy(gameObject);
    }

    public void DealDamage(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}
