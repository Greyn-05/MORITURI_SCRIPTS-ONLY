using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountStarShield : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Main.Player.PlayerInvincibleTrue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Main.Player.PlayerInvincibleFalse();
        }
    }
}
