using UnityEngine;

public class TitleScene : BaseScene
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetSceneUI<UI_Scene_Title>();

        Main.Cinemachne.InstantiateCamera();
        Main.Cinemachne.SetStateDrivenCamera(null);

        Main.Cinemachne.MouseSpeed(Main.Player.gameSetting.mouseSensitivity);
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>(Define.BgmFileName.titleBgm), 0.75f);

        return true;
    }

    private void Update() //TODO 인풋시스템으로 교체할것
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Main.UI.CloseTopPopup();
        }
    }

}
