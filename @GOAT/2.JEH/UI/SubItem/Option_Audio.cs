using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Option_Audio : UI_Base
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private TextMeshProUGUI _volumeText;
    [SerializeField] private Toggle _muteToggle;

    public enum AudioType
    {
        전체볼륨,
        배경음악,
        효과음
    }

    public AudioType _type = AudioType.전체볼륨;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        _volumeSlider.minValue = 0.0001f;
        _volumeSlider.maxValue = 1f;

        switch (_type)
        {
            case AudioType.전체볼륨:
                _volumeSlider.value = Main.Player.gameSetting.allVolume;
                break;
            case AudioType.배경음악:
                _volumeSlider.value = Main.Player.gameSetting.bgmVolume;
                break;
            case AudioType.효과음:
                _volumeSlider.value = Main.Player.gameSetting.sfxVolume;
                break;
        }

        _volumeSlider.onValueChanged.AddListener(Volume);
        _muteToggle.onValueChanged.AddListener(Mute);

        Refresh();

        return true;
    }

    public void Volume(float value)
    {
        if (!_muteToggle.isOn)
            _audioMixer.SetFloat(gameObject.name, Mathf.Log10(value) * 20); // 슬라이더 최소값을 0.0001f로 주면 -40일때 0 나옴

        _volumeText.text = (_volumeSlider.value * 100).ToString("N0");

        switch (_type)
        {
            case AudioType.전체볼륨:
                Main.Player.gameSetting.allVolume = value;
                break;
            case AudioType.배경음악:
                Main.Player.gameSetting.bgmVolume = value; 
                break;
            case AudioType.효과음:
                Main.Player.gameSetting.sfxVolume = value; 
                break;
        }

        Main.Save.SaveToJson_GameSetting();

    }


    public void Mute(bool isMute)
    {
        _muteToggle.isOn = isMute;

        if (isMute)
            _audioMixer.SetFloat(gameObject.name, -80f);
        else
            _audioMixer.SetFloat(gameObject.name, Mathf.Log10(_volumeSlider.value) * 20);

        switch (_type)
        {
            case AudioType.전체볼륨:
                Main.Player.gameSetting.allMute = isMute;
                break;
            case AudioType.배경음악:
                Main.Player.gameSetting.bgmMute = isMute;
                break;
            case AudioType.효과음:
                Main.Player.gameSetting.sfxMute = isMute;
                break;
        }
        Main.Save.SaveToJson_GameSetting();
    }

   public void Refresh()
    {
        switch (_type)
        {
            case AudioType.전체볼륨:
                _nameText.text = "모든소리";
                _volumeSlider.value = Main.Player.gameSetting.allVolume;
                Volume(Main.Player.gameSetting.allVolume);
                Mute(Main.Player.gameSetting.allMute);
                break;

            case AudioType.배경음악:
                _nameText.text = "배경음악";
                _volumeSlider.value = Main.Player.gameSetting.bgmVolume;
                Volume(Main.Player.gameSetting.bgmVolume);
                Mute(Main.Player.gameSetting.bgmMute);
                break;

            case AudioType.효과음:
                _nameText.text = "효과음";
                _volumeSlider.value = Main.Player.gameSetting.sfxVolume;
                Volume(Main.Player.gameSetting.sfxVolume);
                Mute(Main.Player.gameSetting.sfxMute);
                break;
        }

        _volumeText.text = (_volumeSlider.value * 100).ToString("N0");
    }

}
