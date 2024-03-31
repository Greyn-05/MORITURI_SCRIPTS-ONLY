using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputValue : MonoBehaviour
{
    public PlayerInputAction InputActions { get; private set; }
    public PlayerInputAction.PlayerActions PlayerActions { get; private set; }
    public PlayerInputAction.TownActions TownActions { get; private set; }

    private void Awake()
    {
        InputActions = new PlayerInputAction();
        PlayerActions = InputActions.Player;
        TownActions = InputActions.Town;
    }

    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }


    #region SetScene ---------------------------------------------------------------------------------------------------
    public void SetTownScene()
    {
        // 던전씬 구독 취소
        PlayerActions.Menu.started -= DungeonMenuStarted;
        PlayerActions.Interact.started -= DungeonInteractStarted;
        PlayerActions.WheelUp.started -= DungeonWheelUpStarted;
        PlayerActions.WheelDown.started -= DungeonWheelDownStarted;

        // 타운씬 구독
        TownActions.Menu.started += TownMenuStarted;
        TownActions.Interaction.started += TownInteractStarted;
        TownActions.Inventory.started += TownInventoryStarted;
        TownActions.Status.started += TownStatusStarted;
        TownActions.WheelUp.started += TownWheelUpStarted;
        TownActions.WheelDown.started += TowwnWheelDownStarted;
        TownActions.Quest.started += TownQuestStarted;
        TownActions.Skill.started += TownSkillStarted;
    }

    public void SetDungeonScene()
    {
        // 타운씬 구독 취소
        TownActions.Menu.started -= TownMenuStarted;
        TownActions.Interaction.started -= TownInteractStarted;
        TownActions.Inventory.started -= TownInventoryStarted;
        TownActions.Status.started -= TownStatusStarted;
        TownActions.WheelUp.started -= TownWheelUpStarted;
        TownActions.WheelDown.started -= TowwnWheelDownStarted;
        TownActions.Quest.started -= TownQuestStarted;
        TownActions.Skill.started -= TownSkillStarted;

        // 던전씬 구독
        PlayerActions.Menu.started += DungeonMenuStarted;
        PlayerActions.Interact.started += DungeonInteractStarted;
        PlayerActions.WheelUp.started += DungeonWheelUpStarted;
        PlayerActions.WheelDown.started += DungeonWheelDownStarted;
    }
    #endregion





    #region Subscribe --------------------------------------------------------------------------------------------------
    // Dungeon ---------------------------------------------------------------------------------------------------------

    private void DungeonMenuStarted(InputAction.CallbackContext obj)
    {
        if (!Main.UI.CloseTopPopup()) //TODO
        {
             Main.UI.OpenPopup<UI_Popup_Pause>();
        }

    }

    private void DungeonInteractStarted(InputAction.CallbackContext obj)
    {
      //  Debug.Log("던전 상호작용");
    }

    public void DungeonWheelUpStarted(InputAction.CallbackContext obj)
    {
        if (Time.timeScale == 1f)
            Main.Inven.QuickSlotIndex--;

    }

    public void DungeonWheelDownStarted(InputAction.CallbackContext obj)
    {
        if (Time.timeScale == 1f)
            Main.Inven.QuickSlotIndex++;
    }


    // Town ------------------------------------------------------------------------------------------------------------
    private void TownMenuStarted(InputAction.CallbackContext obj)
    {
        if (!Main.UI.CloseTopPopup())
        {
            Main.UI.OpenPopup<UI_Popup_TownMenu>();
        }
    }

    private void TownInteractStarted(InputAction.CallbackContext obj)
    {
        Main.Player.OnInteractClicked();
    }

    private void TownInventoryStarted(InputAction.CallbackContext obj)
    {
        Main.Inven.Open();
    }

    private void TownStatusStarted(InputAction.CallbackContext obj)
    {
        Main.UI.OpenPopup<UI_Popup_PlayerInfo>();
    }

    private void TownWheelUpStarted(InputAction.CallbackContext obj)
    {
        //Debug.Log("마을 휠업");
    }

    private void TowwnWheelDownStarted(InputAction.CallbackContext obj)
    {
        //  Debug.Log("마을 휠 다운");
    }

    private void TownQuestStarted(InputAction.CallbackContext obj)
    {
        Main.Quest.Open();
    }

    private void TownSkillStarted(InputAction.CallbackContext obj)
    {
        Main.UI.OpenPopup<UI_Popup_Combo>();
    }
    #endregion

}

