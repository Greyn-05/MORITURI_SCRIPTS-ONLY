using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTransform : MonoBehaviour
{
    public Transform NPC_750_Alaric;
    public Transform NPC_751_Fierce;
    public Transform NPC_752_Cedric;
    public Transform NPC_753_Daria;
    public Transform NPC_754_Elric;
    public Transform NPC_755_Fendrel;
    public Transform NPC_756_Garvriel;
    public Transform NPC_757_Hilda;
    public Transform NPC_758_Ivor;
    public Transform NPC_759_Jorund;
    public Transform NPC_760_Kaelin;
    public Transform NPC_761_Stout;
    public Transform NPC_762_Nyx;
    public Transform NPC_763_Orla;
    public Transform NPC_764_Pyrrhus;
    
    public Transform Shop;
    public Transform Dungeon;


    public Transform GetTargetTransform(string name)
    {
        switch (name)
        {
            case "Alaric":
                return NPC_750_Alaric;
            case "Fierce":
                return NPC_751_Fierce;
            case "Cedric":
                return NPC_752_Cedric;
            case "Daria":
                return NPC_753_Daria;
            case "Elric":
                return NPC_754_Elric;
            case "Fendrel":
                return NPC_755_Fendrel;
            case "Garvriel":
                return NPC_756_Garvriel;
            case "Hilda":
                return NPC_757_Hilda;
            case "Ivor":
                return NPC_758_Ivor;
            case "Jorund":
                return NPC_759_Jorund;
            case "Kaelin":
                return NPC_760_Kaelin;
            case "Stout":
                return NPC_761_Stout;
            case "Nyx":
                return NPC_762_Nyx;
            case "Orla":
                return NPC_763_Orla;
            case "Pyrrhus":
                return NPC_764_Pyrrhus;
            case "Shop":
                return Shop;
            case "Dungeon":
                return Dungeon;
            default:
                return null;
        }
    }
}
