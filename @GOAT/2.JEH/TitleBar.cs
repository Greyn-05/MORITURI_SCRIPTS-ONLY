using UnityEngine;
using UnityEngine.EventSystems;

public class TitleBar : UI_Base
{
    // 팝업창에서 드래그되는 부분에 연결하세요

    private Vector2 startPos;
    private Vector2 moveBegin;
    private Vector2 moveOffset;

    private void Awake()
    {
        BindEvent(this.gameObject, OnBeginDrag, UIEvent.BeginDrag);
        BindEvent(this.gameObject, OnDrag, UIEvent.Drag);

    }

    void OnBeginDrag(PointerEventData eventData) // 팝업을 최상위로 올리기
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        startPos = transform.position;
        moveBegin = eventData.position;

        if (transform.parent.TryGetComponent(out UI_Popup popup))
        {
          //  Debug.Log(popup.gameObject.name);
            Main.UI.RaisePopup(popup);

        }
    }

    void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;


        moveOffset = eventData.position - moveBegin;
        transform.position = startPos + moveOffset;
    }

}