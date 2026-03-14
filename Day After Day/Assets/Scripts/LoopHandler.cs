using System;
using System.Data.SqlTypes;
using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;

public class LoopHandler : MonoBehaviour
{
    public GameObject UI; 

    public UIHandler uiHandler;

    public float time = 480;

    public int week = 0;

    public int[] foodGathered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void StartLoop(int week, int money)
    {
        uiHandler = UI.GetComponent<UIHandler>();
        uiHandler.UpdateMoney(money);
        foodGathered = new int[5];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * 5;
        uiHandler.UpdateTime((int) time);
    }

    void Collected()
    {
        for (int i = 0; i < 5; i++)
        {
            uiHandler.UpdateBar(uiHandler.bars[i], foodGathered[i], uiHandler.colors[i]);
        }
    }
}
