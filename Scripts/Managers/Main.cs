using UnityEngine;

public class Main : MonoBehaviour
{
    private static bool initialized;
    private static Main instance;
    public static Main Instance
    {
        get
        {
            if (!initialized)
            {
                initialized = true;

                GameObject obj = GameObject.Find("@Main");
                if (obj == null)
                {
                    obj = new() { name = @"Main" };
                    obj.AddComponent<Main>();
                    DontDestroyOnLoad(obj);
                    instance = obj.GetComponent<Main>();
                }
            }
            return instance;
        }
    }


    private readonly PoolManager _pool = new();
    private readonly UIManager _ui = new();
    private readonly ResourceManager _resource = new();
    private readonly AudioManager _audio = new();

    private readonly SaveLoadManager _save = new();

    private readonly PlayerManager _player = new();
    private readonly CinemachineManager _cinemachine = new();
    private readonly GameManager _game = new();

    private readonly QuestManager _quest = new();

    private readonly CSVDataBase _csvDta = new();


    private readonly InventoryManager _inventoryManager = new();


    public static PoolManager Pool => Instance?._pool;
    public static UIManager UI => Instance?._ui;
    public static ResourceManager Resource => Instance?._resource;
    public static AudioManager Audio => Instance?._audio;
    public static QuestManager Quest => Instance?._quest;
    public static SaveLoadManager Save => Instance?._save;
    public static PlayerManager Player => Instance?._player;
    public static CinemachineManager Cinemachne => Instance?._cinemachine;
    public static GameManager Game => Instance?._game;

    public static CSVDataBase CSVData => Instance?._csvDta;

    public static InventoryManager Inven => Instance?._inventoryManager;


}