using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyControllerGun : MonoBehaviour
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


    public Transform modeling;
    public Health health;
    private EnemyBehaviorTreeRunner enemyBehaviorTree;
    private GameObject targetPlayer;
    private EnemyInfo enemyInfo;
    public bool pistolEnable;
    public bool shotgunEnable;
    public bool rifleEnable;
    public bool EvadeAttackEnable;
    public bool superArmor;
    private float pistolCoolTime = 1.0f;
    private float shotgunCoolTime = 2.5f;
    private float rifleCoolTime = 5.0f;
    private int playerBulletHitCount;
    private Coroutine coSuperArmor;
    private bool superArmorRunning;
    public Action HitEvent;

    //private bool

    private void Awake()
    {
        enemyBehaviorTree = GetComponent<EnemyBehaviorTreeRunner>();
        enemyInfo = GetComponent<EnemyInfo>();
        enemyInfo.Initialize(InitHP, InitHP, InitStamina, InitStamina, InitAtk, InitDef, Name, ID);
        health = GetComponent<Health>();
        health.Initialize(enemyInfo.HP);
        health.OnDie += onDeath;
        pistolEnable = true;
        shotgunEnable = true;
        rifleEnable = true;
        superArmor = false;
        playerBulletHitCount = 0;
        coSuperArmor = null;
        superArmorRunning = false;
    }

    // Update is called once per frame


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
            
            if (OtherState == BattleState.Attack_Judge)
            {
                weapon.ChangeJudgeState();
                
                if (enemyBehaviorTree.context.isInvincible == false)
                {
                    Main.Cinemachne.ShakeCamera(1f, 0.2f);
                    HitEvent.Invoke();
                    health.TakeDamage((int)((float)otherDamage*((100.0f -enemyBehaviorTree.context.DamageReduce)/100.0f)));
                    //Debug.Log("EnemyHit : " + enemyInfo.HP.CurValue + " / " + enemyInfo.HP.MaxValue);
                    //StartCoroutine(enemyBehaviorTree.context.StartIFrame(0.5f));
                    if((!superArmor)&& (enemyBehaviorTree.context._myState != BattleState.Guard_Parry))
                    {
                        enemyBehaviorTree.context.animationController.Play("Hit0" + UnityEngine.Random.Range(1, 3).ToString(), 0, 0);
                        if(!superArmorRunning)
                        {
                            coSuperArmor = StartCoroutine(startSuperArmor());
                        }
                    }
                }
            }

        }
        if ((other.tag == "Bullet") && (other.gameObject.layer == 6))
        {
            other.TryGetComponent(out IWeapon weapon);
            (int otherDamage, int otherKnockBack, BattleState OtherState) = weapon.GetOtherInfo();
            
            if (OtherState == BattleState.Attack_Judge)
            {
                weapon.ChangeJudgeState();
                /*if (enemyBehaviorTree.context._myState == BattleState.Guard_Parry)
                {

                }
                else if (enemyBehaviorTree.context._myState == BattleState.Guard)
                {
                    enemyBehaviorTree.context.audioSource.Play();
                    enemyBehaviorTree.context.animationController.animator.SetTrigger("GetHitGuard");
                    enemyBehaviorTree.context.effectController.PlayGuardSpark(other.transform.position, gameObject.transform.rotation);

                }*/
                if (enemyBehaviorTree.context.isInvincible == false)
                {
                    playerBulletHitCount++;
                    health.TakeDamage(otherDamage);
                    //Debug.Log("EnemyHit : " + enemyInfo.HP.CurValue + " / " + enemyInfo.HP.MaxValue);
                    if (playerBulletHitCount >= 1)
                    {
                        enemyBehaviorTree.context.animationController.Play("Knockdown", 0, 0);
                        shutdownTransition();
                        StartCoroutine(stayKnockDownState());
                        playerBulletHitCount = 0;
                    }
                    else
                    {
                        enemyBehaviorTree.context.animationController.Play("Knockback", 0, 0);
                        shutdownTransition();
                        StartCoroutine(enemyBehaviorTree.context.StartIFrame(0.5f));
                    }
                }
            }

        }
    }

    private void shutdownTransition()
    {
        enemyBehaviorTree.context.animationController.rifleAttack = false;
        enemyBehaviorTree.context.animationController.pistolAttack = false;
        enemyBehaviorTree.context.animationController.shotgunAttack = false;
        enemyBehaviorTree.context.animationController.Skill = false;
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

    public void StartPistolAttackInterval()
    {
        StartCoroutine(startPistolCoolDown());
    }

    public void StartShotgunAttackInterval()
    {
        StartCoroutine(startshotgunCoolDown());
    }

    public void StartRifleAttackInterval()
    {
        StartCoroutine(startRifleCoolDown());
    }

    private IEnumerator startPistolCoolDown()
    {
        pistolEnable = false;
        yield return new WaitForSeconds(pistolCoolTime);
        pistolEnable = true;
    }

    private IEnumerator startshotgunCoolDown()
    {
        shotgunEnable = false;
        yield return new WaitForSeconds(shotgunCoolTime);
        shotgunEnable = true;
    }

    private IEnumerator startRifleCoolDown()
    {
        rifleEnable = false;
        yield return new WaitForSeconds(rifleCoolTime);
        rifleEnable = true;
    }

    private IEnumerator stayKnockDownState()
    {
        enemyBehaviorTree.context._myState = BattleState.Guard_Parry;
        AnimatorStateInfo stateInfo = enemyBehaviorTree.context.animationController.animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(4.0f);
        enemyBehaviorTree.context.animationController.animator.SetTrigger("GetUp");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("stayFocusOn");
        enemyBehaviorTree.context.DamageReduce = 70;

        while (true)
        {
            if(enemyBehaviorTree.context.CheckRunningAnimation("Ground"))
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(startshotgunCoolDown());
        StopCoroutine("stayFocusOn");
        enemyBehaviorTree.context.DamageReduce = 0;
        enemyBehaviorTree.context._myState = BattleState.Stop;
        

    }

    private IEnumerator stayFocusOn()
    {
        while(true)
        {
            enemyBehaviorTree.context.FocusOnTarget(10f);
            yield return null;
        }
    }

    public void SetSmoke()
    {
        StartCoroutine(CoSetSomke());
    }

    private IEnumerator CoSetSomke()
    {
        GameObject smoke = Main.Resource.InstantiatePrefab("Gunsliger_Smoke");

        smoke.transform.position = gameObject.transform.position;
        smoke.SetActive(true);

        yield return new WaitForSeconds(10.0f);

        Destroy(smoke);

    }

    private void onDeath()
    {
        StopAllCoroutines();
        enemyBehaviorTree.context.animationController.Play("Death", 0, 0);


        enemyBehaviorTree.context._myState = BattleState.Death;
        
        //퀘스트
        if (Main.Quest.CurrentMainQuest != null && Main.Quest.CurrentMainQuest.CombatID == enemyInfo.ID)
        {
            Main.Quest.CurrentMainQuest.CurrentCount += 1;
            bool questCompleted = Main.Quest.CheckQuestClear(Main.Quest.CurrentMainQuest, Main.Quest.CurrentMainQuest.CurrentCount);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyBehaviorTree.context.rifleDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyBehaviorTree.context.pistolDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, enemyBehaviorTree.context.shotgunDistance);
    }*/
}
