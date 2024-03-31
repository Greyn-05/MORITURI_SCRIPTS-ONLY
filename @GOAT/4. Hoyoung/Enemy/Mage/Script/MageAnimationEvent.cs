using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private EnemyBehaviorTreeRunner BT;
    [SerializeField]
    private EnemyControllerMage controller;
    [SerializeField]
    private Transform[] SpawnPosition;

    public GameObject preCastingLightning;
    public GameObject EXCastingEffect;

    private GameObject Laser;
    private GameObject Circle_Laser;
    private Coroutine focusCoroutine;

    private bool useTerminationMagic;

    private void Awake()
    {
        //focusCoroutine = coFocus(0);
        useTerminationMagic = false;
    }
    private void OnAttackLastE()
    {
        BT.context._myState = BattleState.Stop;
    }

    private void OnAttackStartE()
    {
        BT.context._myState = BattleState.Attack;
    }

    
    public void PrevCastingMagic()
    {

    }

    private void projectileInit(GameObject _proj, int spawnPositionIndex)
    {
        MagicProjectile magicProjectile = _proj.GetComponent<MagicProjectile>();
        _proj.transform.position = SpawnPosition[spawnPositionIndex].position;
        if (spawnPositionIndex == 0)
        {
            Vector3 aimDir = (Main.Player.PlayerObject.transform.position - SpawnPosition[0].position).normalized;
            aimDir.y = 0;
            _proj.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        }
        else
        {
            Vector3 TplayerVector = Main.Player.PlayerObject.transform.position;
            TplayerVector.y = 1.4f;
            Vector3 aimDir = (TplayerVector - SpawnPosition[spawnPositionIndex].position).normalized;
            _proj.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        }
        magicProjectile.projectileStart();
    }

    public void CastFireBall()
    {
        //GameObject Q = Main.Resource.InstantiatePrefab("FireBall", SpawnPosition[0].position, Quaternion.LookRotation(aimDir, Vector3.up), true);
        //GameObject TBullet = Instantiate(T, SpawnPosition[0].position, Quaternion.LookRotation(aimDir, Vector3.up));
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("FireBall" + Random.Range(1, 3)), gameObject.transform);
        GameObject Tprojectile = Main.Resource.InstantiatePrefab("FireBall", null, true);
        projectileInit(Tprojectile, 0);
    }

    public void CastIceBolt()
    {
        //GameObject Tprojectile = Main.Resource.InstantiatePrefab("IceBolt", null, true);
        //projectileInit(Tprojectile, Random.Range(1,4));
        StartCoroutine(coCastIceBolt());
    }

    private IEnumerator coCastIceBolt()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("IceBolt"+Random.Range(1,3)), gameObject.transform);
        for (int a = 1; a < 4; a++)
        {
            GameObject Tprojectile = Main.Resource.InstantiatePrefab("IceBolt", null, true);
            projectileInit(Tprojectile, a);
            MagicProjectile magicProjectile = Tprojectile.GetComponent<MagicProjectile>();
            magicProjectile.projectileStart();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void CastIceTrail()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("iceTrail"), gameObject.transform);
        GameObject T = Main.Resource.InstantiatePrefab("IceTrail", null, true);
        T.transform.position = SpawnPosition[5].position;
        Vector3 _targetPosition = new Vector3(Main.Player.PlayerObject.transform.position.x, transform.position.y, Main.Player.PlayerObject.transform.position.z);
        Vector3 _lookPosition = _targetPosition - transform.position;
        T.transform.rotation = Quaternion.LookRotation(_lookPosition);
    }
    public void OnLaserAttackStartE()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("LaserCharge"), gameObject.transform);
        BT.context._myState = BattleState.Attack;
        focusCoroutine = StartCoroutine(coFocus(8f));
    }
    public void CastLaser()
    {
        StopCoroutine(focusCoroutine);
        Laser = Main.Resource.InstantiatePrefab("Laser", this.gameObject.transform, true);
        Laser.transform.position = SpawnPosition[0].position;
        Laser.transform.rotation = this.gameObject.transform.rotation;
        focusCoroutine = StartCoroutine(coFocus(2f));
    }

    public void EndCastLaser()
    {
        StopCoroutine(focusCoroutine);
    }

    private IEnumerator coFocus(float F)
    {
        while(true)
        {
            BT.context.FocusOnTarget(F);
            yield return null;
        }
    }
    public void CastLightningVolt()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("LightningVoltex"), gameObject.transform);
        GameObject Tprojectile = Main.Resource.InstantiatePrefab("LightningVoltex", null, true);
        projectileInit(Tprojectile, 0);
    }

    public void prevCastLightningVolt()
    {
        preCastingLightning.SetActive(true);
    }
    public void endCastLightningVolt()
    {
        preCastingLightning.SetActive(false);
    }

    public void CastSummonLightning()
    {
        GameObject T = Main.Resource.InstantiatePrefab("SummonLightningStrike", null, true);
        T.transform.position = Main.Player.PlayerObject.transform.position;
    }

    public void CastSummonPillar()
    {
        GameObject T = Main.Resource.InstantiatePrefab("SummonFirePillar", null, true);
        T.transform.position = Main.Player.PlayerObject.transform.position;
    }

    private string[] Pillars = new string[] { "FirePillar", "IcePillar", "IcePillar" };
    public void CastSummonPillarSingle()
    {
        //GameObject T = Main.Resource.InstantiatePrefab(Pillars[(int)controller.cAttribute-1], null, true);
        GameObject T = Main.Resource.InstantiatePrefab(Pillars[Random.Range(0, Pillars.Length)], null, true);
        T.transform.position = Main.Player.PlayerObject.transform.position;
    }
    private IEnumerator coCastSummonPillarSingle()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTerAttackStartE()
    {
        EXCastingEffect.SetActive(true);
        BT.context._myState = BattleState.Attack;
    }

    private void OnTerAttackLastE()
    {
        EXCastingEffect.SetActive(false);
        BT.context._myState = BattleState.Stop;
    }

    public void CastTerminationMagic()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("meteor_coming"), null);
        Debug.Log("TerminationMagic");
        if(useTerminationMagic == false)
        {
            StartCoroutine(coCastTerminationMagic());
        }
    }

    private Vector3[] shieldSpawnPosition = new Vector3[] {new Vector3(18f, 0 ,0), new Vector3(-18f, 0, 0), new Vector3(0, 0, 18f), new Vector3(0, 0, -18f) };
    private IEnumerator coCastTerminationMagic()
    {
        useTerminationMagic = true;
        GameObject StarFall;
        GameObject[] Shields = new GameObject[2];
        int shieldPositionRandomIndex = Random.Range(0, shieldSpawnPosition.Length - 1);
        for(int a = 0; a < 2; a++)
        {
            Shields[a] = Main.Resource.InstantiatePrefab("CounterStarShield", null, true);
            Shields[a].transform.position = shieldSpawnPosition[shieldPositionRandomIndex + a];
        }

        yield return new WaitForSeconds(2f);

        Vector3 TplayerVector = Main.Player.PlayerObject.transform.position;
        Vector3 aimDir = (TplayerVector - gameObject.transform.position);
        aimDir.y = 0;
        if (BT.context.enemyPhase == 1)
        {
            StarFall = Main.Resource.InstantiatePrefab("StarFall1", null, true);
            StarFall.transform.rotation = Quaternion.LookRotation(aimDir, Vector3.up);
        }
        else
        {
            StarFall = Main.Resource.InstantiatePrefab("StarFall2", null, true);
            StarFall.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 350f), 0));
        }

        yield return new WaitForSeconds(5f);
        Main.Resource.Destroy(StarFall);

        for (int a = 0; a < 2; a++)
        {
            Main.Resource.Destroy(Shields[a]);
        }
        Main.Player.PlayerInvincibleFalse();
        useTerminationMagic = false;
    }

    public void WinGameEvent()
    {
        StopAllCoroutines();
        BT.context.animationController.Attack = false;
        BT.context.animationController.SAttack = false;
        BT.context.animationController.Skill = false;
    }
}
