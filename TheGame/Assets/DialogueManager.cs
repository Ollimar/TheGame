using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Speak()
    {
        if(!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
        }
        else if(dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
        }
    }
}
