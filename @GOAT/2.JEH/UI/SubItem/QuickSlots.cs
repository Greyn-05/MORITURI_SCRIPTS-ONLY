using UnityEngine;

public class QuickSlots : UI_Base
{
    public GameObject[] slotPosition = new GameObject[4]; // 슬롯자리
    public QuickSlot[] slots = new QuickSlot[4]; 

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotId = i;
        }

        Main.Inven.QuickSlotChangedAction -= ChangeQuickSlot;
        Main.Inven.QuickSlotChangedAction += ChangeQuickSlot;

        Main.Inven.QuickSlotChangedAction?.Invoke();

        return true;
    }

    private void ChangeQuickSlot() //TODO IF문 안쓰게 바꿀것
    {
      //  Debug.Log("퀵슬롯 자리바꿈");
        int[] ints;

        switch (Main.Inven.QuickSlotIndex)
        {
            default:
            case 0:
                ints = new int[] { 0, 1, 2, 3 };
                break;
            case 1:
                ints = new int[] { 1, 2, 3, 0 };
                break;
            case 2:
                ints = new int[] { 2, 3, 0, 1 };
                break;
            case 3:
                ints = new int[] { 3, 0, 1, 2 };
                break;
        }


        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].transform.SetParent(slotPosition[ints[i]].transform);
            slots[i].slotId = ints[i];

        }

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].DisplayRefresh();
        }


    }

    private void OnDisable()
    {
        Main.Inven.QuickSlotChangedAction -= ChangeQuickSlot;
    }
}