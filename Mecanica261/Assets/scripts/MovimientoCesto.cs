using UnityEngine;

public class MovimientoCesto : MonoBehaviour
{

    public float velocidad = 5f;
    public float limite = 5f;

    Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       float movimiento = Mathf.PingPong(Time.time * velocidad, limite * 2) - limite;

       transform.position = new Vector3(
        posicionInicial.x + movimiento,
        posicionInicial.y,
        posicionInicial.z
       );
    }
}
