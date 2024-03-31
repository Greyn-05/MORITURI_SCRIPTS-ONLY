using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_BossSelect : UI_Popup
{

    [SerializeField] private GameObject slotsObj;
    public Action BossSelectAction; // 해당 칸이 자기가 눌렸을떄 어떤 보스 선택하는지 

    [SerializeField] private Image bossImage;
    [SerializeField] private TextMeshProUGUI bossDicription;
    [SerializeField] private TextMeshProUGUI bossBestClearTime;
    [SerializeField] private Button IntoBattleField;
    [SerializeField] private Button closeBtn;


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Game.SelectedBossIndex = 0;

        for (int i = 0; i < Main.Player.playerData.isBossRelease.Length; i++)
        {
            BossSelectSlot slot = Main.Resource.InstantiatePrefab("BossSelectSlot").GetComponent<BossSelectSlot>();
            slot.transform.SetParent(slotsObj.transform);
            slot.transform.localPosition = Vector3.zero;
            slot.transform.localScale = Vector3.one;
            slot.uiBossSelect = this;
            slot.bossNumber = i;
            slot.Unlock();

        }

        IntoBattleField.onClick.AddListener(OnParticipateColosseum);

        closeBtn.onClick.AddListener(() => OnBtnClose());

        OnSelectButton(Main.Game.SelectedBossIndex);


        return true;
    }

    public void OnSelectButton(int bossNumber) // 이 버튼을 눌렀을때
    {
        Main.Game.SelectedBossIndex = bossNumber;

        bossImage.sprite = Main.Resource.Load<Sprite>(Define.bossImageFile[Main.Game.SelectedBossIndex]);
        bossDicription.text = $"{Define.bossDis[bossNumber]}";

        if (Main.Player.playerData.bossClearTime[bossNumber] == 99999999)
            bossBestClearTime.text = $"최단 클리어기록 : 기록없음";
        else
            bossBestClearTime.text = $"최단 클리어기록 : {Main.Game.FloatTimer(Main.Player.playerData.bossClearTime[bossNumber])}";


    }

    public void OnParticipateColosseum()
    {
        OnBtnClose();
        LoadingScene.LoadScene(Define.SceneName.Dungeon);

    }


}
