using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{

    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 20f;

    private GameObject _hit; //Se guarda el gameobject para que cuando la bala colisiones aplique el daño

    public void Fire() //Esta funcion se llama cuando la bala se dispara de la torreta
    {
        //Obtiene el componente del Rigidbody delobjeto de la bala, se reincia cualquier velocidad de anteriores lanzamientos y por ultimo aplica fuerza con addforce instantanea hacia adelante usando la velocidad configurada
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    public void DealDamage() //Aqui se aplica el daño al objetivo
    {
        if (_hit == null) //La condicion es si hit es nulo entonces no hubo colision lo cual no hace nada
        {  
            return; 
        }

        IDamage damageable = _hit.GetComponent<IDamage>(); //Aqui busca si el objeto golpeado tiene implementada la intrafaz IDamage

        //Si el objetivo puede recibir daño este le aplicara el daño configurado
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
        }
    }
    //En resumen con estas dos devueleven estos datos de la bala para que otras clases lo lean
    public float Speed()
    { 
      return _speed;
    }
    public float Damage()
    {
        return _damage;
    }

    //Este se llama automaticamente cuando colisiona con un objeto
    private void OnCollisionEnter(Collision collision)
    {
        //Se guarda el objeto con el cual colisiono para aplicar el daño llamando a deal damage y destruyendo la bala despues de impactar
        _hit = collision.gameObject;
        DealDamage();
        Destroy(gameObject);
    }
}

