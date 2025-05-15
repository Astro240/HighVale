using System;
using System.Collections.Generic;

[Serializable]
public class DialogueLine
{
    public string characterName;
    public string dialogueText;
}

[Serializable]
public class DialogueData
{
    public List<DialogueLine> lines;
}
