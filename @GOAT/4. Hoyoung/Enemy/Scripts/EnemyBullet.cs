using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour, IWeapon
{
    public enum bullets
    {
        pistol=0,
        shotgun,
        rifle
    }
    public int damage;
    public int knockBack;
    public bullets thisbullet;
    private float[] bulletSpeed = new float[] { 100.0f, 90.0f, 180.0f};
    private BattleState State = BattleState.Attack_Judge;
    private Rigidbody RB;
    private Define.AttackAttribute _attribute;

    [SerializeField]
    private GameObject playerBullet;
    [SerializeField]
    private MeshRenderer Mesh;
    [SerializeField]
    private TrailRenderer Trail;

    [SerializeField]
    private GameObject ExplosionEffect;


    private void Start()
    {
        RB = this.GetComponent<Rigidbody>();
        RB.velocity = transform.forward * bulletSpeed[(int)thisbullet];
        StartCoroutine(selfDestroy());
    }

    private void Update()
    {
        
    }

    public Define.AttackAttribute getAttribute()
    {
        return _attribute;
    }

    public void realHit()
    {
        ExplosionEffect.transform.rotation = Quaternion.Euler(new(-90.0f, (float)Random.Range(0, 270), 0f));
        ExplosionEffect.SetActive(true);
    }

    // Start is called before the first frame update
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }

    // Update is called once per frame
    public void ChangeJudgeState()
    {
        RB.velocity = transform.forward * 0f;
        State = BattleState.Attack;
        Mesh.enabled = false;
        Trail.enabled = false;

    }


    public (int, int, BattleState) GetOtherInfo()
    {
        return (damage, knockBack, State);
    }

    public void youJustParry()
    {
        StartCoroutine(shootPlayerBullet());
    }
    private IEnumerator shootPlayerBullet()
    {
        yield return new WaitForSeconds(0.18f);
        Vector3 aimDir = (Main.Game.currentEnemy.transform.position - gameObject.transform.position).normalized;
        aimDir.y = 0;
        GameObject TBullet = Instantiate(playerBullet, this.gameObject.transform.position, Quaternion.LookRotation(aimDir, Vector3.up));

    }

    private IEnumerator selfDestroy()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

}
