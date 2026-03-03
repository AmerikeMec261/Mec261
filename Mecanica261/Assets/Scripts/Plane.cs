using UnityEngine;

public class Plane : MonoBehaviour
{
    [Header("Plataforma Movment")]

    public float velocidad = 3f;
    public float distancia = 3f;

    private Vector3 _posicionInicial;
    private Vector3 _posicionFinal;

     void Start()
    {
        _posicionInicial = transform.position;

        _posicionFinal = _posicionInicial + new Vector3(0, 0, distancia);        
    }

     void Update()
    {
        float tiempoo = Mathf.PingPong(Time.time * velocidad, 2f);

        transform.position = Vector3.Lerp(_posicionInicial, _posicionFinal, tiempoo);
        
    }
}
