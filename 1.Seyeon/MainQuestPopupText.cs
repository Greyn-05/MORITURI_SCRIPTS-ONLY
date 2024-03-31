// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
//
// public class MainQuestPopupText : MonoBehaviour, IObserver
// {
//     [SerializeField] private TMP_Text _mainQuestBtnText;
//     private QuestManager Main.Quest;
//
//     private void Start()
//     {
//         Main.Quest = Main.Quest;
//
//         if (Main.Quest != null)
//         {
//             Main.Quest.SubscribeObserver(this);
//             UpdateMainQuestButtonText(Main.Quest.CurrentMainQuest);
//         }
//         else
//         {
//             Debug.LogError("QuestManager를 찾을 수 없습니다.");
//         }
//     }
//
//     private void OnDestroy()
//     {
//         if (Main.Quest != null)
//         {
//             Main.Quest.UnsubscribeObserver(this);
//         }
//     }
//
//     public void OnNotifyQuestUpdated(QuestData questData)
//     {
//         if (questData == Main.Quest.CurrentMainQuest)
//         {
//             UpdateMainQuestButtonText(questData);
//         }
//     }
//
//     private void UpdateMainQuestButtonText(QuestData currentMainQuest)
//     {
//         if (currentMainQuest != null)
//         {
//             string questStatus = Main.Quest.GetReadyToClearQuests().Contains(currentMainQuest) ? "[클리어 가능]" : "[진행중]";
//             _mainQuestBtnText.text = $"{currentMainQuest.QuestName} {questStatus}";
//         }
//     }
//
//     public void OnQuestComplete(QuestData questData) { }
//     public void OnNotifyTalk(int npcID, string npcName) { }
//     public void OnNotifyKilled(int enemyID, string npcName) { }
// }
