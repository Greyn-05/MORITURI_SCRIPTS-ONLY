using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Unity.VisualScripting;

public class EnemyBehaviorTreeRunner : MonoBehaviour
{
    // The main behaviour tree asset
    public BehaviourTree tree;

    // Storage container object to hold game object subsystems
    [HideInInspector]
    public Context context;

    public bool BHStopTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitBHtree()
    {
        context = CreateBehaviourTreeContext();
        tree = tree.Clone();
        tree.Bind(context);
        BHStopTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((tree)&&(context._myState!=BattleState.Death) && (context._myState != BattleState.Guard_Parry) && (context._myState != BattleState.Gethit) && (BHStopTrigger != true))
        {
            tree.Update();
        }
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }
    public void StopEnemyWhenPlayerDead()
    {
        context.animationController.animator.Play("EndBattle", 0, 0);
        BHStopTrigger = true;
    }

    public void ParryKnockBack()
    {
        StartCoroutine(CoParryKnockBack());
        if(context.SkillEnable)
        {
            context.enemyController.StartCoolDown();
        }
    }

    private IEnumerator CoParryKnockBack()
    {
        context.animationController.GuardOver = false;
        context._myState = BattleState.Guard_Parry;
        context.animationController.Play("GuardHit", 0, 0);
        yield return new WaitForSeconds(0.15f);
        context.health.TakeDamage(20 * (int)((100.0f - context.DamageReduce) / 100.0f));
        StartCoroutine("CoParryKnockBackMove");

        yield return new WaitForSeconds(1f);

        
        context.animationController.GuardOver = true;
        if (context._myState != BattleState.Death) { context._myState = BattleState.Stop; }
        StopCoroutine("CoParryKnockBackMove");
        context.animationController.move_x = 0;
        context.animationController.move_z = 0;
    }

    private IEnumerator CoParryKnockBackMove()
    {
        float delta = 0;
        float duration = 0.9f;
        float startValue = 15f;
        float endValue = 0f;
        
        
        context.FocusOnTarget(20f);
        while (true) 
        {
            float t = delta / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            context.Move(new Vector3(0, 0, -1.0f), currentValue);

            delta += Time.deltaTime;
            yield return null;
        }
    }

}
