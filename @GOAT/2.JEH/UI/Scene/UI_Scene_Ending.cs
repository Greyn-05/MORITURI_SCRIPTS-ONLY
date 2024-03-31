using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Ending : UI_Scene
{
    [SerializeField] private Button titleBtn;


    [SerializeField] private TextMeshProUGUI cleartext0;
    [SerializeField] private TextMeshProUGUI cleartext1;
    [SerializeField] private TextMeshProUGUI cleartext2;
    [SerializeField] private TextMeshProUGUI cleartext3;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        Main.Player.CursorLock_None();


        titleBtn.onClick.AddListener(() => LoadingScene.LoadScene(Define.SceneName.Title));


        cleartext0.text = $"{Define.bossName[0]} : {Main.Game.FloatTimer(Main.Player.playerData.bossClearTime[0])}";
        cleartext1.text = $"{Define.bossName[1]} : {Main.Game.FloatTimer(Main.Player.playerData.bossClearTime[1])}";
        cleartext2.text = $"{Define.bossName[2]} : {Main.Game.FloatTimer(Main.Player.playerData.bossClearTime[2])}";



        float ddd = 0;

        for (int i = 0; i < 3; i++)
        {
            ddd += Main.Player.playerData.bossClearTime[i];
        }

        //for (int i = 0; i < Define.bossName.Length; i++)
        //{
        //    ddd += Main.Player.playerData.bossClearTime[i];
        //}

        cleartext3.text = $"총 전투시간 : {Main.Game.FloatTimer(ddd)}";


        return true;
    }

}
