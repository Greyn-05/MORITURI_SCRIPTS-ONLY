using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image _cardImage; // Card 프리팹에 있는 Image 컴포넌트
    
    private GameObject _target; // 카드 전달 지점
    private BlackJack _blackJackScript;
    
    private Sprite _frontSprite;
    private Sprite _backSprite;
  
    private Sprite[] _cardSides = new Sprite[2]; // 카드 앞/뒤 스프라이트
    private static List<Sprite> _allCardSprites = new();
    
    private RectTransform[] _playerPosition;
    private RectTransform[] _dealerPosition;
    
    private int _cardNumber; // 카드 번호(Rank 겸)
    
    private static bool _isSpritesLoaded = false;
    private bool _isMoving; // 애니메이션 진행 여부
    
    void Awake ()
    {
        _blackJackScript = GameObject.Find("BlackJackGame").GetComponent<BlackJack>();
        _cardNumber = Random.Range(1, 13); // 랜덤 카드 번호 할당
        
        if (!_isSpritesLoaded)
        {
            LoadAllCardSprites();
            _isSpritesLoaded = true;
        }

        if (_backSprite == null)
        {
            _backSprite = Resources.Load<Sprite>("Card/back/back");
            if (_backSprite == null)
            {
                //Debug.LogError("Back sprite not found!");
            }
        }

        SelectRandomCardFront();

        if (_cardImage != null && _backSprite != null)
        {
            _cardImage.sprite = _backSprite;
        }
        else
        {
            //Debug.LogError("Card image component or back sprite is missing!");
        }
    }
    
    public void SetPositions(RectTransform[] playerPos, RectTransform[] dealerPos)
    {
        _playerPosition = playerPos;
        _dealerPosition = dealerPos;
    }
    
    private void SelectRandomCardFront()
    {
        if (_allCardSprites.Count > 0)
        {
            int randomIndex = Random.Range(0, _allCardSprites.Count);
            _frontSprite = _allCardSprites[randomIndex];
            _allCardSprites.RemoveAt(randomIndex); // 중복을 방지하기 위해 제거
            _cardSides[1] = _frontSprite; // 랜덤하게 선택된 앞면
            _cardSides[0] = _backSprite; // 공통 뒷면
        }
        else
        {
            //Debug.LogError("No more cards available.");
        }
    }
    
    private void LoadAllCardSprites()
    {
        string[] suits = new[] { "Club", "Diamond", "Heart", "Spade" };
        string[] ranks = new[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13" };

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                string cardName = suit + rank;
                Sprite cardSprite = Resources.Load<Sprite>($"Card/{cardName}");
                if (cardSprite != null)
                {
                    _allCardSprites.Add(cardSprite);
                }
            }
        }
    }
    
    public void FlipCards()
    {
        float flipDuration = 0.5f;
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(_cardImage.rectTransform.DOScaleX(0, flipDuration * 0.5f).OnComplete(() =>
        {
            _cardImage.sprite = _cardSides[1]; // 앞면 스프라이트로 변경
        }));

        mySequence.Append(_cardImage.rectTransform.DOScaleX(1, flipDuration * 0.5f));
    }
	
    public IEnumerator MoveToPosition(int pos, bool isPlayer)
    {
        _isMoving = true;
        RectTransform targetRectTransform = isPlayer ? _playerPosition[pos - 1] : _dealerPosition[pos - 1];

        Vector2 targetAnchoredPosition = targetRectTransform.anchoredPosition;
        Vector2 currentAnchoredPosition = _cardImage.rectTransform.anchoredPosition;

        while (Vector2.Distance(currentAnchoredPosition, targetAnchoredPosition) > 0.01f) // 목표 위치에 충분히 가까워질 때까지 반복
        {
            currentAnchoredPosition = Vector2.MoveTowards(currentAnchoredPosition, targetAnchoredPosition, 600 * Time.deltaTime);
            _cardImage.rectTransform.anchoredPosition = currentAnchoredPosition;
            yield return null;
        }
        
        //Debug.Log("Moving card to position: " + targetAnchoredPosition);

        // 목표 위치에 도달하면 바로 목표 위치로 설정하여 위치를 정확히 조정
        _cardImage.rectTransform.anchoredPosition = targetAnchoredPosition;
        
        yield return new WaitForSeconds(0.7f);
        _isMoving = false;
        FlipCards();

        if (isPlayer)
        {
            _blackJackScript.PlayerScore(_cardNumber);
        }
        else
        {
            _blackJackScript.DealerScore(_cardNumber);
        }
    }
    
    public int GetCardValue()
    {
        if (_cardNumber > 10)
        {
            return 10;
        }
        if (_cardNumber == 1)
        {
            int scoreWithEleven = _blackJackScript.playerScore + 11;
            if (scoreWithEleven <= 21)
            {
                return 11;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return _cardNumber;
        }
    }
}
