using UnityEngine;
/*
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _ExplosionRadio = 5f;
    [SerializeField] private float _ExplosionForce = 500f;

    public void Fire()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * _Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _ExplosionRadio);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(_ExplosionForce, transform.position, _ExplosionRadio);
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
*/