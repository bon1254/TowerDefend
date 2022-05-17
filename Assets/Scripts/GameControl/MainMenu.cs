using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button[] Buttons;
    public SoundsManager soundsManager;

    public string levelToload = "Login";

    public SceneFader sceneFader;

    void Awake()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
    }

    // Start is called before the first frame update
    public void GoLogin()
    {
        sceneFader.FadeTo(levelToload);

        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
    }

    // Update is called once per frame
    public void Exit()
    {
        Application.Quit();
    }
}
