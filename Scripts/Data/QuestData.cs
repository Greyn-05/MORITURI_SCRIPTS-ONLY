public class QuestData
{
    // 퀘스트 데이터
    public int QuestID;
    public string QuestName;
    public string QuestText;
    public string QuestCategory;
    public int ResponsibleNPCID;
    
    public int PreviousQuestID;
    public int NextQuestID;
    
    public string ClearNPC;
    public int ItemID;
    public int ItemCount;
    public int CombatID;
    public int CombatCount;
    
    public int RewardGold;
    public int RewardItem;
    public int RewardItemCount;
    
    public string QuestDialogue;
    public string AcceptText;
    public string DeclineText;

    //진행 상태
    public int CurrentCount;
    public Define.QuestType QuestType;

    public bool isActive;
    public bool isClear;
    public bool HasReward { get; set; }
}