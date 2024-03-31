using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCountEffect : MonoBehaviour
{
    [SerializeField] private GameObject _parryEffectPrefab;
    
    private void Awake()
    {
        Main.Player.OnParryCountEvent -= ParryEffect;
        Main.Player.OnParryCountEvent += ParryEffect;
    }

    private void OnDestroy()
    {
        Main.Player.OnParryCountEvent -= ParryEffect;
    }

    public void ParryEffect()
    {
        if (Main.Player.ParryCount == 2) _parryEffectPrefab.SetActive(true);
        else _parryEffectPrefab.SetActive(false);
    }
}
