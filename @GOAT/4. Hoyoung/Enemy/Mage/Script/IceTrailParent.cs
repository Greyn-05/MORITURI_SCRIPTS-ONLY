using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrailParent : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(coSelfDestory());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator coSelfDestory()
    {
        yield return new WaitForSeconds(4f);
        Main.Resource.Destroy(this.gameObject);
    }
}
