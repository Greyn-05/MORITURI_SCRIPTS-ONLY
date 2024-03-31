using UnityEngine;

public class TestGate : MonoBehaviour, IInteract
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) OnInteractionEnter();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) OnInteractableExit();
    }

    public void OnInteractionEnter()
    {
        Main.Player.OnInteractiveEvent += OnInteractable;
    }

    public void OnInteractableExit()
    {
        // Debug.Log("NPC Exit");
        Main.Player.OnInteractiveEvent -= OnInteractable;
    }

    public void OnInteractable()
    {
       Main.UI.OpenPopup<UI_Popup_BossSelect>();
    }


    private void OnDisable()
    {
        Main.Player.OnInteractiveEvent -= OnInteractable;
    }

}
