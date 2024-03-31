using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private bool initialized = false;

    private List<GameObject> enemyPrefab = new List<GameObject>();
    //필요한 기능
    //1. 플레이어 소환
    //2. 보스 소환 인데... 어떤 보스를 어떻게 불러올지

    public GameObject currentEnemy;
    public int SelectedBossIndex;


    public DateTime BattleBeginTime; // 전투시간 계산용

    //프로퍼티 수정
    public EnemyInfo currentEnemyStatus { get; set; }
    public void Initialize()
    {

        if (initialized) return;


        foreach (string str in Define.bossName)
        {
            enemyPrefab.Add(Main.Resource.Load<GameObject>(str));
        }

        /*enemyPrefab.Add(Main.Resource.Load<GameObject>("BasicSwordMan"));
        enemyPrefab.Add(Main.Resource.Load<GameObject>("TwoHandedAxe"));
        enemyPrefab.Add(Main.Resource.Load<GameObject>("Gunslinger"));*/


        initialized = true;
    }


    public List<UnityEngine.Object> GetEnemy()
    {
        return null;
    }

    public GameObject GetEnemyProto(int index)
    {
        return enemyPrefab[index];
    }

    public void LoseEvent()
    {
        Main.UI.OpenPopup<UI_Popup_ResultLose>();
    }

    public void WinEvent()
    {
        Main.Player.playerData.isBossRelease[SelectedBossIndex] = true;

        if (SelectedBossIndex + 1 < Main.Player.playerData.isBossRelease.Length)
        {
            Main.Player.playerData.isBossRelease[SelectedBossIndex + 1] = true;
        }

        Main.Save.SaveToJson_PlayerData();

        Main.UI.OpenPopup<UI_Popup_ResultWin>();
    }

   public string FloatTimer(float time) // 클탐계산기
    {
        string t = TimeSpan.FromSeconds(time).ToString(@"dd\:mm\:ss");
        string[] tokens = t.Split(':');
        return $"{tokens[0]}시 {tokens[1]}분 {tokens[2]}초";
    }


}
