using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://blog.naver.com/seilius/130180310822

public class BJ : MonoBehaviour
{
	public GameObject spawn; //카드 프리팹 스폰위치
	public GameObject card;	//카드
	//public GUIText pMessage;//플레이어 정보 창
	//public GUIText dMessage;//딜러 정보 창
	
	GameObject _cardPrefab;	//프리팹하여 복사된 카드 객체

	private int _playerHand;	// Player의 카드 위치
    private int _dealerHand;	//Dealer의 카드 위치
    private int _playerScore;		//플레이어 카드 점수
    private int _dealerScore;		//딜러 카드 점수
	/*STATE*/
	bool stateStart;
	bool stateStand;
	bool stateEnd;
	/*BUTTON*/
	public Texture2D playBtn;
	public Texture2D callBtn;
	public Texture2D standBtn;
	/*DEALAY TIME*/
	float fireRate = 1.0f;
	float nextFire = 0.0f;
	
	public bool destroy;
	/*	
	void Awake(){	//최초 한번만 실행
		//gameStart();
		destroy = true;		
	}
	*/
	void Update()
    {	//매 프레임 호출
		if(stateStand == true && stateStart == true && Time.time > nextFire)
        {
			nextFire = Time.time + fireRate;
			dealerTurn();			
		}
	}
	
	IEnumerator gameStart()
    {//게임 시작 함수
		destroy = true;		
        _playerHand = 0;
        _dealerHand = 0;	
        _playerScore = 0;
        _dealerScore = 0;
		stateStart = true;
		stateStand = false;	
		stateEnd = false;
		yield return new WaitForSeconds(0.5f);		
		StartCoroutine(cardCall(3,"all"));	
	}
	
	void OnGUI()
    {
		int sw = Screen.width;
		int sh = Screen.height;
	
		if(stateEnd == false)
        {
			if (GUI.Button(new Rect(sw*0.7f,sh*0.7f,sw*0.1f,sw*0.1f),playBtn) && stateStart == false)
            {								
				StartCoroutine(gameStart());	
			}
			
			if (GUI.Button(new Rect(sw*0.8f,sh*0.7f,sw*0.1f,sw*0.1f),callBtn) && stateStand==false && stateStart == true)
            {				
				StartCoroutine(cardCall(1,"Player"));	
			}
			
			if (GUI.Button(new Rect(sw*0.9f,sh*0.7f,sw*0.1f,sw*0.1f),standBtn) && stateStand==false)
            {
				stateStand = true;
			}	
		}
        else
        {
			if (GUI.Button(new Rect(sw*0.9f,sh*0.7f,sw*0.1f,sw*0.1f),playBtn) && stateStart == false)
            {
				Application.LoadLevel("level");
			}	
		}
	}
	
	IEnumerator cardCall(int cardNum, string man) //카드 분배 함수	
    {		
		destroy = false;
		int n;				
		for(n=1;n<=cardNum;n++)
        {
            _cardPrefab = (GameObject)Instantiate(card,spawn.transform.position, spawn.transform.rotation);
			if(man == "Player")
            {				
                _playerHand++;
                _cardPrefab.SendMessage("playerCard",_playerHand);
			}
			if(man == "Dealer")
            {
                _dealerHand++;
                _cardPrefab.SendMessage("dealerCard",_dealerHand);
			}		
			if(man=="all")
            {	
				if(n<3)
                {
                    _playerHand++;
                    _cardPrefab.SendMessage("playerCard",_playerHand);
				}
                else
                {
                    _dealerHand++;
                    _cardPrefab.SendMessage("dealerCard",_dealerHand);
				}
			}		
			yield return new WaitForSeconds(0.7f);		
		}	
	}
	
	void decision()
    {
		//BLACK JACK!!
		if(_playerScore == 21)
        {
			//pMoney = pMoney+deal;
			msg("BLACK JACK!!","player");		
			stateStart = false;	
		}
		if(_dealerScore == 21)
        {
			//pMoney = pMoney-deal;
			msg("BLACK JACK!!","dealer");	
			stateStart = false;
		}
		//BUST
		if(_dealerScore > 21)
        {
			//pMoney = pMoney+deal;
			msg("YOU WIN","player");
			msg("BUST!!","dealer");
			stateStart = false;
		}
		if(_playerScore > 21)
        {
			//pMoney = pMoney-deal;
			msg("DEALER WIN","dealer");
			msg("BUST!!","player");
			stateStart = false;
		}
		//DECISION
		if(stateStart == false && stateStand == true)
        {
			if(21-_dealerScore>21-_playerScore)
            {
				//pMoney = pMoney+deal;
				msg("YOU WIN","player");
			}
			if(21-_dealerScore<21-_playerScore && 21-_dealerScore>0)
            {
				//pMoney = pMoney-deal;
				msg("DEALER WIN","dealer");
			}
			if(21-_dealerScore == 21-_playerScore)
            {
				msg("DRAW","player");
				msg("DRAW","dealer");
			}
		}
	}
	/*DEALER PLAY*/
	void dealerTurn()
    {			
		if(_dealerScore<19 && _dealerScore<_playerScore){			
			StartCoroutine(cardCall(1,"Dealer"));			
			//decision();	
		}
        else
        {
			stateStart = false;
			decision();
		}		
	}
	
	void playerScore(int score)
    {
        _playerScore = _playerScore+score;
		msg("YOU: "+_playerScore,"player");
		decision();
	}

	void dealerScore(int score)
    {
        _dealerScore = _dealerScore+score;
		msg("DEALER: "+_dealerScore,"dealer");
		decision();
	}
	
	void msg(string hint, string target)
    {
		if(target == "player")
        {
			//pMessage.SendMessage("scoreBoard", hint);
		}
        else if(target == "dealer")
        {
			//dMessage.SendMessage("scoreBoard", hint);
		}
        else
        {		
			//moneyBoard.SendMessage("moneyBoard", hint);
		}
	}
}
