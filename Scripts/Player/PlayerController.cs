using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Field ------------------------------------------------------------------------------------------------------
    [SerializeField] private PlayerSO _playerSO;
    private PlayerAnimationData _playerAnimationData;
    [SerializeField] private CinemachineStateDrivenCamera _camera;
    private PlayerStatus _status;

    [field: SerializeField] private Transform _cameraRoot;

    private Rigidbody _playerRBody;
    private Animator _playerAnimator;
    private PlayerInputValue _playerInputValue;
    private CharacterController _characterController;
    private Transform _transformPlayer;

    private FSM_Player _stateMachine;
    private ForceReciever _forceReceiver;
    private EffectController _effectController;
    private PlayerAnimationEvent _playerAnimationEvent;

    private bool _cusorLock;

    [field: SerializeField] private PlayerWeapon _weapon;

    private Health _health;
    private Blood _blood;
    
    public BattleState _myState;
    public bool isInvincible;   //프로퍼티로 변경
    
    public Action OnDodgeEvent;
    
    private NPCInfo npcInfo;
    public static int guardCount;
    public static int parryCount;
    #endregion


    #region Property ---------------------------------------------------------------------------------------------------
    public PlayerSO Data => _playerSO;
    public PlayerAnimationData AnimationData => _playerAnimationData;
    public CinemachineStateDrivenCamera Camera => _camera;
    public PlayerStatus status => _status;


    public Rigidbody RBody => _playerRBody;
    public Animator Animator => _playerAnimator;
    public PlayerInputValue InputValue => _playerInputValue;
    public CharacterController Controller => _characterController;
    public Transform Transform_Player => _transformPlayer;
    
    public FSM_Player StateMachine => _stateMachine;
    public ForceReciever ForceReceiver => _forceReceiver;
    
    public PlayerWeapon Weapon => _weapon;
    public Health Health => _health;
    public PlayerAnimationEvent PlayerAnimationEvent => _playerAnimationEvent;
    
    
    public event Action OnGuardEvent;
    public event Action OnParryingEvent;
    public event Action<Transform> OnParryAttackEvent;
    public event Action OnAttackEvent;
    public event Action OnDamageEvent;
    public event Action OnStateChangeEvent;
    public event Action OnAttributeChangeEvent;
    
    #endregion

    #region Init -------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        InitializeAnimationData();
        InitializeCameraData();
        _status = Main.Player.Status;
        
        _characterController = GetComponent<CharacterController>();
        _playerInputValue = GetComponent<PlayerInputValue>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _playerRBody = GetComponent<Rigidbody>();
        _transformPlayer = GetComponent<Transform>();
        _effectController = GetComponent<EffectController>();

        _stateMachine = new FSM_Player(this);
        _forceReceiver = GetComponent<ForceReciever>();
        
        _health = GetComponent<Health>();
        _blood = GetComponentInChildren<Blood>();
        _playerAnimationEvent = GetComponentInChildren<PlayerAnimationEvent>();
        
        npcInfo = GetComponent<NPCInfo>();
        
        _health.Initialize(_status.HP);
        isInvincible = false;
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        _stateMachine.ChangeState(_stateMachine.GroundState);
        _health.OnDie += OnDie;
        _health.OnDie += Main.Game.LoseEvent;

    }

    private void Update()
    {
        StateMachine.HandleInput();
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
    }

    private void InitializeAnimationData()
    {
        _playerAnimationData = new PlayerAnimationData();

        if (!AnimationData.IsInit)
        {
            AnimationData.InitializeAnimationData();
        }
    }

    private void InitializeCameraData()
    {
        Main.Cinemachne.InstantiateCamera(transform);
        Main.Cinemachne.SetStateDrivenCamera(_cameraRoot);
        _camera = Main.Cinemachne.StateDrivenCamera;
        
        Main.Cinemachne.SetCurrentCamera(Define.EStateDrivenCamera.Player);
    }

    public void SetBattleState(BattleState state)
    {
        OnStateChangeEvent?.Invoke();
        _myState = state;
    }
    #endregion


    #region BattleTrigger ----------------------------------------------------------------------------------------------

    private void OnTriggerStay(Collider other)
    {
        //if (other == _characterController) return;
        

        if (other.CompareTag("Weapon") && other.gameObject.layer != 6)
        {
            other.TryGetComponent(out IWeapon weapon);
            (int otherDamage, int otherKnockBack, BattleState otherState) = weapon.GetOtherInfo();

            if (otherState == BattleState.Attack_Judge && _myState != BattleState.Death)
            {
                var position = transform.position;
                var root = Main.Game.currentEnemy.transform;
                bool isFront = IsFront(root);
                weapon.ChangeJudgeState();
                //Vector3 effectPosition = new Vector3(other.ClosestPoint(position).x, other.ClosestPoint(position).y, position.z+0.8f);
                
                Vector3 direction = (position - root.position).normalized;
                direction.y = position.y;
                _stateMachine.KnockBackDirection = direction * otherKnockBack;
                
                
                if (isFront && _myState == BattleState.Guard_Parry)
                {
                    _playerAnimationEvent._isGuardOn = false;
                    status.Stamina.AddValue(Define.Stamina_Parry);
                    _effectController.PlayParrySpark();
                    if (++Main.Player.ParryCount >= 3)
                    {
                        weapon.youJustParry();
                        Main.Player.PlayerAttribute = weapon.getAttribute();
                    }
                    OnParryingEventStarted(other.transform);
                }
                else if (isFront && _myState == BattleState.Guard && status.Stamina.CurValue > 5f)  
                {
                    weapon.realHit();
                    status.Stamina.SubValue(Define.Stamina_Guard);
                    _effectController.PlayGuardSpark();
                    Animator.SetTrigger(AnimationData.GuardHitTriggerHash);
                    Main.Cinemachne.ShakeCamera(1f, 0.2f);
                    OnGuardEventStarted();

                    //test
                    //if (++Main.Player.ParryCount >= 3) Main.Player.PlayerAttribute = weapon.getAttribute();; //테스트용 지워야함
                    //OnParryingEventStarted(other.transform);
                }
                
                else if(isInvincible == false)
                {
                    weapon.realHit();
                    var damage = otherDamage - (int)status.Def.Value;
                    _health.TakeDamage(damage < 0 ? 0: damage);
                    Main.Cinemachne.ShakeCamera(1f, 0.2f);
                    
                    OnDamageEventStarted();
                    Main.Player.DamageIndicator.Flash();
                    _blood.BloodEffect();
                    OnStartIFrame(0.5f);
                }
            }
        }
    }
    
    private bool IsFront(Transform target)
    {
        Vector3 forward = transform.forward;
        Vector3 dir = (target.position - transform.position).normalized;

        float dot = Vector3.Dot(forward, dir);
        
        if (dot > Mathf.Cos(80 * Mathf.Deg2Rad)) return true;
        return false;
    }
    
    
    public void OnStartIFrame(float itime)
    {
        StartCoroutine(StartIFrame(itime));
    }

    public IEnumerator StartIFrame(float ITime)
    {
        isInvincible = true;
        yield return new WaitForSeconds(ITime);
        isInvincible = false;
    }

    private void OnDie()
    {
        Animator.SetTrigger(AnimationData.DieHash);
        SetBattleState(BattleState.Death);
        enabled = false;
    }

    public void OnGuardEventStarted()
    {
        OnGuardEvent?.Invoke();
        
        guardCount++;
        Main.Quest.UpdateCombatCount(666, guardCount);
    }
    
    public void OnParryingEventStarted(Transform target)
    {
        OnParryingEvent?.Invoke();
        OnParryAttackEvent?.Invoke(target);
        
        parryCount++;
        Main.Quest.UpdateCombatCount(555, parryCount);
    }
    
    public void OnAttackEventStarted()
    {
        OnAttackEvent?.Invoke();
    }

    public void OnDamageEventStarted()
    {
        OnDamageEvent?.Invoke();
    }

    public void OnAttributeChangeEventStarted()
    {
        OnAttributeChangeEvent?.Invoke();
    }
    #endregion
    
    
    
    
    
}
