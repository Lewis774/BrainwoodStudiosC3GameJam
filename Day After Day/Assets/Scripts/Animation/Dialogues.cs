using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Mother Arrays
    public GameObject[] imageFirstVisitMother;
    public GameObject[] imageSecondVisitMother;
    public GameObject[] imageThirdVisitMother;
    public GameObject[] imageFourthVisitMother;
    public GameObject[] imageFifthVisitMother;
    public GameObject[] imageSixthVisitMother;
    public GameObject[] imageSeventhVisitMother;

    // Father Arrays
    public GameObject[] imageFirstVisitFather;
    public GameObject[] imageSecondVisitFather;
    public GameObject[] imageThirdVisitFather;
    public GameObject[] imageFourthVisitFather;
    public GameObject[] imageFifthVisitFather;
    public GameObject[] imageSixthVisitFather;
    public GameObject[] imageSeventhVisitFather;

    // Daughter Arrays
    public GameObject[] imageFirstVisitDaughter;
    public GameObject[] imageSecondVisitDaughter;
    public GameObject[] imageThirdVisitDaughter;
    public GameObject[] imageFourthVisitDaughter;
    public GameObject[] imageFifthVisitDaughter;
    public GameObject[] imageSixthVisitDaughter;
    public GameObject[] imageSeventhVisitDaughter;

    private int index;
    private bool isFinished = false;
    private bool isTyping = false;

    private GameObject[] activeImagesMother;
    private GameObject[] activeImagesFather;
    private GameObject[] activeImagesDaughter;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetCounter()
    {
        visitCount = 0;
    }

    void Start()
    {
        // Determine which set of arrays to use based on visit count
        if (visitCount == 0)
        {
            lines = dialogueFirstTime;
            activeImagesMother = imageFirstVisitMother;
            activeImagesFather = imageFirstVisitFather;
            activeImagesDaughter = imageFirstVisitDaughter;
        }
        else if (visitCount == 1)
        {
            lines = dialogueSecondTime;
            activeImagesMother = imageSecondVisitMother;
            activeImagesFather = imageSecondVisitFather;
            activeImagesDaughter = imageSecondVisitDaughter;
        }
        else if (visitCount == 2)
        {
            lines = dialogueThirdTime;
            activeImagesMother = imageThirdVisitMother;
            activeImagesFather = imageThirdVisitFather;
            activeImagesDaughter = imageThirdVisitDaughter;
        }
        else if (visitCount == 3)
        {
            lines = dialogueFourthTime;
            activeImagesMother = imageFourthVisitMother;
            activeImagesFather = imageFourthVisitFather;
            activeImagesDaughter = imageFourthVisitDaughter;
        }
        else if (visitCount == 4)
        {
            lines = dialogueFifthTime;
            activeImagesMother = imageFifthVisitMother;
            activeImagesFather = imageFifthVisitFather;
            activeImagesDaughter = imageFifthVisitDaughter;
        }
        else if (visitCount == 5)
        {
            lines = dialogueSixthTime;
            activeImagesMother = imageSixthVisitMother;
            activeImagesFather = imageSixthVisitFather;
            activeImagesDaughter = imageSixthVisitDaughter;
        }
        else
        {
            lines = dialogueSeventhTime;
            activeImagesMother = imageSeventhVisitMother;
            activeImagesFather = imageSeventhVisitFather;
            activeImagesDaughter = imageSeventhVisitDaughter;
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
            if (isTyping) return;

            if (textComponent.text == lines[index])
            {
                nextLine();
            }
        }
    }

    void startDialogue()
    {
        index = 0;
        UpdateImageForLine();
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
            UpdateImageForLine();
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

    void UpdateImageForLine()
    {
        // First, hide every single character image from every visit
        DisableAllVisitImages();

        // Then, show the specific images for this specific line and visit
        if (activeImagesMother != null && index < activeImagesMother.Length && activeImagesMother[index] != null)
            activeImagesMother[index].SetActive(true);

        if (activeImagesFather != null && index < activeImagesFather.Length && activeImagesFather[index] != null)
            activeImagesFather[index].SetActive(true);

        if (activeImagesDaughter != null && index < activeImagesDaughter.Length && activeImagesDaughter[index] != null)
            activeImagesDaughter[index].SetActive(true);
    }

    void DisableAllVisitImages()
    {
        // Grouping all arrays into local lists to loop through them efficiently
        GameObject[][] allMother = { imageFirstVisitMother, imageSecondVisitMother, imageThirdVisitMother, imageFourthVisitMother, imageFifthVisitMother, imageSixthVisitMother, imageSeventhVisitMother };
        GameObject[][] allFather = { imageFirstVisitFather, imageSecondVisitFather, imageThirdVisitFather, imageFourthVisitFather, imageFifthVisitFather, imageSixthVisitFather, imageSeventhVisitFather };
        GameObject[][] allDaughter = { imageFirstVisitDaughter, imageSecondVisitDaughter, imageThirdVisitDaughter, imageFourthVisitDaughter, imageFifthVisitDaughter, imageSixthVisitDaughter, imageSeventhVisitDaughter };

        foreach (var array in allMother) if (array != null) foreach (var img in array) if (img != null) img.SetActive(false);
        foreach (var array in allFather) if (array != null) foreach (var img in array) if (img != null) img.SetActive(false);
        foreach (var array in allDaughter) if (array != null) foreach (var img in array) if (img != null) img.SetActive(false);
    }
}