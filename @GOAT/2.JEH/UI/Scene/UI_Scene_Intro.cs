using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Intro : UI_Scene
{
    [SerializeField] private Button StartBtn;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Player.CursorLock_None();


        StartBtn.onClick.AddListener(() => LoadingScene.LoadScene(Define.SceneName.Tutorial));



        return true;
    }

}
