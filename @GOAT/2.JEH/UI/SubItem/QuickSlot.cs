using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : UI_Base
{
    [HideInInspector] public int slotId; // 슬롯번호

    [SerializeField] private TextMeshProUGUI _capacityText;
    [SerializeField] private Image _iconSlotImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _iconImageShadow;
    [SerializeField] private GameObject _shadowObj;


    [SerializeField] private TextMeshProUGUI _slotIdText;
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        //  BindEvent(this.gameObject, OnMouseButtonDown, UIEvent.PointerDown);


        return true;
    }

    public void DisplayRefresh()
    {


        _slotIdText.text = $"{slotId +1}";

        if (Main.Inven.inventory[slotId].item == null)
        {
            // Debug.Log(slotId + "의 칸이 비어있습니다");
            _iconSlotImage.fillAmount = 0;
            _shadowObj.SetActive(false);
            _capacityText.text = string.Empty;
            return;
        }

        _iconSlotImage.fillAmount = 1;
        _shadowObj.SetActive(true);
        _capacityText.text = $"{Main.Inven.inventory[slotId].capacity}";

        _iconImage.sprite = Main.Inven.inventory[slotId].item.iconImage;
        _iconImageShadow.sprite = _iconImage.sprite;

    }



}
