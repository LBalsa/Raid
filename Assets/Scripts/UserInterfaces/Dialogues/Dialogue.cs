using UnityEngine;

namespace UserInterfaces.Dialogues
{
    [CreateAssetMenu(menuName = "Dialogue")]
    public class Dialogue : ScriptableObject, IDialogue
    {
        public IDialogueCanvas DialogueCanvas { get; set; }

        [SerializeField]
        public DialogueEntry firstEntry;
        private DialogueEntry currentEntry;
        private DialogueEntry previousEntry;

        private IDialogueAction dialogueAction;

        public void StartDialogue(IDialogueCanvas dialogueCanvas, IDialogueAction dialogueAction)
        {
            DialogueCanvas = dialogueCanvas;
            DialogueCanvas.ToggleDialogue(true);

            currentEntry = firstEntry;
            DialogueCanvas.SetDialogue(currentEntry);

            this.dialogueAction = dialogueAction;
        }

        public void EndDialogue()
        {
            DialogueCanvas.ClearDialogue();
            DialogueCanvas.ToggleDialogue(false);
        }

        public void ReceiveSelection(int selection)
        {
            int actionIndex = currentEntry.options[selection].actionIndex;

            if (actionIndex > 0)
            {
                dialogueAction.PerformAction(actionIndex);
            }

            DialogueEntry nextEntry = actionIndex == -1 ? previousEntry : currentEntry.NextEntry(selection);

            if (nextEntry != null)
            {
                previousEntry = currentEntry;
                currentEntry = nextEntry;
                DialogueCanvas.SetDialogue(currentEntry);
            }
            else
            {
                EndDialogue();
                dialogueAction.PerformAction(0);
            }

        }
    }
}
