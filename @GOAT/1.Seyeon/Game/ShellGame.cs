using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;
using WaitForSeconds = UnityEngine.WaitForSeconds;

public class ShellGame : MonoBehaviour
{
    /*
묻고 더블로 가면 카드 속도 빨라지게 (했음)
최대 획득 가능 조절 - 10만 (했음)
UI 추가 (규칙 판넬, 베팅금액 오류 등)
다시하기나 묻고 더블로 많이 하다보면 랜덤한 확률로 도박NPC 돈 먹고 나른다 로직 추가
     */
    
    public GameScene gameScene;

    public GameObject rightCardPrefab;
    public GameObject wrongCardPrefab;
    
    private List<GameObject> _cards = new();
    /*IMAGE*/
    public Sprite rightFrontImage;
    public Sprite wrongFrontImage;
    public Sprite backImage;
    /*UI*/
    public GameObject startPannel;
    public GameObject gamePannel;
    public GameObject winPannel;
    public GameObject losePannel;
    public GameObject rulePannel;
    /*FLIP*/
    public float flipDuration = 1f; // 뒤집는 시간
    public float shuffleDuration = 1f; // 섞는 시간

    private bool _isFliped = false;
    /*SHUFFLE*/
    public int shuffleCount = 10; // 섞는 횟수
    public float shuffleSpeed = 3f; // 섞는 속도
    /*BET*/
    private TMP_InputField _betting;
    private int _bettingAmount; // 베팅한 골드
    private int _getGold; // 얻을 골드 임시 저장
    /*GOLD*/
    [SerializeField] private TMP_Text _showTotalGold;
    [SerializeField] private TMP_Text _showBettingGold;
    /*COROUTINE*/
    private Coroutine _delay;
    private Coroutine _shuffle;
    
    
    public void Start()
    {
        Start_BackUp();
    }

    private void Start_BackUp()
    {
        startPannel.SetActive(true);
        gamePannel.SetActive(false);
        winPannel.SetActive(false);
        losePannel.SetActive(false);
        rulePannel.SetActive(false);

        _betting = startPannel.transform.Find("Betting").GetComponent<TMP_InputField>();
        _betting.contentType = TMP_InputField.ContentType.IntegerNumber;
        Button startBtn = startPannel.transform.Find("StartBtn").GetComponent<Button>();
        Button exitBtn = startPannel.transform.Find("ExitBtn").GetComponent<Button>();
        Button ruleBtn = startPannel.transform.Find("RuleBtn").GetComponent<Button>();
        
        startBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
        ruleBtn.onClick.RemoveAllListeners();
        
        startBtn.onClick.AddListener(() => Betting());
        exitBtn.onClick.AddListener(() => Exit());
        ruleBtn.onClick.AddListener(() => GameRule());
    }
    
    void Betting() 
    {
        if (int.TryParse(_betting.text, out _bettingAmount))
        {
            if (_bettingAmount < 100 || _bettingAmount > 10000)
            {
                _betting.text = "베팅은 100골드 ~ 10000골드만 가능합니다.";
                StartCoroutine(ResetBetting());
                return;
            }
            else if (Main.Player.playerData.gold < _bettingAmount)
            {
                _betting.text = "소지금이 부족합니다";
                StartCoroutine(ResetBetting());
                return;

            }
            else
            {
                Main.Player.playerData.gold -= _bettingAmount;
                Main.Quest.UpdateGoldCount(Main.Player.playerData.gold);
                // Debug.Log($"베팅 : {_bettingAmount} / 남은 소지금 : {Main.Player.playerData.gold}");
                GameStart();
            }
        }

        _betting.text = "베팅은 100골드 ~ 10000골드만 가능합니다.";
        StartCoroutine(ResetBetting());
    }

    IEnumerator ResetBetting()
    {
        yield return new WaitForSeconds(0.5f);
        _betting.text = "";
    }

    void GameRule()
    {
        rulePannel.SetActive(!rulePannel.activeSelf);
    }

