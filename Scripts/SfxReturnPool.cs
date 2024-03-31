using System.Collections;
using UnityEngine;

public class SfxReturnPool : MonoBehaviour
{
    AudioSource audioSource;

    IEnumerator enumerator = null;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySfx(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        if (enumerator == null)
        {
            enumerator = DestroyWhenFinished();
            StartCoroutine(enumerator);

        }
      //  Debug.Log("효과음재생");

    }

    private IEnumerator DestroyWhenFinished()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        Main.Resource.Destroy(this.gameObject);
        enumerator = null;

    }

    private void OnDisable()
    {
        if (enumerator != null)
            StopAllCoroutines();
    }


}
