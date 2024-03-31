using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator animator;
    private EnemyBehaviorTreeRunner BT;

    private void Awake()
    {
        BT = GetComponent<EnemyBehaviorTreeRunner>();
    }

    public float move_x
    {
        set => animator.SetFloat("move_x", value);
        get => animator.GetFloat("move_x");
    }

    public float move_z
    {
        set => animator.SetFloat("move_z", value);
        get => animator.GetFloat("move_z");
    }

    public float AttackSpeed
    {
        set => animator.SetFloat("AttackSpeed", value);
        get => animator.GetFloat("AttackSpeed");
    }

    public bool guard
    {
        set => animator.SetBool("guard", value);
        get => animator.GetBool("guard");
    }

    public bool stopGuard
    {
        set => animator.SetBool("stopGuard", value);
        get => animator.GetBool("stopGuard");
    }

    public bool Run
    {
        set => animator.SetBool("Run", value);
        get => animator.GetBool("Run");
    }

    public bool Attack
    {
        set => animator.SetBool("Attack", value);
        get => animator.GetBool("Attack");
    }

    public bool SAttack
    {
        set => animator.SetBool("SAttack", value);
        get => animator.GetBool("SAttack");
    }

    public bool pistolAttack
    {
        set => animator.SetBool("pistolAttack", value);
        get => animator.GetBool("pistolAttack");
    }

    public bool shotgunAttack
    {
        set => animator.SetBool("shotgunAttack", value);
        get => animator.GetBool("shotgunAttack");
    }

    public bool rifleAttack
    {
        set => animator.SetBool("rifleAttack", value);
        get => animator.GetBool("rifleAttack");
    }

    public int ComboNum
    {
        set => animator.SetInteger("ComboNum", value);
        get => animator.GetInteger("ComboNum");
    }

    public int SkillIndex
    {
        set => animator.SetInteger("SkillIndex", value);
        get => animator.GetInteger("SkillIndex");
    }

    public bool Spawn
    {
        set => animator.SetBool("Spawn", value);
        get => animator.GetBool("Spawn");
    }

   

    public bool Walk
    {
        set => animator.SetBool("Walk", value);
        get => animator.GetBool("Walk");
    }

    public bool GuardOver
    {
        set => animator.SetBool("GuardOver", value);
        get => animator.GetBool("GuardOver");
    }

    public bool Skill
    {
        set => animator.SetBool("Skill", value);
        get => animator.GetBool("Skill");
    }


    public void Play(string stateName, int layer, float normalixedTime)
    {
        if(BT.context._myState != BattleState.Death)
        {
            animator.Play(stateName, layer, normalixedTime);
        }
    }
}
