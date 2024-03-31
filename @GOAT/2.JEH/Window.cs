using UnityEngine;
using UnityEngine.EventSystems;

public class Window : UI_Popup
{


    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        BindEvent(this.gameObject, OnMouseButtonDown, UIEvent.PointerDown);

        return true;

    }

    public void Open()
    {
        this.gameObject.SetActive(true);

        int d = transform.parent.childCount;
        transform.SetSiblingIndex(d - 1);

        Vector2 vec = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));
        transform.localPosition = vec;

        //새창 열릴때마다 중앙근처에서 열리게했음. 다른 겜은 마지막 창위치 저장하고 그대로염
        // 창 제목표시줄이 화면 밖으로 안나가게 해야할듯? 안그럼 창밖에 나가면 못뺌
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }


    void OnMouseButtonDown(PointerEventData eventData)
    {
        int d = transform.parent.childCount;
        transform.SetSiblingIndex(d - 1);

        // 제목표시줄에 똑같은 코드있는데 여기다도 쓰는이유
        // 제목표시줄말고 그냥 창 아무대나 만져도 맨위로 올라와야하는데 그게 안되서. 제목표시줄 레이캐스트타겟이 버튼클릭을 막는듯

    }

}
