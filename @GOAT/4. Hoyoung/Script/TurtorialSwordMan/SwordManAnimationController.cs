using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManAnimationController : MonoBehaviour
{
    public Animator animator;

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

    public int ComboNum
    {
        set => animator.SetInteger("ComboNum", value);
        get => animator.GetInteger("ComboNum");
    }

    public void Play(string stateName, int layer, float normalixedTime)
    {
        animator.Play(stateName, layer, normalixedTime);
    }
}
