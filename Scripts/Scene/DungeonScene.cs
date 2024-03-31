using System;
using UnityEngine;

public class DungeonScene : BaseScene
{
    [Header("Spawn Position")]
    [SerializeField]
    private Vector3 _playerSpawnPosition;

    [SerializeField]
    private Vector3 _EnemySpawnPosition;

    public HitEffect hitEffect;

    public GameObject spawnedPlayer;
    public GameObject spawnedEnemy;
    private EnemyBehaviorTreeRunner EnemyBTrunner;
    private PlayerController playerController;

    public GameObject Sun;
    public GameObject Moon;

    public Material SunSkybox;
    public Material MoonSkybox;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        //   Cursor.lockState = CursorLockMode.Locked;
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("DungeonAxeBgm"), 0.85f);
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("DungeonCheeringBGM"), 0.85f);

        


        //플레이어 생성
        Main.Player.SetDungeonScene(_playerSpawnPosition);
        playerController = Main.Player.Controller;
        spawnedPlayer = Main.Player.PlayerObject;
        //적 생성
        // createEnemyProto(0);
        createEnemyProto(Main.Game.SelectedBossIndex);


        // 이 뒤에 UI 가 바인딩 되야됩니다
        Main.UI.SetSceneUI<UI_Scene_Dungeon>();


        Main.Game.BattleBeginTime = DateTime.Now;

        return true;
    }


    private void createEnemyProto(int Eindex)
    {
        spawnedEnemy = Instantiate(Main.Game.GetEnemyProto(Eindex), _EnemySpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        EnemyBTrunner = spawnedEnemy.GetComponent<EnemyBehaviorTreeRunner>();
        EnemyBTrunner.InitBHtree();
        EnemyBTrunner.context.targetPlayer = spawnedPlayer;

        EnemyBTrunner.context.transform.LookAt(new Vector3(spawnedPlayer.transform.position.x, spawnedEnemy.transform.position.y, spawnedPlayer.transform.position.z));
        if (Eindex == 2)
        {
            Sun.SetActive(false);
            Moon.SetActive(true);
            RenderSettings.skybox = MoonSkybox;
            EnemyBTrunner.context.enemyControllerGun.health.OnDie += Main.Game.WinEvent;
            EnemyBTrunner.context.enemyControllerGun.HitEvent += ActiveHitEffect;
        }
        else if(Eindex == 3)
        {
            Sun.SetActive(false);
            Moon.SetActive(true);
            RenderSettings.skybox = MoonSkybox;
            spawnedEnemy.transform.position = Vector3.zero;
            EnemyBTrunner.context.enemyControllerMage.health.OnDie += Main.Game.WinEvent;
            EnemyBTrunner.context.enemyControllerMage.HitEvent += ActiveHitEffect;
        }
        else
        {
            Sun.SetActive(true);
            Moon.SetActive(false);
            RenderSettings.skybox = SunSkybox;
            EnemyBTrunner.context.enemyController.health.OnDie += Main.Game.WinEvent;
            EnemyBTrunner.context.enemyController.HitEvent += ActiveHitEffect;
            EnemyBTrunner.context.DamageReduce = EnemyBTrunner.context.enemyController.damageReduce;
        }
        playerController.Health.OnDie += EnemyBTrunner.StopEnemyWhenPlayerDead;
        Main.Game.currentEnemyStatus = EnemyBTrunner.context.enemyInfo;
        Main.Game.currentEnemy = spawnedEnemy;
    }

    public void ActiveHitEffect()
    {
        hitEffect.ActiveEvent();
        Main.Player.HitBloodEffect.FlashHitBlood();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 Ps = new Vector3(_playerSpawnPosition.x, _playerSpawnPosition.y + 2 / 2, _playerSpawnPosition.z);
        Gizmos.DrawWireCube(Ps, new Vector3(2, 2, 2));

        Gizmos.color = Color.red;
        Vector3 Es = new Vector3(_EnemySpawnPosition.x, _EnemySpawnPosition.y + 2 / 2, _EnemySpawnPosition.z);
        Gizmos.DrawWireCube(Es, new Vector3(2, 2, 2));
    }
}
