using System.Collections.Generic;

public class NPCData
{
    public int NPCID;
    public string NPCName;
    public string NPCRace;
    public string NPCJob;
    public string NPCSex;
    // 추가
    
    public List<DialogueData> Dialogues { get; set; }
}
