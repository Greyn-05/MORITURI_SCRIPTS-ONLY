using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Init Position")]
    [SerializeField]
    private Vector3 _playerSpawnPosition;

    [SerializeField]
    private Vector3 _EnemySpawnPosition;

    #region Parameters
    [Header("PreFabs")]
    public GameObject PreFabPlayer;
    public GameObject PreFabEnemy;

    private GameObject spawnedPlayer;
    private GameObject spanwedEnemy;
    #endregion

    #region Componets
    private EnemyBehaviorTreeRunner EnemyBTrunner;
    #endregion
    private void Awake()
    {
        SpawnPlayer();
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPlayer()
    {
        spawnedPlayer = Instantiate(PreFabPlayer, _playerSpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
    }

    private void SpawnEnemy()
    {
        spanwedEnemy = Instantiate(PreFabEnemy, _EnemySpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        EnemyBTrunner = spanwedEnemy.GetComponent<EnemyBehaviorTreeRunner>();
        EnemyBTrunner.InitBHtree();
        EnemyBTrunner.context.targetPlayer = spawnedPlayer;
        EnemyBTrunner.context.transform.LookAt(spawnedPlayer.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 Ps = new Vector3(_playerSpawnPosition.x, _playerSpawnPosition.y+2/2, _playerSpawnPosition.z);
        Gizmos.DrawWireCube(Ps, new Vector3(2, 2, 2));

        Gizmos.color = Color.red;
        Vector3 Es = new Vector3(_EnemySpawnPosition.x, _EnemySpawnPosition.y + 2/2, _EnemySpawnPosition.z);
        Gizmos.DrawWireCube(Es, new Vector3(2, 2, 2));
    }
}
