public class QuestClearUI : UI_Base
{
    public void ShowClearMessage(string questName)
    {
        Invoke("HideUI", 2f);
    }

    private void HideUI()
    {
        Main.UI.DestroySubItemUI<QuestClearUI>(this.gameObject);
    }
}
