using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour
{
    public enum UIEvent
    {
        Click,
        Press,
        PointerDown,
        PointerUp,
        PointerEnter,
        PointerExit,
        Drag,
        BeginDrag,
        EndDrag,
        Drop

    }

    private bool initialized = false;

    private void Start()
    {
        Initialize();
    }


    public virtual bool Initialize()
    {
        if (initialized) return false;


        if (GameObject.Find("EventSystem") == null)
        {
            GameObject obj = new() { name = "EventSystem" };
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>();
        }


        initialized = true;
        return true;
    }


    public static void BindEvent(GameObject obj, Action<PointerEventData> action = null, UIEvent type = UIEvent.Click)
    {
        UI_EventHandler evt = Utilities.GetOrAddComponent<UI_EventHandler>(obj);

        switch (type)
        {
            case UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case UIEvent.PointerEnter:
                evt.OnPointerEnterHandler -= action;
                evt.OnPointerEnterHandler += action;
                break;
            case UIEvent.PointerExit:
                evt.OnPointerExitHandler -= action;
                evt.OnPointerExitHandler += action;
                break;
            case UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= action;
                evt.OnBeginDragHandler += action;
                break;
            case UIEvent.EndDrag:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
            case UIEvent.Drop:
                evt.OnDropHandler -= action;
                evt.OnDropHandler += action;
                break;
        }
    }
}