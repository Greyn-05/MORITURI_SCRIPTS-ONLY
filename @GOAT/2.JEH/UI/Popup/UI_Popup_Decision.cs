using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Decision : UI_Popup
{
    // TODO 설명 글씨 길이만큼 확인창 크기가 늘어나야한다


    [SerializeField] private TMP_Text explainText;  
    [SerializeField] private TMP_Text acceptText;  
    [SerializeField] private TMP_Text cancelText; 

    [SerializeField] private Button acceptBtn;
    [SerializeField] private Button cancelBtn;

    private bool isOpen = false;

    public Action<bool> decisionAction;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        acceptBtn.onClick.AddListener(AceeptPress);
        cancelBtn.onClick.AddListener(CancelPress);


        return true;
    }


    public void OpenDecision(ref string[] a_textArray)
    {
        if (isOpen) return;

        isOpen = true; 
        decisionAction -= CloseDecision;
        decisionAction += CloseDecision;

        explainText.text = a_textArray[0].Replace("\\n", "\n"); //  \n이 줄바꿈이 된다.
        acceptText.text = a_textArray[1];
        cancelText.text = a_textArray[2];
    }

    public void AceeptPress() // 예 버튼 
    {
        Main.Audio.BtnSound();
        decisionAction?.Invoke(true);
    }

    public void CancelPress() // 아니오 버튼 
    {
        Main.Audio.BtnSound();
        decisionAction?.Invoke(false);
    }


    #region 기타

    void CloseDecision(bool a_selectAccept)
    {
        isOpen = false;
        decisionAction -= CloseDecision;
        OnBtnClose();
    }

    private void OnDisable()
    {
        isOpen = false;
        decisionAction -= CloseDecision;
    }

    #endregion
}


