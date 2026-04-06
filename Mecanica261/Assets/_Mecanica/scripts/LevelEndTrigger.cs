using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEndTrigger : MonoBehaviour
{
    [Header("Canvas will activate")]
    [SerializeField] private GameObject canvaslevelcomplete;

    [Header("Time before change scene")]
    [SerializeField] private float waitTime = 3f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;

            canvaslevelcomplete.SetActive(true);

            StartCoroutine(ChangeSceneAfterTime());
        }
    }

    private IEnumerator
        ChangeSceneAfterTime()
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+ 1);
    }

}
