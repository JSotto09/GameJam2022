using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogueBox;

    public Text BodyText;
    public Text NameText;

    [Space(20)]
    public GameObject ChoicesBox;

    public GameObject[] ChoiceButtons;
    public Text[] ChoiceTexts;

    [Space(20)]
    public bool freezePlayerOnDialogue = true;

    private string choiceMade;

    private bool isChoosing = false;

    private string[] dialogueText;

    private Queue<string> inputStream = new Queue<string>();

    private PlayerMovement playerMove;
    private MouseLook mouseControl;

    // Start is called before the first frame update
    void Start()
    {
        DialogueBox.SetActive(false);
        ChoicesBox.SetActive(false);
        for (int x = 0; x < 4; x++)
        {
            ChoiceButtons[x].SetActive(false);
        }

    }

    private void DisablePlayerMovement()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerMove.Idle();
        playerMove.enabled = false;
    }

    private void EnablePlayerMovement()
    {
        playerMove.enabled = true;
    }

    private void DisableCameraControl()
    {
        mouseControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseLook>();
        mouseControl.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    
    private void EnableCameraControl()
    {
        mouseControl.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartDialogue(Queue<string> dialogue)
    {
        if (freezePlayerOnDialogue)
        {
            DisablePlayerMovement();
        }

        DialogueBox.SetActive(true);
        inputStream = dialogue;
        PrintDialogue();
    }

    public void AdvanceDialogue()
    {
        if (!isChoosing)
        {
            PrintDialogue();
        }
    }

    private void PrintDialogue()
    {
        if (inputStream.Peek().Contains("EndQueue"))
        {
            inputStream.Dequeue();
            EndDialogue();
        }
        else if (inputStream.Peek().Contains("[NAME="))
        {
            string name = inputStream.Peek();
            name = inputStream.Dequeue().Substring(name.IndexOf('=') + 1, name.IndexOf(']') - (name.IndexOf('=') + 1));
            NameText.text = name;
            PrintDialogue();
        }
        else if (inputStream.Peek().Contains("**Choices"))
        {
            inputStream.Dequeue();
            PrintChoices();
        }
        else if (inputStream.Peek().Contains("[CHOICE="))
        {
            PrintChoices();
        }
        else if (inputStream.Peek().Contains("{"))
        {
            if (inputStream.Peek().Contains("{" + choiceMade))
            {
                string[] dialogueText = inputStream.Dequeue().Split(char.Parse("_"));
                BodyText.text = dialogueText[1];
                System.Array.Clear(dialogueText, 0, dialogueText.Length);
            }
            else
            {
                inputStream.Dequeue();
                PrintDialogue();
            }
        }
        else
        {
            BodyText.text = inputStream.Dequeue();
        }
    }

    private void PrintChoices()
    {
        DisableCameraControl();
        ChoicesBox.SetActive(true);
        isChoosing = true;
        for (int x = 0; x < 4; x++)
        {
            if (inputStream.Peek().Contains("[CHOICE="))
            {
                ChoiceButtons[x].SetActive(true);
                inputStream.Dequeue();
                string tempString = inputStream.Dequeue();
                Debug.Log(tempString);
                string[] dialogueText = tempString.Split(char.Parse("_"));
                Debug.Log(dialogueText[1]);
                ChoiceTexts[x].text = dialogueText[1];
                System.Array.Clear(dialogueText, 0, dialogueText.Length);
            }
        }
    }

    public void MadeChoice(int choice)
    {
        choiceMade = choice.ToString();
        ChoicesBox.SetActive(false);
        PrintDialogue();
        EnableCameraControl();
        isChoosing = false;
    }

    public void EndDialogue()
    {
        BodyText.text = "";
        NameText.text = "";
        inputStream.Clear();
        DialogueBox.SetActive(false);
        for (int x = 0; x < 4; x++)
        {
            ChoiceButtons[x].SetActive(false);
        }

        if (freezePlayerOnDialogue)
        {
            EnablePlayerMovement();
        }

        if (mouseControl.enabled == false)
        {
            mouseControl.enabled = true;
        }
    }
}
