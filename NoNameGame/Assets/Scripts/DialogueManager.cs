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
    private string choiceNumber;

    private bool isChoosing = false;

    private string[] dialogueText;

    private Queue<string> inputStream = new Queue<string>();

    private PlayerMovement playerMove;
    private MouseLook mouseControl;

    private List<string> requiredObjects = new List<string>();

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
        else if (inputStream.Peek().Contains("["))
        {
            if (inputStream.Peek().Contains("[" + choiceMade))
            {
                inputStream.Dequeue();
                BodyText.text = inputStream.Dequeue();
            }
            else
            {
                inputStream.Dequeue();
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
            if (inputStream.Peek().Contains("[CHOICE=") || inputStream.Peek().Contains("[*"))
            {
                //Debug.Log("Pass " + x.ToString());
                //Debug.Log(inputStream.Peek() + " Checking...");
                if (CheckChoice())
                {
                    //Debug.Log(inputStream.Peek() + "Checking for [*");
                    if (inputStream.Peek().Contains("[*"))
                    {
                        x--;
                    }
                    else
                    {
                        //Debug.Log("Checks Out.");
                        //Debug.Log(inputStream.Peek() + " 1st peek");
                        ChoiceButtons[x].SetActive(true);
                        //Debug.Log(inputStream.Peek() + " 2nd peek");
                        ChoiceButtons[x].GetComponent<ButtonChoice>().ChangeChoice(choiceNumber);
                        ChoiceTexts[x].text = inputStream.Dequeue();
                        //Debug.Log(inputStream.Peek() + " Checking again...");
                    }
                }
                else
                {
                    Debug.Log("Doesn't check out.");
                    x--;
                    if (!inputStream.Peek().Contains("[*"))
                    {
                        Debug.Log(inputStream.Dequeue());
                    }
                }
            }
        }
    }

    private bool CheckChoice()
    {
        if (inputStream.Peek().Contains("[CHOICE="))
        {

            //Debug.Log("Removing");
            choiceNumber = inputStream.Dequeue().Substring(8, 1);
            //Debug.Log(choiceNumber);
        }

        if (inputStream.Peek().Contains("[*"))
        {
            //Debug.Log("Check Phase 1");
            //Debug.Log(inputStream.Peek().Substring(3, 1) + "Check Phase 2");
            //Debug.Log(inputStream.Peek().Substring(2, 1) + "Check Phase 3");
            if (inputStream.Peek().Contains("[*!"))
            {
                if (requiredObjects.Contains(inputStream.Dequeue().Substring(3, 1)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (requiredObjects.Contains(inputStream.Dequeue().Substring(2, 1)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            Debug.Log("No requirements");
            return true;
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

    public void AddRequiredObjects(int tempObject)
    {
        requiredObjects.Add(tempObject.ToString());
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
