using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float speed;

    [Header("Movement")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Transform target;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector3.MoveTowards( // por qué los saltos de línea? Esto es consistente con output de GPT.
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        Debug.Log(gameObject.name + " Vida restante: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

// El método Vector3.MoveTowards lo use para mover al enemigo
// gradualmente desde su posición actual hacia un punto objetivo
// sin sobrepasarlo, permitiendo un movimiento constante entre
// pointA y pointB.
//
// Utilice Vector3.Distance para detectar cuándo
// el enemigo llegó suficientemente cerca del objetivo y así
// cambiar su dirección utilizando un operador ternario.
//
// Además, Time.deltaTime se implementó para que la velocidad
// del movimiento sea independiente de los FPS y funcione de
// manera consistente en diferentes computadoras.
//
// Documentación oficial:
// https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
// https://docs.unity3d.com/ScriptReference/Vector3.Distance.html
// https://docs.unity3d.com/ScriptReference/Time-deltaTime.html
// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator

