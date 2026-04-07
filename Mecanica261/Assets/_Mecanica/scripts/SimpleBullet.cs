using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float _speed = 40f;
    [SerializeField] private float _damage = 20f;

    public float Speed => _speed;

    #endregion Variables

    #region Unity Methods

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.DealDamage(_damage);
        }

        Destroy(gameObject);
    }

    #endregion Unity Methods

    #region Methods

    public void Fire()
    {
        Destroy(gameObject, 3f);
    }

    #endregion Methods
}