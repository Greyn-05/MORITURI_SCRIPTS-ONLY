using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BattleLogicController : MonoBehaviour
{
    #region Componets
    #endregion

    #region Parameters
    public abstract BattleState _myState
    {
        get;
        set;
    }
    #endregion

    public void getHIt(Health _myhealth, int _hitDamage)
    {
        _myhealth.TakeDamage(_hitDamage);
    }

    public void strike()
    {

    }

    public void guard()
    {

    }

    public void parry()
    {

    }
}
