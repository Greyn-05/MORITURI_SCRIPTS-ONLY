using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TurtorialSwordMan : BattleLogicController
{
    #region Componets
    //private Rigidbody rb;
    //public BoxCollider hitbox;
    public SwordManAnimationController animator;
    public GameObject TargetPlayer;
    private CharacterController _controller;
    public Collider myCollider;
    private Health _health;
    private AudioSource _audioSource;
    public EffectController effectController;

    #endregion

    #region Parameters
    private string _coTutorial_str = "coTutorialPhase";
    private int _turtorialPhase; //튜토리얼 페이즈
    private Vector3 _targetPosition; //플레이어 위치 벡터 (x, z)
    private Vector3 _rocalDirection; //로컬 방향 벡터 (x, z)
    private float gravity;           //작용하는 중력
    private float speed;             //속도

    private BattleState myState;
    public override BattleState _myState
    {
        get { return myState; }
        set { myState = value; }
    }
    #endregion


    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _health = GetComponent<Health>();
        _audioSource = GetComponent<AudioSource>();
        _myState = BattleState.Stop;
        _turtorialPhase = 1;

        StartCoroutine(_coTutorial_str + _turtorialPhase);
    }

    #region Behavior Tree
    
    #endregion

    //움직임에 관한 코드
    #region Movement
    private void lookatPlayer()
    {
        _targetPosition = new Vector3(TargetPlayer.transform.position.x, gameObject.transform.position.y, TargetPlayer.transform.position.z);
        transform.LookAt(_targetPosition);
    }
    private void MoveTest()
    {
        animator.move_x = temp.x;
        animator.move_z = temp.z;
    }

    private void Phase1Move()
    {
        _myState = BattleState.Move;
        _rocalDirection = new Vector3(-1.0f, 0.0f, 0.2f);
        speed = 0.5f;
        _controller.Move(gameObject.transform.TransformDirection(_rocalDirection) * speed * Time.deltaTime);
        animator.move_x = _rocalDirection.x;
        animator.move_z = _rocalDirection.z;
    }

    private void ChasePlayer()
    {
        _myState = BattleState.Move;
        _rocalDirection = new Vector3(0.0f, 0.0f, 1.0f);
        speed = 1.0f;
        _controller.Move(gameObject.transform.TransformDirection(_rocalDirection) * speed * Time.deltaTime);
        animator.move_x = _rocalDirection.x;
        animator.move_z = _rocalDirection.z;

    }

    #endregion

    //튜토리얼에 관련된 로직들
    #region Tutorial
    private IEnumerator coTutorialPhase1()
    {
        while (true)
        {
            
            float TestDistance = Vector3.Distance(gameObject.transform.position, TargetPlayer.transform.position);
           
            if (_myState != BattleState.Guard)
            {
                if (TestDistance < 2f)
                {
                    _myState = BattleState.Guard;
                    animator.guard = true;
                }
                else
                {
                    lookatPlayer();
                    Phase1Move();
                }
            }
            else
            {
                if (TestDistance > 2f)
                {
                    _myState = BattleState.Move;
                    animator.guard = false;
                }
            }
            yield return null;
        }

    }

    private IEnumerator coTutorialPhase2()
    {
        while (true)
        {
            float Distance = Vector3.Distance(gameObject.transform.position, TargetPlayer.transform.position);

            if(Distance < 1.5f)
            {

            }
            else
            {
                lookatPlayer();
                ChasePlayer();
            }

            yield return null;
        }

    }
    public int ChangeTutorialPhase()
    {
        _turtorialPhase++;
        return _turtorialPhase;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider)
        {
            return;
        }

        if ((other.tag == "Weapon")&&(other.gameObject.layer != 7))
        {
            other.TryGetComponent(out IWeapon weapon);
            (int otherDamage, int otherKnockBack, BattleState OtherState) = weapon.GetOtherInfo();
            //너 hit판정이니?
            //내가 가드중인가?
            //근데 0.2초안에 가드한건가?
            //다 아니야? 그럼 맞아
            if(OtherState == BattleState.Attack_Judge)
            {
                if(_myState == BattleState.Guard_Parry)
                {

                }
                else if(_myState == BattleState.Guard)
                {
                    _audioSource.Play();
                    animator.animator.SetTrigger("GetHitGuard");
                    //effectController.PlayParrySpark(other.transform.position, );
                }
                else
                {
                    base.getHIt(_health, otherDamage);
                }
            }
            
        }
    }
    #endregion

    #region Physics
    private void ForceGravity()
    {

    }
    #endregion 

    //테스트용 코드들 나중에 지울 것!!
    #region Test
    private Vector3 temp = new Vector3(1.0f, 0.0f, 1.0f);


    private void RandomMove()
    {
        temp = new Vector3((float)Random.Range(-1, 1), 0.0f, (float)Random.Range(-1, 1));
        // Debug.Log(temp);
    }
    #endregion

}
