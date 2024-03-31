using System.Collections.Generic;
using System.Net.Sockets;

public enum BattleState
{
    Stop = 0,           //정지 상태
    Move = 1,           //이동 상태
    Attack = 2,         //공격 모션 상태
    Attack_Judge = 3,  //공격 판정 상태
    Guard = 4,          //가드 상태
    Guard_Parry = 5,    //패링 상태 
    Gethit = 6,         //피격 상태
    Down = 7,           //다운 상태
    UseItem = 8,        //아이템 사용
    Death = 9,          //사망 상태
}
public class Define  // TODO static 나중에 바꾸기
{
    public enum QuestType
    {
        Main,
        Sub
    }

    public static Dictionary<QuestType, string> questType = new()
    {
        { QuestType.Main, "메인" },
        { QuestType.Sub, "서브" }
    };

    #region State_Driven_Camera
    public enum EStateDrivenCamera
    {
        Player,
        LockOn,
        NPC,
        ParryAttack
    }

    public static Dictionary<EStateDrivenCamera, int> cameraType = new()
    {
        { EStateDrivenCamera.Player, 0 },
        { EStateDrivenCamera.LockOn, 1 },
        { EStateDrivenCamera.NPC, 2 },
        {EStateDrivenCamera.ParryAttack, 3}
    };
    #endregion


    #region Player
    public const float StaminaRegen = 20f;
    public const float Stamina_Attack = 10f;
    public const float Stamina_Dodge = 10f;
    public const float Stamina_Guard = 20f;
    public const float Stamina_Parry = 20f;
    public const float Stamina_Run = 30f;


    public enum EPlayerSceneName
    {
        Title,
        Tutorial,
        Town,
        Dungeon,
        Game
    }

    #endregion

    #region NPC

    public static int FindNPCID(string npcName)
    {
        switch (npcName)
        {
            case "Alaric": return 750;
            case "Fierce": return 751;
            case "Cedric": return 752;
            case "Daria": return 753;
            case "Elric": return 754;
            case "Fendrel": return 755;
            case "Garvriel": return 756;
            case "Hilda": return 757;
            case "Ivor": return 758;
            case "Jorund": return 759;
            case "Kaelin": return 760;
            case "Stout": return 761;
            case "Nyx": return 762;
            case "Orla": return 763;
            case "Pyrrhus": return 764;

            case "Shop": return 1;
            case "Dungeon": return 2;
        }

        return -1;
    }

    #endregion

    public struct SceneName
    {
        public const string intro = "0. IntroScene";
        public const string Title = "1. TitleScene";
        public const string Loading = "2. LoadingScene";
        public const string Town = "3. TownScene";
        public const string Dungeon = "4. DungeonScene";
        public const string Tutorial = "5. TutorialScene";
        public const string Game = "GameScene";
        public const string GamblingEnd = "98. GamblingEndingScene";
        public const string endingScene = "99. EndingScene";

    }
    public struct PrefabName
    {
        public const string audioSource_Sfx = "AudioSource_Sfx";
        public const string audioSource_Bgm = "AudioSource_Bgm";
    }

    public struct AudioType
    {
        public const string bgm = "BGM";
        public const string sfx = "SFX";
        public const string master = "Master";
    }

    public struct BgmFileName
    {
        public const string titleBgm = "683. 보스 레이드";
        public const string introBgm = "883. 전투준비";
        public const string gameblingEndBgm = "838. Secret Agent";
        public const string battleEndBgm = "885. 엔딩";

    }



    public enum ItemType
    {
        Material, //재료
        consumables, // 버프디버프등 먹는 포션
        Production // 전투에 쓸수있는 도구

    }


    public static int[] prizeMoney = new int[] { 10000, 15000, 20000, 100 };

    public static string[] bossName = new string[4] { "SwordMan", "TwoHandedAxe", "Gunslinger", "Scarecrow" }; //보스이름

    public static string[] bossImageFile = new string[4] { "sword", "Axe", "gun", "magician" }; // 보스 이미지 파일명
    public static string[] bossDis = new string[4] {
        "험상궂은 전사 John은 외상값을 벌기 위해 콜로세움에 왔습니다.\n그의 검술은 제국 검술의 기초를 충실히 따르고 있어 동작이 일정하고 절도있습니다.\n하지만 잦은 음주 때문인지 칼을 휘두르고 난뒤 숨이 차서 잠시 움직임이 느려집니다.",
        "콜로세움 관리자 Jack은 콜로세움에서 난동을 부리는 사람들을 제압하는 일을 합니다.\n그의 도끼질은 규칙 없이 오직 힘으로만 휘두릅니다.\n또한 어디서 배웠는지도 모르는 회전공격을 종종 사용하기도 합니다.\n추신. Jack은 성격이 더럽습니다. 패색이 짙어진다면 분명 발악할 것입니다.",
        "총든 PepsiMan은 어디서 구했는지도 모르는 총들을 가지고 싸우는 투사입니다.\n그는 다양한 거리에 따라 다양한 총기를 활용하여 적을 상대합니다.\n특이하게 공격 장비에 비해 방어 장비는 형편없어서 조금 강하게 맞으면 종종 드러눕는 추태를 보입니다.\n추신. 당신의 실력이라면 총알도 튕겨낼 수 있지 않을까요?",
        "절대 입장하지 마십시오!"
    };

    public static int maxquickSlotsCount = 4; // 퀵슬롯 수
    public static int maxInvenSlotCount = 36; //인벤토리 칸 갯수
    public enum AttackAttribute
    {
        None = 0,
        Fire = 1,
        Lightning = 2,
        Ice = 3
    }


    public static string[] loadingComment = new string[]
   {
        "콜로세움은 서기 70~72년에 건설이 시작되고,\n80년에 완공이 되었으므로 건설에 약 8~10년 정도 걸렸습니다.",
        "콜로세움이라는 명칭은 나중에 만들어진 것이다.\n정식 명칭은 건설자인 베스파시아누스 황제의 일족 명을\n딴 플라비우스 원형극장이다.",
        "콜로세움은 이탈리아 5센트 주화의 도안이다.",
        "검투사를 가리키는 라틴어 글라디아토르의 뜻은\n검을 뜻하는 글라디우스를 다루는 사람이다.",
        "모든 검투사가 검을 무기로 사용했던 것은 아니었고\n창, 도끼, 둔기 등 여러 무기를 사용했다.",
        "콜로세움 관리자 Jack은 사실 귀여운 토끼를 매우 좋아합니다.",
        "콜로세움 관리자 Jack은 불같이 성화를 내지만\n높은 혈압은 그에게 좋지 않습니다.",
        "콜로세움 관리자 Jack은 덩치에 비해 의외로 술을 한잔도 못합니다.",
        "총을 든 팹시맨은 방탄복을 입지 않아\n총에 맞을때 매우 아픈 체질입니다. ",
        "총을 든 펩시맨에게 너무 다가가지 마십시오\n그에게는 효과적인 대화수단이 많습니다.",
        "총을 든 펩시맨에게 멀어질 생각 마십시오\n당신에게 총이 있습니까?",
        "마을이 깨끗하다 느낀다면 주말마다 빗자루를 들고있는 \n험상궂은 전사 John의 따뜻한 마음 덕분일 겁니다.",
        "험상궂은 전사 John은 엄청난 주당으로\n상점에 주류 외상값을 벌기위해 콜로세움에 출근합니다."
   };
}