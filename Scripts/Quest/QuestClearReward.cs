using System;
using UnityEngine;

public class QuestClearReward
{
    public bool _isSubscribed = false;
    
    public void OnQuestComplete(QuestData questData)
    {
        TakeReward(questData);
    }

    private void TakeReward(QuestData questData)
    {
        if (questData.HasReward)
        {
            return;
        }
        
        if (questData.RewardGold > 0)
        {
            Main.Player.playerData.gold += questData.RewardGold;
            questData.HasReward = true;
            // Debug.Log($"보상 : 골드 {questData.RewardGold}");
        }

        if (Main.CSVData.itemDatas.TryGetValue(questData.RewardItem, out ItemData rewardItem))
        {
            Main.Inven.Add(rewardItem, questData.RewardItemCount);
            questData.HasReward = true;
            // Debug.Log($"보상 : {rewardItem.itemName}x{questData.RewardItemCount}");
        }

        Main.Quest.UpdateGoldCount(Main.Player.playerData.gold);
    }
    
    #region event -----
    public void Subscribe()
    {
        if (!_isSubscribed)
        {
            Main.Quest.OnQuestCompleted += OnQuestComplete;
            _isSubscribed = true;
        }
    }

    public void Unsubscribe()
    {
        if (_isSubscribed)
        {
            Main.Quest.OnQuestCompleted -= OnQuestComplete;
            _isSubscribed = false;
        }
    }
    
    #endregion
}