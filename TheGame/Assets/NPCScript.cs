using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public GameObject dialogueIndicator;

    public enum NPCcharacter { Bird, Bear, Fish, Fox}
    public NPCcharacter character;

    public int dialogueNumber;

    private DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("GameManager").GetComponent<DialogueManager>();
        dialogueIndicator.SetActive(false);
    }

    public void SetDialogue()
    {
        switch (character)
        {
            case NPCcharacter.Bird:
                dialogueNumber = 1;
                break;

            case NPCcharacter.Bear:
                dialogueNumber = 2;
                break;

            case NPCcharacter.Fish:
                dialogueNumber = 3;
                break;

            case NPCcharacter.Fox:
                dialogueNumber = 4;
                break;

            default:
                break;
        }

        dialogueManager.ChangeDialogue(dialogueNumber);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            dialogueIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dialogueIndicator.SetActive(false);
        }
    }
}
