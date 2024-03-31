using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float InitHP;

    [SerializeField]
    private float InitStamina;

    [SerializeField]
    private float InitAtk;

    [SerializeField]
    private float InitDef;

    [SerializeField]
    private string Name;

    [SerializeField]
    private int ID;

    public float damageReduce;


    public Action HitEvent;
    public Health health;
    public Blood blood;
    private EnemyBehaviorTreeRunner enemyBehaviorTree;
    private GameObject targetPlayer;
    private EnemyInfo enemyInfo;
    private GameObject TAura;
    
    private void Awake()
    {
        enemyBehaviorTree = GetComponent<EnemyBehaviorTreeRunner>();
        enemyInfo = GetComponent<EnemyInfo>();
        enemyInfo.Initialize(InitHP, InitHP, InitStamina, InitStamina, InitAtk, InitDef, Name, ID);
        health = GetComponent<Health>();
        health.Initialize(enemyInfo.HP);
        health.OnDie += onDeath;
    }

    // Update is called once per frame


    private void OnTriggerStay(Collider other)
    {
        if (other == enemyBehaviorTree.context.characterController)
        {
            return;
        }

        if ((other.tag == "Weapon") && (other.gameObject.layer != 7))
        {
            other.TryGetComponent(out IWeapon weapon);
            (int otherDamage, int otherKnockBack, BattleState OtherState) = weapon.GetOtherInfo();
            //너 hit판정이니?
            //내가 가드중인가?
            //근데 0.2초안에 가드한건가?
            //다 아니야? 그럼 맞아
            if (OtherState == BattleState.Attack_Judge)
            {
                weapon.ChangeJudgeState();
                if (enemyBehaviorTree.context._myState == BattleState.Guard_Parry)
                {

                }
                else if (enemyBehaviorTree.context._myState == BattleState.Guard)
                {
                    enemyBehaviorTree.context.animationController.animator.SetTrigger("GetHitGuard");
                    //enemyBehaviorTree.context.effectController.PlayGuardSpark();
                    
                }
                else if(enemyBehaviorTree.context.isInvincible == false)
                {
                    //enemyBehaviorTree.context.animationController.animator.SetTrigger("GetParry");
                    Main.Cinemachne.ShakeCamera(1f, 0.2f);
                    HitEvent.Invoke();
                    health.TakeDamage((int)((float)otherDamage * ((100.0f - enemyBehaviorTree.context.DamageReduce) / 100.0f)));
                    blood.BloodEffect();
                    //StartCoroutine(enemyBehaviorTree.context.StartIFrame(0.5f));
                    //여기 까지 와야 피격 모션이 나오는 거라서
                    //여기서 피격 무적 코루틴 도는데 어자피 무적인 동안은 여기 못들어옴
                    //Debug.Log("EnemyHit : " + enemyInfo.HP.CurValue + " / " + enemyInfo.HP.MaxValue);
                    
                }
            }

        }
    }

    public void StartCoolDown()
    {
        StartCoroutine(enemyBehaviorTree.context.startCoolDown());
    }

    public void StartStepCoolDown()
    {
        StartCoroutine(enemyBehaviorTree.context.startStepCoolDown());
    }

    public void StartTwoHandAxePhase3()
    {
        StartCoroutine(TwoHandAxePhase3());
    }


    public void TurnOnAura()
    {
        TAura = Main.Resource.InstantiatePrefab("2HandAxePhase3Effect", this.gameObject.transform);
        //TAura.transform.position = new Vector3(0, 0.7f, 0);
    }

    public IEnumerator TwoHandAxePhase3()
    {
        while (true)
        {
            if(health._hpModule.CurValue > 3f)
            {
                health.TakeDamage(1);
            }
            yield return new WaitForSeconds(3.0f);
        }
    }




    private void onDeath()
    {
        StopAllCoroutines();
        Destroy(TAura);
        enemyBehaviorTree.context.animationController.Play("Death", 0, 0);
        enemyBehaviorTree.context._myState = BattleState.Death;

        //퀘스트
        if (Main.Quest.CurrentMainQuest != null && Main.Quest.CurrentMainQuest.CombatID == enemyInfo.ID)
        {
            Main.Quest.CurrentMainQuest.CurrentCount += 1;
            bool questCompleted = Main.Quest.CheckQuestClear(Main.Quest.CurrentMainQuest, Main.Quest.CurrentMainQuest.CurrentCount);
        }
    }
}
