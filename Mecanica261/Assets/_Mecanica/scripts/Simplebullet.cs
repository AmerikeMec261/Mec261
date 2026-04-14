using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _Damage = 1f;

    public float Damage=> _Damage;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemies target = collision.gameObject.GetComponent<Enemies>(); //Aqui se debe usar Interfaz y no script. 

        target.Damage(_Damage);
   
    }
    //Sirve más que nada para rendimiento, al no ver ese objeto en la camara se vuelve invisible al no verlo
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}





