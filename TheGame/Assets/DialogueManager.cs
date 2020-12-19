using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public PlayerScript player;
    public GameObject dialogueWindow;
    public Text dialogueText;

    public string[] dialogueLines;

    public string[] dialogueLines1;
    public string[] dialogueLines2;
    public string[] dialogueLines3;
    public string[] dialogueLines4;
    public string[] dialogueLines5;
    public string[] dialogueLines6;
    public string[] dialogueLines7;
    public string[] dialogueLines8;
    public string[] dialogueLines9;

    public string currentLine;

    public string currentText;

    public int lineNumber;
    public int dialogueNumber;

    public CameraScript cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        dialogueWindow.SetActive(false);
        dialogueText.text = currentLine;
        cameraScript = Camera.main.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            ChangeLine();
        }
    }

    public void ChangeDialogue()
    {
        lineNumber = 0;
        dialogueNumber++;
        StartCoroutine("ShowText");


        if (dialogueNumber == 1)
        {
            dialogueLines = dialogueLines1;
        }

        else if(dialogueNumber == 2)
        {
            dialogueLines = dialogueLines2;
        }

        else if (dialogueNumber == 3)
        {
            dialogueLines = dialogueLines3;
        }

        else if (dialogueNumber == 4)
        {
            dialogueLines = dialogueLines4;
        }

        else if (dialogueNumber == 5)
        {
            dialogueLines = dialogueLines5;
        }

        else if (dialogueNumber == 6)
        {
            dialogueLines = dialogueLines6;
        }

        else if (dialogueNumber == 7)
        {
            dialogueLines = dialogueLines7;
        }

        else if (dialogueNumber == 8)
        {
            dialogueLines = dialogueLines8;
        }

        else if (dialogueNumber == 9)
        {
            dialogueLines = dialogueLines9;
        }


        currentLine = dialogueLines[lineNumber];
        dialogueText.text = currentLine;
    }

    public void ChangeLine()
    {
        lineNumber++;

        if (lineNumber > dialogueLines.Length-1)
        {
            cameraScript.ReturnCamera();
            dialogueWindow.SetActive(false);
            player.canMove = true;
        }

        else
        {
            currentLine = dialogueLines[lineNumber];
            StartCoroutine("ShowText");   
        }
    }

    public void CloseDialogue()
    {
        cameraScript.ReturnCamera();
        lineNumber = 0;
        dialogueWindow.SetActive(false);
    }

    public IEnumerator ShowText()
    {
        for (int i = 0; i < currentLine.Length; i++)
        {
            currentText = currentLine.Substring(0, i+1);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
