using UnityEngine;

namespace UserInterfaces.Dialogues
{
    [System.Serializable]
    public class DialogueOption
    {
        [TextArea(1,2)] public string option;
        [SerializeField]
        public int actionIndex;   
        public DialogueEntry dialogueEntry;
    }
}
