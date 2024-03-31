using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyBT : MonoBehaviour
{
    // The main behaviour tree asset
    public BehaviourTree tree;
    //public List<BehaviourTree> trees;
    public GameObject TargetPlayer;
    public int tutorialPhase = 0;

    // Storage container object to hold game object subsystems
    public Context context;


    void Start()
    {
        context = CreateBehaviourTreeContext();
        context.targetPlayer = TargetPlayer;
        context._myState = BattleState.Stop;
        //trees[tutorialPhase] = trees[tutorialPhase].Clone();
        //trees[tutorialPhase].Bind(context);
        tree = tree.Clone();
        tree.Bind(context);
    }

    void Update()
    {
        if (tree)
        {
            tree.Update();
            //Debug.Log(context._myState);
        }
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == context.characterController)
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
                if (context._myState == BattleState.Guard_Parry)
                {

                }
                else if (context._myState == BattleState.Guard)
                {
                    context.audioSource.Play();
                    context.animationController.animator.SetTrigger("GetHitGuard");
                    //context.effectController.PlayGuardSpark(other.transform.position);
                }
                else
                {
                    //base.getHIt(_health, otherDamage);
                    // Debug.Log("EnemyHit");
                }
            }

        }
    }
}
