using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest_Popup : UI_Popup
{
    [SerializeField] private GameObject _questListObj; // 서브 퀘스트 목록 담는 오브젝트
    [SerializeField] private Button _closeBtn; // 닫기
    [SerializeField] private Button _mainQuestBtn; // 메인퀘 버튼 (QuestView 띄우는)
    [SerializeField] private Button _subQuestActiveBtn; // 서브퀘 목록 숨길지 말지
    
    private QuestView _questView;
    private GameObject _currentQuestView;

    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }

        _closeBtn.onClick.AddListener(CloseBtnAndView);
        _subQuestActiveBtn.onClick.AddListener(SubQuestListsActive); //_subQuestsActiveBtn을 누르면 QuestList의 모든 자식 오브젝트들 비활성화하거나 활성화시킴

        MakeSubQuestList(Main.Quest.GetActiveSubQuests());
        MainQuestUpdate(Main.Quest.CurrentMainQuest);

        return true;
    }

    private void UpdateQuestUI(QuestData questData)
    {
        if (questData.isClear)
        {
            for (int i = 0; i < _questListObj.transform.childCount; i++)
            {
                var child = _questListObj.transform.GetChild(i);
                var questText = child.GetComponentInChildren<TMP_Text>().text;
                if (questText.Contains(questData.QuestName))
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
        else
        {
            ShowActiveQuests();
        }
    }
    
    private void ShowActiveQuests()
    {
        foreach (Transform child in _questListObj.transform)
        {
            Destroy(child.gameObject);
        }
        
        MakeSubQuestList(Main.Quest.GetActiveSubQuests());
        MainQuestUpdate(Main.Quest.CurrentMainQuest);
    }
    
    public void MainQuestUpdate(QuestData currentMainQuest)
    {
        string questStatus = Main.Quest.GetReadyToClearQuests().Contains(currentMainQuest) ? "[클리어 가능]" : "[진행중]";
        _mainQuestBtn.GetComponentInChildren<TMP_Text>().text = $"{currentMainQuest.QuestName} {questStatus}";
        _mainQuestBtn.GetComponent<Button>().onClick.AddListener(() => ShowView(currentMainQuest, true));
    }

    public void MakeSubQuestList(List<QuestData> activeSubQuests)
    {
        if (activeSubQuests == null)
        {
            return;
        }
        
        if (_questListObj == null)
        {
            return;
        }
        
        foreach (var subQuest in activeSubQuests)
        {
            GameObject subQuestPrefab = Main.Resource.InstantiatePrefab("ActiveSubQuest", _questListObj.transform);
            string questStatus = Main.Quest.GetReadyToClearQuests().Contains(subQuest) ? "[클리어 가능]" : "[진행중]";
            subQuestPrefab.GetComponentInChildren<TMP_Text>().text = $"{subQuest.QuestName} {questStatus}";
            subQuestPrefab.GetComponent<Button>().onClick.AddListener(() => ShowView(subQuest, false));
        }
    }

    private void ShowView(QuestData questData, bool isMainQuest)
    {
        if (_currentQuestView != null)
        {
            Destroy(_currentQuestView);
        }

        GameObject viewPanelPrefab = Main.Resource.InstantiatePrefab("ViewPanel", transform);
        _currentQuestView = viewPanelPrefab;

        var questViewScript = viewPanelPrefab.GetComponent<QuestView>();
        
        if (questViewScript != null)
        {
            if (isMainQuest)
            {
                questViewScript.MainQuestView(questData);
            }
            else
            {
                questViewScript.SubQuestView(questData);
            }
        }
    }

    private void CloseBtnAndView()
    {
        OnBtnClose();
        Destroy(_currentQuestView);
    }

    private void SubQuestListsActive()
    {
        if (_questListObj != null)
        {
            _questListObj.SetActive(!_questListObj.activeSelf);
        }
    }
    
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
