using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Inventory : UI_Popup
{

    // 인벤토리는 인벤토리매니저가 관리. 여긴 인벤토리를 UI로 보여주는곳

    [HideInInspector] public UI_Popup_Tooltip _tooltip;

    [SerializeField] private GameObject _slotsObj;
    [SerializeField] private Button _closeBtn;

    [HideInInspector] public List<InventorySlot> inventorySlotList = new();

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _closeBtn.onClick.AddListener(OnBtnClose);
        _tooltip = Main.UI.SetSubItemUI<UI_Popup_Tooltip>();

        for (int i = 0; i < _slotsObj.transform.childCount; i++)
        {
            if (_slotsObj.transform.GetChild(i).TryGetComponent<InventorySlot>(out var inventorySlot))
            {
                inventorySlot.invenPopUpUi = this;
                inventorySlot.slotNumber = i;
                inventorySlot.RefreshInventorySlot();

                if (!inventorySlotList.Contains(inventorySlot))
                    inventorySlotList.Add(inventorySlot);
            }
        }

        return true;

    }

    public void RefreshSlot(int num)
    {
        inventorySlotList[num].RefreshInventorySlot();

    }

    private void OnDisable()
    {
        if (_tooltip != null)
            Main.UI.DestroySubItemUI<UI_Popup_Tooltip>(_tooltip.gameObject);
    }
}

