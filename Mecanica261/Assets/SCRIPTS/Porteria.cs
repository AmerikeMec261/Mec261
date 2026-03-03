using UnityEngine;

public class Porteria : MonoBehaviour
{
    public float velocidad = 2f;
    public float Movimiento = 5f;

    void Update()
    {
        float movimiento = Mathf.PingPong(Time.time * velocidad, Movimiento) - (Movimiento / 2);

        transform.position = new Vector3(movimiento, transform.position.y, transform.position.z);
    }
}
