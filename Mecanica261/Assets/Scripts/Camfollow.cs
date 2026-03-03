using System;
using UnityEngine;

public class Camfollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 desirePos= target.position+offset;
        transform.position= Vector3.Lerp(transform.position, desirePos, smoothSpeed*Time.deltaTime);
        transform.LookAt(target);
    }


    void Update()
    {
        
    }
}
