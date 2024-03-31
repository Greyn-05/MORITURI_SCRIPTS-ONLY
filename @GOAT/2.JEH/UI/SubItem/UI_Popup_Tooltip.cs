using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Tooltip : UI_Base
{

    private int _characterWrapLimit = 500;

    [SerializeField] private GameObject _tooltipObj;
    private RectTransform _rectTransform;
    private LayoutElement _layoutElement;

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private Vector2 _position;
    private float _pivotX;
    private float _pivotY;

    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _contentText;

    [SerializeField] private TextMeshProUGUI _abilityText;

    private void FixedUpdate()
    {
        MovePosition();
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = _tooltipObj.GetComponent<RectTransform>();
        _layoutElement = _tooltipObj.GetComponent<LayoutElement>();
        _position = Input.mousePosition;

        _canvas.sortingOrder = 1000;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;

        return true;
    }


    private void MovePosition()
    {
        _position = Input.mousePosition;

        _pivotX = _position.x / Screen.width;
        _pivotY = _position.y / Screen.height;

        _rectTransform.pivot = new Vector2(_pivotX, _pivotY);
        _tooltipObj.transform.position = _position;
    }

    public void Show(string content, string header = null)
    {
        Initialize();

        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.2f);

        _headerText.text = header;
        _contentText.text = content;
        _contentText.gameObject.SetActive(true);

        _iconImage.gameObject.SetActive(false);

        _layoutElement.enabled = _headerText.preferredWidth > _characterWrapLimit || _contentText.preferredWidth > _characterWrapLimit; // 텍스트길면 LayoutElement 활성화해서 글에 자동으로 엔터넣기

    }
    public void Show(ItemData item)
    {

        if (item == null) return;

        Initialize();


        _canvasGroup.alpha = 0;

        _headerText.text = item.itemName;
        _contentText.text = item.description;
        _abilityText.text = item.type.ToString();

        _contentText.gameObject.SetActive(true);

        _iconImage.sprite = item.iconImage;
        _iconImage.gameObject.SetActive(true);

        _layoutElement.enabled = _headerText.preferredWidth > _characterWrapLimit || _contentText.preferredWidth > _characterWrapLimit;


       // _canvasGroup.DOFade(1, 0.2f); 
       _canvasGroup.alpha = 1;


    }

    public void Hide()
    {
       //     _canvasGroup.DOFade(0, 0.1f);
        _canvasGroup.alpha = 0; 
    }


}

