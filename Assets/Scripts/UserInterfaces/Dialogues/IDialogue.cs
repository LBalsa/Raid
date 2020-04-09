using System;
using UserInterfaces.Dialogues;

public interface IDialogue
{
    IDialogueCanvas DialogueCanvas { set; }
    void StartDialogue(IDialogueCanvas dialogueCanvas, IDialogueAction dialogueAction);
    void EndDialogue();
    void ReceiveSelection(int selection);
}