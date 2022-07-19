using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogSystem : MonoBehaviour
{
    private Text dialogText;

    bool dialogGoing;
    int i;
    float startTimer;
    float timer;
    int textIndex;
    string[] textArray;
    string finishtext;
    public string[] words;

    private void Start()
    {
        dialogText = GetComponentInChildren<Text>();

        StartDialog(words, 0.15f);
    }

    public void StartDialog(string[] wordsForText, float speedText)
    {
        dialogGoing = true;
        i = 0;
        textIndex = 0;
        textArray = wordsForText;
        startTimer = speedText;
        finishtext = textArray[textIndex];
    }

    public void EndDialog()
    {
        textIndex = 0;
        i = 0;
        dialogText.text = "";
        dialogGoing = false;
    }

    private void Update()
    {
        if (dialogGoing)
        {
            if (timer <= 0)
            {
                if (i < textArray[textIndex].Length)
                {
                    i++;
                    timer = startTimer;
                    dialogText.text += textArray[textIndex].Substring(i - 1, 1);
                }
                else if (i >= finishtext.Length)
                {
                    textIndex++;
                    dialogText.text = "";
                    if (textIndex >= textArray.Length)
                    {
                        EndDialog();
                    }
                    else
                    {
                        finishtext = textArray[textIndex];
                        i = 0;
                    }
                }
                else
                {
                    i = finishtext.Length;
                    dialogText.text = "";
                    for (int q = 0; q < i + 1; q++)
                    {
                        dialogText.text += textArray[textIndex].Substring(q - 1, 1);
                    }
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

    }
}
