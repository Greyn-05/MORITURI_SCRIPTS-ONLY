using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour, IWeapon
{
    public int damage;
    public int knockBack;
    private BattleState State = BattleState.Attack_Judge;
    public Define.AttackAttribute _attribute;
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }

    public void ChangeJudgeState()
    {
        State = BattleState.Attack;
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
        State = BattleState.Attack_Judge;
        StartCoroutine(coChangeJudge());
    }

    private IEnumerator coChangeJudge()
    {
        yield return new WaitForSeconds(0.2f);
        State = BattleState.Attack;
    }

}
