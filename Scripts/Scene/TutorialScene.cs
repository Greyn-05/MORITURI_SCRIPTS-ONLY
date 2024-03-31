using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialScene : BaseScene
{
    [Header("Spawn Position")]
    [SerializeField]
    private Vector3 _playerSpawnPosition;

    [SerializeField]
    private Vector3 _EnemySpawnPosition;

    public TutorialLine tutorialLine;
    private GameObject spawnedPlayer;
    private GameObject spawnedEnemy;
    private EnemyBehaviorTreeRunner EnemyBTrunner;
    private EnemyController enemyController;
    private PlayerController playerController;
    private PlayerLockOn playerLockOn;

    public HitEffect hitEffect;

    #region Data_For_Tutorial_Phase
    private enum tutorialPhase
    {
        Intro = 0,
        Walk,
        Run,
        LockOn,
        Dodge,
        UseItem,
        Attack,
        Guard,
        Parry,
        Battle,
        End,
    }

    private tutorialPhase currentPhase;
    private Coroutine currentC;
    private float mIntroTime;
    private float mWalkTime;
    private float mRunTime;
    private float mLockOnTime;
    private int mDodgeCount;
    private bool mUseItem;
    private int mAttackCount;
    private int mGuardCount;
    private int mParryCount;
    private bool spacebarInput;
    //UI_Popup_Fade tutorialFade;

    private void tutorialDateInitialize()
    {

        currentPhase = tutorialPhase.Intro;
        mIntroTime = 0;
        mWalkTime = 0;
        mRunTime = 0;
        mLockOnTime = 0;
        mDodgeCount = 0;
        mUseItem = false;
        mAttackCount = 0;
        mGuardCount = 0;
        mParryCount = 0;
        spacebarInput = false;
    }

    private void bindPlayerEvent()
    {
        Main.Player.Controller.OnAttackEvent += increaseAttackCount;
        Main.Player.Controller.OnGuardEvent += increaseGuardCount;
        Main.Player.Controller.OnParryingEvent += increaseParryCount;
        Main.Player.Controller.OnDodgeEvent += increaseDodgeCount;

    }

    private void unbindPlayerEvent()
    {
        Main.Player.Controller.OnAttackEvent -= increaseAttackCount;
        Main.Player.Controller.OnGuardEvent -= increaseGuardCount;
        Main.Player.Controller.OnParryingEvent -= increaseParryCount;
        Main.Player.Controller.OnDodgeEvent -= increaseDodgeCount;
    }
    #endregion

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;
        //   Cursor.lockState = CursorLockMode.Locked;


        Main.Save.DefaultPlayerData();
        for (int i = 0; i < Define.maxInvenSlotCount; i++)
        {
            Main.Inven.inventory[i].item = null;
            Main.Inven.inventory[i].capacity = 0;
        }


        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("DungeonAxeBgm"), 0.85f);
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("DungeonCheeringBGM"), 0.85f);

        //플레이어 생성
        Main.Player.SetDungeonScene(_playerSpawnPosition);
        spawnedPlayer = Main.Player.PlayerObject;
        playerController = Main.Player.Controller;
        playerLockOn = spawnedPlayer.GetComponent<PlayerLockOn>();

        //적 생성
        createEnemy("TutorialPhase1");

        //튜토리얼 Initialize
        tutorialDateInitialize();
        bindPlayerEvent();
        currentC = StartCoroutine(introPhase());

        Main.UI.SetSceneUI<UI_Scene_Tutorial>();
        //tutorialFade = Main.UI.OpenPopup<UI_Popup_Fade>();
        //tutorialFade.FadeAction += enemyController.health.TakeDamage(10000);
        //tutorialFade.FadeAction += Destroy(spawnedEnemy);
        return true;
    }



    #region TutorialPhase
    private void changePhase(IEnumerator nextPhase)
    {
        StopCoroutine(currentC);
        currentPhase++;
        currentC = StartCoroutine(nextPhase);
    }
    private IEnumerator introPhase()
    {

        tutorialLine.TutorialLineOn("튜토리얼 시작");
        yield return new WaitForSeconds(5f);
        tutorialLine.TutorialLineOff();
        changePhase(walkPhase());
    }

    private IEnumerator walkPhase()
    {
        tutorialLine.TutorialLineOn("WASD로 걸을 수 있습니다.");
        yield return new WaitForSeconds(3f);
        tutorialLine.TutorialLineOff();
        while (true)
        {
            tutorialLine.TutorialLineOn("3초동안 걸어보세요 " + mWalkTime.ToString("0.0") + "초");
            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
            {
                mWalkTime += Time.deltaTime;
            }

            if (mWalkTime >= 3.0f)
            {
                tutorialLine.TutorialLineOff();
                yield return new WaitForSeconds(0.8f);
                changePhase(runPhase());
            }
            yield return null;
        }

    }

    private IEnumerator runPhase()
    {
        tutorialLine.TutorialLineOn("걷는 중 LeftShift를 눌러 달릴 수 있습니다.");
        yield return new WaitForSeconds(3f);
        tutorialLine.TutorialLineOff();
        while (true)
        {
            tutorialLine.TutorialLineOn("3초동안 달려보세요 " + mRunTime.ToString("0.0") + "초");
            if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    mRunTime += Time.deltaTime;
                }
            }

            if (mRunTime >= 3.0f)
            {
                tutorialLine.TutorialLineOff();
                yield return new WaitForSeconds(0.8f);
                changePhase(lockonPhase());
            }
            yield return null;
        }
    }

    private IEnumerator lockonPhase()
    {
        tutorialLine.TutorialLineOn("락온은 마우스 휠을 눌러 동작합니다.");
        yield return new WaitForSeconds(3f);
        tutorialLine.TutorialLineOff();
        while (true)
        {
            tutorialLine.TutorialLineOn("2초동안 락온 해보세요 " + mLockOnTime.ToString("0.0") + "초");
            if (playerLockOn.IsLockOned)
            {
                mLockOnTime += Time.deltaTime;
            }
            if (mLockOnTime >= 2.0f)
            {
                changePhase(dodgePhase());
            }
            yield return null;
        }
    }

    private IEnumerator dodgePhase()
    {
        tutorialLine.TutorialLineOn("구르기는 space를 눌러서 동작합니다.");
        yield return new WaitForSeconds(3f);
        tutorialLine.TutorialLineOff();
        mDodgeCount = 0;
        while (true)
        {
            tutorialLine.TutorialLineOn("구르기를 5회 해보세요 " + mDodgeCount + "/5");
            if (mDodgeCount >= 5)
            {
                tutorialLine.TutorialLineOff();

                yield return new WaitForSeconds(2f);
                changePhase(useItemPhase());
            }
            yield return null;
        }
    }

    private IEnumerator useItemPhase()
    {
        Main.Inven.Add(Main.CSVData.itemDatas[12], 20);
        Main.Inven.QuickSlotIndex = 0;

        tutorialLine.TutorialLineOn("마우스 휠을 조작하여 퀵슬롯을 변경합니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();
        yield return new WaitForSeconds(0.5f);

        tutorialLine.TutorialLineOn("E를 눌러 퀵슬롯에서 선택된 아이템을 사용합니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();
        yield return new WaitForSeconds(0.5f);

        tutorialLine.TutorialLineOn("회복 포션을 사용하여 HP를 회복해보세요");
        //Main.Player.Controller.Health.TakeDamage((int)(Main.Player.Status.HP.MaxValue * 0.95));
        Main.Player.Controller.Health.TakeDamage(50);

        Main.Player.DamageIndicator.Flash();



        yield return new WaitForSeconds(1.5f);
        tutorialLine.TutorialLineOff();

        while (true)
        {
            if (Main.Player.Status.HP.CurValue >= Main.Player.Status.HP.MaxValue * 0.95)
            {
                tutorialLine.TutorialLineOff();
                yield return new WaitForSeconds(2f);
                changePhase(attackPhase());
            }
            yield return null;
        }
    }

    private IEnumerator attackPhase()
    {
        tutorialLine.TutorialLineOn("공격은 마우스 좌클릭 또는 우클릭을 눌러서 동작합니다.");
        yield return new WaitForSeconds(3f);
        tutorialLine.TutorialLineOff();
        mAttackCount = 0;
        while (true)
        {
            tutorialLine.TutorialLineOn("공격을 5회 해보세요 " + mAttackCount + "/5");
            if (mAttackCount >= 5)
            {
                //tutorialFade.StartPade(2.0f);
                tutorialLine.TutorialLineOff();
                yield return new WaitForSeconds(2f);
                //enemyController.health.TakeDamage(10000);
                //Destroy(spawnedEnemy);

                Main.Pool._pools.Clear();
                changePhase(guardPhase());
            }
            yield return null;
        }
    }

    private IEnumerator guardPhase()
    {
        tutorialLine.TutorialLineOn("가드는 Q를 눌러서 동작합니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();
        tutorialLine.TutorialLineOn("상대방의 공격을 가드하면 피해는 받지않지만 스태미너를 소모합니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();

        tutorialLine.TutorialLineOff();
        mGuardCount = 0;
        EnemyBTrunner.context.enemyPhase = 2;
        EnemyBTrunner.context.FailTrigger = true;
        while (true)
        {
            tutorialLine.TutorialLineOn("가드을 5회 해보세요 " + mGuardCount + "/5");
            if (mGuardCount >= 5)
            {
                tutorialLine.TutorialLineOff();
                yield return new WaitForSeconds(2f);
                /*enemyController.health.TakeDamage(10000);
                Destroy(spawnedEnemy);
                Main.Pool._pools.Clear();*/

                changePhase(parryPhase());
            }
            yield return null;
        }
    }

    private IEnumerator parryPhase()
    {
        tutorialLine.TutorialLineOn("패링은 적의 공격 타이밍에 맞추어서 가드를 올려 동작합니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();
        tutorialLine.TutorialLineOn("패링을 2회 성공할 경우 다음 패링을 성공할 시 강력한 횡베기를 사용합니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();

        mParryCount = 0;
        Main.Player.ParryCount = 0;

        while (true)
        {
            tutorialLine.TutorialLineOn("패링을 3회 해보세요 " + mParryCount + "/3");
            if (mParryCount >= 3)
            {
                tutorialLine.TutorialLineOff();
                yield return new WaitForSeconds(2f);
                changePhase(battlePhase());
            }
            yield return null;
        }
    }

    private IEnumerator battlePhase()
    {
        tutorialLine.TutorialLineOn("지금까지 배운걸로 이겨보세요.");
        yield return new WaitForSeconds(3f);
        tutorialLine.TutorialLineOff();

        mGuardCount = 0;
        EnemyBTrunner.context.enemyPhase = 3;
        EnemyBTrunner.context.DamageReduce = 0;

        while (true)
        {
            if (EnemyBTrunner.context.health._hpModule.CurValue <= 0)
            {
                changePhase(endPhase());
            }
            yield return null;
        }
    }

    private IEnumerator endPhase()
    {
        unbindPlayerEvent();
        tutorialLine.TutorialLineOn("튜토리얼을 종료합니다. 수고하셨습니다.");
        yield return new WaitForSeconds(2f);
        tutorialLine.TutorialLineOff();
        tutorialLine.TutorialLineOn("마을로 이동합니다.");


        yield return new WaitForSeconds(4.0f);
        LoadingScene.LoadScene(Define.SceneName.Town);
    }


    #endregion
    private void increaseAttackCount()
    {
        mAttackCount++;
    }
    private void increaseGuardCount()
    {
        mGuardCount++;
    }
    private void increaseParryCount()
    {
        mParryCount++;
    }

    private void increaseDodgeCount()
    {
        mDodgeCount++;
    }


    private void createEnemy(string _enemyName)
    {
        spawnedEnemy = Instantiate(Main.Resource.Load<GameObject>(_enemyName), _EnemySpawnPosition, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)));
        EnemyBTrunner = spawnedEnemy.GetComponent<EnemyBehaviorTreeRunner>();
        EnemyBTrunner.InitBHtree();
        EnemyBTrunner.context.targetPlayer = spawnedPlayer;
        EnemyBTrunner.context.enemyController.damageReduce = 100;
        EnemyBTrunner.context.enemyController.HitEvent += ActiveHitEffect;
        EnemyBTrunner.context.DamageReduce = EnemyBTrunner.context.enemyController.damageReduce;
        EnemyBTrunner.context.transform.LookAt(new Vector3(spawnedPlayer.transform.position.x, spawnedEnemy.transform.position.y, spawnedPlayer.transform.position.z));
        enemyController = spawnedEnemy.GetComponent<EnemyController>();
        Main.Game.currentEnemy = spawnedEnemy;
        Main.Game.currentEnemyStatus = EnemyBTrunner.context.enemyInfo;
        
    }

    public void ActiveHitEffect()
    {
        hitEffect.ActiveEvent();
        Main.Player.HitBloodEffect.FlashHitBlood();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 Ps = new Vector3(_playerSpawnPosition.x, _playerSpawnPosition.y + 2 / 2, _playerSpawnPosition.z);
        Gizmos.DrawWireCube(Ps, new Vector3(2, 2, 2));

        Gizmos.color = Color.red;
        Vector3 Es = new Vector3(_EnemySpawnPosition.x, _EnemySpawnPosition.y + 2 / 2, _EnemySpawnPosition.z);
        Gizmos.DrawWireCube(Es, new Vector3(2, 2, 2));
    }

    /*private bool spaceInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (spacebarInput == false)
            {
                spacebarInput = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            spacebarInput = false;
            return false;
        }
    }*/
}
