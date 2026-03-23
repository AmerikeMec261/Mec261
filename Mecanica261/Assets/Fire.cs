using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet: MonoBehaviour , IProjectile
{
    [Header("Settings")]

    [SerializeField] private float _speed = 200.0f;
    public void Fire()
    {
        throw new System.NotImplementedException();
    }



}
