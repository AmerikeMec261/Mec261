/*using System.Collections.Generic;
using UnityEngine;

public class Movementboat : MonoBehaviour;
{
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

public class SimpleFloat : MonoBehaviour
{
    [SerializeField] private float _Max foward = 0;
    [SerializeField] private float _Max reverse = 1;
    [SerializeField] private float _foward movement = 1000;
    [SerializeField] private float _reverse movement;
   
    


    private void Awake()
    {
      
        CalculateHullData();
    }


    private void FixedUpdate()
    {
        
    }



  private float movement()
{

}

40 lines, input, calculos


    private void FloatShip()
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

         ***   _rigidbody.AddForceAtPosition(-velocity * _waterDrag * Submersion, propeler.position, ForceMode.Force); crece a o largo del tiempo multiplicar por timr.deltatime por la fuerza del motor fuerxa de motor * tiepmo de aceleracion * time.deltatime
        }
    }

  
 

    }
}
*/

