using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonLightningStrike : MonoBehaviour
{
    public GameObject[] LightningStrikeList;
    // Start is called before the first frame update
    private void OnEnable()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0.0f, 350.0f), 0));
        StartCoroutine(coSummonLightning());
    }

    private IEnumerator coSummonLightning()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("SummonLightningStrike" + Random.Range(1, 4)), gameObject.transform);
        yield return new WaitForSeconds(1.0f);

        for (int a = 0; a < LightningStrikeList.Length; a++)
        {
            LightningStrikeList[a].SetActive(true);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.0f);

        for (int b = 0; b < LightningStrikeList.Length; b++)
        {
            LightningStrikeList[b].SetActive(false);
        }
        Main.Resource.Destroy(this.gameObject);
    }
}
