using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations; //quita todos los using que no se necesiten


public class Movementboat : MonoBehaviour
{

 
        /* [SerializeField] private float _Maxfoward = 0f;
     [SerializeField] private float _Maxrevere = 1f;
     [SerializeField] private float _fowardmovement = 1000f;  //remueve el código que no se usa
     [SerializeField] private float _reversemovement;*/
        public Transform Motor;
        [SerializeField] private float _steerPower = 500f;
        [SerializeField] private float _power = 5f;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _drag = 0.1f;

        private Rigidbody _Rigidbody;
        [SerializeField] private Quaternion StartRotation;



        private void Awake()
        {
            _Rigidbody = GetComponent<Rigidbody>();
            StartRotation = Motor.localRotation; // Valor declarado sin uso alguno

    }


        private void FixedUpdate()
        {
            var forceDirection = transform.right; //Que avance hacia en frente dependiendo de donde este la transformación y hacia donde este apuntando
            var steer = 0; //dirección del barco 
            //Direccion hacia donde ira el barco 
            if (Input.GetKey(KeyCode.A))
                steer = 1;
            if (Input.GetKey(KeyCode.D))
                steer = -1;

            //Fuerza rotacional
            _Rigidbody.AddForceAtPosition(steer * forceDirection * _steerPower, Motor.position);

            var forward = Vector3.Scale(new Vector3(0, 0, 1), transform.forward);
            var targetVel = Vector3.zero; //Valor declarado sin uso alguno
        var right = Vector3.Scale(new Vector3(-1, 0, 0), transform.forward); // Valor declarado sin uso alguno

        //Mover hacia adelante o hacia atras 

        if (Input.GetKey(KeyCode.W))
                _Rigidbody.AddForce(forward * _power, ForceMode.Acceleration); //Que pasó con el uso de AddForceAtPosition?
        if (Input.GetKey(KeyCode.S))
                _Rigidbody.AddForce(-forward * _power, ForceMode.Acceleration);
        }
    }

//Gracias por investigar
//Video en el que me apoye https://youtu.be/gdW_rXFE1Gk





/*
  private float move()
{

}

//40 lines, input, calculos


    private void movementShip()
    {
        float gravity = Physics.gravity.magnitude;

        float volumenperpoint = _HullVolume / _floatPoints.Count;

        for (int i = 0; i < _floatPoints.Count; i++)
        {
            Transform point = _floatPoints[i];

            float Submersion = Mathf.Clamp01(_waterlevel - point.position.y / _HullHeight);

            if (Submersion <= 0f)
            {
                continue;
            }

            float force = _waterdensity * volumenperpoint * gravity * Submersion;

            _rigidbody.AddForceAtPosition(Vector3.up * force, point.position, ForceMode.Force);

            Vector3 velocity = _rigidbody.GetPointVelocity(point.position);

         //***   _rigidbody.AddForceAtPosition(-velocity * _waterDrag * Submersion, propeler.position, ForceMode.Force); crece a o largo del tiempo multiplicar por timr.deltatime por la fuerza del motor fuerxa de motor * tiepmo de aceleracion * time.deltatime
        }
    }

  
 

    }
}
*/

