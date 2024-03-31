using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    [SerializeField] private GameObject _choiceGameObj;
    [SerializeField] private GameObject _shellGame;
    [SerializeField] private GameObject _blackJack;
    
    [SerializeField] private TMP_Text _goldText;
    
    public override bool Initialize()
    {
        if (!base.Initialize())
        {
            return false;
        }
        
        Main.Audio.BgmPlay(Main.Resource.Load<AudioClip>("CasinoBGM"), 0.3f);


       Main.Player.CursorLock_None();


        GameObject choiceShellGame = Main.Resource.InstantiatePrefab("ChoiceShellGame", _choiceGameObj.transform);
        //GameObject choiceBlackJack = Main.Resource.InstantiatePrefab("ChoiceBlackJack", _choiceGameObj.transform);
        GameObject choiceExit = Main.Resource.InstantiatePrefab("ChoiceExit", _choiceGameObj.transform);
        
        choiceShellGame.GetComponent<Button>().onClick.AddListener(() => PlayShellGame());
        //choiceBlackJack.GetComponent<Button>().onClick.AddListener(() => PlayBlackJack());
        choiceExit.GetComponent<Button>().onClick.AddListener(() => Exit());
        
        _goldText.text = $"{Main.Player.playerData.gold} G";
        
        GamblingHome();
        
        return true;
    }

    public void GamblingHome()
    {
        GameObject choiceShellGame = Main.Resource.InstantiatePrefab("ChoiceShellGame", _choiceGameObj.transform);
        //GameObject choiceBlackJack = Main.Resource.InstantiatePrefab("ChoiceBlackJack", _choiceGameObj.transform);
        GameObject choiceExit = Main.Resource.InstantiatePrefab("ChoiceExit", _choiceGameObj.transform);
        
        choiceShellGame.GetComponent<Button>().onClick.AddListener(() => PlayShellGame());
        //choiceBlackJack.GetComponent<Button>().onClick.AddListener(() => PlayBlackJack());
        choiceExit.GetComponent<Button>().onClick.AddListener(() => Exit());
        
        _goldText.text = $"{Main.Player.playerData.gold} G";
    }
    
    public void PlayShellGame()
    {
        foreach (Transform child in _choiceGameObj.transform)
        {
            Destroy(child.gameObject);
        }
        
        _shellGame.SetActive(true);
        
        Transform startPannel = _shellGame.transform.Find("StartPannel");
        
        if (startPannel != null)
        {
            startPannel.gameObject.SetActive(true);
        }
    }

    public void PlayBlackJack()
    {
        foreach (Transform child in _choiceGameObj.transform)
        {
            Destroy(child.gameObject);
        }
        
        _blackJack.SetActive(true);
    }

    public void Exit()
    {
        Main.Player.CurrentScene = Define.EPlayerSceneName.Game;
        LoadingScene.LoadScene(Define.SceneName.Town);
    }
}
