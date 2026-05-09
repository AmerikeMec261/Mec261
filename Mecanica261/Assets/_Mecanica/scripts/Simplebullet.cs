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
    //Puede servir para rendimiento y te evitan calculos innecesarios cuando el objeto no es visible, si no esta en ejecución los objetos solo son visibles con la camara,
    //https://docs.unity3d.com/6000.4/Documentation/ScriptReference/MonoBehaviour.OnBecameInvisible.html

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}





