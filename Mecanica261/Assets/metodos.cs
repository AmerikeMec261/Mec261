using UnityEngine;

public class metodos : MonoBehaviour
{
    public int vida = 100;
    public void RecibirDaño(int daño)
    {
        vida -= daño;
    } //1

    public int life = 100;
    public bool IsAlive()
    {
        return life > 0;
    } //2

    public float CalcularDistancia(Vector3 pointA, Vector3 pointB)
    {
        return Vector3.Distance(pointA, pointB);
    } //3

    public Vector3 ObtenerDireccion(Vector3 origen, Vector3 destino)
    {
        return (destino - origen);
    } //4

    public string NombreJugador = "Gael";
    public string ObtenerNombre()
    {
        return NombreJugador;
    } //5





 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
