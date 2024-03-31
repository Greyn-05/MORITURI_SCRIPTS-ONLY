using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_GamblingEnding : UI_Scene
{
    [SerializeField] private Button titleBtn;


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Player.CursorLock_None();


        titleBtn.onClick.AddListener(() => LoadingScene.LoadScene(Define.SceneName.Title));





        return true;
    }
}
