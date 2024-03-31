using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region Blackjack -----

public class BlackJack : MonoBehaviour
{
    public enum Who {Player, Dealer, All}
    
    public GameScene gameScene;
    
    public GameObject spawn; //카드 프리팹 스폰위치
    public GameObject card;	//카드
    public GameObject playerPannel;
    public GameObject dealerPannel;
    
    public TMP_Text playerScoreText; // 플레이어 점수
    public TMP_Text dealerScoreText; // 딜러 점수
    
    private GameObject _cardPrefab;	//프리팹하여 복사된 카드 객체
    
    [HideInInspector] public int playerScore;		//플레이어 카드 점수
    [HideInInspector] public int dealerScore;		//딜러 카드 점수
    
    private RectTransform[] _playerPosition;
    private RectTransform[] _dealerPosition;
    
    private int _playerHand;	// Player의 카드 위치
    private int _dealerHand;	//Dealer의 카드 위치
    private int _bettingAmount; // 베팅한 골드
    private int _getGold; // 얻거나 잃을 골드를 임시로 저장할 변수
    /*UI*/
    public GameObject startPannel;
    public GameObject gamePannel;
    public GameObject winPannel;
    public GameObject losePannel;
    private TMP_InputField _betting;
    /*STATE*/
    private bool _isInteractable = true;
    private bool _isBlackJack = false;

    private bool _stateStart;
    private bool _stateStand;
    private bool _stateEnd;
    private bool _destroy = true;

    void Awake()
    {
        _cardPrefab = Resources.Load<GameObject>("Prefabs/UI/Game/BlackJack/Card");
    }
    
    private void InitializeCardPositions()
    {
        _playerPosition = new RectTransform[10];
        _dealerPosition = new RectTransform[10];
    
        for (int i = 0; i < 10; i++)
        {
            _playerPosition[i] = playerPannel.transform.Find($"pos{i + 1}").GetComponent<RectTransform>();
            _dealerPosition[i] = dealerPannel.transform.Find($"pos{i + 1}").GetComponent<RectTransform>();
        }
    }
    
    void Start()
    {
        startPannel.SetActive(true);
        gamePannel.SetActive(false);
        winPannel.SetActive(false);
        losePannel.SetActive(false);
        
        _betting = startPannel.transform.Find("Betting").GetComponent<TMP_InputField>();
        _betting.contentType = TMP_InputField.ContentType.IntegerNumber;
        Button startBtn = startPannel.transform.Find("StartBtn").GetComponent<Button>();
        Button exitBtn = startPannel.transform.Find("ExitBtn").GetComponent<Button>();
        
        startBtn.onClick.AddListener(() => Betting());
        exitBtn.onClick.AddListener(() => Exit());

        Main.Player.playerData.gold = Main.Player.playerData.gold;
        
        Button hitBtn = gamePannel.transform.Find("HitBtn").GetComponent<Button>();
        Button standBtn = gamePannel.transform.Find("StandBtn").GetComponent<Button>();
        
        hitBtn.onClick.AddListener(OnHit);
        standBtn.onClick.AddListener(OnStand);
    }
    
    void Betting() // 판돈 설정
    {
        if (int.TryParse(_betting.text, out _bettingAmount) && _bettingAmount <= Mathf.Min(Main.Player.playerData.gold, 500) && _bettingAmount >= 100)
        {
            Main.Player.playerData.gold -= _bettingAmount; // 베팅 금액을 총 골드에서 차감
            //Debug.Log($"베팅 : {_bettingAmount} / 남은 소지금 : {Main.Player.playerData.gold}");
            StartCoroutine(GameStart());
        }
        else
        {
            // 오류창 하나 띄워준다
        }
    }
    
    IEnumerator GameStart() //게임 시작
    {
        startPannel.SetActive(false);
        gamePannel.SetActive(true);
        winPannel.SetActive(false);
        losePannel.SetActive(false);
        
        _destroy = true;		
        _playerHand = 0;
        _dealerHand = 0;	
        playerScore = 0;
        dealerScore = 0;
        _stateStart = true;
        _stateStand = false;	
        _stateEnd = false;
        yield return new WaitForSeconds(0.5f);	
        InitializeCardPositions();
        StartCoroutine(CardCall(2,Who.All));
    }
    
    private IEnumerator CardCall(int cardNum, Who who)
    {
        Transform parentTransform = (who == Who.Player) ? playerPannel.transform : dealerPannel.transform;
        
        for (int i = 1; i <= cardNum; i++)
        {
            if (_cardPrefab == null)
            {
                //Debug.LogError("Card prefab is not assigned.");
                yield break;
            }

            GameObject newCard = Instantiate(_cardPrefab, spawn.transform.position, Quaternion.identity, parentTransform);
        
            Card cardScript = newCard.GetComponent<Card>();
            cardScript.SetPositions(_playerPosition, _dealerPosition);

            // 이동 후 점수 계산 로직 추가
            yield return StartCoroutine(cardScript.MoveToPosition(i, who == Who.Player || who == Who.All));

            if (who == Who.Player || who == Who.All)
            {
                PlayerScore(cardScript.GetCardValue());
            }
            if (who == Who.Dealer || who == Who.All)
            {
                DealerScore(cardScript.GetCardValue());
            }
        }
    }

