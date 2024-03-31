using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunSlingerForTutorial : MonoBehaviour
{
    #region Componets
    private Rigidbody rb;
    public BoxCollider hitbox;
    public GunSlingerAnimationController animator;
    public GameObject TargetPlayer;

    #endregion

    #region Parameters
    private int turtorialPhase = 1; //튜토리얼 페이즈
    private Vector3 targetPosition; //플레이어 위치 벡터 (x, z)
    private Vector3 rocalDirection; //로컬 방향 벡터 (x, z)


    #endregion

    private void awake()
    {
        rb = GetComponent<Rigidbody>();
        lookatPlayer();
        
    }
    private void Start()
    {
        //InvokeRepeating("RandomMove", 1f, 1f);
        StartCoroutine("coTutorialPhase1");
    }

    /*private void Update()
    {
        MoveTest();
        //transform.RotateAround(TargetPlayer.transform.position, Vector3.down, 2 * Time.deltaTime);
        lookatPlayer();
    }*/

    //움직임에 관함 코드
    #region Movement
    private void lookatPlayer()
    {
        targetPosition = new Vector3(TargetPlayer.transform.position.x, gameObject.transform.position.y, TargetPlayer.transform.position.z);
        transform.LookAt(targetPosition);
    }
    private void MoveTest()
    {
        rb.MovePosition(transform.position + temp * 5 * Time.deltaTime);
        animator.move_x = temp.x;
        animator.move_z = temp.z;
    }

    private void Phase1Move()
    {
        rocalDirection = new Vector3(1.0f, 0.0f, 0.3f);
        rb.MovePosition(transform.position + rocalDirection * 0.5f * Time.deltaTime);
        animator.move_x = rocalDirection.x;
        animator.move_z = rocalDirection.z;
    }

    #endregion

    //튜토리얼에 관련된 로직들
    #region Tutorial
    private IEnumerator coTutorialPhase1()
    {
        while(true)
        {
            Phase1Move();
            Debug.Log("Phase1");
            yield return null;
        }
        
    }
    public int ChangeTutorialPhase()
    {
        turtorialPhase++;
        return turtorialPhase;
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
