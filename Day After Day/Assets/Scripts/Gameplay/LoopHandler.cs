using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class LoopHandler : MonoBehaviour
{
    public GameObject UI; 

    public GameObject sceneShader;

    private Image sceneShaderImage;

    public Image resultsShaderImage;

    public GameObject resultsPanel;

    public TextMeshProUGUI[] collectedTexts;

    public TextMeshProUGUI[] costTexts;

    public GameObject weeklyCostHeader;

    public TextMeshProUGUI weeklyCostText;

    public GameObject continueButton;

    string[] prefixes = {
        "Protein Servings Collected:",
        "Vegetable Servings Collected:",
        "Fruit Servings Collected:",
        "Grain Servings Collected:",
        "Dairy Servings Collected:"
    };

    int[] maxServings;

    private UIHandler uiHandler;
 
    public float time = 480;

    public int week;

    public int[] foodGathered;

    public int[] foodCosts;

    int weeklyCost;

    public bool loopOver = false;
    public MapMovementClass player;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<MapMovementClass>();
        sceneShaderImage = sceneShader.GetComponent<Image>();
        SetAlpha(sceneShaderImage, 1f);
        uiHandler = UI.GetComponent<UIHandler>();    
    }

    public IEnumerator StartLoop(int w, int m)
    {
        player.canMove = true;
        player.gameObject.transform.position = new Vector2(player.home.transform.position.x,
                                                           player.home.transform.position.y);
        week = w;
        uiHandler.UpdateMoney(m);
        uiHandler.UpdateWeek(w);
        maxServings = new int[5] {7,7,7,7,7};
        foodGathered = new int[5] {0,0,0,0,0};
        foodCosts = new int[5] {15,5,10,5,5};
        SetAlpha(resultsShaderImage, 0f);

        foreach (TextMeshProUGUI text in collectedTexts)
        {
            text.gameObject.SetActive(false);
        }
        foreach (TextMeshProUGUI text in costTexts)
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }

        weeklyCostHeader.SetActive(false);
        weeklyCostText.text = "";
        weeklyCostText.gameObject.SetActive(false);
        continueButton.SetActive(false);
        resultsPanel.SetActive(false);

        yield return StartCoroutine(OpenLoop());
        sceneShader.SetActive(false);
        time = 480; 

        StartCoroutine(WaitForNoon());
    }

    IEnumerator WaitForNoon()
    {
        yield return new WaitUntil(() => time >= 1200);
        loopOver = true;
        LoopEnd();
    }

    // Update is called once user frame
    void Update()
    {
        if (!loopOver)
        {
            time += Time.deltaTime * 6;
            uiHandler.UpdateTime((int) time);
        }   
    }

    public void Collected()
    {
        for (int i = 0; i < 5; i++)
        {
            uiHandler.UpdateBar(uiHandler.bars[i], foodGathered[i], uiHandler.colors[i]);
        }
    }

    public void LoopEnd()
    {
        player.currentPantry = player.home.GetComponent<PantryClass>();
        player.canMove = false;
        weeklyCost = 0;
        loopOver = true;
        StartCoroutine(ShowResults());
    }

    public int GetWeeklyCost()
    {
        return weeklyCost;
    }

    public IEnumerator OpenLoop()
    {
        sceneShader.SetActive(true);
        yield return StartCoroutine(FadeTo(sceneShaderImage, 0f, 1f));
    }

    public IEnumerator CloseLoop()
    {
        sceneShader.SetActive(true);
        yield return StartCoroutine(FadeTo(sceneShaderImage, 1f, 1f));
        for (int i = 0; i < collectedTexts.Length; i++)
        {
            costTexts[i].text = "";
        }
        weeklyCostText.text = "";
    }
    

    IEnumerator ShowResults()
    {
        weeklyCost = 0;
        yield return StartCoroutine(FadeTo(resultsShaderImage, 0.7f, 2f));

        resultsPanel.SetActive(true);

        for (int i = 0; i < collectedTexts.Length; i++)
        {
            collectedTexts[i].gameObject.SetActive(true);
            costTexts[i].gameObject.SetActive(true);

            int collected = foodGathered[i];
            int cost = foodCosts[i] * (7 - foodGathered[i]);
            weeklyCost += cost;   

            yield return StartCoroutine(RollNumber(collectedTexts[i], prefixes[i], foodGathered[i], maxServings[i], 0.6f));

            if (cost == 0)
            {
                costTexts[i].color = Color.white;
                costTexts[i].text = "$0";
            }
            else
            {
                costTexts[i].color = Color.red;
                costTexts[i].text = "-$" + cost.ToString();
            }

            yield return new WaitForSeconds(0.25f);
        }

        weeklyCostText.text = "";

        weeklyCostHeader.SetActive(true);
        weeklyCostText.gameObject.SetActive(true);
        if (weeklyCost == 0)
        {
            weeklyCostText.color = Color.white;
            weeklyCostText.text = "$0";
        }
        else
        {
            weeklyCostText.color = Color.red;
            weeklyCostText.text = "-$" + weeklyCost.ToString();
        }

        yield return new WaitForSeconds(2f);

        continueButton.SetActive(true);
    }

    IEnumerator RollNumber(TextMeshProUGUI text, string prefix, int collected, int max, float duration)
    {
        float elapsed = 0f;
        int current = 0;
    
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
    
            float t = Mathf.Clamp01(elapsed / duration);
            current = Mathf.RoundToInt(Mathf.Lerp(0, collected, t));
    
            text.text = $"{prefix} {current}/{max}";
    
            yield return null;
        }
    
        text.text = $"{prefix} {collected}/{max}";
    }

    public IEnumerator FadeTo(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(image, newAlpha);
            yield return null;
        }

        SetAlpha(image, targetAlpha);
    }

    void SetAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
}
