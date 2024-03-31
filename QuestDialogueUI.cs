using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogueUI : UI_Popup
{
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;

    private WaitForSeconds _typingWait;
    private Coroutine _typingCoroutine; 
    private List<QuestDialogueData> _currentquestDialogue;

    private string _npcName;
    private string _currentDialogueText;
    private int _currentLineIndex = 0;
    private int _currentQuestID;
    /*STATE*/
    private bool _isTyping = false;
    private bool _waitingForInput = false;
    
    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    
    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }
        
        _typingWait = new WaitForSeconds(typingSpeed);
        btn.onClick.AddListener(DialogueClick);
        
        return true;
    }
    
    public void OpenDialogue(int questID, NPCInfo npcInfo)
    {
        _currentQuestID = questID;
        _npcName = npcInfo.npcName;
        
        ResetUI();
        StartTalk(questID);
    }
    
    public void StartTalk(int questID)
    {
        _currentLineIndex = 0;
        _currentquestDialogue = Main.CSVData.GetDialoguesByQuestTextID(questID);
        
        if (_currentquestDialogue != null && _currentquestDialogue.Count > 0)
        {
            StartDialogue(_currentquestDialogue[_currentLineIndex]);
        }
    }
    
    private void StartDialogue(QuestDialogueData questDialogueData)
    {
        if (questDialogueData != null)
        {
            _currentDialogueText = questDialogueData.qdText
                .Replace("{PlayerName}", Main.Player.playerData.playerName)
                .Replace("{NPCName}", _npcName);
            StartTypingEffect(_currentDialogueText);
        }
        else
        {
            CloseDialogue();
        }
    }
    
    public void DialogueClick()
    {
        if (_isTyping)
        {
            CompleteTyping();
        }
        else if (_waitingForInput)
        {
            _waitingForInput = false;
            _currentLineIndex++;
            
            if (_currentLineIndex < _currentquestDialogue.Count)
            {
                StartDialogue(_currentquestDialogue[_currentLineIndex]);
            }
            else
            {
                CloseDialogue();
            }
        }
    }
    
    private void CloseDialogue()
    {
        StopTyping();

      //  Main.Player.OnInteractiveEvent = null;

        Main.Player.CursorLock_Locked();
        Main.Cinemachne.SetPlayerCamera();
        OnBtnClose();
        
        if (_currentLineIndex >= _currentquestDialogue.Count - 1)
        {
            foreach (var quest in Main.Quest.ReadyToClear)
            {
                if (quest.ClearNPC == _npcName && quest.QuestID == _currentQuestID)
                {
                    quest.isClear = true;
                    Main.Quest.ClearQuest(quest);
                    break;
                }
            }
        }
    }
    
    private void ResetUI()
    {
        dialogueText.text = "";
        _currentLineIndex = 0;

        StopTyping();
    }
    
    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    
    private void StartTypingEffect(string textToType)
    {
        if(_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _typingCoroutine = StartCoroutine(TypingEffect(textToType));
    }

    private IEnumerator TypingEffect(string textToType)
    {
        _isTyping = true;
        _waitingForInput = false;
        dialogueText.text = "";

        foreach (char letter in textToType)
        {
            dialogueText.text += letter;
            yield return _typingWait;
        }

        _isTyping = false;
        _waitingForInput = true;
    }
    
    private void StopTyping()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _isTyping = false;
    }
    
    private void CompleteTyping()
    {
        dialogueText.text = _currentDialogueText;
        _waitingForInput = true;

        StopTyping();
    }

    private void OnDestroy()
    {
        CloseDialogue();
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
}
