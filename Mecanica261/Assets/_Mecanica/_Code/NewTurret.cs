using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    Transform _player; // nombre de preset
    float dist; // Distancia entre la torreta y el enemigo
    [SerializeField] private float _howClose; // la cercania que se va a calcular con la distancia
    [SerializeField] private Transform _head, _barrel; // los objects de la torreta
    [SerializeField] private GameObject _proyectile; // proyectil de base
    [SerializeField] private float _fireRate, _nextFire; // cadencia y tiempo entre lapsos del fuego
    void Start()
    {
        // Se Buscara al objetivo por medio del tag que posea
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // se calcula la distancia con un Vector3 hacia la del "player" aka objetivo 
        dist = Vector3.Distance(_player.position, transform.position);
        if (dist <= _howClose)
        {
            // si el tiempo el igual o mayor al _next fire
            _head.LookAt(_player);
            if (Time.time >= _nextFire)
            {
                // la torreta ejecutara la funcion de dispara a la par de radio de fuego
                _nextFire = Time.time * 1f/_fireRate;
                Shoot();
            }

        }
    }

    void Shoot()
    {
        // se instancia un GameObject del la bala generado desde la posicion del barril a la par que se ejecuta la rotacion de la cabeza
        GameObject _clone = Instantiate(_proyectile, _barrel.position, _head.rotation);
        _clone.GetComponent<Rigidbody>().AddForce(_head.forward * 1500);
        Destroy(_clone, 10); //Destruye GameObject luego de 10 seg.
        //force forward
    }
    // https://youtu.be/XLCMrguxIs0?si=RvQ4GhlpR8MHDnx3
}   //Trabajo en clase: usar la formula que vimos en clase.