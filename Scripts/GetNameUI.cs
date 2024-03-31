using TMPro;
using UnityEngine;

public class GetNameUI : MonoBehaviour
{
    public TMP_InputField playerNameInputField; // 플레이어 이름 입력
    public GameObject getNameCanvas; // GetNameCanvas 참조

    private void Awake()
    {
        playerNameInputField.onEndEdit.AddListener(delegate { SavePlayerName(); });
        // playerNameInputField의 onEndEdit 이벤트에 SavePlayerName를 추가해 입력이 끝나면 SavePlayerName이 호출되도록 함
    }
    

    public void SavePlayerName()
    {
        string playerName = playerNameInputField.text; // InputField로 입력받은 이름을 playerName으로 저장
        
        if (!string.IsNullOrEmpty(playerName)) // playerName이 Null / Empty가 아니면
        {

            // todo 플레이어 이름저장
            // Main.Data.SavePlayerName(playerName); // DataManager를 통해 이름 저장
            getNameCanvas.SetActive(false); // GetNameCanvas 비활성화
        }
    }
    
}
