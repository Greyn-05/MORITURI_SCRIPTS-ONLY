using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonFirePillar : MonoBehaviour
{
    public GameObject FirePillars;
    void OnEnable()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("SummonFirePillarClip"), gameObject.transform);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0.0f, 350.0f), 0));
        StartCoroutine(coSummonFirePillar());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator coSummonFirePillar()
    {
        yield return new WaitForSeconds(1.0f);

        FirePillars.SetActive(true);

        yield return new WaitForSeconds(7f);
        FirePillars.SetActive(false);

        Main.Resource.Destroy(this.gameObject);
    }
}
