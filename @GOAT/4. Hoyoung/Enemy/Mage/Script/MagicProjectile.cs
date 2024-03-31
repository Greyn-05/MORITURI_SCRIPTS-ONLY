using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour, IWeapon
{
    public enum projectile
    {
        FireBall = 0,
        IceBolt,
        LightningVolt
    }
    public int damage;
    public int knockBack;
    public GameObject ProjectailTrail;
    public projectile _proj;
    private float[] projSpeed = new float[] { 90.0f, 70.0f, 110.0f };
    private BattleState State = BattleState.Attack_Judge;
    
    public Define.AttackAttribute _attribute;

    [SerializeField] 
    private Rigidbody RB;

    [SerializeField]
    private GameObject ExplosionEffect;


    private void OnEnable()
    {
        ProjectailTrail.SetActive(false);
        ProjectailTrail.SetActive(true);
    }

    public void projectileStart()
    {
        State = BattleState.Attack_Judge;
        RB.velocity = transform.forward * projSpeed[(int)_proj];
        StartCoroutine(selfDestroy());
    }


    public Define.AttackAttribute getAttribute()
    {
        return _attribute;
    }

    public void realHit()
    {
        //ExplosionEffect.transform.rotation = Quaternion.Euler(new(-90.0f, (float)Random.Range(0, 270), 0f));
        //ExplosionEffect.SetActive(true);
    }

    public void SetAttack(int damage)
    {
        this.damage = damage;
    }

    public void ChangeJudgeState()
    {
        RB.velocity = transform.forward * 0f;
        State = BattleState.Attack;
        Main.Resource.Destroy(this.gameObject);
    }

    public (int, int, BattleState) GetOtherInfo()
    {
        return (damage, knockBack, State);
    }

    public void youJustParry()
    {

    }
    

    private IEnumerator selfDestroy()
    {
        yield return new WaitForSeconds(5.0f);
        Main.Resource.Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Wall"))
        {
            //StopAllCoroutines();
            Main.Resource.Destroy(this.gameObject);
        }
    }

}