    void GameStart()
    {
        startPannel.SetActive(false);
        gamePannel.SetActive(true);
        winPannel.SetActive(false);
        losePannel.SetActive(false);
        
        //_showTotalGold = gamePannel.transform.Find("TotalGoldText").GetComponent<TMP_Text>();
        _showTotalGold.text = $"소지 골드 : {Main.Player.playerData.gold} G";
        _showBettingGold.text = $"배팅 골드 : {_bettingAmount} G";
        
        InitializeCards();
        Invoke("FlipCards", 1f); // 1.5초 후 뒤집기
    }

    void InitializeCards()
    {
        ClearCards();
        
        Vector3[] positions = new[]
        {
            new Vector3(-500, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(500, 0, 0)
        };
        
        positions = positions.OrderBy(x => Random.value).ToArray();
        
        GameObject rightCard = Instantiate(rightCardPrefab, gamePannel.transform, false);
        _cards.Add(rightCard);
        
        for (int i = 1; i < positions.Length; i++)
        {
            GameObject wrongCard = Instantiate(wrongCardPrefab, gamePannel.transform, false);
            _cards.Add(wrongCard);
        }
        
        foreach (var card in _cards)
        {
            var image = card.GetComponent<Image>();
            image.sprite = card.name == "RightCard(Clone)" ? rightFrontImage : wrongFrontImage;
            card.transform.localPosition = positions[_cards.IndexOf(card)];
        }
    }
    
    void ClearCards()
    {
        foreach (var card in _cards)
        {
            Destroy(card);
        }
        
        _cards.Clear();
    }
    
    void FlipCards()
    {
        Sequence flipSequence = DOTween.Sequence();

        foreach (var card in _cards)
        {
            flipSequence.Join(card.transform.DOScaleX(0, flipDuration * 0.5f).OnComplete(() =>
            {
                var image = card.GetComponent<Image>();
                image.sprite = backImage;
                card.transform.DOScaleX(1, flipDuration * 0.5f);
            }));
        }

        flipSequence.OnComplete(() =>
        {
            if (_delay == null)
            {
                _delay = StartCoroutine(DelayedShuffleCards(0.3f));
            }
        });
    }
    
    IEnumerator DelayedShuffleCards(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (_shuffle == null)
        {
            _shuffle = StartCoroutine(ShuffleCards());
        }

        _delay = null;
    }
    
    IEnumerator ShuffleCards()
    {
        for (int i = 0; i < shuffleCount; i++)
        {
            List<int> availableIndices = new(Enumerable.Range(0, _cards.Count));

            while (availableIndices.Count > 1)
            {
                int index1 = availableIndices[Random.Range(0, availableIndices.Count)];
                availableIndices.Remove(index1);

                int index2 = availableIndices[Random.Range(0, availableIndices.Count)];
                availableIndices.Remove(index2);
                
                Vector3 position1 = _cards[index1].transform.position;
                Vector3 position2 = _cards[index2].transform.position;
                float moveTime = shuffleDuration / shuffleSpeed;
                
                Sequence sequence = DOTween.Sequence();
                
                Vector3[] path1 = GetParabolaPath(position1, position2, 150f);
                Vector3[] path2 = GetParabolaPath(position2, position1, -150f);
                
                sequence.Append(_cards[index1].transform.DOPath(path1, moveTime, PathType.CatmullRom).SetEase(Ease.InOutQuad))
                        .Join(_cards[index2].transform.DOPath(path2, moveTime, PathType.CatmullRom).SetEase(Ease.InOutQuad))
                        .OnComplete(() => 
                        {
                            _cards[index1].transform.position = position2;
                            _cards[index2].transform.position = position1;
                        });

                yield return sequence.Play().WaitForCompletion();
            }
        }
        
        foreach (var card in _cards)
        {
            var button = card.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => ChoiceCard(card));
        }
        
        _shuffle = null;
    }

    Vector3[] GetParabolaPath(Vector3 start, Vector3 end, float height)
    {
        Vector3 controlPoint = (start + end) * 0.5f + Vector3.up * height;
        return new[] { start, controlPoint, end };
    }

