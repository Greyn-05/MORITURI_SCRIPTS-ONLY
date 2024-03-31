using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractionTextUI : UI_Base
{
    [SerializeField] private TextMeshProUGUI _interactionText;
    
    public void ShowInteractionKey(string questName)
    {
        Invoke("HideUI", 2f);
    }

    private void HideUI()
    {
        Main.UI.DestroySubItemUI<QuestClearUI>(this.gameObject);
    }
}
