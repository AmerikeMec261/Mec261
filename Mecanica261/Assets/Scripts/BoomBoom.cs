using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimpleBullet : MonoBehaviour,Iproyectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity=transform.forward*_speed;
    }

}