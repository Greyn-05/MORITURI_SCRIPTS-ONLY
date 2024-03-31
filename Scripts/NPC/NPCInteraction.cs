using System.Linq;
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteract
{
    // 상점 UI 스크립트
    //[SerializeField] private AchievementUI _achievementUI;

    private NPCInfo _npcInfo;

    private CapsuleCollider _capsuleCollider; // 대화중에 콜라이더 exit 작동해서 멈추는 문제처리용
    private float _colliderRadius;

    public void Start()
    {

        _npcInfo = GetComponent<NPCInfo>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _colliderRadius = _capsuleCollider.radius;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) OnInteractionEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) OnInteractableExit();
    }

    public void OnInteractionEnter()
    {

        Main.Player.OnInteractiveEvent = OnInteractable;
        Main.Player.OnNPCEnterStarted(_npcInfo.npcName);

        if (Main.Quest.CurrentMainQuest != null)
        {
            Main.Quest.CheckQuestClear(Main.Quest.CurrentMainQuest, Main.Quest.CurrentMainQuest.CurrentCount);
        }
    }

    public void OnInteractable() // 상호작용키 눌렀을때 실행
    {
        if (Main.UI._popupList.Count > 0) return;


        _capsuleCollider.radius = _colliderRadius * 1.4f;


        var readyToClear = Main.Quest.GetReadyToClearQuests().FirstOrDefault(quest => quest.ClearNPC == _npcInfo.npcName);

        if (readyToClear != null)
        {
            var ui = Main.UI.OpenPopup<QuestDialogueUI>();
            ui.OpenDialogue(readyToClear.QuestID, _npcInfo);
        }
        else
        {
            var ui = Main.UI.OpenPopup<InteractionMenu>();

            if (ui != null)
            {
                ui.IsShop((this.gameObject.name == "NPC_760_Kaelin"));

                ui.npcInfo = _npcInfo;
            }
        }

        Main.Cinemachne.SetNPCCamera(transform);
    }

    public void OnInteractableExit()
    {
        _capsuleCollider.radius = _colliderRadius;
        Main.Player.OnInteractiveEvent -= OnInteractable;
        Main.Player.OnNPCExitStarted(_npcInfo.npcName);

        if (Main.Cinemachne.CurrentCamera !=
            Main.Cinemachne.VirtualCameraList[Define.cameraType[Define.EStateDrivenCamera.Player]])
        {
            Main.Cinemachne.SetPlayerCamera();
        }
    }


    private void OnDisable()
    {
        Main.Player.OnInteractiveEvent -= OnInteractable;
    }
}