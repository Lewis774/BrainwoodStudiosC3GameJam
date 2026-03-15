using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ClsoingHandler : MonoBehaviour
{
    public GameObject closingMessage;
    public TextMeshProUGUI closingText;
    public Image backgroundImage;
    public Image fadePanelImage;
    public Image logoImage;
    public int score;

    void Start()
    {
        closingMessage.SetActive(true);
        fadePanelImage.color = new Color(0f,0f,0f,1f);
        logoImage.color = new Color(1f,1f,1f,0f);

        closingText.text = "Score: " + score + "\n\n";
        if (score > 500)
        {
            closingText.text += "You have succeeded in providing for your family during the crisis. " 
                             + "Afterward, you found a stable new job that supports both you and your family. "
                             + "Food pantries played a crucial role in helping your family survive. "
                             + "Yet millions of people are affected every day by understaffed and under-supported food pantries. "
                             + "Remember the challenges you faced, and consider helping others in their times of need as well. ";
        }                       
        else if (score > 0)
        {
            closingText.text += "You succeeded in providing for your family, but only for this week. "
                                + "Next week, the cycle will continue. You will need to keep looking for a job in hopes of getting something more substantial. "
                                + "Until then, you can only keep doing the odd jobs in your community and relying on food pantries to make ends meet."
                                + "You hope that at some point this cycle ends one way or another. "
                                + "Millions of people are in your scenario, affected every day by understaffed and under-supported food pantries. "
                                + "Remember the challenges you faced, and consider helping others in their times of need as well. ";
        }
        else
        {
            closingText.text = "You tried your best to provide for your family. " 
                                + "However, the food pantries alone were not enough, and now your finances are more uncertain than ever. "
                                + "It feels as though the world is working against you. " 
                                + "Still, you know you would not have made it this far without the help of the food pantries and the people who keep them running."
                                + "Across the country, families face the same uncertainty every day. " 
                                + "Understaffed and under-supported pantries struggle to meet overwhelming demand, while countless households depend on them simply to get through the week. "
                                + "Your story is only one among millions.";
        }

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
        backgroundImage.color = new Color(0f,0f,0f,1f); 

        closingMessage.SetActive(true);

        yield return StartCoroutine(FadeTo(fadePanelImage, 0f, 3f));

        yield return new WaitForSeconds(150f);

        yield return StartCoroutine(FadeTo(fadePanelImage, 1f, 3f));

        backgroundImage.color = new Color(1f,1f,1f,1f);
        closingMessage.SetActive(false);

        yield return StartCoroutine(FadeTo(fadePanelImage, 0f, 3f));

        SetAlpha(logoImage, 0f);
            
        yield return StartCoroutine(FadeTo(logoImage, 1f, 1f));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(FadeTo(fadePanelImage, 1f, 1f));

        Application.Quit();
    }

    public IEnumerator FadeTo(Image image, float targetAlpha, float duration)
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