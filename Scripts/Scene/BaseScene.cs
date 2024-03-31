using UnityEngine;

public class BaseScene : MonoBehaviour
{
    private bool initialized = false;

    private void Start()
    {
        if (!Main.Resource.Loaded)
        {
            Main.Resource.ResourcesAssign();
        }

        Initialize();

    }

    public virtual bool Initialize()
    {
        if (initialized) return false;

        // 씬이 바뀔때마다 초기화하기때문에 main 인스턴스들은 안에 bool값을 만들어서 한번만 실행되게 해야합니다.

        Main.CSVData.Initialize();
        Main.Player.Initialize();

        Main.Save.DataLoad_Player(); // 세이브데이터관리 딴곳에서 하기
        Main.Save.DataLoad_GameSetting();

        Main.Game.Initialize();
        Main.Quest.Initialize();

        Time.timeScale = 1;

        Main.Pool._pools.Clear();
        Main.UI._popupList.Clear();
        Main.Inven.QuickSlotIndex = 0;

        initialized = true;
        return true;
    }
}
