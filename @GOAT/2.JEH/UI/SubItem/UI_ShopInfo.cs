using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopInfo : UI_Base
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

    public Action PurchasAction;

    private int _count = 0;
    public int Count
    {
        get => _count;
        set
        {
            _count = Mathf.Clamp(value, 1, (int)_countSlider.maxValue);
            _countSlider.value = _count;
            _countText.text = $"구매수량 : {_count}개";

            if (SeletedItem != null)
                _priceText.text = $"총 금액 : {SeletedItem.price * Count}G";
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
        _countSlider.maxValue = 10;
        _countSlider.onValueChanged.AddListener((float value) => Count = (int)value);

        Count = 0;

        return true;

    }
    

    public void OnBtnPurchase()
    {
        if (SeletedItem != null)
        {

            if (Main.Player.playerData.gold - (SeletedItem.price * Count) < 0) // TODO  gold 프로퍼티로 바꿀것
            {
                // Debug.Log("소지금이 부족합니다.");
            }
            else
            {
                if (Main.Inven.CanYouAllLiftItUp(SeletedItem.id, _count))
                {
                    DoYouBuyIt();
                }
                else
                {
                    // Debug.Log("아이템공간이 부족합니다.");
                }
            }
        }
    }


    public void DisplayRefresh()
    {
        _itemIconImage.sprite = Main.CSVData.itemDatas[_seletedItem.id].iconImage;
        _itemNametext.text = Main.CSVData.itemDatas[_seletedItem.id].itemName;
        _itemBufftext.text = Main.CSVData.itemDatas[_seletedItem.id].type.ToString();
        _itemDescriptiontext.text = Main.CSVData.itemDatas[_seletedItem.id].description;

        Count = Count;
    }


    private UI_Popup_Decision _decision;
    private string[] buyIt_textArray = new string[] { "구매하시겠습니까?", "네", "아니요" };

    private void DoYouBuyIt()
    {
        _decision = Main.UI.OpenPopup<UI_Popup_Decision>();

        _decision.decisionAction += BuyIt;
        _decision.OpenDecision(ref buyIt_textArray);

    }

    void BuyIt(bool accept)
    {
        if (accept)
        {
            Main.Inven.Add(SeletedItem, _count);

          //  Debug.Log($"{SeletedItem.itemName}을 {_count}개 구매, {SeletedItem.price * Count}원 소모");
            Main.Player.playerData.gold -= (SeletedItem.price * Count);

            Main.Quest.UpdateGoldCount(Main.Player.playerData.gold);
            Main.Save.SaveToJson_PlayerData();

            PurchasAction?.Invoke();
        }
        else
        {
          //  Debug.Log("안사요");
        }
    }

}
