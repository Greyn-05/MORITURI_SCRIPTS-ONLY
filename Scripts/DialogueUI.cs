using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueUI : UI_Popup
{
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;

    private WaitForSeconds _typingWait;
    private Coroutine _typingCoroutine;
    private List<DialogueData> _currentDialogues;

    private NPCInfo _currentNPCInfo;
    private int _npcID;
    private string _npcName;
    private string _currentDialogueText;
    private int _currentLineIndex = 0;
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
        
        Main.Quest.SetDialogueUI(this);
        
        return true;
    }
    
    public void OpenDialogue(NPCInfo npcInfo)
    {
        ResetUI();
        _currentNPCInfo = npcInfo;
        _npcID = npcInfo.npcID;
        _npcName = npcInfo.npcName;

        StartTalk(_npcID);
    }
    
    public void StartTalk(int npcID)
    {
        _currentLineIndex = 0;
        var gatherIDs = new List<int> (Main.CSVData.gatherDialogues.Keys);
        
        if (gatherIDs.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, gatherIDs.Count);
        int randomGatherID = gatherIDs[randomIndex];
        
        _currentDialogues = Main.CSVData.GetDialoguesByGatherID(randomGatherID);

        if (_currentDialogues.Count > 0)
        {
            StartDialogue(_currentDialogues[_currentLineIndex]);
        }
    }
    
    private void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData != null)
        {
            _currentDialogueText = dialogueData.dialogueText
                .Replace("{PlayerName}", Main.Player.playerData.playerName)
                .Replace("{NPCName}", _currentNPCInfo.npcName);
            speakerNameText.text = dialogueData.speakerName
                .Replace("{PlayerName}", Main.Player.playerData.playerName)
                .Replace("{NPCName}", _currentNPCInfo.npcName);

            Image speakerBack = speakerNameText.transform.parent.GetComponent<Image>();
            
            if (dialogueData.speakerName.Contains("{PlayerName}"))
            {
                speakerBack.color = Color.green;
            }
            else if (dialogueData.speakerName.Contains("{NPCName}"))
            {
                speakerBack.color = Color.red;
            }
            
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
            
            if (_currentLineIndex < _currentDialogues.Count)
            {
                StartDialogue(_currentDialogues[_currentLineIndex]);
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    private void OnDisable()
    {
        Main.Player.CursorLock_None();
        Main.Cinemachne.SetPlayerCamera();
        OnBtnClose();


    }
    private void CloseDialogue()
    {
        StopTyping();

     //  Main.Player.OnInteractiveEvent = null;
        Main.Player.CursorLock_None();
        Main.Cinemachne.SetPlayerCamera();
        OnBtnClose();
        
        Main.Quest.UpdateTalkCount();
    }
    
    private void ResetUI()
    {
        speakerNameText.text = "";
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
    
    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    
}