using UnityEngine;

[RequireComponent (typeof(RigidBody))]
public class SimpleBullet : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
}
