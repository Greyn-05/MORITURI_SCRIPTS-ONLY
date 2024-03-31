using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunslingerAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private EnemyBehaviorTreeRunner BT;

    [SerializeField]
    private GameObject pistolbullet;

    [SerializeField]
    private GameObject shotgunbullet;

    [SerializeField]
    private GameObject riflebullet;

    [SerializeField]
    private GameObject shotgunSkillbullet;

    [SerializeField]
    private GameObject rifleSkillbullet;

    [SerializeField]
    private Transform bulletSpawnPosition;


    private string[] pistolSound = new string[] { "pistol3_1", "pistol3_2", "pistol3_3" };
    private string[] shotgunSound = new string[] { "shotgun2_1", "shotgun2_2", "shotgun2_2" };
    private string[] shotgunSkillSound = new string[] { "shotgun3_1", "shotgun3_2", "shotgun3_3" };
    private string[] rifleSound = new string[] { "rifle1_1", "rifle1_2" };
    private string[] rifleSkillSound = new string[] { "rifle2_1", "rifle2_2", "rifle2_3" };

    private void OnAttackLastE()
    {
        BT.context._myState = BattleState.Stop;

    }

    private void OnAttackStartE()
    {
        BT.context._myState = BattleState.Attack;
    }

    private void OnHitStartE()
    {
        BT.context._myState = BattleState.Gethit;
    }

    private void spawnEvent()
    {
        StartCoroutine(CoSetDust());
    }
    
    private IEnumerator CoSetDust()
    {
        GameObject dust = Main.Resource.InstantiatePrefab("DustStorm");

        dust.transform.position = new Vector3(-6.64f ,0, -15.84f);
        dust.SetActive(true);
        yield return new WaitForSeconds(10.0f);

        Destroy(dust);

    }

    private void StartHardAttack()
    {
        Main.Player.OnWarningEvent.Invoke();
    }
    private void HandGunFire()
    {
        //Debug.Log("HandGunFire");
        Vector3 aimDir = (Main.Player.PlayerObject.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(pistolbullet, bulletSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(pistolSound[Random.Range(0, pistolSound.Length)]), bulletSpawnPosition);
    }

    private void ShotGunFire()
    {
        //Debug.Log("ShotGunFire");
        Vector3 aimDir = (Main.Player.PlayerObject.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(shotgunbullet, bulletSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(shotgunSound[Random.Range(0, shotgunSound.Length)]), bulletSpawnPosition);
    }

    private void ShotGunSkillFire()
    {
        //Debug.Log("ShotGunSkillFire");
        Vector3 aimDir = (Main.Player.PlayerObject.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(shotgunSkillbullet, bulletSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(shotgunSkillSound[Random.Range(0, shotgunSkillSound.Length)]), bulletSpawnPosition);
    }

    private void RifleFire()
    {
        //Debug.Log("RifleFire");
        Vector3 aimDir = (Main.Player.PlayerObject.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(riflebullet, bulletSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(rifleSound[Random.Range(0, rifleSound.Length)]), bulletSpawnPosition);
    }

    private void RifleSkillFire()
    {
        //Debug.Log("RifleSkillFire");
        Vector3 aimDir = (Main.Player.PlayerObject.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(rifleSkillbullet, bulletSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>(rifleSkillSound[Random.Range(0, rifleSkillSound.Length)]), bulletSpawnPosition);
    }

    private void fireHandGunBullet()
    {
        Vector3 aimDir = (Main.Player.PlayerObject.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(pistolbullet, bulletSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

    }
}
