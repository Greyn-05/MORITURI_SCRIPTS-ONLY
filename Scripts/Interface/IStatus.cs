using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus 
{
    #region Fields
    float Value { get; }
    #endregion
    
    
    
    #region Methods

    void SetValue(float amount);
    void AddValue(float amount);
    void SubValue(float amount);
    

    #endregion
}
