using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    private bool initialized = false;

    public List<SlotData> inventory = new(); // 실제 인벤토리
    private int _invenSlotInUseCount = 0; // 사용중인 인벤토리 칸 갯수
    //  public Action InventoryChangedAction;
    //  public List<int> cooldownItemIds = new(); // 쿨타임돌고있는 아이템의 사용 막기

    public int maxCapacity = 99; // 임시 한 칸에 들어가는 아이템 최대갯수. 아이템별로 다르게 할것.

    public UI_Popup_Inventory invenPopUpUi = null; // 팝업내용물 새로고침하는용.

    public Action QuickSlotChangedAction;
    private int _quickSlotIndex = 0;
    public int QuickSlotIndex
    {
        get => _quickSlotIndex;
        set
        {
            _quickSlotIndex = value;

            if (_quickSlotIndex >= Define.maxquickSlotsCount)
                _quickSlotIndex = 0;
            else if (_quickSlotIndex < 0)
                _quickSlotIndex = Define.maxquickSlotsCount - 1;

            QuickSlotChangedAction?.Invoke();
        }
    }


    public void Initialize()
    {
        if (initialized) return;
        GenerateInvenSlot();

        initialized = true;
    }

    private void GenerateInvenSlot()
    {
        for (int i = 0; i < Define.maxInvenSlotCount; i++)
        {
            SlotData slotData = new();
            slotData.item = (Main.Player.playerData.inventoryId[i] < 0) ? null : Main.CSVData.itemDatas[Main.Player.playerData.inventoryId[i]];
            slotData.capacity = (slotData.item == null) ? 0 : Main.Player.playerData.inventoryCap[i];

            inventory.Add(slotData);
        }
    }

    public void Open()
    {
        invenPopUpUi = Main.UI.OpenPopup<UI_Popup_Inventory>();
    }

    public void Close()
    {
        if (invenPopUpUi != null)
            Main.UI.ClosePopup(invenPopUpUi);

        invenPopUpUi = null;
    }


    public bool Add(ItemData item) // 인벤에 아이템추가
    {
        if (item == null) return false;

        //      int itemindex = _inventory.FindIndex(data => data.item != null && data.item.id.Equals(item.id));  

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item != null && inventory[i].item.id == item.id)
            {
                if (inventory[i].capacity < maxCapacity)
                {
                    inventory[i].capacity++;

                    if (invenPopUpUi != null) 
                        invenPopUpUi.RefreshSlot(i);
                   
                    //   InventoryChangedAction?.Invoke();
                    Main.Quest.UpdateQuestItemCount(item.id);

                    Main.Player.playerData.inventoryCap[i] = inventory[i].capacity;
                    Main.Save.SaveToJson_PlayerData();

                    return true;
                }
            }
        }

        if (_invenSlotInUseCount < Define.maxInvenSlotCount)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].item == null)
                {
                    inventory[i].item = item;
                    inventory[i].capacity++;
                    _invenSlotInUseCount++;

                    if (invenPopUpUi != null) 
                        invenPopUpUi.RefreshSlot(i);

                    //  InventoryChangedAction?.Invoke();
                    Main.Quest.UpdateQuestItemCount(item.id);

                    Main.Player.playerData.inventoryId[i] = inventory[i].item.id;
                    Main.Player.playerData.inventoryCap[i] = inventory[i].capacity;
                    Main.Save.SaveToJson_PlayerData();

                    return true;
                }
            }
        }
        else
        {
            return false; // 인벤토리 공간부족
        }

        return false;
    }

    public bool Delete(ItemData item) // 인벤에서 같은 id템 삭제
    {
        if (item == null) return false;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item != null && inventory[i].item.id == item.id)
            {
                inventory[i].capacity--;

                if (inventory[i].capacity <= 0)
                {
                    inventory[i].item = null;
                    inventory[i].capacity = 0;

                    Main.Player.playerData.inventoryId[i] = -1;
                    _invenSlotInUseCount--;
                }

                if (invenPopUpUi != null) invenPopUpUi.RefreshSlot(i);

                //  InventoryChangedAction?.Invoke();
                Main.Quest.UpdateQuestItemCount(item.id);

                Main.Player.playerData.inventoryCap[i] = inventory[i].capacity;
                Main.Save.SaveToJson_PlayerData();

                return true;
            }
        }

        return false;

    }

    public List<int> ShowMeInventoryItems() // TODO 가진템 id 리스트주기
    {
        List<int> keep = new();

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item != null && !keep.Contains(inventory[i].item.id))
                keep.Add(inventory[i].item.id);
        }

        //for (int i = 0; i < keep.Count; i++)
        //{
        //    Debug.Log($"{Main.CSVData.itemDatas[keep[i]].itemName} : {HowManyThisItemYouHave(keep[i])}개");
        //}

        return keep;

    }

    public int HowManyThisItemYouHave(int id) // 해당 아이템을 몇개나 갖고있는지.
    {
        int count = 0;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item != null && inventory[i].item.id == id)
                count += inventory[i].capacity;
        }

        return count;
    }

    public bool CanYouAllLiftItUp(int id, int count) // 해당아이템을 다 받을수있는지
    {
        int leftSlot = Define.maxInvenSlotCount - _invenSlotInUseCount;

        if (leftSlot == 0)
        {
            if (HowManyThisItemYouHave(id) % maxCapacity == 0) // 모자란칸도 없다면
                return false;
        }

        if (leftSlot * maxCapacity >= count)
        {
            return true;
        }
        else
        {
            int leftCapacity = count - (leftSlot * maxCapacity); // 더 채워야하는 템수
            int deficient = HowManyThisItemYouHave(id) % maxCapacity; // 최대적재량을 나누고 나온 나머지는 꽉차지않은 해당 아이템 슬롯이라는것

            if ((maxCapacity - deficient) >= leftCapacity)
                return true;
        }

        return false; // 인벤토리 자리부족
    }


    // TODO 임시 여러개 추가 삭제  ■■■■■■■■■■■■■■■■■■■■■■

    public bool Add(ItemData item, int count)
    {
        if (item == null) return false;

        for (int i = 0; i < count; i++)
            if (!Add(item)) return false;

        return true;

    }

    public bool Delete(ItemData item, int count)
    {
        if (item == null) return false;
        if (HowManyThisItemYouHave(item.id) < count) return false;

        for (int i = 0; i < count; i++)
            if (!Delete(item)) return false;

        return true;

    }

    // TODO 퀵슬롯 ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    public void QuickSlotItemUse()
    {
        if (IsQuickSlotEmpty()) return;

        if (inventory[_quickSlotIndex].item.Use())
        {
            QuickSlotChangedAction?.Invoke();
            return;
        }
        else
        {
            return;
        }
    }

    public bool IsQuickSlotEmpty()
    {
        return inventory[_quickSlotIndex].item == null;
    }

}
