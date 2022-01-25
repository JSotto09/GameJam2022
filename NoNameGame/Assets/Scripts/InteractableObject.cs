using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool isInteracting = false;
    private bool isInteractedWith = false;

    public int objectRequirement;

    // Start is called before the first frame update
    //private void Start()
    //{
        
    //}

    private void DisplayObjectInfo()
    {
        if (!isInteractedWith)
        {
            ChangeStatus();
            isInteractedWith = true;
        }
        // Code for displaying object info goes here.
        // Will require new UI with a text box.
        // Will preferabbly use a text file for reading the displayed info.
        // Refer to DialogueManager.cs & DialogueTrigger.cs and my example dialogue files for help in reading text.
        // Ask me if you have questions.
    }

    private void ChangeStatus()
    {
        FindObjectOfType<DialogueManager>().AddRequiredObjects(objectRequirement);
    }

    private void EndInteraction()
    {
        isInteracting = false;
        isInteractedWith = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E) && !isInteracting)
        {
            isInteracting = true;
            DisplayObjectInfo();
            Debug.Log("Interacted");
            EndInteraction();
        }
    }
}
