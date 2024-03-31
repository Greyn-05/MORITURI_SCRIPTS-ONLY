using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BossSelectSlot : UI_Base
{
    [HideInInspector] public UI_Popup_BossSelect uiBossSelect;
    [HideInInspector] public int bossNumber;

    public TextMeshProUGUI bossName;
    public Button slotBtn;
    public Image bossIconImage;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        slotBtn.onClick.AddListener(OnSelectButton);

        return true;
    }


    public void Unlock()
    {

        if (Main.Player.playerData.isBossRelease[bossNumber])
        {
            slotBtn.interactable = true;
            bossIconImage.sprite = Main.Resource.Load<Sprite>(Define.bossImageFile[bossNumber]);
            bossName.text = $"{Define.bossName[bossNumber]}";
            // 해당 버튼 종료 제대로 나오게
        }
        else
        {
            slotBtn.interactable = false;
            // 버튼에 수수께기가 가득.

            bossName.text = $"???";
        }
    }


    public void OnSelectButton() // 이 버튼을 눌렀을때
    {
        uiBossSelect.OnSelectButton(bossNumber);
    }


}
