using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationEvent : MonoBehaviour
{
    [field: SerializeField] private PlayerController _controller;
    public bool _isGuardOn;

    private string[] _attackSound;
    private AudioClip[] _attackSoundClip;
    private int _attackSoundIndex;
    
    
    private void Awake()
    {
        _controller = transform.root.GetComponent<PlayerController>();
        _attackSound = new[]
        {
            "Nioh2_sword_1", "Nioh2_sword_2", "Nioh2_sword_3", "Nioh2_sword_4", "Nioh2_sword_5", "Nioh2_sword_6", 
            "Nioh2_sword_7", "Nioh2_sword_8", "Nioh2_sword_9", "Nioh2_sword_10", "Nioh2_sword_11", "Nioh2_sword_12", "Nioh2_sword_13"
        };

        _attackSoundClip = new AudioClip[_attackSound.Length];
        for (int i = 0; i < _attackSound.Length; i++)
        {
            _attackSoundClip[i] = Main.Resource.Load<AudioClip>(_attackSound[i]);
        }
    }
    private void Start()
    {
        _controller.InputValue.PlayerActions.Parrying.performed -= OnGuardPerformed;
        _controller.InputValue.PlayerActions.Parrying.canceled -= OnGuardCanceled;
        _controller.InputValue.PlayerActions.Parrying.performed += OnGuardPerformed;
        _controller.InputValue.PlayerActions.Parrying.canceled += OnGuardCanceled;
    }


    private void OnAttackTrue()
    {
        if (_controller._myState == BattleState.Attack)
        {
            _controller.SetBattleState(BattleState.Attack_Judge);
            //Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("Swing by XxChr0nosxX Id-268227"), transform.root);
            Main.Audio.SfxPlay(_attackSoundClip[_attackSoundIndex++], _controller.transform);
            if (_attackSoundIndex >= _attackSound.Length) _attackSoundIndex = 0;
        }

    }

    private void OnAttackFalse()
    {
        if (_controller._myState == BattleState.Attack_Judge)
        {
            _controller.SetBattleState(BattleState.Attack);
        }

    }

    private void OnParryingTrueP()
    {
        if (_isGuardOn && _controller._myState == BattleState.Guard)
        {
            _controller.SetBattleState(BattleState.Guard_Parry);
        }
    }

    private void OnParryingFalseP()
    {
        if (_controller._myState == BattleState.Guard_Parry)
        {
            _controller.SetBattleState(BattleState.Guard);
        }
    }

    private void OnItemUse()
    {
        _controller.StateMachine.OnItemUseEvent?.Invoke();
    }
    
    
    
    
    private void OnGuardPerformed(InputAction.CallbackContext context)
    {
        _isGuardOn = true;
    }

    private void OnGuardCanceled(InputAction.CallbackContext context)
    {
        _isGuardOn = false;
    }
    
}
