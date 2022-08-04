using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject UI;

    public string SceneMenuName = "Menu";

    public SceneFader sceneFader;

    MusicManager musicManager;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>().GetComponent<MusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.P)))
        {
            GamePause();
        }
    }

    public void GamePause()
    {
        UI.SetActive(!UI.activeSelf);

        if (UI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        GamePause();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        GamePause();
        sceneFader.FadeTo(SceneMenuName);
        musicManager.BGMLevel00();
    }
}
