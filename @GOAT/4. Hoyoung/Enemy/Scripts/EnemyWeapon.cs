using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour, IWeapon
{
    public int damage = 10;
    public int knockBack = 3;
    private BattleState State;
    private Define.AttackAttribute _attribute;

    [SerializeField]
    private EnemyBehaviorTreeRunner BT;
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }

    public void realHit()
    {

    }

    public Define.AttackAttribute getAttribute()
    {
        return _attribute;
    }
    #region Information
    //새로운 버전 : weapon이 맞는 상대에서 정보 가져다 주는 방식

    public void ChangeJudgeState()
    {
        if(BT.context._myState==BattleState.Attack_Judge)
        {
            BT.context._myState = BattleState.Attack;
        }
    }


    public (int, int, BattleState) GetOtherInfo()
    {
        State = BT.context._myState;
        return (damage, knockBack, State);
    }

    public void youJustParry()
    {
        //BT.context._myState = BattleState.Guard_Parry;
        BT.ParryKnockBack();
    }
    #endregion
}
