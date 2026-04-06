using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float impacto = 20f;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IRecibeImpacto objetivo = collision.gameObject.GetComponent<IRecibeImpacto>();

        if (objetivo != null)
        {
            objetivo.RecibirImpacto(impacto);
            Debug.Log("Impacto a: " + collision.gameObject.name);
        }

        Destroy(gameObject);
    }
}