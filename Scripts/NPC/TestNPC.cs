using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNPC : MonoBehaviour , IInteract
{
    public void OnInteractionEnter()
    {
        // Debug.Log("NPC Enter");
    }

    public void OnInteractable()
    {
        // Debug.Log("NPC Interact");
    }

    public void OnInteractableExit()
    {
        // Debug.Log("NPC Exit");
    }
    
    
}
