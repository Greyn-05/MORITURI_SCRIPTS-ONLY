using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Option : UI_Popup
{
    [SerializeField] private Button _closeBtn;


    [SerializeField]
    private Slider _mouseSlider;
    [SerializeField]
    private TextMeshProUGUI _mouseSliderValue;



    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        _mouseSlider.onValueChanged.AddListener(MouseSensitivity);
        _mouseSlider.minValue = 0.1f;
        _mouseSlider.maxValue = 2;

        _mouseSliderValue.text = $"{Mathf.FloorToInt(Main.Player.gameSetting.mouseSensitivity * 50)}";

        _closeBtn.onClick.AddListener(OnBtnClose);

        return true;
    }
    public void MouseSensitivity(float value)
    {
        _mouseSlider.value = value;
        _mouseSliderValue.text = $"{Mathf.FloorToInt(value * 50)}";

        Main.Cinemachne.MouseSpeed(value);
        Main.Player.gameSetting.mouseSensitivity = value;
        Main.Save.SaveToJson_GameSetting();

    }


}
