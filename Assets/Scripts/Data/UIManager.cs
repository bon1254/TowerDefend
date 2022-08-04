using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public FirebaseManager firebaseManager;

    public string ToLevelSelect = "LevelSelect";
    public string ToMenu = "Menu";
    public SceneFader sceneFader;

    //Screen object variables
    public GameObject MainUI;
    public GameObject LoginUI;
    public GameObject RegisterUI;
    public GameObject UserDataUI;
    public GameObject ScoreboardUI;
    public GameObject LevelSelectUI;
    public GameObject MessegeUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SwitchUIScreen(string UIname)
    {
        switch (UIname)
        {
            case "Main":
                MainUI.SetActive(true);
                LoginUI.SetActive(false);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(false);
                LevelSelectUI.SetActive(false);
                break;
            case "Login":
                MainUI.SetActive(false);
                LoginUI.SetActive(true);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive (false);
                ScoreboardUI.SetActive(false);
                LevelSelectUI.SetActive(false);
                break;
            case "Register":
                MainUI.SetActive(false);
                LoginUI.SetActive(false);
                RegisterUI.SetActive(true);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(false);
                LevelSelectUI.SetActive(false);
                break;
            case "UserData":
                MainUI.SetActive(false);
                LoginUI.SetActive(false);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(true);
                ScoreboardUI.SetActive(false);
                LevelSelectUI.SetActive(false);
                break;
            case "ScoreBoard":
                MainUI.SetActive(false);
                LoginUI.SetActive(false);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(true);
                LevelSelectUI.SetActive(false);
                break;
            case "LevelSelect":
                MainUI.SetActive(false);
                LoginUI.SetActive(false);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(false);
                LevelSelectUI.SetActive(true);
                break;
        }
    }
    public void OpenLevelSelectUI()
    {
        LevelSelectUI.SetActive(true);
    }

    public void CloseLevelSelectUI()
    {
        LevelSelectUI.SetActive(false);
        SwitchUIScreen("UserData");
    }

    public void OpenMessegeUI()
    {
        MessegeUI.SetActive(true);
    }

    public void CloseMessegeUI()
    {
        MessegeUI.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}