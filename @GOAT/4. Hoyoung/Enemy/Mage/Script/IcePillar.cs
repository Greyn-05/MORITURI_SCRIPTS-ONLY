using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePillar : MonoBehaviour, IWeapon
{
    public int damage;
    public int knockBack;
    private BattleState State = BattleState.Attack;
    public Define.AttackAttribute _attribute;
    public ParticleSystem Pillar;
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
        
        State = BattleState.Attack;
        StartCoroutine(coChangeJudge());
        StartCoroutine(coSelfDestory());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator coChangeJudge()
    {
        yield return new WaitForSeconds(1f);
        State = BattleState.Attack_Judge;
        Pillar.Play();
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("IcePillar" + Random.Range(1, 3)), gameObject.transform);
    }

    private IEnumerator coSelfDestory()
    {
        yield return new WaitForSeconds(5f);
        Main.Resource.Destroy(this.gameObject);
    }
}
