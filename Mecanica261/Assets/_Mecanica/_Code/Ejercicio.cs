using System;
using UnityEngine;

public class Ejercicio : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;//1
    [SerializeField] private RigidBody _rigidBody;//5
    [SerializeField] private bool _playerIsDead = false;//6
    [SerializeField] private string _index;//7

    [Range(0f, 100f)]
    public float Attack;//8
    private protected float _damage = 10f;//3
    internal int currentHealth; //4
 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
