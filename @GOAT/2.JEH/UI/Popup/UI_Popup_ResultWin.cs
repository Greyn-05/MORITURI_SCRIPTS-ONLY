using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_ResultWin : UI_Popup
{
    [SerializeField] private Button _backToTheTownBtn;
    [SerializeField] private TextMeshProUGUI _clearTime;
    [SerializeField] private GameObject _newArm; //신기록 알람

    [SerializeField] private TextMeshProUGUI _prizeMoney;


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _newArm.SetActive(false);

        _backToTheTownBtn.onClick.AddListener(OnBtnBackToTheTown);


        if (Main.Player.playerData.bossClearTime[Main.Game.SelectedBossIndex] > TimeAfterLastPlay)
        {
            Main.Player.playerData.bossClearTime[Main.Game.SelectedBossIndex] = TimeAfterLastPlay;
            _newArm.SetActive(true);
        }

        _prizeMoney.text = $"우승 상금 : {Define.prizeMoney[Main.Game.SelectedBossIndex]}G";
        _clearTime.text = $"클리어시간 : {Main.Game.FloatTimer(TimeAfterLastPlay)}";

        Main.Player.playerData.gold += Define.prizeMoney[Main.Game.SelectedBossIndex];

        Main.Save.SaveToJson_PlayerData();

        return true;
    }


    void OnBtnBackToTheTown()
    {
        OnBtnClose();
        LoadingScene.LoadScene(Define.SceneName.Town);
    }


    public int TimeAfterLastPlay // 마지막플레이부터 현재까지 걸린시간 계산용
    {
        get
        {
            DateTime currentTime = DateTime.Now;
            return (int)currentTime.Subtract(Main.Game.BattleBeginTime).TotalSeconds;

            // currentTime.Subtract(lastPlayDate) = 자기자신(currentTime)과 상대방(lastPlayDate)의 시간차이를 DateTime 형태로 준다.
            // 이걸 .TotalSeconds; 해서 초단위로 가져오고 int로 변환한다.

        }
    }


}


