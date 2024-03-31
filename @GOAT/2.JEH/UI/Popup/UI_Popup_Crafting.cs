using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Crafting : UI_Popup
{

    // TODO 제작 해금된것 제작가능한것 새로고침해서 표시

    [SerializeField] private Button _closeBtn;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        _closeBtn.onClick.AddListener(OnBtnClose);


        return true;
    }



}
