using UnityEngine;

public class EnemyBoat : MonoBehaviour, IDamage, IEnemyInfo
{
    [Header("Estadistics")]
    [SerializeField] private float _life = 100f;
    [SerializeField] private float _shield = 0f;
    [SerializeField] private float _speed = 2f;

    private void Start()
    {
        Data(); 
    }
    public void TakeDamage(float damage)
    {
        _life -= damage; 

        Debug.Log(gameObject.name + "Daño Recibido" + damage);
        Debug.Log(gameObject.name + "Vida Faltante" + _life);

        if (_life <= 0f) 
        {
            Destroy(gameObject);
        }
    }
    public float Life()
    {
        return _life;
    }
    public float Shield()
    {
        return _shield;
    }
    public float Speed()
    {
        return _speed;
    }
    public void Data() //Muestra las estadisticas del enemigo en un solo mensaje
    {
        Debug.Log(gameObject.name + "Vida:" + Life() + "Escudo:" + Shield() + "Velocidad:" + Speed());
    }
}
