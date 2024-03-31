using UnityEngine;

public class TownScene : BaseScene
{
    private Vector3 spawnPosition = new Vector3(7.5f, 0.0f, 35.0f);
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.UI.SetSceneUI<UI_Scene_Town>();
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("TownBGM"));
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("TownNoise"));


        switch (Main.Player.CurrentScene)
        {
            case Define.EPlayerSceneName.Game:
                Main.Player.SetTownScene(new Vector3(-25f, 0f, -14.5f), new Vector3(0, -95, 0));
                break;
            case Define.EPlayerSceneName.Dungeon:
                Main.Player.SetTownScene(new Vector3(6, 0, -23), new Vector3(0, -35, 0));
                break;
            default:
                Main.Player.SetTownScene(spawnPosition);
                break;
        }

        Main.Player.CurrentScene = Define.EPlayerSceneName.Town;
        return true;
    }

}
