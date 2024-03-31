using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : UI_Base
{
    [HideInInspector] public UI_Popup_Inventory _invenPopUpUi;
    public int currentSlotNumber;

    [SerializeField] private TextMeshProUGUI _capacityText;
    [SerializeField] private Image _iconImage;

    private Transform previousParent;
    private CanvasGroup canvasGroup;

    private bool wasItDragged = false;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        BindEvent(this.gameObject, OnMousePointerClick, UIEvent.Click);
        BindEvent(this.gameObject, OnMousePointerEnter, UIEvent.PointerEnter);
        BindEvent(this.gameObject, OnMousePointerExit, UIEvent.PointerExit);
        BindEvent(this.gameObject, OnMousePointerDown, UIEvent.PointerDown);
        BindEvent(this.gameObject, OnMousePointerBeginDrag, UIEvent.BeginDrag);
        BindEvent(this.gameObject, OnMousePointerDrag, UIEvent.Drag);
        BindEvent(this.gameObject, OnMousePointerEndDrag, UIEvent.EndDrag);

        //  Main.Inven.InventoryChangedAction -= DisplayRefresh;
        //  Main.Inven.InventoryChangedAction += DisplayRefresh;

        DisplayRefresh();

        return true;
    }

    public void DisplayRefresh()
    {
        _iconImage.sprite = Main.Inven.inventory[currentSlotNumber].item.iconImage;
        _capacityText.text = $"{Main.Inven.inventory[currentSlotNumber].capacity}";
    }

    public bool UseItemInSlot()
    {
        if (Main.Inven.inventory[currentSlotNumber] == null || Main.Inven.inventory[currentSlotNumber].item == null) return false;

        if (Main.Inven.inventory[currentSlotNumber].item.Use())
            return true;

        return false;
    }


    private void OnMousePointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
        }
        else
        {
            //  if (!UseItemInSlot())    Debug.Log("아이템사용실패");
        }

    }

    private void OnMousePointerEnter(PointerEventData eventData)
    {
        if (wasItDragged)
        {
            wasItDragged = false;
            return;
        }

        if (!eventData.dragging)
        {
            _invenPopUpUi._tooltip.Show(Main.Inven.inventory[currentSlotNumber].item);
        }

    }

    private void OnMousePointerExit(PointerEventData eventData)
    {
        _invenPopUpUi._tooltip.Hide();

    }

    private void OnMousePointerDown(PointerEventData eventData)
    {
        _invenPopUpUi._tooltip.Hide();
    }


    private void OnMousePointerBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        wasItDragged = true;

        previousParent = transform.parent;

        transform.SetParent(_invenPopUpUi.gameObject.transform);
        transform.SetAsLastSibling(); // 맨아래 자식으로

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;

    }

    private void OnMousePointerDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        transform.position = eventData.position;
    }

    private void OnMousePointerEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (transform.parent == _invenPopUpUi.gameObject.transform) // 어떤 슬롯에서 ondrop이 안일어났으면 관련없는곳에서 드래그한거라 돌려보냄 
        {
            transform.SetParent(previousParent);
            transform.position = previousParent.position;
            wasItDragged = false;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }

    public void Swap<T>(int to)
    {
        int from = currentSlotNumber;
        var fromSlot = Main.Inven.inventory[from];
        var dragId = fromSlot.item.id;
        var dragCap = fromSlot.capacity;

        Main.Player.playerData.inventoryId[currentSlotNumber] = -1;
        Main.Player.playerData.inventoryCap[currentSlotNumber] = 0;

        Main.Inven.inventory[from] = Main.Inven.inventory[to];

        Main.Inven.inventory[to] = fromSlot;
        currentSlotNumber = to;
        Main.Player.playerData.inventoryId[to] = dragId;
        Main.Player.playerData.inventoryCap[to] = dragCap;

        Main.Save.SaveToJson_PlayerData();


    }


    //private void OnDisable()
    //{
    //    Main.Inven.InventoryChangedAction -= DisplayRefresh;
    //}

}
