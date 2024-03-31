using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _walkSound;
    private AudioSource _audioSource;

    private PlayerController _controller;
    private FSM_Player _stateMachine;
    private ForceReciever _forceReciever;

    private bool _isStop;
    private bool _isWalk;
    private bool _isRun;
    private int _walkType;

    private void Start()
    {
        _controller = Main.Player.Controller;
        _stateMachine = _controller.StateMachine;
        _audioSource = GetComponent<AudioSource>();
        _forceReciever = _controller.ForceReceiver;
    }

    private void Update()
    {
        if(Time.timeScale < 1 || _stateMachine.IsForce || _stateMachine.IsGuard)
        {
            if(!_isStop)StopSound();
            return;
        }
        switch (WalkType())
        {
            case 0: // 정지
                if (!_isStop) StopSound();
                break;
            case 1 : // 걷기
                if(!_isWalk) WalkSound();
                break;
            case 2 : // 달리기
                if (!_isRun) RunSound();
                break;
        }
        
    }

    private bool CompareVector3(Vector3 a)
    {
        bool x = a.x > -0.5f && a.x < 0.5f;
        bool y = a.y > -0.5f && a.y < 0.5f;
        bool z = a.z > -0.5f && a.z < 0.5f;
        if (x && y && z) return true;
        return false;
    }
    

    private int WalkType()
    {
        if (_stateMachine.IsStoped) return 0; // 정지
        if (!_stateMachine.IsRuned) return 1; // 걷기
        return 2;                             // 달리기
    }

    private void StopSound()
    {
        _isStop = true;
        _isWalk = false;
        _isRun = false;
        _audioSource.pitch = 0;
        _audioSource.Stop();
    }

    private void WalkSound()
    {
        _isStop = false;
        _isWalk = true;
        _isRun = false;
        _audioSource.pitch = 0.75f;
        if(!_audioSource.isPlaying) _audioSource.Play();
    }

    private void RunSound()
    {
        _isStop = false;
        _isWalk = false;
        _isRun = true;
        _audioSource.pitch = 1.3f;
        if(!_audioSource.isPlaying) _audioSource.Play();
    }
}