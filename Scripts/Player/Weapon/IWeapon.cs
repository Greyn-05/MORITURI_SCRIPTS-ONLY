using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void SetAttack(int damage);

    public void ChangeJudgeState();

    public void youJustParry();

    public void realHit();

    public Define.AttackAttribute getAttribute();

    public (int, int, BattleState) GetOtherInfo();
}
