using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SellInfo : UI_Base
{
    [SerializeField] private Image _itemIconImage;
    [SerializeField] private TextMeshProUGUI _itemNametext;
    [SerializeField] private TextMeshProUGUI _itemBufftext;
    [SerializeField] private TextMeshProUGUI _itemDescriptiontext;

    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Slider _countSlider;
    [SerializeField] private Button _plusBtn;
    [SerializeField] private Button _minusBtn;

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _purchasBtn;

    public Action sellAction;

    private int _count = 0;
    public int Count
    {
        get => _count;
        set
        {
            _count = Mathf.Clamp(value, 1, (int)_countSlider.maxValue);
            _countSlider.value = _count;
            _countText.text = $"판매할 수량 : {_count}개";

            if (SeletedItem != null)
                _priceText.text = $"받는 금액 : {SeletedItem.price * Count}G";
        }
    }

    private ItemData _seletedItem = null;
    public ItemData SeletedItem
    {
        get => _seletedItem;
        set
        {
            _seletedItem = value;
            DisplayRefresh();
        }
    }
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _purchasBtn.onClick.AddListener(OnBtnPurchase);

        _plusBtn.onClick.AddListener(() => Count++);
        _minusBtn.onClick.AddListener(() => Count--);

        _countSlider.minValue = 0;
        //_countSlider.maxValue = 10;
        _countSlider.onValueChanged.AddListener((float value) => Count = (int)value);

        Count = 0;

        return true;

    }

    public void DisplayRefresh()
    {
        // 슬라이더 최대치를 현재 가진 템 수만큼 바꾸고 슬라이더 1로 변경

        _itemIconImage.sprite = Main.CSVData.itemDatas[_seletedItem.id].iconImage;
        _itemNametext.text = Main.CSVData.itemDatas[_seletedItem.id].itemName;
        _itemBufftext.text = Main.CSVData.itemDatas[_seletedItem.id].type.ToString();
        _itemDescriptiontext.text = Main.CSVData.itemDatas[_seletedItem.id].description;

        Count = Count;
    }

    public void OnBtnPurchase()
    {
        if (SeletedItem != null)
        {
            DoYouSellIt();
        }
    }

    private UI_Popup_Decision _decision;
    private string[] sellIt_textArray = new string[] { "판매하시겠습니까?", "네", "아니요" };

    private void DoYouSellIt()
    {
        _decision = Main.UI.OpenPopup<UI_Popup_Decision>();
        _decision.decisionAction += SellIt;
        _decision.OpenDecision(ref sellIt_textArray);

    }

    void SellIt(bool accept)
    {
        if (accept)
        {
            Main.Inven.Delete(SeletedItem, _count);
            Main.Player.playerData.gold += (SeletedItem.price * Count);
            Main.Save.SaveToJson_PlayerData();
            sellAction?.Invoke();
        }
    }

}
