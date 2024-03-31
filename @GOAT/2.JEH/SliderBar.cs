using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : UI_Base
{
    public Slider _mainSlider;
   public Slider _subSlider;

    IEnumerator enumerator = null;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        return true;
    }

    public void Refresh(float mainValue)
    {
        if (enumerator == null)
        {
            _subSlider.value = _mainSlider.value;
            enumerator = SlowSliderStart();
            StartCoroutine(enumerator);
        }

        _mainSlider.value = mainValue;
    }

    IEnumerator SlowSliderStart()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (_subSlider.value <= _mainSlider.value)
            {
                _subSlider.value = _mainSlider.value;
                enumerator = null;
                yield break;
            }

            _subSlider.value -= 0.3f;

            yield return new WaitForSeconds(0.01f);
        }


    }
}
