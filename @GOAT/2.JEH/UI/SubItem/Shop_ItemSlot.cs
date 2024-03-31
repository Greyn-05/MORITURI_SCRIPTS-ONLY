using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_ItemSlot : UI_Base
{

    [HideInInspector] public UI_Popup_Shop uiPopupShop;


    [HideInInspector] public ItemData _item; // 이 슬롯이 가지고있는 아이템의 ID

    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _haveText; // 가진 갯수

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        if (_item != null)
        {
            _itemNameText.text = _item.itemName;
            _iconImage.sprite = _item.iconImage;
            _priceText.text = $"{_item.price}G";
            _haveText.text = $"보유수 : {Main.Inven.HowManyThisItemYouHave(_item.id)}";

        }


        uiPopupShop.info.PurchasAction -= AmountTextRefresh;
        uiPopupShop.info.PurchasAction += AmountTextRefresh;

        BindEvent(this.gameObject, OnMousePointerClick);


        return true;
    }

    private void OnMousePointerClick(PointerEventData eventData)
    {
        if (_item != null)
        {
            uiPopupShop.info.SeletedItem = _item;
            uiPopupShop.info.Count = 1;
            uiPopupShop.info.DisplayRefresh();
        }
    }


    private void AmountTextRefresh()
    {
        if (_item != null)
            _haveText.text = $"보유수 : {Main.Inven.HowManyThisItemYouHave(_item.id)}";

    }



    private void OnDisable()
    {
        uiPopupShop.info.PurchasAction -= AmountTextRefresh;
    }

}