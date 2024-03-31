using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : UI_Base
{
    [SerializeField] private Image _image;
    [SerializeField] private float _flashSpeed;

    private Coroutine _coroutine;
    
    public override bool Initialize()
    {
        _image.enabled = false;
        
        return base.Initialize();
    }


    public void Flash()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _image.enabled = true;
        _image.color = Color.red;
        _coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float alpha = startAlpha;

        while (alpha > 0.0f)
        {
            alpha -= (startAlpha / _flashSpeed) * Time.deltaTime;
            _image.color = new Color(1, 0, 0, alpha);
            yield return null;
        }

        _image.enabled = false;
    }
}
