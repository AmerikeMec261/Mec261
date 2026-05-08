using UnityEngine;

public class ExplosiveBullet2 : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 200f;
    [SerializeField] private float _explosionRadius = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollision(Collision collision)
    {
        Collider[] objeto = Physics.OverlapSphere(transform.position, _explosionRadius);
        for (int i = 0; i < objeto.Length; i++)
        {
            Rigidbody rigidbody = objeto[i].GetComponent<Rigidbody>();
            
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionForce);
            }
        }

        Destroy(gameObject);
    }
}
