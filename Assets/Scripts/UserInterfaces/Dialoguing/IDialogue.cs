namespace UserInterfaces.Dialoguing
{
    public interface IDialogue
    {
        IDialogueCanvas DialogueCanvas { set; }
        void StartDialogue(IDialogueCanvas dialogueCanvas, IDialogueAction dialogueAction);
        void EndDialogue();
        void ReceiveSelection(int selection);
    }
}