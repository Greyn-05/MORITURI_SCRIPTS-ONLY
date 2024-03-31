using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVDataBase
{
    private bool initialized = false;


    public Dictionary<int, NPCData> npcDatas = new();
    public Dictionary<int, DialogueData> dialogues = new();
    public Dictionary<int, List<DialogueData>> gatherDialogues = new();
    public Dictionary<int, QuestData> mainQuests = new();
    public Dictionary<int, QuestData> subQuests = new();
    public Dictionary<int, ItemData> itemDatas = new();
    public Dictionary<int, QuestDialogueData> questDialogues = new();
    public Dictionary<int, List<QuestDialogueData>> questText = new();
    //  public Dictionary<int, > craftDatas = new();

    public void Initialize()
    {
        if (initialized) return;

        LoadDialogueData();
        LoadNPCData();
        LoadQuestData();
        LoadItemData();
        LoadCraftDatas();
        LoadQuestDialogueData();

        initialized = true;
    }



    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■



    private void LoadNPCData()
    {
        var dataList = CSVReader.Read("NPCInfoCSV");
        foreach (var data in dataList)
        {
            NPCData npc = new NPCData
            {
                NPCID = int.Parse(data["NPCID"].ToString()),
                NPCName = data["NPCName"].ToString(),
                NPCRace = data["NPCRace"].ToString(),
                NPCJob = data["NPCJob"].ToString(),
                NPCSex = data["NPCSex"].ToString(),
                Dialogues = new List<DialogueData>()
            };
            //Debug.Log($"Loaded NPC: ID = {npc.NPCID}, Name = {npc.NPCName}");
            npcDatas.Add(npc.NPCID, npc);
        }
    }


    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    private void LoadDialogueData()
    {
        var dataList = CSVReader.Read("DialogueCSV");
        foreach (var data in dataList)
        {
            var gatherID = int.Parse(data["GatherID"].ToString());
            var dialogue = new DialogueData
            {
                dialogueID = int.Parse(data["DialogueID"].ToString()),
                speakerName = data["SpeakerName"].ToString(),
                dialogueText = data["DialogueText"].ToString(),
                gatherID = gatherID
            };

            if (!dialogues.ContainsKey(dialogue.dialogueID))
            {
                dialogues.Add(dialogue.dialogueID, dialogue);
            }

            if (!gatherDialogues.ContainsKey(gatherID))
            {
                gatherDialogues.Add(gatherID, new List<DialogueData>());
            }
            gatherDialogues[gatherID].Add(dialogue);
        }
    }

    public List<DialogueData> GetDialoguesByGatherID(int gatherID)
    {
        if (gatherDialogues.TryGetValue(gatherID, out var dialogues))
        {
            return dialogues;
        }
        else
        {
            return new List<DialogueData>();
        }
    }



    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■


    private void LoadQuestData()
    {
        var dataList = CSVReader.Read("QuestCSV");

        foreach (var data in dataList)
        {
            try
            {
                int.TryParse(data["QuestID"].ToString(), out int questID);
                string clearNPC = data["ClearNPC"].ToString();
                int itemID = -1; // 기본값 설정
                int itemCount = -1; // 기본값 설정
                int combatID = -1;
                int combatCount = -1;
                int rewardGold = -1; // 기본값 설정
                int rewardItem = -1; // 기본값 설정
                int rewardItemCount = -1;
                int responsibleNPCID = -1; // 기본값 설정
                int previousQuestId = -1; // 기본값 설정
                int nextQuestId = -1; // 기본값 설정

                if (data.ContainsKey("ItemID") && !string.IsNullOrWhiteSpace(data["ItemID"].ToString()))
                {
                    int.TryParse(data["ItemID"].ToString(), out itemID);
                }
                if (data.ContainsKey("ItemCount") && !string.IsNullOrWhiteSpace(data["ItemCount"].ToString()))
                {
                    int.TryParse(data["ItemCount"].ToString(), out itemCount);
                }
                if (data.ContainsKey("CombatID") && !string.IsNullOrWhiteSpace(data["CombatID"].ToString()))
                {
                    int.TryParse(data["CombatID"].ToString(), out combatID);
                }
                if (data.ContainsKey("CombatCount") && !string.IsNullOrWhiteSpace(data["CombatCount"].ToString()))
                {
                    int.TryParse(data["CombatCount"].ToString(), out combatCount);
                }
                if (data.ContainsKey("RewardGold") && !string.IsNullOrWhiteSpace(data["RewardGold"].ToString()))
                {
                    int.TryParse(data["RewardGold"].ToString(), out rewardGold);
                }
                if (data.ContainsKey("RewardItem") && !string.IsNullOrWhiteSpace(data["RewardItem"].ToString()))
                {
                    int.TryParse(data["RewardItem"].ToString(), out rewardItem);
                }
                if (data.ContainsKey("RewardItemCount") && !string.IsNullOrWhiteSpace(data["RewardItemCount"].ToString()))
                {
                    int.TryParse(data["RewardItemCount"].ToString(), out rewardItemCount);
                }
                if (data.ContainsKey("ResponsibleNPCID") && !string.IsNullOrWhiteSpace(data["ResponsibleNPCID"].ToString()))
                {
                    int.TryParse(data["ResponsibleNPCID"].ToString(), out responsibleNPCID);
                }
                if (data.ContainsKey("PreviousQuestID") && !string.IsNullOrWhiteSpace(data["PreviousQuestID"].ToString()))
                {
                    int.TryParse(data["PreviousQuestID"].ToString(), out previousQuestId);
                }
                if (data.ContainsKey("NextQuestID") && !string.IsNullOrWhiteSpace(data["NextQuestID"].ToString()))
                {
                    int.TryParse(data["NextQuestID"].ToString(), out nextQuestId);
                }

                QuestData quest = new QuestData
                {
                    QuestID = questID,
                    QuestName = data["QuestName"].ToString(),
                    QuestText = data["QuestText"].ToString(),
                    QuestCategory = data["QuestCategory"].ToString(),
                    ClearNPC = clearNPC,
                    ItemID = itemID,
                    ItemCount = itemCount,
                    CombatID = combatID,
                    CombatCount = combatCount,
                    RewardGold = rewardGold,
                    RewardItem = rewardItem,
                    RewardItemCount = rewardItemCount,
                    ResponsibleNPCID = responsibleNPCID,
                    QuestDialogue = data.ContainsKey("QuestDialogue") ? data["QuestDialogue"].ToString() : "",
                    PreviousQuestID = previousQuestId,
                    NextQuestID = nextQuestId,
                    QuestType = questID >= 900 ? Define.QuestType.Main : Define.QuestType.Sub
                };

                // Debug.Log($"Quest Loaded: ID = {quest.QuestID}, Name = {quest.QuestName}");

                if (quest.QuestID >= 900)
                {
                    mainQuests.Add(quest.QuestID, quest);
                }
                else
                {
                    subQuests.Add(quest.QuestID, quest);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"error {data}: {ex.Message}");
            }
        }
    }


    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■


    public void LoadItemData()
    {

        var datas = CSVReader.Read("ItemDatas");

        foreach (var data in datas)
        {
            ItemData itemData = new()
            {
                id = int.Parse(data["ID"].ToString()),
                itemName = data["이름"].ToString(),
                description = data["설명"].ToString(),
                price = (int)data["가격"],
                iconImage = Main.Resource.Load<Sprite>(data["이미지파일명"].ToString()),
                type = (Define.ItemType)int.Parse(data["종류"].ToString())
            };


            itemDatas.Add(itemData.id, itemData);
        }


        //  Main.Resource.Unload("");

    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    public void LoadCraftDatas()
    {
        // 제작 레시피 데이터

        //   var datas = CSVReader.Read("CraftDatas");


    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    private void LoadQuestDialogueData()
    {
        var dataList = CSVReader.Read("QuestDialogueCSV");
        foreach (var data in dataList)
        {
            int questTextID = int.Parse(data["QuestTextID"].ToString());
            var questDialogue = new QuestDialogueData
            {
                subID = int.Parse(data["SubID"].ToString()),
                qdText = data["QDText"].ToString(),
                questTextID = questTextID
            };

            if (!questDialogues.ContainsKey(questDialogue.subID))
            {
                questDialogues.Add(questDialogue.subID, questDialogue);
            }

            if (!questText.ContainsKey(questTextID))
            {
                questText.Add(questTextID, new List<QuestDialogueData>());
            }

            questText[questTextID].Add(questDialogue);
        }
    }

    public List<QuestDialogueData> GetDialoguesByQuestTextID(int questTextID)
    {
        if (questText.TryGetValue(questTextID, out var questDialoguesList))
        {
            return questDialoguesList;
        }
        else
        {
            return new List<QuestDialogueData>();
        }
    }
}