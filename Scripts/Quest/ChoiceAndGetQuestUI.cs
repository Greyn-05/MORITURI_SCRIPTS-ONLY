using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoiceAndGetQuestUI : UI_Popup
{
    [SerializeField] private GameObject _choiceListObj;
    [SerializeField] private GameObject _getPanelObj;
    [SerializeField] private GameObject _acceptOrDeclineObj;

    [SerializeField] private float typingSpeed = 0.08f;

    private UIState _currentState;

    private WaitForSeconds _typingWait;
    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private bool _waitingForInput = false;

    private TMP_Text _questDialogueText;
    private Button _questActionButton;

    //-

    private string _fullText;
    private string[] _textParts;
    private int _partIndex;

    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }

        _typingWait = new WaitForSeconds(typingSpeed);

        return true;
    }

    public void MakeMeChoiceQuest(List<QuestData> questsForNPC)
    {
        foreach (Transform child in _choiceListObj.transform)
        {
            Destroy(child.gameObject);
        }

        HashSet<int> activeOrCompletedQuestID = new(Main.Quest.GetActiveSubQuests().Select(q => q.QuestID));
        activeOrCompletedQuestID.UnionWith(Main.Quest.GetCompletedQuests().Select(q => q.QuestID));

        foreach (var quest in questsForNPC)
        {
            if (!activeOrCompletedQuestID.Contains(quest.QuestID))
            {
                GameObject choiceQuestPrefab = Main.Resource.InstantiatePrefab("HaveQuest", _choiceListObj.transform);
                choiceQuestPrefab.GetComponentInChildren<TMP_Text>().text = quest.QuestName;
                choiceQuestPrefab.GetComponent<Button>().onClick.AddListener(() => ShowQuest(quest));
                activeOrCompletedQuestID.Add(quest.QuestID);
            }
        }
    }

    private enum UIState
    {
        ShowingQuest,
        EndOfShowingQuest,
        AcceptedQuest,
        DeclinedQuest,
        WaitingToHide
    }

    private void OnQuestButtonClicked(QuestData questData)
    {
        if (_isTyping)
        {
            CompleteTyping();
        }
        else if (_waitingForInput)
        {
            _waitingForInput = false;

            switch (_currentState)
            {
                case UIState.ShowingQuest:
                    if (_partIndex < _textParts.Length - 1)
                    {
                        _partIndex++;
                        StartTypingEffect(_textParts[_partIndex]);

                        if (_partIndex == _textParts.Length - 1)
                        {
                            _currentState = UIState.EndOfShowingQuest;
                        }
                    }
                    break;
                case UIState.EndOfShowingQuest:
                    AcceptOrDecline(questData);
                    break;
                case UIState.AcceptedQuest:
                case UIState.DeclinedQuest:
                case UIState.WaitingToHide:
                    HideQuestUI();
                    break;
            }
        }
    }

    public void ShowQuest(QuestData questData)
    {
        foreach (Transform child in _getPanelObj.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _choiceListObj.transform)
        {
            Destroy(child.gameObject);
        }

        bool isQuestActive = Main.Quest.IsQuestActive(questData.QuestID);
        bool isQuestCompleted = Main.Quest.GetCompletedQuests().Contains(questData);

        if (!isQuestActive && !isQuestCompleted)
        {
            GameObject getQuestPrefab = Main.Resource.InstantiatePrefab("QuestPanel", _getPanelObj.transform);
            _questDialogueText = getQuestPrefab.GetComponentInChildren<TMP_Text>();
            _questActionButton = getQuestPrefab.GetComponent<Button>();

            _fullText = questData.QuestDialogue;
            _textParts = _fullText.Split(new[] { "{A}" }, System.StringSplitOptions.RemoveEmptyEntries); // {A} 를 기준으로 자르고 줄바꿈
            _partIndex = 0;

            if (_textParts.Length == 1)
            {
                _currentState = UIState.EndOfShowingQuest;
                StartTypingEffect(_textParts[_partIndex]);
            }
            else
            {
                StartTypingEffect(_textParts[_partIndex]);
                _currentState = UIState.ShowingQuest;
            }

            _questActionButton.onClick.RemoveAllListeners();
            _questActionButton.onClick.AddListener(() => OnQuestButtonClicked(questData));
        }
        else if (isQuestActive)
        {
            _questDialogueText.text = "퀘스트가 이미 진행 중입니다.";
        }
        else if (isQuestCompleted)
        {
            _questDialogueText.text = "이미 완료된 퀘스트입니다.";
        }
    }

    GameObject selectPrefab;
    Button yesBtn;
    Button noBtn;



    private void AcceptOrDecline(QuestData questData)
    {

        selectPrefab = Main.Resource.InstantiatePrefab("YesOrNo", _acceptOrDeclineObj.transform);

        yesBtn = selectPrefab.transform.Find("YesBtn").GetComponent<Button>();
        noBtn = selectPrefab.transform.Find("NoBtn").GetComponent<Button>();

        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();
        yesBtn.onClick.AddListener(() => AcceptQuest(questData));
        noBtn.onClick.AddListener(() => DeclineQuest(questData));
        yesBtn.onClick.AddListener(() => Main.Audio.BtnSound());
        noBtn.onClick.AddListener(() => Main.Audio.BtnSound());

    }

    private void AcceptQuest(QuestData questData)
    {
        Destroy(selectPrefab);

        Main.Quest.AcceptQuestFromNPC(questData.QuestID);

        questData.AcceptText = "퀘스트를 수락합니다";
        StartTypingEffect(questData.AcceptText);
        _currentState = UIState.AcceptedQuest;
    }

    private void DeclineQuest(QuestData questData)
    {
        Destroy(selectPrefab);

        questData.DeclineText = "퀘스트를 거절합니다";
        StartTypingEffect(questData.DeclineText);
        _currentState = UIState.DeclinedQuest;
    }

    private void OnDestroy()
    {
        HideQuestUI();
    }

    private void HideQuestUI()
    {
        OnBtnClose();
        Main.Player.CursorLock_Locked();
        Main.Cinemachne.SetPlayerCamera();
        var openedQuest = Main.Quest.Open();
        Main.UI.ClosePopup(openedQuest);
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    private void StartTypingEffect(string textToType)
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _typingCoroutine = StartCoroutine(TypingEffect(textToType));
    }

    private IEnumerator TypingEffect(string textToType)
    {
        _isTyping = true;
        _waitingForInput = false;
        _questDialogueText.text = "";

        foreach (char letter in textToType)
        {
            _questDialogueText.text += letter;
            yield return _typingWait;
        }

        _isTyping = false;
        _waitingForInput = true;
    }

    private void CompleteTyping()
    {
        if (_isTyping && _typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _questDialogueText.text = _textParts[_partIndex];
            _isTyping = false;
            _waitingForInput = true;

            if (_currentState == UIState.AcceptedQuest)
            {
                _questDialogueText.text = "퀘스트를 수락합니다";
               _currentState = UIState.WaitingToHide;
            }
            else if (_currentState == UIState.DeclinedQuest)
            {
            _questDialogueText.text = "퀘스트를 거절합니다";
                _currentState = UIState.WaitingToHide;
            }
            else if (_partIndex == _textParts.Length - 1)
            {
                _currentState = UIState.EndOfShowingQuest;
            }
        }
    }
}
