using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateColorControllerForEnemy : MonoBehaviour
{
    public Material stateColor;
    public EnemyBehaviorTreeRunner BT;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        BattleState T = BT.context._myState;
        switch (T)
        {
            case (BattleState.Stop):
                stateColor.color = Color.black;
                break;
            case (BattleState.Move):
                stateColor.color = Color.grey;
                break;
            case (BattleState.Attack):
                stateColor.color = Color.green;
                break;
            case (BattleState.Attack_Judge):
                stateColor.color = Color.red;
                break;
            case (BattleState.Guard):
                stateColor.color = Color.blue;
                break;
            case (BattleState.Guard_Parry):
                stateColor.color = Color.yellow;
                break;
            case (BattleState.Gethit):
                stateColor.color = Color.magenta;
                break;
            case (BattleState.Down):
                break;
            case (BattleState.UseItem):
                break;
            case (BattleState.Death):
                stateColor.color = Color.white;
                break;
        }

        Color tempColor = new Color(stateColor.color.r, stateColor.color.g, stateColor.color.b, 150.0f/255f);
        stateColor.color = tempColor;


    }
}
