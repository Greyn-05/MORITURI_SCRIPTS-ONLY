using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    #region Field

    [SerializeField] private GameObject _RFootStepPrefap;
    [SerializeField] private GameObject _LFootStepPrefap;

    #endregion

    public void RightFootStepEffect()
    {
        _RFootStepPrefap.SetActive(false);
        _RFootStepPrefap.SetActive(true);
    }

    public void LeftFootStepEffect()
    {
        _LFootStepPrefap.SetActive(false);
        _LFootStepPrefap.SetActive(true);
    }
}
