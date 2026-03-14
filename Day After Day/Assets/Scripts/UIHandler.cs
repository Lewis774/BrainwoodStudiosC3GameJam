using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI moneyText; 

    public TextMeshProUGUI weekText;

    public GameObject proteinBar;

    public GameObject vegetableBar;

    public GameObject carbsBar;

    public GameObject fruitBar;

    public GameObject dairyBar;

    private Color defaultGray;

    private Color proteinRed;

    private Color vegetableGreen;

    private Color yellowGrain;

    private Color pinkFruit;

    private Color dairyBlue;

    public GameObject[] bars;

    public Color[] colors;

    void Start()
    {
        defaultGray = new Color(204f/255f, 204f/255f, 204f/255f, 1f);
        proteinRed = new Color(255f/255f, 81f/255f, 81f/255f, 1f);
        vegetableGreen = new Color(73f/255f, 210f/255f, 86f/255f, 1f);
        yellowGrain = new Color(255f/255f, 231f/255f, 50f/255f, 1f);
        pinkFruit = new Color(255f/255f, 179f/255f, 186f/255f, 1f);
        dairyBlue = new Color(119f/255f, 201f/255f, 255f/255f, 1f);

        bars = new GameObject[5] {proteinBar, vegetableBar, carbsBar, fruitBar, dairyBar};
        colors = new Color[5] {proteinRed, vegetableGreen, yellowGrain, pinkFruit, dairyBlue};

        ResetBars();
    }

    void ResetBars()
    {
        UpdateBar(proteinBar, 7, defaultGray);
        UpdateBar(vegetableBar, 7, defaultGray);
        UpdateBar(carbsBar, 7, defaultGray);
        UpdateBar(fruitBar, 7, defaultGray);  
        UpdateBar(dairyBar, 7, defaultGray);      
    }


    public void UpdateBar(GameObject bar, int filled, Color color) 
    {
        Transform transform = bar.GetComponent<Transform>();

        for (int i = 0; i < Math.Clamp(filled, 0, 7); i++)
        {
            Transform child = transform.GetChild(i);

            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                img.color = color;
            }
        }
    }

    public void UpdateTime(int timeInMinutes)
    {
        int hours = timeInMinutes / 60;
        int minutes = timeInMinutes % 60;

        minutes = (minutes / 15) * 15;

        string period = hours >= 12 ? "PM" : "AM";

        int displayHour = hours % 12;
        if (displayHour == 0)
            displayHour = 12;

        timeText.text = $"{displayHour}:{minutes:00} {period}";
    }

    public void UpdateMoney(int money) 
    {
        moneyText.text = "$  " + money;
    }

    public void UpdateWeek(int week) 
    {
        weekText.text = "Week " + week;
    }

}

