using UnityEngine;
using UnityEngine.UI;

public class Enceste : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;
    public AudioClip swishsound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pelota"))
        {
            score++;
            scoreText.text=score.ToString();
            AudioSource.PlayClipAtPoint(swishsound,transform.position);
            Destroy(other.gameObject,2f);
        }
    }
}