    public void OnHit() // hitBtn 눌렀을 때
    {
        // 플레이어가 카드를 더 받는 로직
        StartCoroutine(CardCall(1,Who.Player));
        DealerTurn();
    }

    public void OnStand() // standBtn 눌렀을 때
    {
        // 플레이어가 스탠드하는 로직
        _stateStand = true;
        DealerTurn();
    }
    
    private void DealerTurn()
    {
        if (dealerScore < 19 && dealerScore < playerScore)
        {			
            StartCoroutine(CardCall(1,Who.Dealer));			
        }
        else
        {
            _stateStart = false;
            Decision();
        }	
    }
    
    private void Decision() // 누가 이김? - 로직 수정예정
    {
        if (!_stateStart || _stateStand)
        {
            return;
        }
        
        //BLACK JACK
        if (playerScore == 21) // 플레이어 이김
        {
            GameWin();		
            _stateStart = false;
            _isBlackJack = true;
        }
        if (dealerScore == 21)
        {
            GameLose();
            _stateStart = false;
        }
        //BUST
        if (dealerScore > 21) // 플레이어 이김
        {
            GameWin();
            _stateStart = false;
        }
        if (playerScore > 21)
        {
            GameLose();
            _stateStart = false;
        }
        //DECISION
        if (_stateStart == false && _stateStand == true)
        {
            if(21-dealerScore>21-playerScore)
            {
                GameWin();
            }
            if(21-dealerScore<21-playerScore && 21-dealerScore>0)
            {
                GameLose();
            }
            if(21-dealerScore == 21-playerScore)
            {
                // 비김
            }
        }
    }
    
    public void PlayerScore(int score)
    {
        playerScore += score;
        UpdateScoreDisplay(playerScore,"player");
        Decision();
    }

    public void DealerScore(int score)
    {
        dealerScore += score;
        UpdateScoreDisplay(dealerScore,"dealer");
        Decision();
    }
    
    void UpdateScoreDisplay(int score, string target)
    {
        // 점수를 UI에 보여주는 로직
        
        if (target == "player")
        {
            playerScoreText.text = $"Player: {playerScore}";
        }
        else if (target == "dealer")
        {
            dealerScoreText.text = $"Dealer: {dealerScore}";
        }
    }
    
    void GameWin()
    {
        gamePannel.SetActive(false);
        winPannel.SetActive(true);
        
        Button doubleBtn = winPannel.transform.Find("DoubleBtn").GetComponent<Button>();
        Button retryBtn = winPannel.transform.Find("RetryBtn").GetComponent<Button>();
        Button exitBtn = winPannel.transform.Find("ExitBtn").GetComponent<Button>();
        TMP_Text rewardTxt = winPannel.transform.Find("RewardTxt").GetComponent<TMP_Text>();
        
        doubleBtn.onClick.AddListener(() => DoubleBetting());
        retryBtn.onClick.AddListener(() => Retry());
        exitBtn.onClick.AddListener(() => Exit());

        if (_getGold > 0) // DoubleBetting을 선택한 후 승리한 경우
        {
            rewardTxt.text = $"{_getGold} G";
            //Debug.Log($"DoubleBetting 후 승리! 보상: {_getGold} G");
        }
        else if (_isBlackJack) // 블랙잭으로 이긴 경우
        {
            _getGold = (int)(_bettingAmount * 2f);
            rewardTxt.text = $"{_getGold} G";
            //Debug.Log($"블랙잭! 보상: {_getGold} G");
        }
        else // 일반 승리의 경우
        {
            _getGold = (int)(_bettingAmount * 1.5f);
            rewardTxt.text = $"{_getGold} G";
            //Debug.Log($"일반 승리! 보상: {_getGold} G");
        }

        Main.Player.playerData.gold += _getGold;
        Main.Save.SaveToJson_PlayerData();


        //Debug.Log($"현재 보상: {_getGold} G, 소지 골드: {Main.Player.playerData.gold}");
    }

    void GameLose()
    {
        gamePannel.SetActive(false);
        losePannel.SetActive(true);
        
        Button retryBtn = losePannel.transform.Find("RetryBtn").GetComponent<Button>();
        Button exitBtn = losePannel.transform.Find("ExitBtn").GetComponent<Button>();
        
        retryBtn.onClick.AddListener(() => Retry());
        exitBtn.onClick.AddListener(() => Exit());
    }

    void DoubleBetting()
    {
        int initialReward = _bettingAmount * 3;
        _getGold = initialReward;
        _bettingAmount *= 2;
        GameStart();
    }

    void Retry()
    {
        if (_getGold > 0)
        {
            Main.Player.playerData.gold += _getGold;
        }
        
        _getGold = 0;
        Start();
    }

    void Exit()
    {
        if (_getGold > 0)
        {
            Main.Player.playerData.gold += _getGold;
        }
        
        startPannel.SetActive(false);
        gamePannel.SetActive(false);
        winPannel.SetActive(false);
        losePannel.SetActive(false);
        
        gameScene.GamblingHome(); // 초기화면으로
    }
}

#endregion