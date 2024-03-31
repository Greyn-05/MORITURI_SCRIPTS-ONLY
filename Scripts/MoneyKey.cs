using UnityEngine;

public class MoneyKey : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Main.Player.playerData.gold += 100000;
        }
    }
}
