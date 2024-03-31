using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttributeCube : MonoBehaviour
{
    [SerializeField] private Define.AttackAttribute _attribute;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Main.Player.PlayerAttributeChange(_attribute);
    }
}