    void ChoiceCard(GameObject card)
    {
        foreach (var c in _cards)
        {
            if (c == card) // 선택한 카드만 처리합니다.
            {
                var image = c.GetComponent<Image>();
                image.sprite = c.name == "RightCard(Clone)" ? rightFrontImage : wrongFrontImage; // 선택한 카드 앞면 보여주기
                break;
            }
        }

        if (_isFliped) return;
        _isFliped = true;
        StartCoroutine(DelayedCardChoice(card)); // 0.5초 후 승패 처리
        
    }

    IEnumerator DelayedCardChoice(GameObject card)
    {
        
        yield return new WaitForSeconds(0.3f); // 0.5초 기다림

        var image = card.GetComponent<Image>();
        
        if (image.sprite == rightFrontImage)
        {
            GameWin();
        }
        else
        {
            GameLose();
        }
        _isFliped = false;
    }

    void GameWin()
    {
        gamePannel.SetActive(false);
        winPannel.SetActive(true);
        
        Button doubleBtn = winPannel.transform.Find("DoubleBtn").GetComponent<Button>();
        Button retryBtn = winPannel.transform.Find("RetryBtn").GetComponent<Button>();
        Button exitBtn = winPannel.transform.Find("ExitBtn").GetComponent<Button>();
        TMP_Text rewardTxt = winPannel.transform.Find("RewardTxt").GetComponent<TMP_Text>();
        
        
        doubleBtn.onClick.RemoveAllListeners();
        retryBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
        
        doubleBtn.onClick.AddListener(() => DoubleBetting());
        retryBtn.onClick.AddListener(() => { Main.Player.playerData.gold += _getGold; Retry(); });
        exitBtn.onClick.AddListener(() => { Main.Player.playerData.gold += _getGold; Exit(); });

        if (_getGold > 0) // DoubleBetting을 선택한 후 승리한 경우
        {
            _getGold = Mathf.Min(_getGold * 2, 1000000); // 최대 획득 가능 100만
            rewardTxt.text = $"{_getGold} G";
            // Debug.Log($"DoubleBetting 후 승리! 보상: {_getGold} G");
        }
        else // 일반 승리의 경우
        {
            _getGold = (int)(_bettingAmount * 1.5f);
            _getGold = Mathf.Min(_getGold, 1000000); // 최대 획득 가능 100만
            rewardTxt.text = $"{_getGold} G";
            // Debug.Log($"일반 승리! 보상: {_getGold} G");
        }
        
        Main.Quest.UpdateGameCount();
        Main.Save.SaveToJson_PlayerData();

        // Debug.Log($"현재 보상: {_getGold} G, 소지 골드: {Main.Player.playerData.gold}");
    }

    void GameLose()
    {
        gamePannel.SetActive(false);
        losePannel.SetActive(true);
        
        
        Button retryBtn = losePannel.transform.Find("RetryBtn").GetComponent<Button>();
        Button exitBtn = losePannel.transform.Find("ExitBtn").GetComponent<Button>();
        
        retryBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
        
        retryBtn.onClick.AddListener(() => Retry());
        exitBtn.onClick.AddListener(() => Exit());
    }

    void DoubleBetting()
    {
        //_getGold = Mathf.Min(_getGold * 2, 100000); // 최대 획득 가능 10만

        _bettingAmount = _getGold; // 직전 획득 금액만큼을 그대로 베팅하고
        
        shuffleCount += 3;
        shuffleSpeed *= 1.2f;
        
        ClearCards();
        GameStart();
    }

    void Retry()
    {
        ResetData();
        ClearCards();
        Start_BackUp();
    }

    void ResetData()
    {
        _bettingAmount = 0;
        _getGold = 0;
        shuffleCount = 10;
        shuffleSpeed = 3f;
        _betting.text = "";
    }

    void Exit()
    {
        Main.Quest.UpdateGoldCount(Main.Player.playerData.gold);
        
        startPannel.SetActive(false);
        gamePannel.SetActive(false);
        winPannel.SetActive(false);
        losePannel.SetActive(false);
        
        ClearCards();
        ResetData();
        
        this.gameObject.SetActive(false);
        
        gameScene.GamblingHome(); // 초기화면으로
    }
}
