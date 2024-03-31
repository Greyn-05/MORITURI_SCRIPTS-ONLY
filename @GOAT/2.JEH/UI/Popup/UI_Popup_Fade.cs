using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Fade : UI_Popup
{
    [SerializeField] private Image img;
    private IEnumerator _enumerator = null;

    public Action FadeAction;


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        img.color = Color.clear;
        img.raycastTarget = false;

        return true;
    }


    public void StartPade(float time)
    {
        if (_enumerator != null) return;

        _enumerator = CoFade(time);
        StartCoroutine(_enumerator);
    }

    private void CancelFade()
    {
        if (_enumerator != null)
        {
            StopCoroutine(_enumerator);
            _enumerator = null;
        }

        StopAllCoroutines();
        FadeAction = null;
    }


    private IEnumerator CoFade(float time)
    {
        img.raycastTarget = true;
        img.DOFade(1, time);
        yield return new WaitForSeconds(time);

        FadeAction?.Invoke();

        yield return new WaitForSeconds(time);


        img.DOFade(0, time);
        yield return new WaitForSeconds(time);
        img.raycastTarget = false;


        CancelFade();

    }
    private void OnDisable()
    {
        CancelFade();
    }
}
