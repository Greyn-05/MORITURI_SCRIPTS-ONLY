using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunSlingerState
{
    none = -1,
    idle = 0,
    vigilance = 1,
    dodge = 2,
    make_distance = 3,
    attack_handgun = 4,
    attack_shotgun = 5,
    attack_rifle = 6
}

public class GunSlingerBehavior : MonoBehaviour
{
    [Header("GunSlinger Info")]
    [SerializeField]
    private float vigilanceRange;

    [SerializeField]
    private float MakeDistanceRange;

    [SerializeField]
    private float DodgeRange;

    [SerializeField]
    private float GunSlingerHP;

    [SerializeField]
    private float GunSlingerDamage;

    public GameObject bullet;
    private BoxCollider hitBox;
    public GunSlingerAnimationController animator;

    public Transform targetPlayer;
    private float targetDistance;
    private void awake()
    {
        hitBox = GetComponent<BoxCollider>();
    }
    
    void Update()
    {


    }

    private void MoveTest()
    {

    }


    

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, vigilanceRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MakeDistanceRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, DodgeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(gameObject.transform.position, targetPlayer.position);
    }
}
