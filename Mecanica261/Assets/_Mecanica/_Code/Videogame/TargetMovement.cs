using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float speed = 3f;
    public float range = 5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, range);
        transform.position = startPos + new Vector3(0, 0, offset);
    }
}