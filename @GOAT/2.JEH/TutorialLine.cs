using DG.Tweening;
using UnityEngine;
using TMPro;

public class TutorialLine : UI_Popup
{

    /// <summary>
    ///  사용법 : TutorialLine 프리팹을 튜토리얼씬 하이어라키에 올려둔다.
    ///  
    /// 글이 떠야하는 타이밍에  TutorialLineOn("띄울글씨");   를 호출한다.
    /// 
    /// </summary>



    private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _lineText;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;

        return true;
    }

    public void TutorialLineOn(string txt)
    {
        CancelInvoke();

        _lineText.text = txt;
        _canvasGroup.DOFade(0.7f, 0.2f);

    }


    public void TutorialLineOff()
    {
        _canvasGroup.DOFade(0, 0.2f);
    }


}
