using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMenu : UI_Popup
{

    [SerializeField] private Button _dialogue;
    [SerializeField] private Button _quest;
    [SerializeField] private Button _gambling;
    [SerializeField] private Button _shop;
    [HideInInspector] public NPCInfo npcInfo;


    bool ddd = false;

    public void IsShop(bool open)
    {

        if (open)
        {
            _shop.gameObject.SetActive(true);

            if (Main.Quest.CurrentMainQuest.QuestID >= 903)
            {
                _shop.interactable = true;
            }
            else
            {
                _shop.interactable = false;
            }
        }
        else
        {

            _shop.gameObject.SetActive(false);


        }
    }

    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }

        _dialogue.onClick.AddListener(SelectDialogue);

        _shop.onClick.AddListener(SelectShop);

        List<QuestData> questsForNPC = Main.Quest.GetAllQuestsForNPC(npcInfo.npcID);
        HashSet<int> activeOrCompletedQuestIDs = new(Main.Quest.GetActiveSubQuests().Select(quest => quest.QuestID).Concat(Main.Quest.GetCompletedQuests().Select(quest => quest.QuestID)));

        bool hasAvailableQuests = questsForNPC.Any(quest => !activeOrCompletedQuestIDs.Contains(quest.QuestID));
        if (hasAvailableQuests) // 진행 가능한 퀘스트 있으면 퀘스트 버튼 생김
        {
            _quest.onClick.AddListener(SelectQuest);
            _quest.gameObject.SetActive(true);
        }
        else
        {
            _quest.gameObject.SetActive(false);
        }

        if (npcInfo.npcID == 751 && Main.Quest.CurrentMainQuest.QuestID >= 908) // 메인퀘스트 907번 클리어 후 751번 npc만 도박 버튼 생기게
        {
            _gambling.gameObject.SetActive(true);
            _gambling.onClick.AddListener(SelectGambling);
        }
        else
        {
            _gambling.gameObject.SetActive(false);
        }

        return true;
    }

    public void SelectDialogue() // 대화 눌렀을 때 켜짐
    {
        ddd = true;
        OnBtnClose();
        DialogueUI ui = Main.UI.OpenPopup<DialogueUI>();
        ui.OpenDialogue(npcInfo);

    }

    public void SelectQuest() // 퀘스트 눌렀을때
    {
        ddd = true;
        OnBtnClose();
        ChoiceAndGetQuestUI ui = Main.UI.OpenPopup<ChoiceAndGetQuestUI>();
        ui.MakeMeChoiceQuest(Main.Quest.GetAllQuestsForNPC(npcInfo.npcID));

    }

    public void SelectGambling() // 도박 눌렀을 때
    {
        OnGamblingGo();
    }

    public void SelectShop() // 상점 눌렀을 때
    {
        ddd = false;
        OnBtnClose();
        Main.UI.OpenPopup<UI_Popup_Shop>();
    }


    private void OnDisable()
    {

        if (ddd) return;

        Main.Player.CursorLock_Locked();
        Main.Cinemachne.MenuOffCamera();
        Main.Cinemachne.SetPlayerCamera();
    }


    private UI_Popup_Decision _decision;
    private string[] gamblingGo_textArray = new string[] { "도박장으로 이동하시겠습니까?", "네", "아니요" };

    private void OnGamblingGo()
    {
        _decision = Main.UI.OpenPopup<UI_Popup_Decision>();

        _decision.decisionAction += GamblingWarp;
        _decision.OpenDecision(ref gamblingGo_textArray);
    }

    void GamblingWarp(bool accept)
    {
        _decision.OnBtnClose();

        if (accept)
        {
            OnBtnClose();
            LoadingScene.LoadScene(Define.SceneName.Game);
        }
    }
}
