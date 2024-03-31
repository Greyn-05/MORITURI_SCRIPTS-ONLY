using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class QuestView : UI_Base
{
    [SerializeField] private GameObject viewPanelPrefab;

    public TMP_Text questNameText;
    public TMP_Text questText;
    
    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }
        
        return true;
    }
    
    private void UpdateQuestUI(QuestData questData)
    {
        if (questData == null)
        {
            return;
        }
        
        if (questData.QuestType == Define.QuestType.Main)
        {
            MainQuestView(questData);
        }
        else if (questData.QuestType == Define.QuestType.Sub)
        {
            SubQuestView(questData);
        }
    }
    
    public void MainQuestView(QuestData currentMainQuest)
    {
        string questNameInfo = "";
        string questTextInfo = "";
        
        if (currentMainQuest != null)
        {
            string questStatus = Main.Quest.GetReadyToClearQuests().Contains(currentMainQuest) ? "[클리어 가능]" : "[진행중]";
            string[] splitText = currentMainQuest.QuestText.Split(new[] { "{A}" }, StringSplitOptions.RemoveEmptyEntries);
            string rewardItemName = currentMainQuest.RewardItem > 0 && Main.CSVData.itemDatas.TryGetValue(currentMainQuest.RewardItem, out var itemData) ? itemData.itemName : "";
            string rewardItemCount = currentMainQuest.RewardItemCount > 0 ? "x" + currentMainQuest.RewardItemCount.ToString() : "";
            string rewardGoldCount = currentMainQuest.RewardGold > 0 ? currentMainQuest.RewardGold.ToString() + "G" : "";
            string joinedText = string.Join("\n", splitText);
            
            questNameInfo += $"{currentMainQuest.QuestName} {questStatus}";
            questTextInfo += $"{joinedText}\n\n\n보상 : {rewardGoldCount} / {rewardItemName} {rewardItemCount}\n\n";
        }

        questNameText.text = questNameInfo;
        questText.text = questTextInfo;
    }

    public void SubQuestView(QuestData subQuest)
    {
        if (subQuest == null)
        {
            return;
        }

        string questStatus = Main.Quest.GetReadyToClearQuests().Contains(subQuest) ? "[클리어 가능]" : "[진행중]";
        string[] splitText = subQuest.QuestText.Split(new[] { "{A}" }, StringSplitOptions.RemoveEmptyEntries);
        string joinedText = string.Join("\n", splitText);
        
        string rewardItemName = subQuest.RewardItem > 0 && Main.CSVData.itemDatas.TryGetValue(subQuest.RewardItem, out var itemData) ? itemData.itemName : "";
        string rewardItemCount = subQuest.RewardItemCount > 0 ? "x" + subQuest.RewardItemCount.ToString() : "";
        string rewardGoldCount = subQuest.RewardGold > 0 ? subQuest.RewardGold.ToString() + "G" : "";

        string questNameInfo = $"{subQuest.QuestName} {questStatus}";
        string progressInfo = GetProgressInfo(subQuest);
        string questTextInfo = $"{joinedText}\n{progressInfo}\n\n클리어 NPC : {subQuest.ClearNPC} \n\n 보상 : {rewardGoldCount} / {rewardItemName} {rewardItemCount}";
        
        questNameText.text = questNameInfo;
        questText.text = questTextInfo + "\n\n";
    }
    
    private string GetProgressInfo(QuestData subQuest)
    {
        switch (subQuest.QuestType)
        {
            case Define.QuestType.Sub:
                if (subQuest.ItemCount > 0)
                {
                    return $"{subQuest.CurrentCount} / {subQuest.ItemCount}";
                }
                if (subQuest.CombatCount > 0)
                {
                    return $"{subQuest.CurrentCount} / {subQuest.CombatCount}";
                }
                break;
            default:
                Debug.LogError($"Unknown QuestType: {subQuest.QuestType}");
                //return "정보 없음";
                break;
        }

        return "";
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    
    #region event -----
    
    private void OnEnable()
    {
        Main.Quest.OnQuestUpdated += UpdateQuestUI;
        Main.Quest.OnQuestAdded += UpdateQuestUI;
        Main.Quest.OnQuestCompleted += UpdateQuestUI;
    }

    private void OnDisable()
    {
        Main.Quest.OnQuestUpdated -= UpdateQuestUI;
        Main.Quest.OnQuestAdded -= UpdateQuestUI;
        Main.Quest.OnQuestCompleted -= UpdateQuestUI;
    }
    
    #endregion
}
