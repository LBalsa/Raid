using Controllers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterfaces
{
    public class SkipMenu : MonoBehaviour
    {
        private readonly string storyText =
        "Leading a pirate life in no easy task. To survive yee must raid villages and steal their posessions. Surely the people understand! How else are you supposed to get rum?" +
        "\nEveryone knows WASD moves, space defies gravity and mice bite...Holding shift will put you in mean pirate mode." +
        "\nThen yee click the mice to fight off them pesky villagers, either swordlike or kick 'em. If you switch back and forth it's a mean combo move. " +
        "\nIf the sword gets heavy, just drop it with R and use these mighty beaters!" +
        "\nThe numbers above call the men back on the ship for backup, skillz as them gamers call it." +
        "\nSaavy? Figure the rest out me ladie, and if you ever need help, well don't count on me, as I can't count! And forget about the dev, he don't like instructions." +
        "\n\nNow, let's go get me gold!";
        public Text textBox;
        public LevelLoader load;

        // Use this for initialization
        private void Start()
        {
            StartCoroutine(Sentence(storyText));
        }

        private IEnumerator Sentence(string sentence)
        {
            textBox.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                textBox.text += letter;
                yield return null;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("Square") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                LevelLoader.inst.LoadLevel(1);
            }
        }
    }
}
