using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private EnemyBehaviorTreeRunner BT;

    private GameObject GroundCrack;

    [SerializeField]
    private Transform crackPosition;

    private string[] groundAttackSound = new string[] { "AxeGround", "Odachi_ground", "Odachi_ground2" };

    private void Start()
    {
        GroundCrack = Main.Resource.InstantiatePrefab("2handAxe_Crack");
    }
    private void OnAttackTrueE()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("Axe"+ Random.Range(1, 9).ToString()), this.gameObject.transform);
        if (BT.context._myState == BattleState.Attack)
        {
            BT.context._myState = BattleState.Attack_Judge;
        }
    }

    private void onGroundAttack()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(groundAttackSound[Random.Range(0, groundAttackSound.Length)]), this.gameObject.transform);
        
        if (BT.context._myState == BattleState.Attack)
        {
            BT.context._myState = BattleState.Attack_Judge;
        }
    }

    private void SpawnCrack()
    {
        StartCoroutine(CoStartCrack());
    }

    public IEnumerator CoStartCrack()
    {
        Vector3 tPosition = new Vector3(crackPosition.position.x, 0.09f, crackPosition.position.z);
        GroundCrack.transform.position = tPosition;
        GroundCrack.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        GroundCrack.SetActive(false);
    }

    private void StartHardAttack()
    {
        Main.Player.OnWarningEvent.Invoke();
    }

    private void OnAttackFalseE()
    {
        if (BT.context._myState == BattleState.Attack_Judge)
        {
            BT.context._myState = BattleState.Attack;
        }
    }

    private void OnAttackLastE()
    {
        if ((BT.context._myState == BattleState.Attack_Judge) || (BT.context._myState == BattleState.Attack))
        {
            BT.context._myState = BattleState.Stop;
        }
    }

    private void OnAttackStartE()
    {
        BT.context._myState = BattleState.Attack;
    }
}
