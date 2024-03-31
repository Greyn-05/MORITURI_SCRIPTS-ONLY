using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Stamina : BaseStatus_Max
{
    #region Init
    public Status_Stamina(float setValue) : base(setValue) { }
    public Status_Stamina(float curValue, float maxValue) : base(curValue, maxValue) { }

    #endregion


    #region Override
    protected override void PerformSetting(float amount)
    {
        amount = Mathf.Clamp(amount, 0f, _maxValue);
        ValueChangedHandle(amount, _maxValue);
    }

    protected override void PerformAddition(float amount)
    {
        var newCurValue = Mathf.Min(_curValue + amount, _maxValue);
        ValueChangedHandle(newCurValue, _maxValue);
    }

    protected override void PerformSubtraction(float amount)
    {
        var newCurValue = Mathf.Max(_curValue - amount, 0f);
        ValueChangedHandle(newCurValue, _maxValue);
    }

    protected override void PerformMaxSetting(float amount)
    {
        amount = Mathf.Clamp(amount, _maxValue, 999f);
        ValueChangedHandle(_curValue, amount);
    }

    protected override void PerformMaxAddition(float amount)
    {
        var newMaxValue = Mathf.Min(_maxValue + amount, 999f);
        ValueChangedHandle(_curValue, newMaxValue);
    }

    protected override void PerformMaxSubtraction(float amount)
    {
        var newMaxValue = Mathf.Max(_maxValue - amount, 1f);
        ValueChangedHandle(_curValue, newMaxValue);
    }
    #endregion
}
