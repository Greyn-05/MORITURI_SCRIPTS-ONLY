using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Save : UI_Popup
{


    [SerializeField] private Button closeBtn;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        closeBtn.onClick.AddListener(OnBtnClose);



        return true;
    }
}
