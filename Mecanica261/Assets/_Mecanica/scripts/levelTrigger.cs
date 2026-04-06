using UnityEngine;

public class levelTrigger : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                fadeManager.FadeToNextScence();
            }
        }
}
