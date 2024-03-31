using UnityEngine;
using UnityEngine.UI;

public class Title_Main : UI_Base
{
    [SerializeField] private Button _newStartBtn;
    [SerializeField] private Button _continueBtn;
    [SerializeField] private Button _optionBtn;
    [SerializeField] private Button _exitBtn;

    [SerializeField] private Button _continueBtnTEST;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _newStartBtn.onClick.AddListener(OnBtnNewGame);
        _continueBtn.onClick.AddListener(OnBtnContinue);
        _optionBtn.onClick.AddListener(OnBtnOption);
        _exitBtn.onClick.AddListener(OnBtnExit);
        // _continueBtnTEST.onClick.AddListener(OnBtnContinueTest);

        _continueBtn.interactable = Main.Player.playerData.isTutorialClear;

        Main.Player.isReset = false;

        return true;
    }

    private void OnBtnNewGame()
    {
        if (Main.Player.playerData.isTutorialClear) // 세이브파일 여부로 고를수가 없다 겜 처음시작때 기본값 저장하니까
        {
            _decision2 = Main.UI.OpenPopup<UI_Popup_Decision>();

            _decision2.decisionAction += NewGameStart;
            _decision2.OpenDecision(ref reset_textArray);
        }
        else
        {
            Main.Player.isReset = true;
            Main.Player.playerData.isTutorialClear = true;
            Main.Save.SaveToJson_PlayerData();
            LoadingScene.LoadScene(Define.SceneName.intro);

        }


    }

    private UI_Popup_Decision _decision2;
    private string[] reset_textArray = new string[] { "데이터를 초기화하고\n새게임을 진행합니까?", "네", "아니요" };


    void NewGameStart(bool accept)
    {
        _decision2.OnBtnClose();

        if (accept)
        {
            Main.Player.isReset = true;
            LoadingScene.LoadScene(Define.SceneName.intro);
        }
        else
        {
        }
    }

    private void OnBtnContinue()
    {
        Main.Player.isReset = false;
        LoadingScene.LoadScene(Define.SceneName.Town);
    }

    private void OnBtnOption()
    {
        Main.UI.OpenPopup<UI_Popup_Option>();
    }

    private UI_Popup_Decision _decision;
    private string[] GameExit_textArray = new string[] { "게임을 종료합니다 \n동의합니까?!", "네", "아니요" };

    private void OnBtnExit()
    {
        _decision = Main.UI.OpenPopup<UI_Popup_Decision>();

        _decision.decisionAction += GameExit;
        _decision.OpenDecision(ref GameExit_textArray);
    }

    void GameExit(bool accept)
    {
        _decision.OnBtnClose();

        if (accept)
        {
            Application.Quit();
        }
        else
        {
        }
    }


}


