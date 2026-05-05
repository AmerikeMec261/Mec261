using UnityEngine;

public class ExplosiveBullett : MonoBehaviour
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float _explosionForce = 3000f;

    [SerializeField] private float _explosionRadius = 5f;

    [SerializeField] private float _lifeTime = 5f;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    #endregion Unity Methods

    #region Methods

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            _explosionRadius
        );

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rigidbody = colliders[i].GetComponent<Rigidbody>();

            if (rigidbody == null) { continue; }

            rigidbody.AddExplosionForce(
                _explosionForce,
                transform.position,
                _explosionRadius
            );
        }
    }

    #endregion Methods
}
