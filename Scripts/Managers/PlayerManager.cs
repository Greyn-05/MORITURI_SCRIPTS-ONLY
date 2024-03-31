using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager
{
    private bool initialized = false;

    public bool isReset = false; // 세이브파일날리는용


    public PlayerData playerData; 
    public GameSettingData gameSetting; 


    private GameObject _playerObject;
    private PlayerController _controller;
    private PlayerStatus _status = new();
    
    public GameObject PlayerObject => _playerObject;
    public PlayerController Controller => _controller;
    public PlayerStatus Status => _status;
    public int ParryCount;

    public DamageIndicator DamageIndicator;
    public HitBloodEffect HitBloodEffect;
    public Define.AttackAttribute PlayerAttribute;
    

    public Action OnResetLockOnEvnet;
    public Action OnParryCountEvent;

    public Action OnWarningEvent;
    public Action OnHeal;

    public Action<string> OnMainQuest;
    public Action OnInteractiveEvent;
    public Action<string> OnNPCEnter;
    public Action<string> OnNPCExit;
    
    
    // 임시
    public Define.EPlayerSceneName CurrentScene;

    public void Initialize()
    {
        if (initialized) return;



        initialized = true;
    }

    
    #region SetInit ----------------------------------------------------------------------------------------------------
    public void CreatePlayer(Vector3 position, Quaternion rotate)
    {
        _playerObject = Main.Resource.InstantiatePrefab("Player", position, rotate);
        Main.Cinemachne.SetPlayerCamera();
        _controller = _playerObject.GetComponent<PlayerController>();
    }

    public void SetTownScene(Vector3 spawnPosition)
    {
        CreatePlayer(spawnPosition, Quaternion.Euler(new Vector3(0, 180, 0)));
        SetTownScenePlayer();
    }
    public void SetTownScene(Vector3 spawnPosition, Vector3 spawnRotation)
    {
        CreatePlayer(spawnPosition, Quaternion.Euler(spawnRotation));
        SetTownScenePlayer();
    }

    private void SetTownScenePlayer()
    {
        Main.Resource.InstantiatePrefab("PlayerMinimapMaker", PlayerObject.transform);
        Main.Resource.InstantiatePrefab("MainQuestNavigation", _playerObject.transform);
        _controller.InputValue.SetTownScene();
        _controller.status.HP.SetValue(_controller.status.HP.MaxValue);
        _controller.status.Stamina.SetValue(_controller.status.Stamina.MaxValue);
        _controller.Weapon.gameObject.SetActive(false);
        
        Main.Cinemachne.SetMinimapCamera();
        CursorLock_Locked();
    }
    
    public void SetDungeonScene(Vector3 spawnPosition)
    {
        CreatePlayer(spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180f, 0.0f)));
        Main.Resource.InstantiatePrefab("Simpson", _playerObject.transform);
        _controller.InputValue.SetDungeonScene();
        _controller.status.HP.SetValue(_controller.status.HP.MaxValue);
        _controller.status.Stamina.SetValue(_controller.status.Stamina.MaxValue);
        _controller.Weapon.gameObject.SetActive(true);
        ParryCount = 0;
        DamageIndicator = Main.UI.SetSubItemUI<DamageIndicator>();
        HitBloodEffect = Main.UI.SetSubItemUI<HitBloodEffect>();
        
        Main.Player.CurrentScene = Define.EPlayerSceneName.Dungeon;
        CursorLock_Locked();
    }
    
    #endregion





    #region InputEvent -------------------------------------------------------------------------------------------------
    public void OnInteractClicked()
    {
        OnInteractiveEvent?.Invoke();
    }

    public void OnResetLockOnClicked()
    {
        OnResetLockOnEvnet?.Invoke();
    }
    #endregion


    #region CursorLock -------------------------------------------------------------------------------------------------

    public void CursorLock_Locked()
    {
        if (SceneManager.GetActiveScene().name.Equals(Define.SceneName.Title)) return;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CursorLock_None()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion


    #region Method -----------------------------------------------------------------------------------------------------


    public void PlayerAttributeChange(Define.AttackAttribute attribute)
    {
        PlayerAttribute = attribute;
        _controller.OnAttributeChangeEventStarted();
    }

    public void PlayerInvincibleTrue() => _controller.isInvincible = true;
    public void PlayerInvincibleFalse() => _controller.isInvincible = false;

    public void OnMainQuestStarted(QuestData currentQuest)
    {
        // if (currentQuest.QuestID >= 998) // 최종 메인퀘스트
        // {
        //     OnMainQuest?.Invoke(" ");
        //     return;
        // }
        
        // 아이템퀘스트 , 클리어 현황
        bool isItemQuest = !(currentQuest.ItemCount == -1);
        bool isItemQuestClear = false;
        
        // 던전퀘스트, 클리어 현황
        bool isDungeonQuest = !(currentQuest.CombatCount == -1);
        bool isDungeonQuestClear = false;
        
        // 아이템 퀘스트 클리어 확인
        int haveItem = Main.Inven.HowManyThisItemYouHave(currentQuest.ItemID);
        if (haveItem >= currentQuest.ItemCount) isItemQuestClear = true;
        
        // 던전퀘스트 클리어 확인
        if (currentQuest.CurrentCount >= 1) isDungeonQuestClear = true;

        
        
        if (isItemQuest && !isItemQuestClear) // 아이템퀘스트, 클리어불가 시
        {
            OnMainQuest?.Invoke("Shop");
            return;
        }
        else if (isDungeonQuest && !isDungeonQuestClear) // 던전퀘스트, 클리어불가 시
        {
            OnMainQuest?.Invoke("Dungeon");
            return;
        }
        
        
        // 그외 (기본메인퀘, 아이템퀘 클리어O, 던전퀘 클리어O)
        OnMainQuest?.Invoke(currentQuest.ClearNPC);
    }

    #endregion


    #region Event ------------------------------------------------------------------------------------------------------
    public void OnNPCEnterStarted(string npcName) => OnNPCEnter?.Invoke(npcName);
    public void OnNPCExitStarted(string npcName) => OnNPCExit?.Invoke(npcName);
    
    

    #endregion

}