using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadelmage;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private string nextScenceName;

    public void FadeToNextScence()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 0;

        while (alpha <1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadelmage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

       SceneManager.LoadScene(nextScenceName);
    }
    
}
