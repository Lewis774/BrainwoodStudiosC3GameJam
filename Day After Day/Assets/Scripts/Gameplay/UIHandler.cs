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

    public TextMeshProUGUI gameLogPanel1;

    public TextMeshProUGUI gameLogPanel2;
    
    public TextMeshProUGUI gameLogPanel3;

    public TextMeshProUGUI weeklyUpdateText;

    public TextMeshProUGUI infoTimeText;

    public TextMeshProUGUI infoUpdateText;

    int lastLoggedQuarter = -1;

    void Start()
    {
        defaultGray = new Color(204f/255f, 204f/255f, 204f/255f, 1f);
        proteinRed = new Color(255f/255f, 81f/255f, 81f/255f, 1f);
        vegetableGreen = new Color(73f/255f, 210f/255f, 86f/255f, 1f);
        yellowGrain = new Color(255f/255f, 231f/255f, 50f/255f, 1f);
        pinkFruit = new Color(255f/255f, 179f/255f, 186f/255f, 1f);
        dairyBlue = new Color(119f/255f, 201f/255f, 255f/255f, 1f);

        gameLogPanel1.text = "";
        gameLogPanel2.text = "";
        gameLogPanel3.text = "";
        weeklyUpdateText.text = "";
        infoTimeText.text = "";
        infoUpdateText.text = "";

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

        int quarter = minutes / 15;

        string period = hours >= 12 ? "PM" : "AM";

        int displayHour = hours % 12;
        if (displayHour == 0)
            displayHour = 12;

        int displayMinutes = quarter * 15;

        timeText.text = $"{displayHour}:{displayMinutes:00} {period}";

        if (quarter != lastLoggedQuarter)
        {
            lastLoggedQuarter = quarter;

            UpdateGameLog("Time is now " + $"{displayHour}:{displayMinutes:00} {period}");
        }
    }

    public void UpdateMoney(int money) 
    {
        moneyText.text = "$  " + money;
    }

    public void UpdateWeek(int week) 
    {
        weekText.text = "Week " + week;
    }

    public void UpdateGameLog(string message)
    {
        gameLogPanel3.text = gameLogPanel2.text;
        gameLogPanel2.text = gameLogPanel1.text;
        gameLogPanel1.text = message;
    }

    public void UpdateWeeklyInfo(string message)
    {
        weeklyUpdateText.text = message;
    }

    public void UpdateInfoPanel(int time, string message)
    {
        infoTimeText.text = "Time: " + time + "min";
        infoTimeText.text = "Time: " + time + "min";
    }    

}

