using System.Collections.Generic;
using UnityEngine;

namespace UserInterfaces.Dialoguing
{
    [System.Serializable]
    public class DialogueEntry
    {
        //public DialogueEntry[] dialogueEntry = new DialogueEntry[4];
        [SerializeField]
        [TextArea(1,5)]
        public string dialogue;
        [SerializeField]
        public List<DialogueOption> options = new List<DialogueOption>();

        //[TextArea(1,2)] public string[] options = new string[4];
        //[TextArea(1,2)] public string option0;
        //[TextArea(1,2)] public string option1;
        //[TextArea(1,2)] public string option2;
        //[TextArea(1,2)] public string option3;

        public DialogueEntry(string dialogue, string option1, string option2 = null, string option3 = null, string option4 = null)
        {
            //this.dialogue = dialogue;
            //options.Add(new DialogueOption)
            //this.options[0].option = option1 ?? null;
            //this.options[1].option = option2 ?? null;
            //this.options[2].option = option3 ?? null;
            //this.options[3].option = option4 ?? null;
        }

        public DialogueEntry NextEntry(int selection)
        {
            return options[selection].dialogueEntry.options.Count > 0 ? options[selection].dialogueEntry : null;
        }
    }
}
