public class UI_Scene_Title : UI_Scene
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Player.CursorLock_None();

        //TODO 볼륨적용안되는 문제 해결용
        UI_Popup_Option dd = Main.UI.OpenPopup<UI_Popup_Option>();
        Main.UI.ClosePopup(dd);


        return true;
    }


}
