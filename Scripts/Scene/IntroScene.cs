using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;



        Main.UI.SetSceneUI<UI_Scene_Intro>();

        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>(Define.BgmFileName.introBgm), 0.75f);




        return true;
    }

}