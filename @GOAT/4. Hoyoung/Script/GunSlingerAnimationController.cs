using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSlingerAnimationController : MonoBehaviour
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

    public void Play(string stateName, int layer, float normalixedTime)
    {
        animator.Play(stateName, layer, normalixedTime);
    }
}
