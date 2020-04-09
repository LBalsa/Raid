using UserInterfaces.Dialogues;

public interface IDialogueCanvas
{
    IDialogue Dialogue { set; }

    void ToggleDialogue(bool state);
    void ToggleCallout(bool state);
    void SetCallout(string dialogue);
    void SetDialogue(DialogueEntry dialogue);
    void ClearDialogue();
    void ClearCallout();
    void ReturnSelection(int selection);
    void Destroy();
}