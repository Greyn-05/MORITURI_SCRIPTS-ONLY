using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinemachineManager
{
    #region Field ------------------------------------------------------------------------------------------------------
    private GameObject _cameraObject;
    private Transform _cameraRoot;
    
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineStateDrivenCamera _stateDrivenCamera;
    private List<CinemachineVirtualCamera> _virtualCameraList;
    private CinemachineVirtualCamera _parryAttackCamera;
    private CinemachinePOV _pov;
    private GameObject _targetGroupObject;
    private CinemachineTargetGroup _targetGroup;
    private Animator _cameraAnimator;
    [Range(0f, 2f)] private float  _mouseSpeed = 1f;

    private CoroutineHandler _handler;
    #endregion

    #region Property ---------------------------------------------------------------------------------------------------
    public GameObject CameraObject => _cameraObject;
    
    public CinemachineVirtualCamera CurrentCamera => _currentCamera;
    public CinemachineStateDrivenCamera StateDrivenCamera => _stateDrivenCamera;
    public List<CinemachineVirtualCamera> VirtualCameraList => _virtualCameraList;
    public Animator CameraAnimator => _cameraAnimator;
    #endregion



    public void InstantiateCamera(Transform parent = null)
    {
        _targetGroupObject = Main.Resource.InstantiatePrefab("Target Group");
        _cameraObject = Main.Resource.InstantiatePrefab("State-Driven Camera");
        _virtualCameraList = new List<CinemachineVirtualCamera>();
        var parryAttckCemera = Main.Resource.InstantiatePrefab("ParryAttackCamera",parent);
        _parryAttackCamera = parryAttckCemera.GetComponent<CinemachineVirtualCamera>();
        _parryAttackCamera.Priority = 9;
    } 

    public void SetStateDrivenCamera(Transform cameraRoot)
    {
        _stateDrivenCamera = _cameraObject.GetComponent<CinemachineStateDrivenCamera>();
        _cameraAnimator = _stateDrivenCamera.GetComponent<Animator>();
        _cameraRoot = cameraRoot;
        
        var _stateDrivenCameraList = _stateDrivenCamera.ChildCameras;
        for (int i = 0; i < _stateDrivenCamera.ChildCameras.Length; i++)
        {
            _stateDrivenCameraList[i].Follow = _cameraRoot;
            _stateDrivenCameraList[i].LookAt = _cameraRoot;
            _virtualCameraList.Add(_stateDrivenCameraList[i].GetComponent<CinemachineVirtualCamera>());
        }


    }


    public void MenuOnCamera()
    {
        if (_currentCamera != _virtualCameraList[Define.cameraType[Define.EStateDrivenCamera.Player]]) return;
        _pov = _currentCamera.GetCinemachineComponent<CinemachinePOV>();
        _pov.m_VerticalAxis.m_InputAxisValue = 0;
        _pov.m_HorizontalAxis.m_InputAxisValue = 0;
        _pov.m_VerticalAxis.m_InputAxisName = "";
        _pov.m_HorizontalAxis.m_InputAxisName = "";
    }
    
    public void MenuOffCamera()
    {
        if (_pov == null) return;
        _pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        _pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
    }

    
    #region SetCamera
    public void SetCurrentCamera(Define.EStateDrivenCamera Eindex)
    {
        int index = Define.cameraType[Eindex];
        //_currentCamera = _stateDrivenCameraList[index].GetComponent<CinemachineVirtualCamera>();
        _currentCamera = _virtualCameraList[index];
    }
    
    public void SetPlayerCamera()
    {
        SetCurrentCamera(Define.EStateDrivenCamera.Player);
        _cameraAnimator.Play("PlayerCamera");
        if (!TryGetCinemachineComponent<CinemachinePOV>(_currentCamera,out var pov)) return;
        var rotation = Main.Player.PlayerObject.transform.rotation;
        var vec3 = rotation.eulerAngles;
        MouseSpeed(_mouseSpeed);
        
        if (SceneManager.GetActiveScene().name.Equals(Define.SceneName.Town) 
            && Main.Player.CurrentScene == Define.EPlayerSceneName.Town) return;
        pov.m_HorizontalAxis.Value = vec3.y;
        pov.m_VerticalAxis.Value = 15;
        
        
    }

    public void SetLockOnCamera(Transform target = null)
    {
        SetCurrentCamera(Define.EStateDrivenCamera.LockOn);
        _currentCamera.LookAt = target;
        _cameraAnimator.Play("LockOnCamera");
    }
    
    public void SetNPCCamera(Transform target)
    {
        SetCurrentCamera(Define.EStateDrivenCamera.NPC);
        _currentCamera.Follow = null;

        _targetGroup = _targetGroupObject.GetComponent<CinemachineTargetGroup>();
        _targetGroup.m_Targets[0].target = target;

        var targetRotation = target.transform.rotation;
        Vector3 rotation = targetRotation.eulerAngles;
        float x = rotation.x;
        float y = rotation.y + 180f;
        float z = rotation.z;
        _targetGroupObject.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
        
        _currentCamera.Follow = _targetGroupObject.transform;
        _currentCamera.LookAt = _targetGroupObject.transform;
        

        _cameraAnimator.Play("NPCCamera");
    }

    public void SetParryAttackCamera(Transform target)
    {
        //_parryAttackCamera.LookAt = target;
        _parryAttackCamera.Priority = 11;
    }

    public void ResetParryAttackCamera()
    {
        _parryAttackCamera.Priority = 9;
    }


    public void SetMinimapCamera()
    {
        Main.Resource.InstantiatePrefab("MiniMapCamera");
    }
    
    #endregion


    #region CameraShake

    public bool TryGetCinemachineComponent<T>(CinemachineVirtualCamera vcam, out T component) where T : CinemachineComponentBase
    {
        if (_currentCamera != _virtualCameraList[Define.cameraType[Define.EStateDrivenCamera.Player]])
        {
            component = null;
            return false;
        }

        component = vcam.GetCinemachineComponent<T>(); // 지정된 타입의 컴포넌트를 가져옵니다.
        return component != null; // 컴포넌트가 존재하는지 여부를 반환합니다.
    }
    
    public void ShakeCamera(float amplitudeGain, float time)
    {
        var noise = _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() ?? null;
        if (noise == null) return;
        
        noise.m_AmplitudeGain = amplitudeGain;
        if(_handler != null) _handler.Stop();
        _handler = CoroutineHandler.Start_Coroutine(CoShakeCamera(noise, time));
    }

    private IEnumerator CoShakeCamera(CinemachineBasicMultiChannelPerlin noise, float time)
    {
        yield return new WaitForSeconds(time);
        noise.m_AmplitudeGain = 0;
        _handler.Stop();
    }

    #endregion


    #region MouseSpeed

    public void MouseSpeed(float speed)
    {
        if (!TryGetCinemachineComponent<CinemachinePOV>(_currentCamera, out var pov)) return;

        _mouseSpeed = speed;
        pov.m_VerticalAxis.m_MaxSpeed = _mouseSpeed;
        pov.m_HorizontalAxis.m_MaxSpeed = _mouseSpeed;
    }


    #endregion

}
