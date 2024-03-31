using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;



public class PlayerQuestNavigation : MonoBehaviour
{
    [field: SerializeField] private NPCTransform _npcList;
    [field: SerializeField] private GameObject _navigationArrrowObject;
    private Transform _target;
    private int _npcID;

    private int _mainQuestID;

    private bool _isTargetEmpty = true;
    private bool _isEnterNPC;

    private void Awake()
    {
        Main.Player.OnMainQuest -= SetTarget;
        Main.Player.OnMainQuest += SetTarget;

        Main.Player.OnNPCEnter -= SetNPCEnter;
        Main.Player.OnNPCEnter += SetNPCEnter;
        Main.Player.OnNPCExit -= SetNPCExit;
        Main.Player.OnNPCExit += SetNPCExit;
        _npcList = GameObject.Find("NPC").GetComponent<NPCTransform>();
    }

    private void OnDestroy()
    {
        Main.Player.OnMainQuest -= SetTarget;
        Main.Player.OnNPCEnter -= SetNPCEnter;
        Main.Player.OnNPCExit -= SetNPCExit;
    }

    private void Start()
    {
        Main.Player.OnMainQuestStarted(Main.Quest.CurrentMainQuest);
    }

    private void Update()
    {
        //if (_isTargetEmpty) return;
        if (_isEnterNPC)
        {
            transform.position = _target.position + (Vector3.up * 4.5f);
            //return;
        }
        transform.LookAt(_target);
    }

    public void SetTarget(String name)
    {
        if (_npcList == null) return;
        Transform target = _npcList.GetTargetTransform(name);
        _npcID = Define.FindNPCID(name);
        
        if (target == null)
        {
            ResetTarget();
            return;
        }
        _target = target;
        _navigationArrrowObject.SetActive(true);
        _isTargetEmpty = false;
    }

    public void ResetTarget()
    {
        _target = null;
        _navigationArrrowObject.SetActive(false);
        _isTargetEmpty = true;
    }

    private void SetNPCEnter(string npcName)
    {
        if (_npcID == Define.FindNPCID(npcName))
        {
            _isEnterNPC = true;
        }
    }

    private void SetNPCExit(string npcName)
    {
        if (_npcID == Define.FindNPCID(npcName))
        {
            transform.localPosition = Vector3.zero + (new Vector3(0, 0.5f, 0));
            _isEnterNPC = false;
        }
    }
}
