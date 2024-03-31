using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtorialTest : BaseScene
{
    [Header("Spawn Position")]
    [SerializeField]
    private Vector3 _playerSpawnPosition;

    [SerializeField]
    private Vector3 _EnemySpawnPosition;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    private GameObject spawnedPlayer;
    private GameObject spawnedEnemy;
    private EnemyBehaviorTreeRunner EnemyBTrunner;
    private PlayerController playerController;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;



        //   Cursor.lockState = CursorLockMode.Locked;
        Main.UI.SetSceneUI<UI_Scene_Dungeon>();

        //테스트
        Main.Player.CreatePlayer(_playerSpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        spawnedPlayer = Main.Player.PlayerObject;
        playerController = Main.Player.Controller;
        playerController.InputValue.SetDungeonScene();
        playerController.status.HP.SetValue(playerController.status.HP.MaxValue);


        spawnEnemyProto2();

        return true;
    }



    private void spawnEnemyProto()
    {
        spawnedEnemy = Instantiate(EnemyPrefab, _EnemySpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        EnemyBTrunner = spawnedEnemy.GetComponent<EnemyBehaviorTreeRunner>();
        EnemyBTrunner.InitBHtree();
        EnemyBTrunner.context.targetPlayer = spawnedPlayer;
        EnemyBTrunner.context.transform.LookAt(new Vector3(spawnedPlayer.transform.position.x, spawnedEnemy.transform.position.y, spawnedPlayer.transform.position.z));
        playerController.Health.OnDie += EnemyBTrunner.StopEnemyWhenPlayerDead;
        Main.Game.currentEnemyStatus = EnemyBTrunner.context.enemyInfo;
        Main.Game.currentEnemy = spawnedEnemy;
        EnemyBTrunner.context.enemyController.health.OnDie += Main.Game.WinEvent;
    }

    private void spawnEnemyProto2()
    {
        spawnedEnemy = Instantiate(EnemyPrefab, _EnemySpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        EnemyBTrunner = spawnedEnemy.GetComponent<EnemyBehaviorTreeRunner>();
        EnemyBTrunner.InitBHtree();
        EnemyBTrunner.context.targetPlayer = spawnedPlayer;
        EnemyBTrunner.context.transform.LookAt(new Vector3(spawnedPlayer.transform.position.x, spawnedEnemy.transform.position.y, spawnedPlayer.transform.position.z));
        playerController.Health.OnDie += EnemyBTrunner.StopEnemyWhenPlayerDead;
        Main.Game.currentEnemyStatus = EnemyBTrunner.context.enemyInfo;
        Main.Game.currentEnemy = spawnedEnemy;
        EnemyBTrunner.context.enemyControllerMage.health.OnDie += Main.Game.WinEvent;
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
