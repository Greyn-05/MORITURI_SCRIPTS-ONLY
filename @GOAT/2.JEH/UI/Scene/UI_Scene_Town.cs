using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Town : UI_Scene
{
    [SerializeField] private GameObject _questCheckUI;

    private Tweener _tweener;
    private Image _checkColor;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _checkColor = _questCheckUI.GetComponent<Image>();


        if (Main.Player.isReset)
        {
            // 플레이데이터 초기화시키기

            Main.Save.DefaultPlayerData();

            for (int i = 0; i < Define.maxInvenSlotCount; i++)
            {
                Main.Inven.inventory[i].item = null;
                Main.Inven.inventory[i].capacity = 0;
            }

            Main.Player.playerData.activeQuestID.Clear();
            Main.Player.playerData.activeQuestCurrentCount.Clear();
            Main.Player.playerData.readyToClearQuestID.Clear();
            Main.Player.playerData.completedQuestID.Clear();
            
            Main.Player.playerData.currentMainQuestID = -1;
            Main.Player.playerData.currentMainQuestCurrentCount = 0;
            
            Main.Quest.ReadyToClear.Clear();
            Main.Quest.ActiveSubQuests.Clear();
            Main.Quest.CompletedQuests.Clear();
            
            Main.Player.playerData.isTutorialClear = true;
            Main.Player.isReset = false;
            Main.Quest.CurrentMainQuest.QuestID = 900;
            Main.CSVData.mainQuests[900].isClear = false;
            Main.Quest.StartMainQuest(900);
            Main.Quest.Initialize();
            Main.Quest.InitQuest();

            Main.Save.SaveToJson_PlayerData();

        }
        Main.Quest.RestoreQuest();
        
        Main.Quest.OnQuestStatusUpdated -= HandleQuestStatusUpdated;
        Main.Quest.OnQuestStatusUpdated += HandleQuestStatusUpdated;
        
        

        if (Main.Quest.CurrentMainQuest != null && Main.Quest.CurrentMainQuest.QuestID == 900)
        {
            StartEffect();
        }


        return true;
    }


    private void StartEffect()
    {
        _questCheckUI.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f)
                                .SetLoops(-1, LoopType.Yoyo)
                                .SetEase(Ease.InOutQuad);
        if (_checkColor != null)
        {
            _checkColor.DOColor(new Color(1f, 0.5f, 0.5f, 1f), 0.5f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutQuad);
        }
    }

    private void StopEffect()
    {
        DOTween.Kill(_questCheckUI.transform); // Scale 변경 트윈을 중지합니다.
        if (_checkColor != null)
        {
            DOTween.Kill(_checkColor); // Color 변경 트윈을 중지합니다.
            _checkColor.color = Color.white; // Image의 색상을 원래대로 복원합니다.
        }
        _questCheckUI.transform.localScale = Vector3.one; // Transform의 Scale을 원래대로 복원합니다.
    }

    private void HandleQuestStatusUpdated(bool startEffect)
    {
        if (startEffect)
        {
            StartEffect();
        }
        else
        {
            StopEffect();
        }
    }

    private void OnDisable()
    {
        Main.Quest.OnQuestStatusUpdated -= HandleQuestStatusUpdated;
    }

    // Main.Quest.UpdateQuestProgress에서 _readyToClear.Add(quest);가 될때마다 두트윈으로 좀 두근두근한 효과? 살짝씩 커졌다 작아졌다 하는 효과

}
