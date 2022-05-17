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
    public GameObject LoginUI;
    public GameObject RegisterUI;
    public GameObject UserDataUI;
    public GameObject ScoreboardUI;
    public GameObject MessegeUI;

    public Animator MessegeUIanim;



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

    public void GoToMenu()
    {
        sceneFader.FadeTo(ToMenu);
    }

    public void GoToSelectLevel()
    {
        sceneFader.FadeTo(ToLevelSelect);
    }

    public void SwitchUIScreen(string UIname)
    {
        switch (UIname)
        {
            case "Login":
                LoginUI.SetActive(true);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(false);
                break;
            case "Register":
                LoginUI.SetActive(false);
                RegisterUI.SetActive(true);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(false);
                break;
            case "UserData":
                LoginUI.SetActive(false);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(true);
                ScoreboardUI.SetActive(false);
                break;
            case "ScoreBoard":
                LoginUI.SetActive(false);
                RegisterUI.SetActive(false);
                UserDataUI.SetActive(false);
                ScoreboardUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OpenMessegeUI()
    {
        MessegeUI.SetActive(true);
        MessegeUIanim.SetBool("IsAppear", true);
    }

    public void CloseMessegeUI()
    {
        MessegeUIanim.SetBool("IsAppear", false);

        Invoke("CloseUILater", 0.3f);
    }

    public void CloseUILater()
    {
        MessegeUI.SetActive(false);
    }
}