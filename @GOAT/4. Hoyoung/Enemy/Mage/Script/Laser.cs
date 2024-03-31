using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IWeapon
{
    public int damage;
    public int knockBack;
    private BattleState State = BattleState.Attack_Judge;
    public Define.AttackAttribute _attribute;
    private bool coRoutineRunning;
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }

    public void ChangeJudgeState()
    {
        if(!coRoutineRunning)
        {
            StartCoroutine(coChangeJudgeState());
        }
    }
    private IEnumerator coChangeJudgeState()
    {
        coRoutineRunning = true;
        State = BattleState.Attack;
        yield return new WaitForSeconds(0.12f);
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
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("LaserClip"), gameObject.transform);
        State = BattleState.Attack_Judge;
        StartCoroutine(coSelfDestory());
        coRoutineRunning = false;
    }

    /*private void OnDisable()
    {
        StopAllCoroutines();
    }*/

    private IEnumerator coSelfDestory()
    {
        yield return new WaitForSeconds(3.5f);
        Main.Resource.Destroy(this.gameObject);
    }


}
