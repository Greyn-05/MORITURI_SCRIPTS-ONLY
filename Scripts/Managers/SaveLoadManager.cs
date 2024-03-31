using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadManager
{
    private string path;

    // 퀵세이브 현재 세이브파일에 저장. 현재 플레이중인게 몇번데이터인지 알아야할듯

    #region 플레이어정보


    public void DataLoad_Player() // 파일 불러오기
    {
        Main.Player.playerData = new();

        path = PlayerDataFilePath();
        FileInfo fileInfo = new(path);

        if (fileInfo.Exists)
        {
            LoadFromJson_PlayerData();
        }
        else
        {
            DefaultPlayerData();
            SaveToJson_PlayerData();
        }

        Main.Inven.Initialize();
        Main.Quest.RestoreQuest();

    }

    public void SaveToJson_PlayerData()
    {
        path = PlayerDataFilePath();

        // 파일이 존재한다면 덮어씌울 겁니까? 아니면 팝업창 꺼짐.
        // 덮어씌운후에 슬롯들 새로고침.

        Main.Player.playerData.activeQuestID = Main.Quest.ActiveSubQuests?.Select(q => q.QuestID).ToList() ?? new List<int>();
        Main.Player.playerData.activeQuestCurrentCount = Main.Quest.ActiveSubQuests?.Select(q => q.CurrentCount).ToList() ?? new List<int>();
        Main.Player.playerData.readyToClearQuestID = Main.Quest.ReadyToClear?.Select(q => q.QuestID).ToList() ?? new List<int>();
        Main.Player.playerData.completedQuestID = Main.Quest.CompletedQuests?.Select(q => q.QuestID).ToList() ?? new List<int>();

        if (Main.Quest.CurrentMainQuest != null)
        {
            Main.Player.playerData.currentMainQuestID = Main.Quest.CurrentMainQuest.QuestID;
            Main.Player.playerData.currentMainQuestCurrentCount = Main.Quest.CurrentMainQuest.CurrentCount;
        }
        else
        {
            Main.Player.playerData.currentMainQuestID = -1;
            Main.Player.playerData.currentMainQuestCurrentCount = 0;
        }


        Main.Player.playerData.hP_cur = Main.Player.Status.HP.CurValue; // 스탯들은 클래스안에없어서 저장할때 별도로 저장해줘야한다
        Main.Player.playerData.hP_max = Main.Player.Status.HP.MaxValue;

        Main.Player.playerData.stamina_cur = Main.Player.Status.Stamina.CurValue;
        Main.Player.playerData.stamina_max = Main.Player.Status.Stamina.MaxValue;

        Main.Player.playerData.exp_cur = Main.Player.Status.Exp.CurValue;
        Main.Player.playerData.exp_max = Main.Player.Status.Exp.MaxValue;

        Main.Player.playerData.atk = Main.Player.Status.Atk.Value;
        Main.Player.playerData.def = Main.Player.Status.Def.Value;
        Main.Player.playerData.speed = Main.Player.Status.Speed.Value;

        string jsonData = JsonUtility.ToJson(Main.Player.playerData, true);
        System.IO.File.WriteAllText(path, Crypto.AESEncrypt(jsonData));
    }

    private void LoadFromJson_PlayerData()
    {
        path = PlayerDataFilePath();
        string jsonData = System.IO.File.ReadAllText(path);
        string ddd = Crypto.AESDecrypt(jsonData);
        

        Main.Player.playerData = JsonUtility.FromJson<PlayerData>(ddd);

        //스탯도 다 넣어야함
        // 퀘스트도!

    }

    public bool Have_PlayerDataFile() // 세이브파일을 가지고있는지
    {
        path = PlayerDataFilePath();
        FileInfo fileInfo = new(path);

        return fileInfo.Exists;
    }


    private string PlayerDataFilePath()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            return Path.Combine(Application.persistentDataPath + "/SaveData.json");
        else
            return Path.Combine(Application.dataPath + "/SaveData.json"); // 웹에선 실행안됨

    }

    public void DefaultPlayerData()
    {
        // Debug.Log("저장파일이 없습니다. 기본값으로 설정됩니다.");

        Main.Player.playerData.isTutorialClear = false;

        Main.Player.playerData.playerName = "고트";
        Main.Player.playerData.gold = 10000;

        Main.Player.playerData.hP_cur = Main.Player.Status.HP.CurValue; //저장용스탯 스탯계산에 쓰지마세요!
        Main.Player.playerData.hP_max = Main.Player.Status.HP.MaxValue;

        Main.Player.playerData.stamina_cur = Main.Player.Status.Stamina.CurValue;
        Main.Player.playerData.stamina_max = Main.Player.Status.Stamina.MaxValue;

        Main.Player.playerData.exp_cur = Main.Player.Status.Exp.CurValue;
        Main.Player.playerData.exp_max = Main.Player.Status.Exp.MaxValue;

        Main.Player.playerData.atk = Main.Player.Status.Atk.Value;
        Main.Player.playerData.def = Main.Player.Status.Def.Value;
        Main.Player.playerData.speed = Main.Player.Status.Speed.Value;

        Main.Player.playerData.isBossRelease = new bool[4] { true, false, false, false };
        Main.Player.playerData.bossClearTime = new float[4] { 99999999, 99999999, 99999999, 99999999 };

        Main.Player.playerData.inventoryId = new int[Define.maxInvenSlotCount];
        Main.Player.playerData.inventoryCap = new int[Define.maxInvenSlotCount];

        for (int i = 0; i < Define.maxInvenSlotCount; i++)
        {
            Main.Player.playerData.inventoryId[i] = -1;
            Main.Player.playerData.inventoryCap[i] = 0;
        }

        //퀘스트 하나도 안깨짐상태로
    }

    #endregion


    #region 게임옵션정보


    public void DataLoad_GameSetting() // 파일 불러오기
    {
        Main.Player.gameSetting = new();
        path = GameSettingFilePath();
        FileInfo fileInfo = new(path);

        if (fileInfo.Exists)
        {
            LoadFromJson_GameSetting();
        }
        else
        {
            DefaultGameSettingData();
        }

    }

    public void SaveToJson_GameSetting()
    {
        path = GameSettingFilePath();
        string jsonData = JsonUtility.ToJson(Main.Player.gameSetting, true);

        System.IO.File.WriteAllText(path, Crypto.AESEncrypt(jsonData));
    }

    private void LoadFromJson_GameSetting()
    {
        path = GameSettingFilePath();
        string jsonData = System.IO.File.ReadAllText(path);
        string ddd = Crypto.AESDecrypt(jsonData);

        Main.Player.gameSetting = JsonUtility.FromJson<GameSettingData>(ddd);
    }

    public bool Have_GameSettingFile()
    {
        path = GameSettingFilePath();
        FileInfo fileInfo = new(path);
        return fileInfo.Exists;
    }

    private string GameSettingFilePath()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            return Path.Combine(Application.persistentDataPath + "/GameSetting.json");
        else
            return Path.Combine(Application.dataPath + "/GameSetting.json"); // 웹에선 실행안됨
    }

    public void DefaultGameSettingData()
    {
        Main.Player.gameSetting.isFullscreen = true;
        Main.Player.gameSetting.resolution = Screen.resolutions.Length - 1;

        Main.Player.gameSetting.allVolume = 0.5f;
        Main.Player.gameSetting.allMute = false;
        Main.Player.gameSetting.bgmVolume = 80;
        Main.Player.gameSetting.bgmMute = false;
        Main.Player.gameSetting.sfxVolume = 80;
        Main.Player.gameSetting.sfxMute = false;

        Main.Player.gameSetting.mouseSensitivity = 1f;
    }

    #endregion

}
