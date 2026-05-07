using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [SerializeField] private Transform enemytarget;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(enemytarget);
    }

    // video de referencia para el target enemigo https://youtu.be/qxKEHKJ0e8A

}
