using UnityEngine;

public class ItemData
{
    public int id;
    public string itemName;
    public string description;
    public int price;
    public Sprite iconImage;


    public Define.ItemType type;

    public bool isOverlap; // 겹치는 템인지. 
    public int capacity; // 겹쳐지는 갯수

    // 버프 디버프 일단 1개만 작동

    public bool isConsumable; // 소모품인지 (퀵슬롯에 넣을수있는템인지)
    public float cooldown; // 쿨타임
    public float durationTime; // 지속시간
    public float repeatedTime; // 반복주기

    public string buff_name;
    public string buff_value;

    public string debuff_name;
    public string debuff_value;


    public bool Use()
    {
        //  if (!isConsumable) return false;

        switch (itemName)
        {
            case "회복포션":
                Main.Player.Status.HP.AddValue(30);
                Main.Player.OnHeal?.Invoke();
                break;
            case "튜토리얼포션":
                Main.Player.Status.HP.AddValue(500);
                Main.Player.OnHeal?.Invoke();
                break;
            case "스태미나포션":
                Main.Player.Status.Stamina.AddValue(30);
                break;
        }
        

      //  Debug.Log(itemName + "사용합니다");

        Main.Inven.Delete(this);

        // 자기타입, 자기 능력치 등등 적용. 템 능력적용은 따른 스크립트에서 하는게 나을지도?


        return true;
    }

}




public enum BuffType
{
    hp,
    stamina,
    atk,
    def,
    speed,
    maxHp,
    maxStamina

}


/*
 
모든 버프 종류


value / percentage 값인지 퍼센트인지
instant /  constantly  즉발인지 지속효과인지

hp
stamina
atk
def
speed

maxHp
maxStamina

 
 
 */