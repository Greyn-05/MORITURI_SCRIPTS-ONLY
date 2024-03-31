using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus_Max : IStatus_Max
{
    protected float _curValue;
    protected float _maxValue;

    public float CurValue => _curValue;
    public float MaxValue => _maxValue;
    
    #region Init -------------------------------------------------------------------------------------------------------
    protected BaseStatus_Max(float setValue)
    {
        _maxValue = setValue;
        _curValue = _maxValue;
    }

    protected BaseStatus_Max(float curValue, float maxValue)
    {
        _curValue = curValue;
        _maxValue = maxValue;
    }
    #endregion
    
    
    #region Method -----------------------------------------------------------------------------------------------------
    public void SetValue(float amount)
    {
        if (!CheckNegativeNumber(amount)) return;
        PerformSetting(amount);
    }
    public void AddValue(float amount)
    {
        if (!CheckNegativeNumber(amount)) return;
        PerformAddition(amount);
    }

    public void SubValue(float amount)
    {
        if (!CheckNegativeNumber(amount)) return;
        PerformSubtraction(amount);
    }

    public void SetMaxValue(float amount)
    {
        if (!CheckNegativeNumber(amount)) return;
        PerformMaxSetting(amount);
    }

    public void AddMaxValue(float amount)
    {
        if (!CheckNegativeNumber(amount)) return;
        PerformMaxAddition(amount);
    }

    public void SubMaxValue(float amount)
    {
        if (!CheckNegativeNumber(amount)) return;
        PerformMaxSubtraction(amount);
    }

    public float GetPercentage() => MaxValue > 0 ? CurValue / MaxValue : 0f;
    #endregion


    #region Abstract ---------------------------------------------------------------------------------------------------
    protected abstract void PerformSetting(float amount);
    protected abstract void PerformAddition(float amount);
    protected abstract void PerformSubtraction(float amount);
    protected abstract void PerformMaxSetting(float amount);
    protected abstract void PerformMaxAddition(float amount);
    protected abstract void PerformMaxSubtraction(float amount);
    #endregion


    #region SubMethod

    public void AddPercentageValue(float percent)
    {
        AddValue(GetPercentageAmount(CurValue, percent));
    }

    public void SubPercentageValue(float percent)
    {
        SubValue(GetPercentageAmount(CurValue, percent));
    }

    public void AddPercentageMaxValue(float percent)
    {
        AddMaxValue(GetPercentageAmount(CurValue, percent));
    }

    public void SubPercentageMaxValue(float percent)
    {
        SubMaxValue(GetPercentageAmount(CurValue, percent));
    }

    
    
    private bool CheckNegativeNumber(float amount)
    {
        return !(amount < 0);
    }
    private float GetPercentageAmount(float value, float percent)
    {
        return value * (percent / 100f);
    }

    protected void ValueChangedHandle(float newCurValue, float newMaxValue)
    {
        bool isCurValueChanged = !Mathf.Approximately(_curValue, newCurValue);
        bool isMaxValueChanged = !Mathf.Approximately(_maxValue, newMaxValue);

        if (!isCurValueChanged && !isMaxValueChanged) return;

        _curValue = newCurValue;
        _maxValue = newMaxValue;
        
    }


    #endregion
}
