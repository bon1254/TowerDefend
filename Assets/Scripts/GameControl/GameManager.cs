using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver = false;

    public GameObject GameOverUI;
    public GameObject CompleteLevelUI;

    public TMP_Text _Exp;
    public TMP_Text _Kills;
    private int m_loss = 0;
    private int m_highestlevel = 0;

    public int nextLevelIndex;

    FirebaseManager firebaseManager;
    MusicManager musicManager;
    
    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>().GetComponent<FirebaseManager>();
        musicManager = FindObjectOfType<MusicManager>().GetComponent<MusicManager>();
        musicManager.BGMLevel01();
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsOver)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            WinLevel();
        }

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }    
    }

    void EndGame()
    {
        GameIsOver = true;
        m_loss += 1;
        int load_loss = PlayerPrefs.GetInt("Loss");
        m_loss += load_loss;

        //firebaseManager.SaveData(int.Parse(_Exp.text.ToString()), int.Parse(_Kills.text.ToString()), load_loss, m_highestlevel);
        GameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        GameIsOver = true;
        int load_loss = PlayerPrefs.GetInt("Loss");
        int load_highestlevel = PlayerPrefs.GetInt("HighestLevel");

        m_highestlevel += 1;
        m_highestlevel += load_highestlevel;

        firebaseManager.SaveData(int.Parse(_Exp.text.ToString()), int.Parse(_Kills.text.ToString()), load_loss, m_highestlevel);
        CompleteLevelUI.SetActive(true);
    }
}
