using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Popup_Pause : UI_Popup
{

    [SerializeField] private Button _comboBtn;
    [SerializeField] private Button _backToTheTownBtn;

    [SerializeField] private Button _optionBtn;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private TextMeshProUGUI _backText;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        _comboBtn.onClick.AddListener(OnBtnCombo);
        _backToTheTownBtn.onClick.AddListener(OnBtnBackToTheTown);
        _closeBtn.onClick.AddListener(OnBtnClose);

        _optionBtn.onClick.AddListener(Option);


        Time.timeScale = 0.0001f;
        Main.Cinemachne.MenuOnCamera();

        if (SceneManager.GetActiveScene().name == Define.SceneName.Tutorial)
        {
            _backText.text = "튜토리얼 스킵";
        }
        else
        {
            _backText.text = "마을로 돌아가기";
        }



        return true;
    }


    void OnBtnCombo()
    {
        Main.UI.OpenPopup<UI_Popup_Combo>();
    }

    void OnBtnBackToTheTown()
    {
        _decision = Main.UI.OpenPopup<UI_Popup_Decision>();

        _decision.decisionAction += GameExit;

        if (SceneManager.GetActiveScene().name == Define.SceneName.Tutorial)
        {
            _decision.OpenDecision(ref tutoExit_textArray);
        }
        else
        {
            _decision.OpenDecision(ref GameExit_textArray);
        }

    }

    public override void OnBtnClose()
    {
        Time.timeScale = 1f;
        Main.Cinemachne.MenuOffCamera();
        base.OnBtnClose();
    }

    private void Option()
    {
        Main.UI.OpenPopup<UI_Popup_Option>();
    }

    private UI_Popup_Decision _decision;
    private string[] GameExit_textArray = new string[] { "전투를 포기하시겠습니까?", "네", "아니요" };
    private string[] tutoExit_textArray = new string[] { "튜토리얼을 스킵하시겠습니까?", "네", "아니요" };

    void GameExit(bool accept)
    {
        _decision.OnBtnClose();

        if (accept)
        {
            OnBtnClose();
            LoadingScene.LoadScene(Define.SceneName.Town);
        }
        else
        {
        }

    }

    private void OnDisable()
    {
        OnBtnClose();
    }

}
