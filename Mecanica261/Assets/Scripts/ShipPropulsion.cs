using System.Runtime.CompilerServices;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ShipPropulsion : MonoBehaviour
{
    [Header("Force Points")]
    [SerializeField] private Transform _propulsorPoint;
    [SerializeField] private Transform _rudderPoint;

    [Header("Settings")]
    [SerializeField] private float _engineForce = 5000f;
    [SerializeField] private float _rudderForce = 3000f;
    [SerializeField] private float _aceleration = 0.5f;

    private Rigidbody _rigidbody;
    private float _throttle;
    private float _rudder;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float throttleInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1 : 0f;
        float rudderInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1 : 0f;

        _throttle = Mathf.MoveTowards(_throttle, throttleInput, _aceleration * Time.deltaTime);
        _rudder = Mathf.MoveTowards(_rudder, throttleInput, _aceleration * Time.deltaTime);

        _rigidbody.AddForceAtPosition(transform.forward * _engineForce * _throttle, _propulsorPoint.position, ForceMode.Force);
        _rigidbody.AddForceAtPosition(transform.right * _rudderForce * _rudder, _rudderPoint.position, ForceMode.Force);
    }

    //https://discussions.unity.com/t/sailing-ship-control/374700/20
    //https://www.habrador.com/tutorials/unity-boat-tutorial/
    //https://medium.com/@joshua.wiscaver/movement-in-unity-with-mathf-2aca0f649dc6

}
