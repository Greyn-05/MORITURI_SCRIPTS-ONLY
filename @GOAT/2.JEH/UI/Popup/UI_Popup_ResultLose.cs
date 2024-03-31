using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_ResultLose : UI_Popup
{


    [SerializeField] private Button _backToTheTownBtn;


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        _backToTheTownBtn.onClick.AddListener(OnBtnBackToTheTown);


        return true;
    }
    void OnBtnBackToTheTown()
    {
        OnBtnClose();
        LoadingScene.LoadScene(Define.SceneName.Town);

    }

}
