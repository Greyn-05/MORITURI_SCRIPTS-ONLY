using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WarningEffect : MonoBehaviour
{
    [SerializeField] private Image _warningEffectPrefImage;

    private bool _isFade;
    private float _fadeTime = 4f;
    private float _time;
    

    private void Start()
    {
        Main.Player.OnWarningEvent -= WarningEffectActive;
        Main.Player.OnWarningEvent += WarningEffectActive;
    }

    private void OnDestroy()
    {
        Main.Player.OnWarningEvent -= WarningEffectActive;
    }

    private void WarningEffectActive()
    {
        Main.Audio.SfxPlay(Main.Resource.Load<AudioClip>("warning"), transform.root);
        _warningEffectPrefImage.color = Color.white;
        StartCoroutine(FadeOut());
    }

   

    private IEnumerator FadeOut()
    {
        _isFade = true;
        Color fadeColor = _warningEffectPrefImage.color;
        _time = 0f;

        while (fadeColor.a > 0f)
        {
            _time += Time.deltaTime /_fadeTime;
            fadeColor.a = Mathf.Lerp(fadeColor.a, 0, _time);
            _warningEffectPrefImage.color = fadeColor;
            yield return null;
        }

        _isFade = false;
    }
    
    
}
