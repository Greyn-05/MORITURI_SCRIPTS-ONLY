using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    #region Field
    public Status_HP HP { get; private set; }
    public Status_Stamina Stamina { get; private set; }
    public Status_Exp Exp { get; private set; }
    public Status_Atk Atk { get; private set; }
    public Status_Def Def { get; private set; }
    public Status_Speed Speed { get; private set; }

    #endregion


    #region Init
    public PlayerStatus()
    {
        Initialize();
    }

    public PlayerStatus(float hp, float maxHP, float stamina, float maxStamina, float exp, float maxExp, 
        float atk, float def, float speed)
    {
        Initialize(hp, maxHP, stamina, maxStamina, exp, maxExp, atk, def, speed);
    }

    #endregion


    #region Initialize_Method

    private void Initialize()
    {
        HP = new Status_HP(100f, 100f);        
        Stamina = new Status_Stamina(100f, 100f);
        Exp = new Status_Exp(0f, 100f);
        
        Atk = new Status_Atk(5f);
        Def = new Status_Def(5f);
        Speed = new Status_Speed(8.5f);
    }


    private void Initialize(float hp, float maxHP, float stamina, float maxStamina, float exp, float maxExp, 
        float atk, float def, float speed)
    {
        HP = new Status_HP(hp, maxHP);
        Stamina = new Status_Stamina(stamina, maxStamina);
        Exp = new Status_Exp(exp, maxExp);

        Atk = new Status_Atk(atk);
        Def = new Status_Def(def);
        Speed = new Status_Speed(speed);
    }
    

    #endregion
    
}
