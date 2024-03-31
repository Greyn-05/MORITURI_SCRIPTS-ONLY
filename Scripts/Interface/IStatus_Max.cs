using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus_Max 
{
    float CurValue { get; }
    float MaxValue { get; }


    void SetValue(float amount);
    void AddValue(float amount);
    void SubValue(float amount);

    void SetMaxValue(float amount);
    void AddMaxValue(float amount);
    void SubMaxValue(float amount);

    float GetPercentage();
}
