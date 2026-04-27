using UnityEngine;
using UnityEngine.Rendering;

public class Tower : MonoBehaviour // La torreta ya no debe de funcionar con A y W. Debe seguir a la posición del mouse. 
{
    [Header("Dependences")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector3 _yawLimits = new Vector3(-90f, 90f, 10f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector3 _pitchLimits = new Vector3(-10f, 90f, -90f);

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<SimpleBullet>()?.Fire(_bulletSpawn);
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void RotationMouse()
    {
        
    }
} //Trabajo en clase: Agregar lo solicitado para el exámen. 