using UnityEngine;

public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 20f;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        rigid.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void DamageProjectile()
    {
        Debug.Log("Le quite: " + damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();

        if (enemy != null)
        {
            enemy.DamageEnemy(damage);
        }

        DamageProjectile();
        Destroy(gameObject);
    }
}