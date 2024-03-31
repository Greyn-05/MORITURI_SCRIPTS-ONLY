using UnityEngine;

public class GamblingEndingScene : BaseScene
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;



        Main.UI.SetSceneUI<UI_Scene_GamblingEnding>();


        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>(Define.BgmFileName.gameblingEndBgm), 0.8f);



        return true;
    }
}
