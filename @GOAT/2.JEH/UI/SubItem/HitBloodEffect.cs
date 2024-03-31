using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HitBloodEffect : UI_Base
{
    [SerializeField] private GameObject _hitBloodEffect;
    [SerializeField] private Image _blur;
    [SerializeField] private Image[] _image;
    [SerializeField] private float _flashSpeed;

    private Image _bloodImage;
    private Coroutine _coroutine;
    
    public override bool Initialize()
    {
        foreach (var item in _image)
        {
            item.enabled = false;
        }
        _blur.enabled = false;
        
        return base.Initialize();
    }


    public void FlashHitBlood()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _bloodImage = _image[Random.Range(0, _image.Length)];
        _blur.enabled = true;
        _bloodImage.enabled = true;

        _coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float alpha = startAlpha;

        while (alpha > 0.0f)
        {
            alpha -= (startAlpha / _flashSpeed) * Time.deltaTime;
            _bloodImage.color = new Color(1, 0, 0, alpha);
            _blur.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        
        _bloodImage.enabled = false;
        _blur.enabled = false;
        //_hitBloodEffect.SetActive(false);
    }
    
}
