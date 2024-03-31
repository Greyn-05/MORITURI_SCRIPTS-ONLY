using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIManager
{
    private GameObject UIRoot
    {
        get
        {
            GameObject uiRoot = GameObject.Find("@UI_Root");
            if (uiRoot == null)
            {
                uiRoot = new GameObject { name = "@UI_Root" };
            }
            return uiRoot;
        }
    }

    private int _orderByLayer = 10;


    /// <summary>
    /// 
    /// ESC로 가장위에있는 팝업창 닫으려면 리스트를 정렬해야함.
    /// 
    /// X버튼이나 해당창단축키로 끌때는 그냥 꺼짐. 해당창이 최상위가 아닌데 그창 버튼을 또눌렀으면  최상위로 올리기?
    /// 
    /// 
    /// 인풋시스템에서 같은 창 여는걸 또 눌렀을떄. 이미 창이 열려있다면 열기가 아니라 닫기를 해야한다.
    /// 
    /// 
    /// </summary>


    public List<UI_Popup> _popupList = new();

    public UI_Base SceneUI { get; private set; }

    public event Action OnUIPopupOpenEvent;
    public event Action OnUIPopupCloseEvent;

    #region Scene
    public T SetSceneUI<T>() where T : UI_Scene
    {
        string sceneUIName = typeof(T).Name;
        SceneUI = SetUI<T>(sceneUIName, UIRoot.transform);

        OrderLayerToCanvas(SceneUI.gameObject, false);

        return (T)SceneUI;
    }

    #endregion

    #region Popup


    public T OpenPopup<T>() where T : UI_Popup
    {
        string uiName = typeof(T).Name;
        string ui = NameOfUI<T>(uiName);

        int tmp = _popupList.FindIndex(pop => pop.name.Equals(ui));

        if (tmp != -1)
        {
            ClosePopup(_popupList[tmp]);
            return null;
        }


        T popup = SetUI<T>(ui, UIRoot.transform);
        popup.name = $"{uiName}";

        OrderLayerToCanvas(popup.gameObject);
        popup.SortOrder = _orderByLayer; // 최상위창읽기용 솔트오더 변수 입력

        _popupList.Add(popup);

        OnUIPopupOpenEvent?.Invoke();
        Main.Player.CursorLock_None();
        Main.Cinemachne.MenuOnCamera();
        return popup;
    }


    public void ClosePopup(UI_Popup popup)
    {
        _popupList.Remove(popup);

        // _orderByLayer--;
        Object.Destroy(popup.gameObject);

        if (_popupList.Count <= 0)
        {
            _orderByLayer = 10; // 팝업창 없으면 기본값으로 돌림
            OnUIPopupCloseEvent?.Invoke();
            Main.Player.CursorLock_Locked();
            Main.Cinemachne.MenuOffCamera();
        }
    }


    public void RaisePopup(UI_Popup popup) // 이미 열린 팝업의 솔트오더만 위로 올리기
    {
        Canvas canvas = Utilities.GetOrAddComponent<Canvas>(popup.gameObject);
        SortingOrder(canvas);
        popup.SortOrder = _orderByLayer;

        // 이미 열려있는데 여는키 누르면 -> 최상위가 아니면 최상위로 띄우고?  아니면 창끄기? 
    }

    public bool CloseTopPopup() // 최상위 팝업 끄기 끌 창이없거나 끄기 실패하면 false. 인풋에서 읽을것임
    {
        if (_popupList.Count <= 0) return false;


        int index = 0;

        for (int i = 1; i < _popupList.Count; i++)
        {
            if (_popupList[index].SortOrder < _popupList[i].SortOrder)

                index = i;
        }
        ClosePopup(_popupList[index]);
        return true;

        //int index = _popupList.FindIndex(pop => pop.SortOrder == _orderByLayer);

        //if (index != -1)
        //{
        //    ClosePopup(_popupList[index]);

        //    return true;
        //}

        //  return false;

    }


    #endregion

    #region SubItem

    public T SetSubItemUI<T>(Transform parent = null) where T : UI_Base
    {
        string subitemUIName = typeof(T).Name;
        return SetUI<T>(subitemUIName, parent);
    }

    public void DestroySubItemUI<T>(GameObject obj) where T : UI_Base
    {
        Object.Destroy(obj);
    }


    #endregion




    private string NameOfUI<T>(string uiName)
    {
        return string.IsNullOrEmpty(uiName) ? typeof(T).Name : uiName;
    }

    private T SetUI<T>(string uiName, Transform parent = null) where T : Component
    {
        GameObject uiObject = Main.Resource.InstantiatePrefab(uiName, parent);
        T ui = Utilities.GetOrAddComponent<T>(uiObject);
        return ui;
    }


    public void OrderLayerToCanvas(GameObject uiObject, bool sort = true)
    {
        Canvas canvas = Utilities.GetOrAddComponent<Canvas>(uiObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        SortingOrder(canvas, sort);

        CanvasScaler scales = Utilities.GetOrAddComponent<CanvasScaler>(canvas.gameObject);
        scales.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scales.referenceResolution = new Vector2(1920, 1080);


        Utilities.GetOrAddComponent<GraphicRaycaster>(uiObject);

        canvas.referencePixelsPerUnit = 100;
    }

    private void SortingOrder(Canvas canvas, bool sort = true) // 씬 여러개 한번에 띄울때 bool 이 필요할지도?
    {
        canvas.sortingOrder = sort ? _orderByLayer++ : 0;
    }

}