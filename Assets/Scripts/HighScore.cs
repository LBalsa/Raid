using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void AddScore(string name, int score)
    {
        int newScore;
        string newName;
        int oldScore;
        string oldName;
        newScore = score;
        newName = name;

        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey(i + "HScore"))
            {
                if (PlayerPrefs.GetInt(i + "HScore") < newScore)
                {
                    // new score is higher than the stored score
                    oldScore = PlayerPrefs.GetInt(i + "HScore");
                    oldName = PlayerPrefs.GetString(i + "HScoreName");
                    PlayerPrefs.SetInt(i + "HScore", newScore);
                    PlayerPrefs.SetString(i + "HScoreName", newName);
                    newScore = oldScore;
                    newName = oldName;
                }
            }
            else
            {
                PlayerPrefs.SetInt(i + "HScore", newScore);
                PlayerPrefs.SetString(i + "HScoreName", newName);
                newScore = 0;
                newName = "";
            }
        }
    }
}
