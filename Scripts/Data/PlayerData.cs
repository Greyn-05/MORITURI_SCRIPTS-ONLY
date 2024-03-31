using System.Collections.Generic;

public class PlayerData
{
    public string playerName;
    public int gold;

    public float hP_cur; //저장용스탯 스탯계산에 쓰지마세요!
    public float hP_max;
    public float stamina_cur;
    public float stamina_max;
    public float exp_cur;
    public float exp_max;

    public float atk;
    public float def;
    public float speed;

    public int level;
    
    // 인벤토리템
    // 퀵슬롯템

    //퀘스트 상태
    public List<int> activeQuestID; // 진행중인 서브퀘스트의 ID
    public List<int> activeQuestCurrentCount; // 진행중인 서브퀘스트의 CurrentCount
    public List<int> readyToClearQuestID; // 클리어 가능한 퀘스트의 ID
    public List<int> completedQuestID; // 완료된 퀘스트의 ID 목록
    public int currentMainQuestID; // 현재 진행중인 메인 퀘스트의 ID
    public int currentMainQuestCurrentCount; // 현재 진행중인 메인 퀘스트의 CurrentCount

    
    public bool[] isBossRelease; // 보스해금여부   배열로만들면 보스 추가됐을때 세이브파일 깨지지않을까...  현재 보스 총4

    public float[] bossClearTime;

    public int[] inventoryId;
    public int[] inventoryCap;

    public string saveTime; // 세이브파일 저장한시간


    //public Vector3 playerPos; 위치저장은 마을이 소환지점느낌으로해서 그냥 둬야할듯.
    //  public Vector3 playerRot;

    public bool isTutorialClear;
}