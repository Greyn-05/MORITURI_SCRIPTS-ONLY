using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Option_Resolution : UI_Base
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Button yesBtn;

    private List<Resolution> _resolutionsList = new(); //해상도 목록

    private int resolutionNum;
    private FullScreenMode screenMode;

    int Gcd(int n, int m) // a와 b의 최대공약수 계산. 해상도 비율계산
    {
        //두 수 n, m 이 있을 때 어느 한 수가 0이 될 때 까지
        //gcd(m, n%m) 의 재귀함수 반복
        if (m == 0) return n;
        else return Gcd(m, n % m);
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _fullScreenToggle.onValueChanged.AddListener(FullScreenToggle);
        _dropdown.onValueChanged.AddListener(Dropbox);
        yesBtn.onClick.AddListener(Preview);

        _dropdown.value = Main.Player.gameSetting.resolution;
        resolutionNum = Main.Player.gameSetting.resolution;

        // 해상도목록 만들어서 드롭박스에 추가

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            _resolutionsList.Add(Screen.resolutions[i]);
        }

        _dropdown.options.Clear();

        for (int i = 0; i < _resolutionsList.Count; i++)
        {
            TMP_Dropdown.OptionData option = new();

            //해상도 비율계산

            int width = _resolutionsList[i].width;
            int height = _resolutionsList[i].height;

            option.text = $"{width}x{height} {width / Gcd(width, height)}:{height / Gcd(width, height)} {_resolutionsList[i].refreshRateRatio.value.ToString("F0")}hz";

            _dropdown.options.Add(option);
            _dropdown.value = i;

        }

        _dropdown.RefreshShownValue();
        Refresh();

        //   Nope(); //TODO 모니터 바뀌어서 저장된 해상도목록번호가 없을때 대비해서 예외처리해둬야함

        return true;
    }

    void Refresh()
    {
        //  _fullScreenToggle.isOn = true;
        //  _dropdown.value = _resolutionsList.Count - 1;
        //  Dropbox(_resolutionsList.Count - 1);

        FullScreenToggle(Main.Player.gameSetting.isFullscreen);
        Dropbox(Main.Player.gameSetting.resolution);

        // 저장된 데이터있으면 그 체크로 바꿔야함. 해상도목록에서 현재 해상도 체크한게 계속 초기화되는중.
    }

    public void Dropbox(int num)
    {
        resolutionNum = num;
        _dropdown.value = num; // 해상도목록에 체크마크도 갱신
    }

    public void FullScreenToggle(bool On)
    {
        _fullScreenToggle.isOn = On;
        screenMode = On ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

    }

    private UI_Popup_Decision _decision;
    private string[] turnOff_textArray = new string[] { "해상도를 적용하시겠습니까?\n5초 후 변경전 해상도로 돌아갑니다", "네", "아니요" };

    public void Preview()
    {
        _decision = Main.UI.OpenPopup<UI_Popup_Decision>();

        _decision.decisionAction += TurnOff;
        _decision.OpenDecision(ref turnOff_textArray);

         Screen.SetResolution(_resolutionsList[resolutionNum].width, _resolutionsList[resolutionNum].height, screenMode);

        StartCoroutine(PreviewCountdown());
        //   GetObject((int)GameObjects.Preview).gameObject.SetActive(true);

    }

    void TurnOff(bool accept)
    {
        _decision.OnBtnClose();
        StopAllCoroutines();

        if (accept)
        {
           // Debug.Log("해상도적용");

            Main.Player.gameSetting.resolution = resolutionNum;
            Main.Player.gameSetting.isFullscreen = screenMode == FullScreenMode.FullScreenWindow;

            Screen.fullScreen = screenMode == FullScreenMode.FullScreenWindow;
            Screen.SetResolution(_resolutionsList[_dropdown.value].width, _resolutionsList[_dropdown.value].height, screenMode);
            Main.Save.SaveToJson_GameSetting();
        }
        else
        {
           // Debug.Log("해상도적용취소");

            _fullScreenToggle.isOn = Main.Player.gameSetting.isFullscreen;
            screenMode = _fullScreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.SetResolution(_resolutionsList[Main.Player.gameSetting.resolution].width, _resolutionsList[Main.Player.gameSetting.resolution].height, _fullScreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

            resolutionNum = Main.Player.gameSetting.resolution;
            _dropdown.value = Main.Player.gameSetting.resolution;

        }
    }


    IEnumerator PreviewCountdown()
    {
        float countdown = 5;

        //   GetText((int)Texts.CountdownText).text = countdown.ToString();

        while (true)
        {
            if (countdown <= 0) //0초되면 해상도변경취소
            {
                _decision.decisionAction?.Invoke(false);
                break;
            }

            yield return new WaitForSecondsRealtime(1f);

            countdown--;

            //    GetText((int)Texts.CountdownText).text = countdown.ToString();

        }
    }


    //public void DefaultSetting()
    //{
    //    // 해상도 저장할땐 resolutionNum 저장
    //    // 로드 Screen.SetResolution(resolutions[resolutionNumSet].width, resolutions[resolutionNumSet].height, screenMode);
    //}

}


