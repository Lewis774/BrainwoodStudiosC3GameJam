using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public DinnerTransitions transitions;
    public string nextSceneName;
    public float fadeDuration = 1.0f;
    public static int visitCount = 0;
    public string[] dialogueFirstTime;
    public string[] dialogueSecondTime;
    public string[] dialogueThirdTime;
    public string[] dialogueFourthTime;
    public string[] dialogueFifthTime;
    public string[] dialogueSixthTime;
    public string[] dialogueSeventhTime;
    private int index;
    private bool isFinished = false;
    private bool isTyping = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetCounter()
    {
        visitCount = 0;
    }
    void Start()
    {
        lines = dialogueFirstTime;
        if (visitCount == 0)
        {
            lines = dialogueFirstTime;
        }
        else if (visitCount == 1)
        {
            lines = dialogueSecondTime;
        }
        else if (visitCount == 2)
        {
            lines = dialogueThirdTime;
        }
        else if (visitCount == 3)
        {
            lines = dialogueFourthTime;
        }
        else if (visitCount == 4)
        {
            lines = dialogueFifthTime;
        }
        else if (visitCount == 5)
        {
            lines = dialogueSixthTime;
        }
        else
        {
            lines = dialogueSeventhTime;
        }
        visitCount++;
        index = 0;
        textComponent.text = string.Empty;
        startDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                return;
            }
            if (textComponent.text == lines[index])
            {
                nextLine();
            }
            else
            {

            }
        }
    }
    void startDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }
    void nextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (!isFinished)
        {
            isFinished = true;
            if (transitions != null)
            {
                transitions.FadeAndLoad(nextSceneName, fadeDuration);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
