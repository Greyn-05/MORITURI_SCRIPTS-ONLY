using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStarHit : MonoBehaviour
{
    void OnEnable()
    {
        GameObject T = Main.Resource.InstantiatePrefab("StarHitCollider", null, true);
        T.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
    }
}
