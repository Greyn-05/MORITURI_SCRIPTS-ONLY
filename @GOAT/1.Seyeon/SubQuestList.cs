// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class SubQuestList : UI_Base
// {
//     [SerializeField] private TMP_Text _questNameText;
//     [SerializeField] private Button _subQuestViewBtn;
//     
//     public override bool Initialize()
//     {
//         if (!base.Initialize())
//         {
//             return false;
//         }
//
//         return true;
//     }
//     
//     public void MakeSubQuestList(QuestManager questManager)
//     {
//         foreach (Transform child in transform)
//         {
//             Destroy(child.gameObject);
//         }
//
//         var activeSubQuests = questManager.GetActiveSubQuests();
//
//         foreach (var subQuest in activeSubQuests)
//         {
//             GameObject subQuestPrefab = Main.Resource.InstantiatePrefab("ActiveSubQuest", transform);
//             SubQuestList subList = subQuestPrefab.GetComponent<SubQuestList>();
//             
//             if (subList != null)
//             {
//                 subList.SetData(subQuest);
//
//                 Button _subQuestViewBtn = subQuestPrefab.GetComponentInChildren<Button>();
//                 //_subQuestViewBtn.onClick.AddListener(() => { QuestView.SubQuestView(subQuest); QuestView.gameObject.SetActive(true); }
//             }
//             else
//             {
//                 Debug.LogError("ActiveSubQuest 프리팹에 SubQuestList 컴포넌트가 없습니다.");
//             }
//         }
//     }
//
//     public void SetData(QuestData subQuest)
//     {
//         if (_questNameText != null && subQuest != null)
//         {
//             string questStatus = Main.Quest.GetReadyToClearQuests().Contains(subQuest) ? "[클리어 가능]" : "[진행중]";
//             _questNameText.text = $"{subQuest.QuestName} {questStatus}";
//         }
//         else
//         {
//             Debug.LogError("QuestNameText가 할당되지 않았거나 subQuest가 null입니다.");
//         }
//     }
// }