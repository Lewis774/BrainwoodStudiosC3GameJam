using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpeningHandler : MonoBehaviour
{
    public GameObject introMessage;
    public Image backgroundImage;
    public Image fadePanelImage;
    public Image logoImage;

    void Start()
    {
        introMessage.SetActive(false);
        fadePanelImage.color = new Color(0f,0f,0f,0f);
        logoImage.color = new Color(1f,1f,1f,0f);

        StartCoroutine(AnimationSequence());
    }

    void SetAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    IEnumerator AnimationSequence()
    {   
        yield return new WaitForSeconds(2f);
    
        // Fade in logo
        yield return StartCoroutine(FadeTo(logoImage, 1f, 1f));

        yield return new WaitForSeconds(2f);

        // Fade in panel
        yield return StartCoroutine(FadeTo(fadePanelImage, 1f, 1f));

        SetAlpha(logoImage, 0f);

        yield return new WaitForSeconds(2.5f);

        introMessage.SetActive(true);

        // Ensure background is fully black
        backgroundImage.color = new Color(0f,0f,0f,1f);

        // Fade out panel
        yield return StartCoroutine(FadeTo(fadePanelImage, 0f, 5f));

        // Wait 7s
        yield return new WaitForSeconds(5f);

        // Fade in panel again
        yield return StartCoroutine(FadeTo(fadePanelImage, 1f, 3f));

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeTo(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration); // ensure t stays 0-1
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(image, newAlpha);
            yield return null;
        }

        SetAlpha(image, targetAlpha);
    }
}