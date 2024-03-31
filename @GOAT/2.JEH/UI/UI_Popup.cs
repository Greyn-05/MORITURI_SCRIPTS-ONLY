public class UI_Popup : UI_Base
{

    public int SortOrder { get; set; }


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        Main.Audio.BtnSound();

        return true;
    }


    // TODO 열고 닫고 함수



    public virtual void OnBtnClose()
    {
        Main.UI.ClosePopup(this);

    }

}