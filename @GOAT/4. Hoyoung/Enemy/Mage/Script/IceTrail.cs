using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrail : MonoBehaviour, IWeapon
{
    public int damage;
    public int knockBack;
    private BattleState State = BattleState.Attack_Judge;
    public Define.AttackAttribute _attribute;
    public GameObject CubeCollider;
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
        CubeCollider.transform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        CubeCollider.transform.transform.Translate(new Vector3(0, 0, 1) * 120 * Time.deltaTime);
    }
   

}
