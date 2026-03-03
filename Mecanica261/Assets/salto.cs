using UnityEngine;

public class salto : MonoBehaviour
{
    public float fuerza = 10f;
    float velocidadY;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            velocidadY = fuerza;
        }

        velocidadY -= 9.8f * Time.deltaTime;

        transform.position += new Vector3(0, velocidadY * Time.deltaTime, 0);
    }
}
