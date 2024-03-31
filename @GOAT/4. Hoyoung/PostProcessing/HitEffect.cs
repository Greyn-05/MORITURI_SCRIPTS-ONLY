using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class HitEffect : MonoBehaviour
{
    public float blurSize = 0.1f;

    public Vector2 blurCenterPos = new Vector2(0.5f, 0.5f);

    public int samples;

    public Material hitEffect = null;

    private void Awake()
    {
        samples = 1;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(blurSize > 0.0f)
        {
            hitEffect.SetInt("_Samples", samples);
            hitEffect.SetFloat("_BlurSize", blurSize);
            hitEffect.SetVector("_BlurCenterPos", blurCenterPos);
            Graphics.Blit(source, destination, hitEffect);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void ActiveEvent()
    {
        StartCoroutine(coActiveEvent());
    }

    private IEnumerator coActiveEvent()
    {
        samples = 30;
        yield return new WaitForSeconds(0.2f);
        samples = 1;
    }
}
