using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class EnemyControllerMage : MonoBehaviour
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


    public Define.AttackAttribute cAttribute;
    public Transform modeling;
    public Health health;
    private EnemyBehaviorTreeRunner enemyBehaviorTree;
    private GameObject targetPlayer;
    private EnemyInfo enemyInfo;
    public bool BasicMagicEnable;
    public bool InterMagicEnable;
    public bool HighMagicEnable;
    public bool superArmor;
    private float BasicCoolTime = 1.0f;
    private float InterCoolTime = 4.5f;
    private float HighCoolTime = 6.0f;
    private Coroutine coSuperArmor;
    private bool superArmorRunning;
    public bool PhaseTrigger;
    public Action HitEvent;

    private void Awake()
    {
        enemyBehaviorTree = GetComponent<EnemyBehaviorTreeRunner>();
        enemyInfo = GetComponent<EnemyInfo>();
        enemyInfo.Initialize(InitHP, InitHP, InitStamina, InitStamina, InitAtk, InitDef, Name, ID);
        health = GetComponent<Health>();
        health.Initialize(enemyInfo.HP);
        health.OnDie += onDeath;
        BasicMagicEnable = true;
        InterMagicEnable = true;
        HighMagicEnable = true;
        superArmor = false;
        coSuperArmor = null;
        superArmorRunning = false;
        //cAttribute = (Define.AttackAttribute)Random.Range(1, 4);
        cAttribute = (Define.AttackAttribute)1;
        PhaseTrigger = false;
        StartPhaseTime();
    }


    private void OnTriggerStay(Collider other)
    {
        /*if (other == enemyBehaviorTree.context.characterController)
        {
            return;
        }*/

        if ((other.tag == "Weapon") && (other.gameObject.layer == 6))
        {
            other.TryGetComponent(out IWeapon weapon);
            (int otherDamage, int otherKnockBack, BattleState OtherState) = weapon.GetOtherInfo();
            Define.AttackAttribute Wattribute = weapon.getAttribute();

            if (OtherState == BattleState.Attack_Judge)
            {
                weapon.ChangeJudgeState();

                if (enemyBehaviorTree.context.isInvincible == false)
                {
                    Main.Cinemachne.ShakeCamera(1f, 0.2f);
                    HitEvent.Invoke();
                    if (Wattribute == Define.AttackAttribute.None)
                    {
                        health.TakeDamage((int)((float)otherDamage * ((100.0f - enemyBehaviorTree.context.DamageReduce) / 100.0f)));
                    }
                    else
                    {
                        health.TakeDamage((int)((float)otherDamage*(1.5f) * ((100.0f - enemyBehaviorTree.context.DamageReduce) / 100.0f)));
                    }
                    
                    //Debug.Log("EnemyHit : " + enemyInfo.HP.CurValue + " / " + enemyInfo.HP.MaxValue);
                    //StartCoroutine(enemyBehaviorTree.context.StartIFrame(0.5f));
                    if ((!superArmor) && (enemyBehaviorTree.context._myState != BattleState.Guard_Parry))
                    {
                        //enemyBehaviorTree.context.animationController.Play("Hit0" + Random.Range(1, 3).ToString(), 0, 0);
                        if (!superArmorRunning)
                        {
                            coSuperArmor = StartCoroutine(startSuperArmor());
                        }
                    }
                }
            }

        }
        
    }

    

    private IEnumerator startSuperArmor()
    {
        superArmorRunning = true;
        yield return new WaitForSeconds(2.0f);
        //Debug.Log("SuperArmor Start");
        superArmor = true;

        yield return new WaitForSeconds(3.0f);
        //Debug.Log("SuperArmor End");
        superArmor = false;
        superArmorRunning = false;
    }

    public void StartCoolDown()
    {
        StartCoroutine(enemyBehaviorTree.context.startCoolDown());
    }

    public void StartPhaseTime()
    {
        StartCoroutine("coStartPhaseTime");
    }

    public void StopPhaseTime()
    {
        StopCoroutine("coStartPhaseTime");
    }

    private IEnumerator coStartPhaseTime()
    {
        PhaseTrigger = false;
        yield return new WaitForSeconds(45.0f);
        Debug.Log("Phase Time Out");
        PhaseTrigger = true;
    }


    public void StartBasicCoolDown()
    {
        StartCoroutine(coStartBasicCoolDown());
    }

    private IEnumerator coStartBasicCoolDown()
    {
        BasicMagicEnable = false;
        yield return new WaitForSeconds(BasicCoolTime);
        BasicMagicEnable = true;
    }

    public void StartInterCoolDown()
    {
        StartCoroutine(coStartInterCoolDown());
    }

    private IEnumerator coStartInterCoolDown()
    {
        InterMagicEnable = false;
        yield return new WaitForSeconds(InterCoolTime);
        InterMagicEnable = true;
    }

    public void StartHighCoolDown()
    {
        StartCoroutine(coStartHighCoolDown());
    }

    private IEnumerator coStartHighCoolDown()
    {
        HighMagicEnable = false;
        yield return new WaitForSeconds(HighCoolTime);
        HighMagicEnable = true;
    }


    private IEnumerator stayFocusOn()
    {
        while (true)
        {
            enemyBehaviorTree.context.FocusOnTarget(10f);
            yield return null;
        }
    }

    
    private void onDeath()
    {
        StopAllCoroutines();
        enemyBehaviorTree.context.animationController.Play("Death", 0, 0);

        enemyBehaviorTree.context._myState = BattleState.Death;
        //퀘스트
        /*foreach (var quest in Main.Quest.ActiveSubQuests.Where(q => q.QuestType == Define.QuestType.Sub && q.CombatID == enemyInfo.ID))
        {
            bool questCompleted = Main.Quest.CheckQuestClear(quest.QuestID);
        }

        if (Main.Quest.CurrentMainQuest != null && Main.Quest.CurrentMainQuest.CombatID == enemyInfo.ID)
        {
            bool questCompleted = Main.Quest.CheckQuestClear(Main.Quest.CurrentMainQuest.QuestID);
        }*/
    }

}
