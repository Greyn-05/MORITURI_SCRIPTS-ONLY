using UnityEngine;

public class EndingScene : BaseScene
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;



        Main.UI.SetSceneUI<UI_Scene_Ending>();



        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>(Define.BgmFileName.battleEndBgm), 0.9f);


        return true;
    }

}
