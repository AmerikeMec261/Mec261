using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimpleBullet : MonoBehaviour
     {
        [Header("Setings")]
        [SerializeField] private float _speed = 20f;
        public void Fire()
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
        }
    }

