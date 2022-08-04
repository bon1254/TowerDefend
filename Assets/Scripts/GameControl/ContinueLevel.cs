using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueLevel : MonoBehaviour
{
    public string SceneMenuName = "Login";

    public SceneFader sceneFader;

    public string nextlevel = "Level02";
    public int LevelToUnlock;

    MusicManager musicManager;

    public void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>().GetComponent<MusicManager>();
    }

    public void Continue()
    {
        sceneFader.FadeTo(nextlevel);
    }

    public void Menu()
    {
        sceneFader.FadeTo(SceneMenuName);

        musicManager.BGMLevel00();
    }
}
