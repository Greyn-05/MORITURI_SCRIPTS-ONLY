using TMPro;
using UnityEngine;

public class PlayDataSlot : UI_Base
{


    [HideInInspector] public int slotNumbr = 0; // 몇번 칸인지.

    public GameObject noDataPanel;


    [SerializeField] TextMeshProUGUI slotNumberText;


    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI saveTimeText;
    [SerializeField] TextMeshProUGUI goldText;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //자기 슬롯번호에 맞는 세이브파일이 존재하면 그 데이터로 갱신하고 없으면 그냥 노데이터 띄우기






        return true;
    }

    public void Refresh()
    {
        slotNumberText.text = $"{slotNumbr}";

        noDataPanel.SetActive(true);


    }

}