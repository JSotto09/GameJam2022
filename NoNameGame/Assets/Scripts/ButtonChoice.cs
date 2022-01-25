using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChoice : MonoBehaviour
{
    public int thisChoice;

    public void ChangeChoice(string choice)
    {
        thisChoice = int.Parse(choice);
    }

    public void PressedChoice()
    {
        FindObjectOfType<DialogueManager>().MadeChoice(thisChoice);
    }
}
