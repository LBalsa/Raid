using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserInterfaces.Dialogues;

public class DialogueCanvas : MonoBehaviour, IDialogueCanvas
{
    public IDialogue Dialogue { get; set; }

    private  int buttonId = 0;
    // Canvas objects.
    [Header("Canvas")]
    public GameObject calloutPanel;
    public GameObject dialoguePanel;

    private Text calloutText;
    private Text dialogueText;
    private List<Button> buttons = new List<Button>();
    private List<Text> buttonText = new List<Text>();

    private void Awake()
    {
        buttons = new List<Button>();
        buttonText = new List<Text>();

        foreach (var button in GetComponentsInChildren<Button>())
        {
            int i = buttonId;
            button.onClick.AddListener(() => ReturnSelection(i));
            buttonId++;

            buttons.Add(button);
            buttonText.Add(button.GetComponentInChildren<Text>());
        }

        calloutText = calloutPanel.GetComponentInChildren<Text>();
        dialogueText = dialoguePanel.GetComponentInChildren<Text>();

        ToggleCallout(false);
        ToggleDialogue(false);
    }

    void Start()
    {
        Debug.Log("");
    }
    public DialogueCanvas(IDialogue dialogue)
    {
        this.Dialogue = dialogue;
    }

    public void ClearCallout()
    {
        calloutText.text = "";
    }

    public void ClearDialogue()
    {
        dialogueText.text = "";
        foreach (var button in buttonText)
        {
            button.text = "";
        }
    }

    public void ToggleCallout(bool state)
    {
        calloutPanel.SetActive(state);
    }

    public void ToggleDialogue(bool state)
    {
        dialoguePanel.SetActive(state);
    }


    public void ReturnSelection(int selection)
    {
        Dialogue.ReceiveSelection(selection);
    }

    public void SetCallout(string callout)
    {
        calloutText.text = callout;
    }

    public void SetDialogue(DialogueEntry dialogueEntry)
    {
        dialogueText.text = dialogueEntry.dialogue;

        int options = dialogueEntry.options.Count;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < options)
            {
                buttonText[i].text = dialogueEntry.options[i].option;
                buttons[i].gameObject.SetActive(true);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }

        //if (dialogue.option() == null) { button1.SetActive(false); }
        //else
        //{
        //    button1.GetComponentInChildren<Text>().text = dialogue.option1;
        //    button1.SetActive(true);
        //}
        //if (dialogue.option2 == null) { button2.SetActive(false); }
        //else
        //{
        //    button2.SetActive(true);
        //    button2.GetComponentInChildren<Text>().text = dialogue.option2;
        //}
        //if (dialogue.option3 == null) { button3.SetActive(false); }
        //else
        //{
        //    button3.SetActive(true);
        //    button3.GetComponentInChildren<Text>().text = dialogue.option3;
        //}
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
