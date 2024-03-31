using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillar : MonoBehaviour, IWeapon
{
    public int damage;
    public int knockBack;
    private BattleState State = BattleState.Attack;
    public Define.AttackAttribute _attribute;
    public bool isSingle;
    private bool coRoutineRunning;
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }

    public void ChangeJudgeState()
    {
        if (!coRoutineRunning)
        {
            StartCoroutine(coChangeJudgeState());
        }
    }

    private IEnumerator coChangeJudgeState()
    {
        coRoutineRunning = true;
        State = BattleState.Attack;
        yield return new WaitForSeconds(0.4f);
        State = BattleState.Attack_Judge;
        coRoutineRunning = false;
    }

    public void youJustParry()
    {

    }

    public void realHit()
    {

    }

    public Define.AttackAttribute getAttribute()
    {
        return _attribute;
    }


    public (int, int, BattleState) GetOtherInfo()
    {
        return (damage, knockBack, State);
    }

    private void OnEnable()
    {
        coRoutineRunning = false;
        State = BattleState.Attack;
        StartCoroutine(coChangeJudgeInit());
        if(isSingle)
        {
            StartCoroutine(coSelfDestory());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator coChangeJudgeInit()
    {
        yield return new WaitForSeconds(0.5f);
        State = BattleState.Attack_Judge;
    }

    private IEnumerator coSelfDestory()
    {
        yield return new WaitForSeconds(5.0f);
        State = BattleState.Attack;
        yield return new WaitForSeconds(1.0f);
        Main.Resource.Destroy(this.gameObject);
    }

}
