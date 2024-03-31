using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private GameObject _healEffectPrefab;

    private void Start()
    {
        Main.Player.OnHeal -= HealEffect;
        Main.Player.OnHeal += HealEffect;
    }

    private void OnDestroy()
    {
        Main.Player.OnHeal -= HealEffect;
    }

    public void HealEffect()
    {
        _healEffectPrefab.gameObject.SetActive(false);
        _healEffectPrefab.gameObject.SetActive(true);
    }
}
