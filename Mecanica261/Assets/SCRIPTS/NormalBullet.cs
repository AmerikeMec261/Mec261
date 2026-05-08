using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField] private float _bulletTime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, _bulletTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollision(Collision collision)
    {
        Destroy(gameObject);
    }
}
