using UnityEngine;

/*public class EstandarizadoPropio : MonoBehaviour
{
    public bool esJugador1 = false;

    [SerializeField] public float velocidad = 7f;
    [SerializeField] public float fuerzaGolpe = 10f;
    [SerializeField] public bool usarFlechas = false;
    [SerializeField] private Vector2 minLimites = new Vector2(-3.5f, -4f);
    [SerializeField] private Vector2 maxLimites = new Vector2(3.5f, 4f);

     void Update()
    {
        float movX = 0f;// movY= 0f;

        float movY = 0f;
        if(usarFlechas)
        {
            if (Input.GetKey(KeyCode.UpArrow)) movY += 1;
            if (Input.GetKey(KeyCode.DownArrow)) movY -= 1;
            if (Input.GetKey(KeyCode.LeftArrow)) movX -= 1;
            if (Input.GetKey(KeyCode.RightArrow)) movX += 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.W)) movY += 1;
            if (Input.GetKey(KeyCode.S)) movY -= 1;
            if (Input.GetKey(KeyCode.A)) movX -= 1;
            if (Input.GetKey(KeyCode.D)) movX += 1;
        }
        Vector3 movimiento = new Vector3(movX, movY, 0).normalized * velocidad * Time.deltaTime;
        Vector3 nuevaPos = transform.position + movimiento;
        nuevaPos.x = Mathf.Clamp(nuevaPos.x, minLimites.x, maxLimites.x);
        nuevaPos.y = Mathf.Clamp(nuevaPos.y, minLimites.y, maxLimites.y);
        transform.position = nuevaPos;
    }
}*/
 // Este fue un codigo que hicimos en 2do semeste para un juego de pelota 2d , estaba desordenado y lo astandarizé
