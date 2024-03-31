using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    #region Field
    public Status_HP HP { get; private set; }
    public Status_Stamina Stamina { get; private set; }
    public Status_Atk Atk { get; private set; }
    public Status_Def Def { get; private set; }

    public string Name { get; private set; }

    public int ID { get; private set; }

    #endregion

    public void Initialize(float hp, float maxHP, float stamina, float maxStamina, float atk, float def, string name, int id)
    {
        HP = new Status_HP(hp, maxHP);
        Stamina = new Status_Stamina(stamina, maxStamina);

        Atk = new Status_Atk(atk);
        Def = new Status_Def(def);
        Name = name;
        ID = id;
    }
}
