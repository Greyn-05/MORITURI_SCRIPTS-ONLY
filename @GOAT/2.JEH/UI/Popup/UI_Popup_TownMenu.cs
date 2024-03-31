using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_TownMenu : UI_Popup
{

    [SerializeField] private Button _optionBtn;
    [SerializeField] private Button _goTitleBtn;


    [SerializeField] private Button _loadBtn;
    [SerializeField] private Button _saveBtn;

    [SerializeField] private Button _closeBtn;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _closeBtn.onClick.AddListener(OnBtnClose);
        _loadBtn.onClick.AddListener(OnPressLoad);
        _saveBtn.onClick.AddListener(OnPressSave);
        _goTitleBtn.onClick.AddListener(OnPressTitle); 
        _optionBtn.onClick.AddListener(OnPressOption);


        return true;

    }

    private void OnPressSave()
    {
        Main.UI.OpenPopup<UI_Popup_Save>();
    }

    private void OnPressLoad()
    {
        Main.UI.OpenPopup<UI_Popup_Load>();
    }

    private void OnPressOption()
    {
        Main.UI.OpenPopup<UI_Popup_Option>();
    }


    private UI_Popup_Decision _decision;
    private string[] GameExit_textArray = new string[] { "타이틀로 돌아갈까요?", "네", "아니요" };

    private void OnPressTitle()
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
            OnBtnClose();
            LoadingScene.LoadScene(Define.SceneName.Title);
        }
        else
        {
            OnBtnClose(); // todo
        }
    }
}
