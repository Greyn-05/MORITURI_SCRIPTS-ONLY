using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_PlayerInfo : UI_Popup
{

    [SerializeField] TextMeshProUGUI _playerNameText;
    [SerializeField] TextMeshProUGUI _hPText;
    [SerializeField] TextMeshProUGUI _staminaText;
    [SerializeField] TextMeshProUGUI _expText;
    [SerializeField] TextMeshProUGUI _atkText;
    [SerializeField] TextMeshProUGUI _defText;
    [SerializeField] TextMeshProUGUI _speedText;
    [SerializeField] TextMeshProUGUI _goldText;

    [SerializeField] private Button _closeBtn;


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _closeBtn.onClick.AddListener(OnBtnClose);

        Refresh();

        return true;
    }


    void Refresh()
    {
        _playerNameText.text = Main.Player.playerData.playerName;
        _hPText.text = $"체력 : {Main.Player.Status.HP.CurValue} / {Main.Player.Status.HP.MaxValue}";
        _staminaText.text = $"스태미나 : {Main.Player.Status.Stamina.CurValue} / {Main.Player.Status.Stamina.MaxValue}";
        _atkText.text = $"공격력 : {Main.Player.Status.Atk.Value}";
        _defText.text = $"방어력 : {Main.Player.Status.Def.Value}";
        _speedText.text = $"속도 : {Main.Player.Status.Speed.Value}";
        _expText.text = $"경험치 : {Main.Player.Status.Exp.CurValue} / {Main.Player.Status.Exp.MaxValue}";
        _goldText.text = $"소지금 : {Main.Player.playerData.gold}";

    }
}
