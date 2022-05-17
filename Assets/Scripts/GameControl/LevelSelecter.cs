using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelecter : MonoBehaviour
{
    public SceneFader sceneFader;

    public Button[] LevelButtons;

    void Start()
    {
        int HigheseLevel = PlayerPrefs.GetInt("HigheseLevel", 1);

        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if (i + 1 > HigheseLevel)
            {
                LevelButtons[i].interactable = false;
                LevelButtons[i].image.raycastTarget = false;
                LevelButtons[i].GetComponentInChildren<Text>().raycastTarget = false;
            }          
        }
    }

    public void Select(string levelName)
    {
        sceneFader.FadeTo(levelName);
    }
}
