using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWeapon : MonoBehaviour, IWeapon
{
    #region Regacy
    /*
    [SerializeField] private Collider myCollider;
    private List<Collider> colliders = new List<Collider>();
    private void OnEnable()
    {
        colliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        colliders.Clear();
        if (other == myCollider) return;
        if (colliders.Contains(other)) return;
        colliders.Add(other);

        if (other.TryGetComponent(out Health health))
        {
            Debug.Log(other.name);
            health.TakeDamage(damage);
        }
        
    }*/

    
    #endregion
    private int damage;
    private int knockBack;
    private BattleState State;
    private Define.AttackAttribute _attribute;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField] private GameObject paricleSystem;
    [SerializeField] private GameObject attributeParticle;
    private ParticleSystem attributeParticleSystem;

    private void Awake()
    {
        playerController.OnStateChangeEvent += StateChangeStarted;
        playerController.OnAttributeChangeEvent += AttributeChangeStarted;
        attributeParticleSystem = attributeParticle.GetComponent<ParticleSystem>();
    }

    private void OnDestroy()
    {
        playerController.OnStateChangeEvent -= StateChangeStarted;
        playerController.OnAttributeChangeEvent -= AttributeChangeStarted;
    }

    public void realHit()
    {

    }
    public Define.AttackAttribute getAttribute()
    {
        return _attribute;
    }
    public void ChangeJudgeState()
    {
        if (playerController._myState == BattleState.Attack_Judge)
        {
            playerController._myState = BattleState.Attack;
        }
    }
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
    #region Information
    //새로운 버전 : weapon이 맞는 상대에서 정보 가져다 주는 방식

    
    public (int, int, BattleState) GetOtherInfo()
    {
        State = playerController._myState;
        return (damage, knockBack, State);
    }

    public void youJustParry()
    {
    }

    private void StateChangeStarted()
    {
        if (Main.Player.PlayerAttribute != Define.AttackAttribute.None) return;
        
        if (playerController._myState == BattleState.Attack || playerController._myState == BattleState.Attack_Judge)
        {
            paricleSystem.SetActive(true);
        }
        else
        {
            paricleSystem.SetActive(false);
        }
    }

    private void AttributeChangeStarted()
    {
        AttributeChange();
        StopAllCoroutines();
        StartCoroutine(WaitAttributeTime());
    }
    
    
    private void AttributeChange()
    {
        var attributeMainModule = attributeParticleSystem.main;
        switch (Main.Player.PlayerAttribute)
        {
            case Define.AttackAttribute.None:
                attributeParticle.SetActive(false);
                break;
            case Define.AttackAttribute.Ice:
                attributeParticle.SetActive(true);
                attributeMainModule.startColor = Color.cyan;
                break;
            case Define.AttackAttribute.Lightning:
                attributeParticle.SetActive(true);
                attributeMainModule.startColor = Color.yellow;
                break;
            case Define.AttackAttribute.Fire:
                attributeParticle.SetActive(true);
                attributeMainModule.startColor = Color.red;
                break;
                
        }
    }

    private IEnumerator WaitAttributeTime()
    {
        yield return new WaitForSeconds(10f);
        Main.Player.PlayerAttribute = Define.AttackAttribute.None;
        AttributeChange();
    }
    
    #endregion
}
