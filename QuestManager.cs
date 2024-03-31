using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager
{
    /*PROPERTY*/
    public QuestData CurrentMainQuest;// 진행중인 메인퀘
    public List<QuestData> ActiveSubQuests; // 진행중인 서브퀘
    public List<QuestData> AvailableSubQuests;// 진행 가능한 서브퀘
    public List<QuestData> ReadyToClear;// 클리어 가능한 퀘스트
    public List<QuestData> CompletedQuests; // 완료된 퀘스트 목록
    public List<QuestData> AllQuestList;// 모든 퀘스트 목록
    /*UI*/
    private QuestClearUI _questClearUI;
    private DialogueUI _dialogueUI;
    private QuestClearReward _questClearReward;
    private EnemyInfo _enemyInfo;
    public Quest_Popup questPopUpUi;
    private NPCInfo _npcInfo;
    /*EVENT*/
    public event Action<QuestData> OnQuestCompleted; // 퀘스트 완료 이벤트
    public event Action<QuestData> OnQuestAdded; // 퀘스트 추가 이벤트
    public event Action<QuestData> OnQuestUpdated; // 퀘스트 업데이트 이벤트
    public event Action<bool> OnQuestStatusUpdated;
    public event Action OnQuestMarkUpdate;
    /*STATUS*/
    private bool initialized = false;
    /*ECT*/

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    
    public void Initialize()
    {
        if (initialized)
        {
            return;
        }

        CompletedQuests ??= new List<QuestData>();
        AvailableSubQuests ??= new List<QuestData>(Main.CSVData.subQuests.Values);
        ReadyToClear ??= new List<QuestData>();
        AllQuestList ??= new List<QuestData>(Main.CSVData.mainQuests.Values.Concat(Main.CSVData.subQuests.Values));
        ActiveSubQuests ??= new List<QuestData>();

        InitQuest();
    }

    public void InitQuest()
    {
        ResetQuest();
        RestoreQuest();
        
        int savedMainQuestId = Main.Player.playerData.currentMainQuestID;
        if (savedMainQuestId != -1 && Main.CSVData.mainQuests.ContainsKey(savedMainQuestId))
        {
            StartMainQuest(savedMainQuestId);
        }
        else
        {
            StartMainQuest(900); // 저장된 메인 퀘스트가 없으면 900번 퀘스트 시작
        }

        FilterAvailableSubQuests();

        if (CurrentMainQuest != null)
        {
            ReadyToClear.Add(CurrentMainQuest);
        }

        _questClearReward = new QuestClearReward();

        if (!_questClearReward._isSubscribed)
        {
            _questClearReward.Subscribe();
        }
        
        OnQuestMarkUpdate?.Invoke();
        initialized = true;
        
         
    }
    
    private void FilterAvailableSubQuests()
    {
        var filteredQuests = new List<QuestData>();

        foreach (var quest in Main.CSVData.subQuests.Values)
        {
            bool isPrevQuestCompleted = quest.PreviousQuestID == -1 || CompletedQuests.Any(cq => cq.QuestID == quest.PreviousQuestID);
            bool isQuestNotStarted = !ActiveSubQuests.Any(aq => aq.QuestID == quest.QuestID) && !CompletedQuests.Any(cq => cq.QuestID == quest.QuestID);

            if (isPrevQuestCompleted && isQuestNotStarted)
            {
                filteredQuests.Add(quest);
            }
            else if (quest.PreviousQuestID != -1 && isPrevQuestCompleted)
            {
                AddQuest(quest);
            }
        }

        AvailableSubQuests = filteredQuests;
    }

    private void OnDestroy()
    {
        _questClearReward.Unsubscribe();
    }

    public bool CheckQuestClear(QuestData quest, int count)
    {
        if (quest.isClear)
        {
            return false;
        }

        bool isCompleted = false;
        quest.CurrentCount = count;

        if (quest.QuestType == Define.QuestType.Main) // 메인
        {
            switch (quest.QuestID)
            {
                case 900:
                    isCompleted = true;
                    break;
                case 901:
                    isCompleted = true;
                    break;
                case 902:
                    isCompleted = true;
                    break;
                case 903:
                    // 정제수 다섯개 갖다주기 (ItemID = 5, ItemCount = 5)
                    isCompleted = quest.CurrentCount >= 5;
                    break;
                case 904:
                    isCompleted = true;
                    break;
                case 905:
                    // 소드맨 잡기 (CombatID = 0, CombatCount = 1)
                    isCompleted = quest.CurrentCount >= 1;
                    break;
                case 906:
                    isCompleted = true;
                    break;
                case 907:
                    isCompleted = true;
                    if (isCompleted)
                    {
                        var subQuest = AllQuestList.FirstOrDefault(q => q.QuestID == 15);
                        if (subQuest != null)
                        {
                            AddQuest(subQuest); // 도박엔딩 서브퀘 시작
                        }
                    }
                    break;
                case 908:
                    isCompleted = true;
                    break;
                case 909:
                    // 펩시맨 잡으면 (CombatID = 1, CombatCount = 1) 998, 999 퀘스트 진행시작
                    isCompleted = quest.CurrentCount >= 1;
                    break;
                case 910:
                    isCompleted = quest.CurrentCount >= 1;
                    break;
                case 999:
                    break;
            }
        }

        if (quest.QuestType == Define.QuestType.Sub) // 서브
        {
            if (count != -1)
            {
                quest.CurrentCount = count;
            }

            if (quest.ItemCount >= 0 && quest.ItemID >= 0) // 아이템 배달
            {
                if (count >= quest.ItemCount)
                {
                    isCompleted = true;
                }
                else
                {
                    return false;
                }
            }
            else if (quest.ItemID == 444) // 444 도박
            {
                if (quest.CurrentCount >= quest.ItemCount)
                {
                    isCompleted = true;
                }
                else
                {
                    return false;
                }
            }
            else if (quest.ItemID == 777) // 777 대화
            {
                if (count >= quest.ItemCount)
                {
                    isCompleted = true;
                }
                else
                {
                    return false;
                }
            }
            else if (quest.CombatID == 555) // 555 패링
            {
                if (quest.CurrentCount >= quest.CombatCount)
                {
                    isCompleted = true;
                }
                else
                {
                    return false;
                }
            }
            else if (quest.CombatID == 666) // 666 가드
            {
                if (quest.CurrentCount >= quest.CombatCount)
                {
                    isCompleted = true;
                }
                else
                {
                    return false;
                }
            }
            else if (quest.ItemCount == 500000) // 탈출엔딩
            {
                if (quest.CurrentCount >= quest.ItemCount)
                {
                    isCompleted = true;
                }
                else
                {
                    return false;
                }
            }
        }

        if (isCompleted)
        {
            quest.isActive = false;
            quest.isClear = true;
            OnQuestMarkUpdate?.Invoke();
            UpdateQuestState(quest);
            Main.Save.SaveToJson_PlayerData();
        }

        return false;
    }

    public void UpdateQuestState(QuestData questData)
    {
        bool stateChange = false;

        if (questData.isActive && !ActiveSubQuests.Contains(questData))
        {
            ActiveSubQuests.Add(questData);
            stateChange = true;
        }
        else if (questData.isClear && !ReadyToClear.Contains(questData))
        {
            Main.Player.OnMainQuestStarted(CurrentMainQuest);
            ReadyToClear.Add(questData);
            stateChange = true;
        }

        if (stateChange)
        {
            OnQuestUpdated?.Invoke(questData);
            OnQuestMarkUpdate?.Invoke();
        }
        
        Main.Save.SaveToJson_PlayerData();
    }

    public void UpdateQuestItemCount(int itemID)
    {
        int currentItemCount = Main.Inven.HowManyThisItemYouHave(itemID);

        if (CurrentMainQuest != null && CurrentMainQuest.ItemID == itemID)
        {
            CurrentMainQuest.CurrentCount = currentItemCount;

            if (CheckQuestClear(CurrentMainQuest, currentItemCount))
            {
                OnQuestUpdated?.Invoke(CurrentMainQuest);
            }
        }

        foreach (var quest in ActiveSubQuests.Where(q => q.ItemID == itemID).ToList())
        {
            if (CheckQuestClear(quest, currentItemCount))
            {
                OnQuestUpdated?.Invoke(quest);
            }
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void UpdateTalkCount()
    {
        foreach (var quest in ActiveSubQuests.Where(q => q.ItemID == 777))
        {
            quest.CurrentCount++;

            if (CheckQuestClear(quest, quest.CurrentCount))
            {
                OnQuestUpdated?.Invoke(quest);
            }
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void UpdateCombatCount(int combatID, int combatCount)
    {
        foreach (var quest in ActiveSubQuests.Where(q => q.CombatID == combatID).ToList())
        {
            quest.CurrentCount = combatCount;

            if (CheckQuestClear(quest, combatCount))
            {
                OnQuestUpdated?.Invoke(quest);
            }
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void UpdateGameCount()
    {
        foreach (var quest in ActiveSubQuests.Where(q => q.ItemID == 444))
        {
            quest.CurrentCount++;

            if (CheckQuestClear(quest, quest.CurrentCount))
            {
                OnQuestUpdated?.Invoke(quest);
            }
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void UpdateGoldCount(int currentGold)
    {
        currentGold = Main.Player.playerData.gold;
        
        foreach (var quest in ActiveSubQuests.Where(q => q.ItemCount == 500000))
        {
            quest.CurrentCount = currentGold;

            if (CheckQuestClear(quest, quest.CurrentCount))
            {
                OnQuestUpdated?.Invoke(quest);
            }
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void StartMainQuest(int questID) // 메인퀘 시작
    {
        if (Main.CSVData.mainQuests.TryGetValue(questID, out var quest))
        {
            CurrentMainQuest = quest;

        }
        else
        {
            Debug.LogWarning("메인 퀘스트 없음 : " + questID);
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void AddQuest(QuestData questData) // 진행중인 서브퀘 리스트로 추가
    {
        if (!IsQuestActive(questData.QuestID))
        {
            questData.isActive = true;

            if (questData.ItemID >= 0)
            {
                questData.CurrentCount = Main.Inven.HowManyThisItemYouHave(questData.ItemID);
            }
            else
            {
                questData.CurrentCount = 0;
            }

            UpdateQuestState(questData);
            OnQuestAdded?.Invoke(questData);
            OnQuestStatusUpdated?.Invoke(true);
            OnQuestMarkUpdate?.Invoke();

            if (questData.CurrentCount >= questData.ItemCount)
            {
                CheckQuestClear(questData, questData.CurrentCount);
            }
        }
        else
        {
            // Debug.Log($"퀘스트 이미 진행 중: {questData.QuestName}");
        }
        
        Main.Save.SaveToJson_PlayerData();
    }

    public bool IsQuestActive(int questID) // 퀘스트 지금 진행중?
    {
        return ActiveSubQuests.Any(quest => quest.QuestID == questID) || (CurrentMainQuest?.QuestID == questID);
    }

    public void ClearQuest(QuestData questData) // 퀘스트 클리어
    {
        if (questData.isClear)
        {
            CompletedQuests.Add(questData);
            ActiveSubQuests.Remove(questData);
            AvailableSubQuests.Remove(questData);
            ReadyToClear.Remove(questData);
            OnQuestMarkUpdate?.Invoke();

            if (questData.CombatID == 555)
            {
                PlayerController.parryCount = 0;
            }
            else if (questData.CombatID == 666)
            {
                PlayerController.guardCount = 0;
            }
            
            if (questData.ItemID <= 10 && questData.ItemCount != -1)
            {
                var itemToDelete = Main.Inven.inventory.FirstOrDefault(slot => slot.item != null && slot.item.id == questData.ItemID);
                if (itemToDelete != null)
                {
                    Main.Inven.Delete(itemToDelete.item, questData.ItemCount);
                }
            }
            
            ProceedToNextQuest(questData);
        }
        
        if (questData.QuestID == 910)
        {
            LoadingScene.LoadScene(Define.SceneName.endingScene);
        }
        else if (questData.QuestID == 15)
        {
            LoadingScene.LoadScene(Define.SceneName.GamblingEnd);
        }
        
        QuestClearUI ui = Main.UI.SetSubItemUI<QuestClearUI>();
        var openedQuest = Main.Quest.Open();
        Main.UI.ClosePopup(openedQuest);
        ui.ShowClearMessage(questData.QuestName);
        OnQuestCompleted?.Invoke(questData);
        Main.Save.SaveToJson_PlayerData();
    }

    private void ProceedToNextQuest(QuestData completedQuest)
    {
        if (completedQuest.NextQuestID != -1)
        {
            if (completedQuest.QuestType == Define.QuestType.Main && Main.CSVData.mainQuests.TryGetValue(completedQuest.NextQuestID, out var nextMainQuest))
            {
                Main.Player.OnNPCExitStarted(CurrentMainQuest.ClearNPC);
                CurrentMainQuest = nextMainQuest;
                Main.Player.OnMainQuestStarted(CurrentMainQuest);
            }
            else if (completedQuest.QuestType == Define.QuestType.Sub && Main.CSVData.subQuests.TryGetValue(completedQuest.NextQuestID, out var nextSubQuest))
            {
                AddQuest(nextSubQuest);
            }
            else
            {
                Debug.LogWarning("[Debug] 다음 퀘스트를 찾을 수 없음");
            }
        }
        else
        {
            // Debug.Log("[Debug] 다음 퀘스트 없음");
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public void AcceptQuestFromNPC(int questID)
    {
        var quest = Main.CSVData.subQuests.Values.FirstOrDefault(q => q.QuestID == questID);

        if (quest != null && (quest.PreviousQuestID == -1 || CompletedQuests.Any(cq => cq.QuestID == quest.PreviousQuestID)))
        {
            if (!IsQuestActive(quest.QuestID))
            {
                AddQuest(quest);
                AvailableSubQuests.Remove(quest);
            }
        }
        else
        {
            // Debug.Log("선행 퀘스트가 없거나 아직 완료되지 않았습니다.");
        }

        Main.Save.SaveToJson_PlayerData();
    }

    public List<QuestData> GetAllQuestsForNPC(int npcID)
    {
        return AvailableSubQuests.Where(quest => quest.ResponsibleNPCID == npcID).ToList();
    }

    public List<QuestData> GetCompletedQuests()
    {
        return CompletedQuests;
    }

    public List<QuestData> GetActiveSubQuests()
    {
        return ActiveSubQuests;
    }

    public List<QuestData> GetAvailableSubQuests()
    {
        return AvailableSubQuests;
    }

    public List<QuestData> GetReadyToClearQuests()
    {
        return ReadyToClear;
    }

    public QuestData GetAllQuestByID(int questID)
    {
        return AllQuestList.FirstOrDefault(quest => quest.QuestID == questID);
    }

    public Quest_Popup Open()
    {
        questPopUpUi = Main.UI.OpenPopup<Quest_Popup>();
        OnQuestStatusUpdated?.Invoke(false);
        OnQuestMarkUpdate?.Invoke();
        return questPopUpUi;
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    public void SetDialogueUI(DialogueUI dialogueUI)
    {
        _dialogueUI = dialogueUI;
    }

    public void SetNPCInfo(NPCInfo npcInfo)
    {
        _npcInfo = npcInfo;
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    public void ResetQuest()
    {
        foreach (var quest in AllQuestList)
        {
            quest.isClear = false;
            quest.CurrentCount = 0;
        }

        
        // 현재 메인 퀘스트
        if (CurrentMainQuest != null)
        {
            CurrentMainQuest.isClear = false; // 메인 퀘스트 클리어 상태 초기화
            CurrentMainQuest.CurrentCount = 0; // 메인 퀘스트 진행 상태 초기화
        }
        // 진행 중인 서브 퀘스트
        ActiveSubQuests.Clear();
        // 클리어 가능한 퀘스트
        ReadyToClear.Clear();
        // 완료된 퀘스트
        CompletedQuests.Clear();
        // 진행 가능한 서브 퀘스트
        AvailableSubQuests.Clear();
        
        OnQuestMarkUpdate?.Invoke();
        OnQuestStatusUpdated?.Invoke(false);
        
        //Main.Save.SaveToJson_PlayerData();
    }
    
    public void RestoreQuest()
    {
        if (Main.CSVData == null || Main.CSVData.subQuests == null || Main.CSVData.mainQuests == null)
        {
            Debug.LogError("CSVData or its dictionaries are not initialized.");
            return;
        }

        if (ActiveSubQuests == null)
        {
            ActiveSubQuests = new List<QuestData>();
        }

        if (CompletedQuests == null)
        {
            CompletedQuests = new List<QuestData>();
        }

        if (ReadyToClear == null)
        {
            ReadyToClear = new List<QuestData>();
        }

        if (ActiveSubQuests.Count > 0 || CompletedQuests.Count > 0 || ReadyToClear.Count > 0)
        {
            return;
        }

        // 진행 중인 서브 퀘스트 복원
        foreach (int questID in Main.Player.playerData.activeQuestID)
        {
            if (Main.CSVData.subQuests.TryGetValue(questID, out QuestData questData))
            {
                questData.isActive = true;
                ActiveSubQuests.Add(questData);
            }
        }

        // 클리어 가능한 퀘스트 복원
        foreach (int questID in Main.Player.playerData.readyToClearQuestID)
        {
            if (Main.CSVData.subQuests.TryGetValue(questID, out QuestData questData))
            {
                ReadyToClear.Add(questData);
            }
            if (Main.CSVData.mainQuests.TryGetValue(questID, out QuestData questMainData))
            {
                ReadyToClear.Add(questMainData);
            }
        }

        // 완료된 퀘스트 복원
        foreach (int questID in Main.Player.playerData.completedQuestID)
        {
            if (Main.CSVData.subQuests.TryGetValue(questID, out QuestData questData))
            {
                questData.isClear = true;
                CompletedQuests.Add(questData);
            }
            if (Main.CSVData.mainQuests.TryGetValue(questID, out QuestData questMainData))
            {
                questMainData.isClear = true;
                CompletedQuests.Add(questMainData);
            }
        }

        // 현재 진행 중인 메인 퀘스트 복원
        if (Main.Player.playerData.currentMainQuestID != -1)
        {
            if (Main.CSVData.mainQuests.TryGetValue(Main.Player.playerData.currentMainQuestID, out QuestData questData))
            {
                questData.CurrentCount = Main.Player.playerData.currentMainQuestCurrentCount;
                CurrentMainQuest = questData;
            }
        }

        // 복원된 퀘스트의 CurrentCount 설정
        for (int i = 0; i < Main.Player.playerData.activeQuestID.Count; i++)
        {
            int questID = Main.Player.playerData.activeQuestID[i];
            QuestData questData = ActiveSubQuests.FirstOrDefault(q => q.QuestID == questID);
            if (questData != null)
            {
                if (questData.ItemID >= 0)
                {
                    questData.CurrentCount = Main.Inven.HowManyThisItemYouHave(questData.ItemID);
                }
                else
                {
                    questData.CurrentCount = Main.Player.playerData.activeQuestCurrentCount[i];
                }
            }
        }
        
        OnQuestMarkUpdate?.Invoke();
    }
}