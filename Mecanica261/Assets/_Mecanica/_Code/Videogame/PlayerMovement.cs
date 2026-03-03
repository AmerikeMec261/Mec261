using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        transform.Translate(Vector3.right * move * speed * Time.deltaTime);
    }
}