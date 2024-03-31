using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : UI_Base
{
    public int slotNumber;
    public UI_Popup_Inventory invenPopUpUi;
    private Image _image;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _image = GetComponent<Image>();
       // _image.color = Color.white;
        BindEvent(this.gameObject, OnMousePointerEnter, UIEvent.PointerEnter);
        BindEvent(this.gameObject, OnMousePointerExit, UIEvent.PointerExit);
        BindEvent(this.gameObject, OnMousePointerDrop, UIEvent.Drop);

        return true;
    }

    public void RefreshInventorySlot()
    {

        if (transform.childCount == 0) // 자식이 있음 = 템 들어있음
        {
            if (Main.Inven.inventory[slotNumber].item != null)
            {
                ItemSlot slot = Main.Resource.InstantiatePrefab("ItemSlot", transform).GetComponent<ItemSlot>();
                slot.currentSlotNumber = slotNumber;
                slot._invenPopUpUi = invenPopUpUi;
            }
        }
        else
        {
            if (transform.GetChild(0).TryGetComponent(out ItemSlot slot))
            {
                slot.DisplayRefresh();
            }
        }
    }

    private void OnMousePointerEnter(PointerEventData eventData)
    {
        //_image.color = Color.green;
    }

    private void OnMousePointerExit(PointerEventData eventData)
    {
       // _image.color = Color.white;
    }

    private void OnMousePointerDrop(PointerEventData eventData)
    {

        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (eventData.pointerDrag != null)  // pointerDrag는 현재 드래그하고 있는 오브젝트
        {
            if (eventData.pointerDrag.TryGetComponent(out ItemSlot slot))   // 드래그중인게 itemslot 컴포넌트를 갖고있을때만 실행. 
            {
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.transform.position = transform.position;

                slot.Swap<InventorySlot>(slotNumber);

            }
        }
    }

    //이미 템이 있는 슬롯은 템이 레이캐스트를 막아서 drop이 안되는듯. 드래그시작되면 모든 아이템칸 레이캐스트 꺼야할듯?


}
