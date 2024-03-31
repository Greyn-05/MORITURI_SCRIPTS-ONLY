using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Shop : UI_Popup
{
    [HideInInspector] public List<int> productList = new(); // 상점이 파는 아이템목록 중복템 없어야함. 템 id만 가지고있으면 될듯.
    [SerializeField] TMP_Text _shopName;
    [SerializeField] private Button _closeBtn;
    [SerializeField] TMP_Text _goldText;

    [SerializeField] Transform _slotsTransform; // 샵 판매목록 넣는칸
    public UI_ShopInfo info;

    List<int> myItems = new(); //소지한템id
    [SerializeField] Transform _sellTransform; // 소지템 넣는칸
    public UI_SellInfo sellInfo;

    [SerializeField] private Button _sellBtn;
    [SerializeField] private Button _buyBtn;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        productList.Clear(); // 임시 모든템 상점에 추가
        for (int i = 0; i < Main.CSVData.itemDatas.Count; i++)
        {
            if (Main.CSVData.itemDatas.Count - 1 != i)
                productList.Add(Main.CSVData.itemDatas[i].id);
        }

        _closeBtn.onClick.AddListener(OnBtnClose);

        _sellBtn.onClick.AddListener(SellPanel);
        _buyBtn.onClick.AddListener(BuyPanel);

        GoldTextRefresh();

        info.PurchasAction -= GoldTextRefresh;
        info.PurchasAction += GoldTextRefresh;

        GenerateSlot();

        return true;
    }

    private void GenerateSlot()
    {
        for (int i = 0; i < productList.Count; i++)
        {
            if (i == 0) info.SeletedItem = Main.CSVData.itemDatas[productList[i]];


            Shop_ItemSlot slot = Main.Resource.InstantiatePrefab("Shop_ItemSlot").GetComponent<Shop_ItemSlot>();
            slot.uiPopupShop = this;
            slot._item = Main.CSVData.itemDatas[productList[i]];
            slot.transform.SetParent(_slotsTransform);
            slot.transform.localPosition = Vector3.zero;
            slot.transform.localScale = Vector3.one;
            slot.Initialize();
        }
    }


    public void GoldTextRefresh()
    {
        _goldText.text = $"소지금 : {Main.Player.playerData.gold}G";
    }
    private void OnDisable()
    {
        info.PurchasAction -= GoldTextRefresh;
    }


    private void SellPanel()
    {
        //  Debug.Log("판매");

    }
    private void BuyPanel()
    {

        //  Debug.Log("구매");
    }

    public void GenerateSellSlot() //내 인벤 물건 추가
    {
        myItems = Main.Inven.ShowMeInventoryItems();

        for (int i = 0; i < myItems.Count; i++)
        {
            Shop_SellSlot slot = Main.Resource.InstantiatePrefab("Shop_SellSlot").GetComponent<Shop_SellSlot>();
            slot.uiPopupShop = this;
            slot._item = Main.CSVData.itemDatas[myItems[i]];
            slot.transform.SetParent(_sellTransform);
            slot.transform.localPosition = Vector3.zero;
            slot.transform.localScale = Vector3.one;
            slot.Initialize();
        }
    }


}
