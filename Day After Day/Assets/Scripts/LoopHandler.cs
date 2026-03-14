using System;
using System.Data.SqlTypes;
using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;

public class LoopHandler : MonoBehaviour
{
    public GameObject UI; 

    private UIHandler uiHandler;

    public float time = 480;

    public int week = 0;

    public int[] foodGathered;

    public void Start()
    {
        uiHandler = UI.GetComponent<UIHandler>();        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void StartLoop(int week, int money)
    {
        uiHandler.UpdateMoney(money);
        foodGathered = new int[5];
    }

    // Update is called once user frame
    void Update()
    {
        time += Time.deltaTime * 6;
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
