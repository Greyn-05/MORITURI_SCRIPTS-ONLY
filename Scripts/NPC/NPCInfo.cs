using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInfo : MonoBehaviour
{
    [HideInInspector] public int npcID;
    [HideInInspector] public string npcName;
    private string npcRace;
    private string npcJob;
    private string npcSex;

    public NPCData Data { get; private set; }
    
    private Canvas _npcCanvas;
    private GameObject _mark;
    private Sprite _exclamationMark;
    private Sprite _questionMark;
    private TMP_Text _nameText;
    private GameObject _name;
    private GameObject _questMinimapMarkerPrefab;
    private Material _questMinimapMarkerMaterial;

    private void Start()
    {
        //Main.Quest.OnQuestMarkUpdate -= UpdateMark;
        //Main.Quest.OnQuestMarkUpdate += UpdateMark;

        Main.Quest.SetNPCInfo(this);

        _npcCanvas = this.gameObject.GetComponentInChildren<Canvas>();
        _mark = Main.Resource.InstantiatePrefab("Mark", _npcCanvas.transform);
        _name = Main.Resource.InstantiatePrefab("Name", _npcCanvas.transform);
        
        _mark.SetActive(false);
        
        _nameText = _name.GetComponent<TMP_Text>();
        _nameText.text = npcName;
        
        _exclamationMark = Resources.Load<Sprite>("Prefabs/UI/Quest/Mark/ExclamationMark");
        _questionMark = Resources.Load<Sprite>("Prefabs/UI/Quest/Mark/QuestionMark");
        
        _questMinimapMarkerPrefab = Main.Resource.InstantiatePrefab("QuestMinimapMaker", this.gameObject.transform);
        _questMinimapMarkerPrefab.SetActive(false);
        _questMinimapMarkerMaterial = _questMinimapMarkerPrefab.GetComponent<Renderer>().material;

        AssignAllNPC();
        UpdateMark();
    }

    private void AssignAllNPC()
    {
        foreach (var npcDataPair in Main.CSVData.npcDatas)
        {
            if (npcDataPair.Key == npcID)
            {
                AssignData(npcDataPair.Value);
                break;
            }
        }
    }

    public void AssignData(NPCData data)
    {
        Data = data;
        npcID = data.NPCID;
        npcName = data.NPCName;
        npcRace = data.NPCRace;
        npcJob = data.NPCJob;
        npcSex = data.NPCSex;
        UpdateMark();
    }

    public void UpdateMark()
    {
        var markImage = _mark.GetComponent<Image>();
        
        List<QuestData> questsForNPC = Main.Quest.GetAllQuestsForNPC(npcID);
        HashSet<int> activeOrCompletedQuestIDs = new(Main.Quest.GetActiveSubQuests().Select(quest => quest.QuestID).Concat(Main.Quest.GetCompletedQuests().Select(quest => quest.QuestID)));

        bool hasAvailableQuests = questsForNPC.Any(quest => !activeOrCompletedQuestIDs.Contains(quest.QuestID)); // 진행가능한 서브
        bool hasReadyToClearQuest = Main.Quest.GetReadyToClearQuests().Any(quest => quest.ClearNPC == npcName); // 클리어 가능한 서브
        bool hasActiveMainQuest = Main.Quest.CurrentMainQuest != null && Main.Quest.CurrentMainQuest.ClearNPC == npcName; // 진행중인 메인

        if (hasActiveMainQuest)
        {
            // 메인 퀘스트 관련 NPC - 빨간색 느낌표
            _mark.SetActive(true);
            markImage.sprite = _exclamationMark;
            markImage.color = Color.red;
        }
        else if (hasReadyToClearQuest)
        {
            // 클리어 가능한 서브 퀘스트 - 노란색 느낌표
            _mark.SetActive(true);
            markImage.sprite = _exclamationMark;
            markImage.color = Color.yellow;
        }
        else if (hasAvailableQuests)
        {
            // 수락 가능한 서브퀘스트 - 노란색 물음표
            _mark.SetActive(true);
            markImage.sprite = _questionMark;
            markImage.color = Color.yellow;
        }
        else
        {
            _mark.SetActive(false);
        }

        // 미니맵 마커 업데이트
        _questMinimapMarkerPrefab.SetActive(_mark.activeSelf);
        if (_questMinimapMarkerPrefab.activeSelf) 
        {
            _questMinimapMarkerMaterial.color = markImage.color;
        }
    }

    private void OnEnable()
    {
        Main.Quest.OnQuestMarkUpdate += UpdateMark;
    }

    private void OnDisable()
    {
        Main.Quest.OnQuestMarkUpdate -= UpdateMark;
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
}