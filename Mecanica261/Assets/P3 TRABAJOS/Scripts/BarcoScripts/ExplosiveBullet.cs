using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //Nota: Utilice casi la misma estrucutura que SimpleBullet
public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 30f;
    [SerializeField] private float _radius = 30f;

    public void Fire()//Se sigue utilizando misma estructura que simple bullet
    {
       Rigidbody rb = GetComponent<Rigidbody>();   
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    public void DealDamage()
    {
        //Con esto buscara todos los colliders dentro de la esfera que estara centrada en la posicion de la bala junto con el radio definido
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        
        //Aqui recirrera todos los objetos encontrados en la radio de la explosion
        for (int i = 0; i < colliders.Length; i++)
        {
            //Buscara si el objeto tiene implementada la interfaz IDamage
            IDamage damageable = colliders[i].GetComponent<IDamage>();

            //La condicion es si el objeto puede recibir daño se le aplicara ese dao
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
        }
    }
    //Al igual que SimpleBullet en resumen con estas dos devueleven estos datos de la bala para que otras clases lo lean
    public float Speed()
    {
        return _speed;
    }

    public float Damage()
    {
        return _damage;
    }

    //Al colisionar con un objeto la bala aplicara el daño al objetivo en este caso un radio de explosion y se destruira la bala despuesd e colisionar
    private void OnCollisionEnter(Collision collision)
    {
        DealDamage();
        Destroy(gameObject);
    }
}



