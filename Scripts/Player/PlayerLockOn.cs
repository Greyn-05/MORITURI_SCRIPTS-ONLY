using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;


public class PlayerLockOn : MonoBehaviour
{
    private PlayerController _controller;
    private CinemachineStateDrivenCamera _camera;
    private Animator _cinemachineAnimator;

    private float _angleRange = 300f;
    private float _distance = 100f;
    private bool _isLockOned;
    private bool _isLockOnClicked;

    private float _height;
    
    private List<GameObject> targetList = new List<GameObject>();
    
    private Transform _currentTarget;
    private GameObject _lockOnUI;


    public bool IsLockOned => _isLockOned;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        _camera = Main.Cinemachne.StateDrivenCamera;
        _cinemachineAnimator = Main.Cinemachne.CameraAnimator;
        if (!SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town))
        {
            _controller.InputValue.PlayerActions.LockOn.started += OnLockOnStarted;
        }
        var lockOnUI = Resources.Load("Prefabs/UI/LockOnUI") as GameObject;
        _lockOnUI = Instantiate(lockOnUI);
        _lockOnUI.SetActive(false);
        Main.Player.OnResetLockOnEvnet -= ResetTarget;
        Main.Player.OnResetLockOnEvnet += ResetTarget;
    }

    private void OnDestroy()
    {
        _controller.InputValue.PlayerActions.LockOn.started -= OnLockOnStarted;
        Main.Player.OnResetLockOnEvnet -= ResetTarget;
    }

    private void Update()
    {
        if (_controller.Health.IsDead)
        {
            if (_isLockOned) ResetTarget();
            return;
        }
        if (_isLockOnClicked)
        {
            _isLockOnClicked = false;

            if (!_isLockOned && FindTarget())
            {
                LockOn();
                _controller.StateMachine.IsLockOn = true;
                _controller.Animator.SetBool(_controller.AnimationData.LockOnHash, true);
                
                return;
            }
            ResetTarget();
            
            
            Main.Cinemachne.SetCurrentCamera(Define.EStateDrivenCamera.Player);
        }

        if (_isLockOned)
        {
            LockOn_UI();
            var dir = _currentTarget.transform.position;
            dir.y = transform.position.y;
            transform.LookAt(dir);
        }

    }
    
    

    private void OnLockOnStarted(InputAction.CallbackContext obj)
    {
        _isLockOnClicked = true;
    }

    private void ResetTarget()
    {
        targetList.Clear();
        Main.Cinemachne.SetPlayerCamera();
        _lockOnUI.SetActive(false);

        _controller.StateMachine.IsLockOn = false;
        _controller.Animator.SetBool(_controller.AnimationData.LockOnHash, false);
        _isLockOned = false;
    }

    
    
    private bool FindTarget()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position + Vector3.up, _distance, LayerMask.GetMask("Enemy"));
        
        if (objs.Length == 0) return false;
        targetList.Clear();

        for (int i = 0; i < objs.Length; i++)
        {
            float targetDeg = Vector3.Dot(transform.forward,
                (objs[i].transform.position - transform.position).normalized);
            targetDeg = Mathf.Acos(targetDeg) * Mathf.Rad2Deg;
            

            if (targetDeg <= _angleRange/2 && objs[i].gameObject.CompareTag("Enemy"))
            {
                targetList.Add(objs[i].gameObject);
                Debug.DrawLine(transform.position, objs[i].transform.position, Color.red);
            }
        }
        
        if (targetList.Count != 0) return true;
        else return false;
    }
    
    
    
    private Transform FindNearestEnemy(List<GameObject> hits)
    {
        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = hit.transform;
            }
        }

        return nearestEnemy;
    }

    private void LockOn()
    {
        _currentTarget = FindNearestEnemy(targetList);
        _currentTarget.GetComponent<Health>().OnDie += TargetDeath;
        Main.Cinemachne.SetLockOnCamera(_currentTarget);

                
        _lockOnUI.transform.position = _currentTarget.transform.position;
        _lockOnUI.SetActive(true);

        _height = LockOn_UI_Height();
        
        _isLockOned = true;
    }
    
    private float LockOn_UI_Height()
    {
        float height = _currentTarget.GetComponent<CharacterController>().height;
        float positionY = height - (height / 4);
        
        return positionY;
    }
    
    
    private void LockOn_UI()
    {
        _lockOnUI.transform.position = _currentTarget.transform.position + new Vector3(0, _height ,0 );
    }


    private void TargetDeath()
    {
        if (_isLockOned) ResetTarget();
    }
    
}
