using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProyectilExplosivo : MonoBehaviour, IProjectile
{
    [Header("Configuracion")]
    [SerializeField] private float velocidad = 20f;
    [SerializeField] private float impacto = 30f;
    [SerializeField] private float radioExplosion = 3f;

    private Rigidbody rb;
    private bool exploto = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        rb.linearVelocity = transform.forward * velocidad;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (exploto) return;

        exploto = true;

        Collider[] objetos = Physics.OverlapSphere(transform.position, radioExplosion);

        for (int i = 0; i < objetos.Length; i++)
        {
            IRecibeImpacto objetivo = objetos[i].GetComponent<IRecibeImpacto>();

            if (objetivo != null)
            {
                objetivo.RecibirImpacto(impacto);
            }
        }

        Destroy(gameObject);
    }
}