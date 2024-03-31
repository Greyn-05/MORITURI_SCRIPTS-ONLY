using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour
{
    public Status_HP _hpModule;
    
    public event Action OnDie;

    public bool IsDead => _hpModule.CurValue == 0;

    private string[] damageSound = new string[] {"SEKIRO_ATTACK_1", "SEKIRO_ATTACK_2", "SEKIRO_ATTACK_3"};
    
    public void Initialize(Status_HP hp)
    {
        _hpModule = hp;
    }

    public void TakeDamage(int damage)
    {
        if (_hpModule.CurValue <= 0) return;
        _hpModule.SubValue(damage);
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(damageSound[Random.Range(0, damageSound.Length)]), transform);
        
        if (_hpModule.CurValue <= 0)
        {
            OnDie?.Invoke();
        }
    }

}
