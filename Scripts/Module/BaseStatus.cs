using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus : IStatus
{
    protected float _value;

    public float Value => _value;

    
    #region Init --------------------------------------------------------------------------------------------------
    protected BaseStatus(float setValue)
    {
        _value = setValue;
    }
    #endregion

    
    #region Interface Method ---------------------------------------------------------------------------------------


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
    #endregion

    
    #region Abstract ----------------------------------------------------------------------------------------------
    protected abstract void PerformSetting(float amount);
    protected abstract void PerformAddition(float amount);
    protected abstract void PerformSubtraction(float amount);
    #endregion



    #region SubMethod ---------------------------------------------------------------------------------------------
    public void AddPercentageValue(float percentage)
    {
        var addAmount = Value * (percentage / 100f);

        AddValue(addAmount);
    }

    public void SubPercentageValue(float percentage)
    {
        var subAmount = Value * (percentage / 100f);

        SubValue(subAmount);
    }
    private bool CheckNegativeNumber(float value)
    {
        return !(value < 0);
    }
    
    protected void ValueChangedHandle(float newValue)
    {
        if (Mathf.Approximately(_value, newValue)) return;

        _value = newValue;
    }

    #endregion
    
    
}
